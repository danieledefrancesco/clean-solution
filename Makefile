WEB_DEV_SUFFIX=-web-dev
WEB_PROD_SUFFIX=-web-prod
DOCKER_IMAGE_PREFIX=clean-solution
WEB_DEV_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_DEV_SUFFIX}
WEB_PROD_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_PROD_SUFFIX}

LOCAL_DEV_DOCKER_COMPOSE_COMMAND=WEB_ENV=dev docker-compose --env-file ./.env
LOCAL_TEST_DOCKER_COMPOSE_COMMAND=WEB_ENV=dev docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml --env-file ./.env
CI_DEV_DOCKER_COMPOSE_COMMAND=WEB_ENV=dev docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file ./.env
CI_PROD_DOCKER_COMPOSE_COMMAND=WEB_ENV=prod docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml --env-file ./.env

docker_username=local
version=latest

build_and_tag_web_prod:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_PROD_IMAGE_NAME):$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) build web

build_and_tag_web_dev:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) build web

start_sonar_scan_ci:	
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) up -d
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T sonarqube bash /scripts/create_sonar_project.sh
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make begin_sonar_scan
	
end_sonar_scan_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make end_sonar_scan

shut_containers_down_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) down
	
run_whole_sonar_flow: start_sonar_scan_ci run_unit_tests_ci run_behavioral_tests_ci end_sonar_scan_ci

start_development_mode:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) up -d
    
stop_development_mode:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) down
    
run_unit_tests_dev:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) run web bash -c "dotnet test"
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) down

run_behavioral_tests_dev:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) up -d
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) run karate make test
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) down
	
run_unit_tests_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make run_unit_tests
	
run_behavioral_tests_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T -d web make start_with_coverlet_watch
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) run karate bash -c "make test"
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make end_coverlet

run_behavioral_tests_prod_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_PROD_IMAGE_NAME):$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) up -d
	WEB_IMAGE_NAME="$(docker_username)/$(WEB_PROD_IMAGE_NAME):$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) run karate bash -c "make test"
