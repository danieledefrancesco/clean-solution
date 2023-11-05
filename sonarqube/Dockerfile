FROM ubuntu:latest as dos2unix

RUN apt-get update && apt-get install -y dos2unix
COPY ./scripts /scripts
RUN dos2unix /scripts/healthcheck.sh && dos2unix /scripts/create_sonar_project.sh && dos2unix /scripts/wait_sonar_is_green.sh

FROM sonarqube:10.2-community

USER root
RUN apt update && apt install -y curl

USER sonarqube
COPY --from=dos2unix /scripts /scripts