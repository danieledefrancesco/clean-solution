env ?= 'dev'
SDK_SUFFIX=-sdk
WEB_SUFFIX=-web
DOCKER_IMAGE_PREFIX=clean-solution
SDK_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${SDK_SUFFIX}
WEB_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_SUFFIX}

run_unit_tests:
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml exec sdk sh ./scripts/run_unit_tests.sh
	
run_behavioral_tests:
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml exec -d sdk bash -c "sh ./scripts/start_with_coverlet_watch.sh > coverlet.log"
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml -f docker-compose.karate.yml run karate make test
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml exec sdk bash ./scripts/end_coverlet.sh

build_and_tag_web:
	docker build -f web.Dockerfile -t "$(WEB_IMAGE_NAME):latest" -t "$(docker_username)/$(WEB_IMAGE_NAME):$(version)" .

build_and_tag_sdk:
	docker build -f sdk.Dockerfile -t "$(SDK_IMAGE_NAME):latest" -t "$(docker_username)/$(SDK_IMAGE_NAME):$(version)" .

start_sonar_scan:	
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml up -d
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml exec sdk bash ./scripts/wait_sonar_is_green.sh
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml exec sdk bash ./scripts/create_sonar_project.sh
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml exec sdk bash ./scripts/begin_sonar_scan.sh
	
end_sonar_scan:
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml exec sdk bash ./scripts/end_sonar_scan.sh

shut_containers_down:
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml -f docker-compose.kibana.yml down
	
run_whole_sonar_flow: start_sonar_scan run_unit_tests run_behavioral_tests end_sonar_scan