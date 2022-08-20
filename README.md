# Clean Solution

The purpose of this project is to provide a template for an AspNet 6 Web API project based on the clean architecture.
The project also provides a template for a CI pipeline, built with github actions, which executes the following steps:
- builds and tags the docker containers
- runs the unit tests and uploads an artifact with the coverage report
- runs the functional tests and uploads an artifact with the report
- runs the sonarqube analysis
- generates the swagger and uploads it as an artifact
- pushes the containers to the docker registry

## Dependencies

In order to run the project you need:

- A linux OS or WSL if using windows
- docker and docker-compose executable without root privileges
- make installed

## Useful commands

The following commands can be executed from the project root directory.

### 1. Build
```
make build_and_tag
```
Builds and tags the docker containers

### 2. Run the local environment
```
make start_development_mode
```
Runs the container needed to test the application locally.
The containers are:
- web: the AspNet 6 Web API app
- wiremock: the mocks of the dependencies of web
- sqlserver: the SQL Server database

In development mode the web application is started with the `dotnet watch` command, therefore each change made to the code will immediately be visible in the running application.

### 3. Stop the local environment

```
make stop_development_mode
```
Shuts the docker containers down

### 4. Run the unit tests
```
make run_unit_tests_dev
```
Runs the unit tests. The coverage report will be stored in the `<project_root>/app/test-report` directory.
### 5. Run the functional tests
```
make run_functional_tests_dev
```
Runs the functional tests. The report will be stored in the `<project-root>/app/behavioral-test-report` directory.
### 6. Generate the swagger
```
make generate_swagger
```
Generates the swagger file. It will be stored in the `<project-root>/app/swagger-output` directory.
### 6. Run the sonarqube analysis
```
make run_sonar_flow
```
Runs the sonarqube analysis. The results will be available at http://localhost:9000