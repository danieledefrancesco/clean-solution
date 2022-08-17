WEB_DEV_SUFFIX=-web-dev
WEB_PROD_SUFFIX=-web-prod
DOCKER_IMAGE_PREFIX=clean-solution
WEB_DEV_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_DEV_SUFFIX}
WEB_PROD_IMAGE_NAME=${DOCKER_IMAGE_PREFIX}${WEB_PROD_SUFFIX}

LOCAL_DEV_DOCKER_COMPOSE_COMMAND=WEB_ENV=dev docker-compose --env-file ./.env
LOCAL_TEST_DOCKER_COMPOSE_COMMAND=WEB_ENV=dev docker-compose -f docker-compose.yaml -f docker-compose.functional.yaml --env-file ./.env
CI_DEV_DOCKER_COMPOSE_COMMAND=WEB_ENV=dev docker-compose -f docker-compose.yaml -f docker-compose.functional.yaml -f docker-compose.sonarqube.yaml --env-file ./.env
CI_PROD_DOCKER_COMPOSE_COMMAND=WEB_ENV=prod docker-compose -f docker-compose.yaml -f docker-compose.functional.yaml --env-file ./.env

docker_username=local
version=latest

build_and_tag:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) build functional


start_sonar_scan_ci:	
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) up -d
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T sonarqube bash /scripts/create_sonar_project.sh
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) ecec -T web make begin_sonar_scan
	
end_sonar_scan_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make end_sonar_scan
	
generate_sonar_report:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make generate_sonar_report

shut_containers_down_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) down
	
run_whole_sonar_flow: start_sonar_scan_ci run_unit_tests_ci run_functional_tests_ci end_sonar_scan_ci generate_sonar_report

start_development_mode:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) up -d
    
stop_development_mode:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) down
    
run_unit_tests_dev:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) run dev make run_unit_tests

run_functional_tests_dev:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) up -d
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) run dev /bin/bash -c "make update_database"
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) run functional /bin/bash -c "make run_functional_tests"
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_TEST_DOCKER_COMPOSE_COMMAND) down
	
run_unit_tests_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make run_unit_tests
	
run_functional_tests_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T -d web make start_with_coverlet_watch
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) run dev /bin/bash -c "make update_database"
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) run functional /bin/bash -c "make run_functional_tests"
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make end_coverlet

run_functional_tests_prod_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) up -d
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) run functional /bin/bash -c "make run_functional_tests"

MIFGRATION_NAME?=DEFAULT_MIGRATION
add_migration_local:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) run web /bin/bash -c "make add_migration MIGRATION_NAME=$(MIGRATION_NAME)"

update_database_local:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) run web /bin/bash -c "make update_database"

push_images:
	docker push $(docker_username)/$(DOCKER_IMAGE_PREFIX)-prod:$(version)
	docker push $(docker_username)/$(DOCKER_IMAGE_PREFIX)-dev:$(version)

generate_swagger:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) run dev make generate_swagger

save_dev_image:
	docker save --output dev.tar $(docker_username)/$(DOCKER_IMAGE_PREFIX)-dev:$(version)

save_prod_image:
	docker save --output prod.tar $(docker_username)/$(DOCKER_IMAGE_PREFIX)-prod:$(version)

save_all_images: save_dev_image save_prod_image

load_dev_image:
	docker load dev.tar

load_prod_image:
	docker load prod.tar

load_all_images: load_dev_image load_prod_image