WEB_DEV_SUFFIX=-web-dev
WEB_PROD_SUFFIX=-web-prod
DOCKER_IMAGE_PREFIX=clean-solution
WEB_DEV_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_DEV_SUFFIX}
WEB_PROD_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_PROD_SUFFIX}

WEB_DEV_DOCKER_COMPOSE_FILE=docker/web/docker-compose.web-dev.yml
WEB_DEV_CI_DOCKER_COMPOSE_FILE=docker/web/docker-compose.web-dev-ci.yml
WEB_PROD_CI_DOCKER_COMPOSE_FILE=docker/web/docker-compose.web-prod-ci.yml
MONGODB_TEST_DOCKER_COMPOSE_FILE=docker/mongo/docker-compose.mongodb-test.yml
MONGODB_DOCKER_COMPOSE_FILE=docker/mongo/docker-compose.mongodb.yml
SONARQUBE_DOCKER_COMPOSE_FILE=docker/sonarqube/docker-compose.sonarqube.yml
ELK_DOCKER_COMPOSE_FILE=docker/elk/docker-compose.elk.yml
KARATE_DOCKER_COMPOSE_FILE=docker/karate/docker-compose.karate.yml

WEB_DEV_DOCKER_FILE=docker/web/Dockerfile.web.dev
WEB_PROD_DOCKER_FILE=docker/web/Dockerfile.web.prod

LOCAL_DEV_DOCKER_COMPOSE_COMMAND=WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f $(MONGODB_DOCKER_COMPOSE_FILE) -f $(WEB_DEV_DOCKER_COMPOSE_FILE) -f $(ELK_DOCKER_COMPOSE_FILE)
LOCAL_TEST_DOCKER_COMPOSE_COMMAND=WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f $(MONGODB_TEST_DOCKER_COMPOSE_FILE) -f $(WEB_DEV_DOCKER_COMPOSE_FILE) -f $(ELK_DOCKER_COMPOSE_FILE) -f $(KARATE_DOCKER_COMPOSE_FILE)
CI_DEV_DOCKER_COMPOSE_COMMAND=WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f ${SONARQUBE_DOCKER_COMPOSE_FILE} -f ${MONGODB_TEST_DOCKER_COMPOSE_FILE} -f ${WEB_DEV_CI_DOCKER_COMPOSE_FILE} -f ${ELK_DOCKER_COMPOSE_FILE} -f ${KARATE_DOCKER_COMPOSE_FILE}
CI_PROD_DOCKER_COMPOSE_COMMAND=WEB_PROD_IMAGE_NAME=$(WEB_PROD_IMAGE_NAME) docker-compose -f ${SONARQUBE_DOCKER_COMPOSE_FILE} -f ${MONGODB_TEST_DOCKER_COMPOSE_FILE} -f ${WEB_PROD_CI_DOCKER_COMPOSE_FILE} -f ${ELK_DOCKER_COMPOSE_FILE} -f ${KARATE_DOCKER_COMPOSE_FILE}


build_and_tag_web_prod:
	docker build -f ${WEB_PROD_DOCKER_FILE} -t "$(WEB_PROD_IMAGE_NAME):latest" -t "$(docker_username)/$(WEB_PROD_IMAGE_NAME):$(version)" .

build_and_tag_web_dev:
	docker build -f ${WEB_DEV_DOCKER_FILE} -t "$(WEB_DEV_IMAGE_NAME):latest" -t "$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" .

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
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) run karate make test
	$(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web bash -c "sh ./scripts/end_coverlet.sh"

run_behavioral_tests_prod_ci:
	$(CI_PROD_DOCKER_COMPOSE_COMMAND) run karate make test
