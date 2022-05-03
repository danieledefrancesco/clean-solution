FROM ubuntu:latest as dos2unix

RUN apt-get update && apt-get install -y dos2unix
COPY ./scripts /scripts
RUN dos2unix /scripts/healthcheck.sh
RUN dos2unix /scripts/create_new_sonar_project.sh

FROM sonarqube:8.2-community
COPY --from=dos2unix /scripts /scripts