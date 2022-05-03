FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS prepare-build-env

#Downloading dependencies
RUN apt-get update
RUN apt-get -y install default-jre
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash
RUN apt-get -y install nodejs
RUN apt-get -y install make
RUN node -v
RUN npm -v
RUN npm install -g sonar-report

RUN dotnet tool install --global dotnet-sonarscanner --version 5.1.0
RUN dotnet tool install --global dotnet-reportgenerator-globaltool
RUN dotnet tool install --global coverlet.console --version 3.0.3
ENV PATH=${PATH}:/root/.dotnet/tools


WORKDIR /app
RUN devNull=$(mkdir sonar-report)
RUN devNull=$(mkdir test-report)
RUN devNull=$(mkdir test-results)
RUN devNull=$(mkdir behavioral-test-results)

FROM prepare-build-env AS build-env

COPY ./App.sln ./App.sln
COPY ./scripts ./scripts/
COPY ./settings ./settings/
COPY ./src ./src/
COPY ./test ./test/

ARG PROJECT_PATH
RUN dotnet publish ${PROJECT_PATH} -c Debug -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS prepare-prod-env
RUN apt-get update && apt-get -y install curl

FROM prepare-prod-env AS prod
WORKDIR /web
COPY --from=build-env /app/out .
COPY scripts/run_prod.sh .
ARG DLL_NAME
ENV DLL_NAME=$DLL_NAME
CMD ["sh", "./run_prod.sh"]

FROM build-env as dev
ARG PROJECT_PATH
ENV PROJECT_PATH=$PROJECT_PATH
CMD ["sh", "./scripts/run_dev.sh"]

