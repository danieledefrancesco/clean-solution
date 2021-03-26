env ?= 'dev'
SDK_SUFFIX=-sdk
WEB_SUFFIX=-web
DOCKER_IMAGE_PREFIX=clean-solution
SDK_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${SDK_SUFFIX}
WEB_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_SUFFIX}
       
up:
	ENVIRONMENT=$(env) docker-compose -f docker-compose.web.yml -f docker-compose.kibana.yml -f docker-compose.mongodb.yml up -d
	
down:
	ENVIRONMENT=$(env) docker-compose -f docker-compose.web.yml -f docker-compose.kibana.yml -f docker-compose.mongodb.yml down
	
unit_test:
	docker-compose -f docker-compose.sdk.yml run sdk sh ./scripts/test_runner.sh unit

behavioral_test:
	WEB_IMAGE_NAME=$(WEB_IMAGE_NAME) ENVIRONMENT=test docker-compose -f docker-compose.web.yml -f docker-compose.karate.yml -f docker-compose.mongodb-test.yml build
	WEB_IMAGE_NAME=$(WEB_IMAGE_NAME) ENVIRONMENT=test docker-compose -f docker-compose.web.yml -f docker-compose.karate.yml -f docker-compose.mongodb-test.yml up -d --force-recreate
	WEB_IMAGE_NAME=$(WEB_IMAGE_NAME) ENVIRONMENT=test docker-compose -f docker-compose.web.yml -f docker-compose.karate.yml -f docker-compose.mongodb-test.yml run karate make test
	WEB_IMAGE_NAME=$(WEB_IMAGE_NAME) ENVIRONMENT=test docker-compose -f docker-compose.web.yml -f docker-compose.karate.yml -f docker-compose.mongodb-test.yml down
	
build_and_tag_web:
	docker build -f web.Dockerfile -t "$(WEB_IMAGE_NAME):latest" -t "$(docker_username)/$(WEB_IMAGE_NAME):$(version)" .

build_and_tag_sdk:
	docker build -f sdk.Dockerfile -t "$(SDK_IMAGE_NAME):latest" -t "$(docker_username)/$(SDK_IMAGE_NAME):$(version)" .
	
run_sonar_scan:
	docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml up -d
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml run sdk bash ./scripts/wait_sonar_is_green.sh
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml run sdk bash ./scripts/create_sonar_project.sh
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml run sdk bash ./scripts/sonar_scan.sh
	SDK_IMAGE_NAME=$(SDK_IMAGE_NAME) docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml down
