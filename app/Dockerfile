FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS prepare-build-env

#Downloading dependencies
RUN apt-get update
RUN apt-get -y install default-jre
RUN apt-get -y install make procps

RUN dotnet tool install --global dotnet-sonarscanner --version 5.5.3
RUN dotnet tool install --global dotnet-reportgenerator-globaltool
RUN dotnet tool install --global coverlet.console --version 3.0.3
RUN dotnet tool install --global Microsoft.dotnet-openapi --version 6.0.8
RUN dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI --version 3.9.57
RUN dotnet tool install --global Swashbuckle.AspNetCore.Cli --version 6.4.0
RUN dotnet tool install --global dotnet-ef --version 6.0.8
ENV PATH=${PATH}:/root/.dotnet/tools

WORKDIR /app
RUN devNull=$(mkdir sonar-report)
RUN devNull=$(mkdir test-report)
RUN devNull=$(mkdir test-results)
RUN devNull=$(mkdir behavioral-test-results)
RUN devNull=$(mkdir swagger-output)

FROM prepare-build-env AS build-env

COPY ./App.sln ./App.sln
COPY ./scripts ./scripts/
COPY ./settings ./settings/
COPY ./src ./src/
COPY ./test ./test/
COPY ./Makefile ./Makefile

RUN apt-get update && apt-get install -y dnsutils ca-certificates curl apt-transport-https lsb-release gnupg
RUN curl -sL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor | tee /etc/apt/trusted.gpg.d/microsoft.gpg > /dev/null
RUN echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $(lsb_release -cs) main" | tee /etc/apt/sources.list.d/azure-cli.list
RUN apt-get update
RUN apt-get install azure-cli -y

RUN dotnet restore
RUN dotnet build
ARG PROJECT_PATH
RUN dotnet publish ${PROJECT_PATH} -c Release -o out

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

FROM build-env as functional

CMD ["echo", "shutting down"]

