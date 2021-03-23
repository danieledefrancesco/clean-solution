FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build-env

#Downloading dependencies
RUN mkdir /usr/share/man/man1/
RUN apt-get update
RUN apt-get -y install default-jre
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash
RUN apt-get -y install nodejs
RUN node -v
RUN npm -v
RUN npm install -g sonar-report

RUN dotnet tool install --global dotnet-sonarscanner --version 5.1.0
RUN dotnet tool install --global dotnet-reportgenerator-globaltool
ENV PATH=${PATH}:/root/.dotnet/tools


WORKDIR /app
RUN devNull=$(mkdir sonar-report)
RUN devNull=$(mkdir test-report)
RUN devNull=$(mkdir test-results)

COPY ./CleanSolution.sln ./CleanSolution.sln
COPY ./scripts ./scripts/
COPY ./settings ./settings/
COPY ./src ./src/
COPY ./test ./test/
