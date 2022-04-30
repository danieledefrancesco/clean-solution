FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS prepare-build-env

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
RUN dotnet tool install --global coverlet.console --version 3.0.3
ENV PATH=${PATH}:/root/.dotnet/tools


WORKDIR /app
RUN devNull=$(mkdir sonar-report)
RUN devNull=$(mkdir test-report)
RUN devNull=$(mkdir test-results)
RUN devNull=$(mkdir behavioral-test-results)

FROM prepare-build-env AS build-env

COPY ./CleanSolution.sln ./CleanSolution.sln
COPY ./scripts ./scripts/
COPY ./settings ./settings/
COPY ./src ./src/
COPY ./test ./test/

RUN dotnet publish src/AspNetCore.Examples.ProductService/AspNetCore.Examples.ProductService.csproj -c Debug -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS prod
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AspNetCore.Examples.ProductService.dll"]

FROM build-env as dev

CMD ["dotnet", "watch", "run", "--project", "src/AspNetCore.Examples.ProductService/AspNetCore.Examples.ProductService.csproj", "--urls=http://0.0.0.0:80"]

