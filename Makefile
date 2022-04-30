WEB_DEV_SUFFIX=-web-dev
WEB_PROD_SUFFIX=-web-prod
DOCKER_IMAGE_PREFIX=clean-solution
WEB_DEV_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_DEV_SUFFIX}
WEB_PROD_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_PROD_SUFFIX}

LOCAL_DEV_DOCKER_COMPOSE_COMMAND=WEB_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) WEB_ENV=dev docker-compose --env-file ./.env
LOCAL_TEST_DOCKER_COMPOSE_COMMAND=WEB_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) WEB_ENV=dev docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml --env-file ./.env
CI_DEV_DOCKER_COMPOSE_COMMAND=WEB_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) WEB_ENV=dev docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file ./.env
CI_PROD_DOCKER_COMPOSE_COMMAND=WEB_IMAGE_NAME=$(WEB_PROD_IMAGE_NAME) WEB_ENV=prod docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml --env-file ./.env


build_and_tag_web_prod:
	docker build -t "$(WEB_PROD_IMAGE_NAME):latest" -t "$(docker_username)/$(WEB_PROD_IMAGE_NAME):$(version)" app

build_and_tag_web_dev:
	docker build -t "$(WEB_DEV_IMAGE_NAME):latest" -t "$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" app

start_sonar_scan_ci:	
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) up -d
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web bash ./scripts/wait_sonar_is_green.sh
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web bash ./scripts/create_sonar_project.sh
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web bash ./scripts/begin_sonar_scan.sh
	
end_sonar_scan_ci:
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web bash ./scripts/end_sonar_scan.sh

shut_containers_down_ci:
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) down
	
run_whole_sonar_flow: start_sonar_scan_ci run_unit_tests_ci run_behavioral_tests_ci end_sonar_scan_ci

start_development_mode:
	$(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) up -d
    
stop_development_mode:
	$(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) down
    
run_unit_tests_dev:
	$(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) up -d
	$(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) exec -T web bash -c "dotnet test"
	$(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) down

run_behavioral_tests_dev:
	$(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) up -d
	sleep 30s	
	$(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) run karate make test
	$(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) down
	
run_unit_tests_ci:
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web bash -c "sh ./scripts/run_unit_tests.sh"
	
run_behavioral_tests_ci:
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T -d web bash -c "sh ./scripts/start_with_coverlet_watch.sh > coverlet.log"
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) run karate bash -c "make test"
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web bash -c "sh ./scripts/end_coverlet.sh"

run_behavioral_tests_prod_ci:
	$(CI_PROD_DOCKER_COMPOSE_COMMAND) up -d
	$(CI_PROD_DOCKER_COMPOSE_COMMAND) run karate bash -c "make test"
