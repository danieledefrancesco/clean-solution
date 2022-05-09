$WEB_DEV_SUFFIX="-web-dev"
$WEB_PROD_SUFFIX="-web-prod"
$DOCKER_IMAGE_PREFIX="clean-solution"
$WEB_DEV_IMAGE_NAME="${DOCKER_IMAGE_PREFIX}${WEB_DEV_SUFFIX}"
$WEB_PROD_IMAGE_NAME="${DOCKER_IMAGE_PREFIX}${WEB_PROD_SUFFIX}"

function Exec-Command ($command) {
    switch ($command) {
        "start_development_mode" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose --env-file .env up -d
            break
        }
        "stop_development_mode" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose --env-file .env down
            break
        }
        "run_behavioral_tests_dev" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose --env-file ./.env up -d
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml --env-file .env run karate make test
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml --env-file .env down
            break
        }
        "run_behavioral_tests_ci" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file ./.env exec -T -d web make start_with_coverlet_watch
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env run karate make test
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env exec -T web make end_coverlet
            break
        }
        "run_behavioral_tests_prod" {
            $env:WEB_IMAGE_NAME=$WEB_PROD_IMAGE_NAME
            $env:WEB_ENV="prod"
            docker-compose --env-file ./.env up -d
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml --env-file .env run karate /bin/bash -c "make test"
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml --env-file .env down
            break
        }
        "run_unit_tests" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose --env-file .env up -d
            docker-compose --env-file .env exec web make run_unit_tests
            docker-compose --env-file .env down
            break
        }
        "run_unit_tests_ci" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env exec -T web make run_unit_tests
            break
        }
        "build_prod" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="prod"
            docker-compose --env-file .env build web
            break
        }
        "build_dev" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose --env-file .env build web
            break
        }
        "start_sonar_scan_ci" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env up -d
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env exec -T sonarqube bash /scripts/create_sonar_project.sh
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env exec -T web make begin_sonar_scan      
            break
        }
        "end_sonar_scan_ci" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env exec -T web make end_sonar_scan     
            break
        }
        "generate_sonar_report" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env exec -T web make generate_sonar_report     
            break
        }
        "shut_containers_down_ci" {
            $env:WEB_IMAGE_NAME=$WEB_DEV_IMAGE_NAME
            $env:WEB_ENV="dev"
            docker-compose -f docker-compose.yaml -f docker-compose.karate.yaml -f docker-compose.sonarqube.yaml --env-file .env down    
            break
        }
        "run_whole_sonar_flow" {
            Exec-Command start_sonar_scan_ci
            Exec-Command run_unit_tests_ci
            Exec-Command run_behavioral_tests_ci
            Exec-Command shut_containers_down_ci
            Exec-Command generate_sonar_report
        }
        default {
            Write-Host "Command $command is not supported"
            break
        }           
    }
}

Exec-Command $args[0]