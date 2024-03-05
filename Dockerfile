FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY . .
RUN dotnet restore "./src/Api/Api.csproj"
RUN dotnet publish "./src/Api/Api.csproj" -c $BUILD_CONFIGURATION -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app

EXPOSE 8080
EXPOSE 8081

VOLUME /app/logs

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Api.dll"]