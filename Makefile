env ?= 'dev'    
       
up:
	ENVIRONMENT=$(env) docker-compose -f docker-compose.web.yml build
	ENVIRONMENT=$(env) docker-compose -f docker-compose.web.yml -f docker-compose.kibana.yml -f docker-compose.mongodb.yml up -d
	
bash_web:
	ENVIRONMENT=$(env) docker-compose -f docker-compose.web.yml run web /bin/sh
	
bash_sdk:
	ENVIRONMENT=$(env) docker-compose -f docker-compose.sdk.yml run sdk /bin/sh
	
down:
	ENVIRONMENT=$(env) docker-compose -f docker-compose.web.yml -f docker-compose.kibana.yml -f docker-compose.mongodb.yml down
	
unit_test:
	docker-compose -f docker-compose.sdk.yml run sdk bash -c "bash ./scripts/test_runner.sh unit"

behavioral_test:
	ENVIRONMENT=test docker-compose -f docker-compose.web.yml -f docker-compose.karate.yml -f docker-compose.mongodb-test.yml build
	ENVIRONMENT=test docker-compose -f docker-compose.web.yml -f docker-compose.karate.yml -f docker-compose.mongodb-test.yml up -d --force-recreate
	ENVIRONMENT=test docker-compose -f docker-compose.web.yml -f docker-compose.karate.yml -f docker-compose.mongodb-test.yml run karate bash "-c make test"
    ENVIRONMENT=test docker-compose -f docker-compose.web.yml -f docker-compose.karate.yml -f docker-compose.mongodb-test.yml down
	
build_and_tag_web:
	docker build -f web.Dockerfile -t clean-solution-web:$(version) .

build_and_tag_sdk:
	docker build -f sdk.Dockerfile -t clean-solution-sdk:$(version) .
	
run_sonar_scan:
	docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml up -d
	docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml run sdk bash -c "bash ./scripts/wait_sonar_is_green.sh"
	docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml run sdk bash -c "bash ./scripts/create_sonar_project.sh"
	docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml -f docker-compose.sdk.yml run sdk bash -c "bash ./scripts/sonar_scan.sh"
	docker-compose -f docker-compose.sonarqube.yml -f docker-compose.mongodb-test.yml down
    	