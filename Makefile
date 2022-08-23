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
branch_name?=main

build_and_tag:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) build


start_sonar_scan_ci:	
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) up -d
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T sonarqube bash /scripts/create_sonar_project.sh
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web /bin/bash -c "make begin_sonar_scan BRANCH_NAME=$(branch_name)"
	
end_sonar_scan_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make end_sonar_scan
	
generate_sonar_report:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make generate_sonar_report

shut_containers_down_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) down
	
run_sonar_flow: start_sonar_scan_ci run_unit_tests_ci end_sonar_scan_ci

start_development_mode:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) up -d sqlserver
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) run dev make update_database
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) up -d queues
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) run dev bash -c "make create_queue QUEUE_NAME=onproductcreated"
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) up -d
    
stop_development_mode:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) down
    
run_unit_tests_dev:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(LOCAL_DEV_DOCKER_COMPOSE_COMMAND) run dev make run_unit_tests

run_unit_tests_ci:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_DEV_DOCKER_COMPOSE_COMMAND) exec -T web make run_unit_tests
	
run_functional_tests:
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) up -d sqlserver
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) run dev /bin/bash -c "make update_database"
	WEB_IMAGE_NAME="$(docker_username)/$(DOCKER_IMAGE_PREFIX)" VERSION="$(version)" $(CI_PROD_DOCKER_COMPOSE_COMMAND) up -d web
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
	docker save --output tmp-images/dev.tar $(docker_username)/$(DOCKER_IMAGE_PREFIX)-dev:$(version)

save_prod_image:
	docker save --output tmp-images/prod.tar $(docker_username)/$(DOCKER_IMAGE_PREFIX)-prod:$(version)

save_all_images: save_dev_image save_prod_image

load_dev_image:
	docker load tmp-images/dev.tar

load_prod_image:
	docker load tmp-images/prod.tar

load_all_images: load_dev_image load_prod_image