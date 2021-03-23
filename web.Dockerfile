FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore

RUN dotnet publish src/AspNetCore.Examples.ProductService/AspNetCore.Examples.ProductService.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY --from=build-env /app/out .
RUN ls
ENTRYPOINT ["dotnet", "AspNetCore.Examples.ProductService.dll"]