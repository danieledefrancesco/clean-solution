$WEB_DEV_SUFFIX="-web-dev"
$WEB_PROD_SUFFIX="-web-prod"
$DOCKER_IMAGE_PREFIX="clean-solution"
$WEB_DEV_IMAGE_NAME="${DOCKER_IMAGE_PREFIX}${WEB_DEV_SUFFIX}"
$WEB_PROD_IMAGE_NAME="${DOCKER_IMAGE_PREFIX}${WEB_PROD_SUFFIX}"

switch ($args[0])
{
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
        docker-compose --env-file .env run web bash -c "dotnet test"
        docker-compose --env-file .env down
        break
    }     
}