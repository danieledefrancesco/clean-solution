FROM maven:3.6.3-jdk-11 as prepare-env
ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get clean
RUN apt-get update
RUN apt-get install -y --quiet \
        make \
        apt-transport-https \
        ca-certificates \
        curl \
        gnupg-agent \
        software-properties-common

RUN curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add -

RUN add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/debian buster stable"

RUN apt-get update && apt-get install -y --quiet docker-ce-cli
RUN chsh -s /bin/bash

FROM prepare-env

WORKDIR /usr/src/karate
COPY ./Makefile /usr/src/karate/Makefile
COPY ./pom.xml /usr/src/karate/pom.xml

RUN make install

COPY . /usr/src/karate/
