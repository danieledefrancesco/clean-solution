env ?= 'dev'
WEB_DEV_SUFFIX=-web-dev
WEB_PROD_SUFFIX=-web-prod
DOCKER_IMAGE_PREFIX=clean-solution
WEB_DEV_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_DEV_SUFFIX}
WEB_PROD_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_PROD_SUFFIX}

build_and_tag_web_prod:
	docker build -f Dockerfile.web.prod -t "$(WEB_PROD_IMAGE_NAME):latest" -t "$(docker_username)/$(WEB_PROD_IMAGE_NAME):$(version)" .

build_and_tag_web_dev:
	docker build -f Dockerfile.web.dev -t "$(WEB_DEV_IMAGE_NAME):latest" -t "$(docker_username)/$(WEB_DEV_IMAGE_NAME):$(version)" .

start_sonar_scan_ci:	
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml up -d
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml exec web bash ./scripts/wait_sonar_is_green.sh
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml exec web bash ./scripts/create_sonar_project.sh
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml exec web bash ./scripts/begin_sonar_scan.sh
	
end_sonar_scan_ci:
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml exec web bash ./scripts/end_sonar_scan.sh

shut_containers_down_ci:
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml down
	
run_whole_sonar_flow: start_sonar_scan_ci run_unit_tests_ci run_behavioral_tests_ci end_sonar_scan_ci

start_development_mode:
	docker-compose -f docker-compose.mongodb.yml -f docker-compose.web-dev.yml -f docker-compose.kibana.yml up -d
    
stop_development_mode:
	docker-compose -f docker-compose.mongodb.yml -f docker-compose.web-dev.yml -f docker-compose.kibana.yml down
    
run_unit_tests_dev:
	docker-compose -f docker-compose.mongodb-test.yml -f docker-compose.web-dev.yml -f docker-compose.kibana.yml exec web bash -c "dotnet test"
	
run_behavioral_tests_dev:
	docker-compose -f docker-compose.mongodb-test.yml -f docker-compose.web-dev.yml -f docker-compose.kibana.yml -f docker-compose.karate.yml run karate make test
	
run_unit_tests_ci:
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml exec web sh ./scripts/run_unit_tests.sh
	
run_behavioral_tests_ci:
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml exec -d web bash -c "sh ./scripts/start_with_coverlet_watch.sh > coverlet.log"
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml -f docker-compose.karate.yml run karate make test
	WEB_DEV_IMAGE_NAME=$(WEB_DEV_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-dev-ci.yml -f docker-compose.kibana.yml exec web bash ./scripts/end_coverlet.sh

run_behavioral_tests_prod_ci:
	WEB_DEV_IMAGE_NAME=$(WEB_PROD_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.web-prod-ci.yml -f docker-compose.kibana.yml -f docker-compose.karate.yml run karate make test
	
