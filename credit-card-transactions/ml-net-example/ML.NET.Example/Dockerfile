FROM mcr.microsoft.com/dotnet/sdk
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet

# Run the application as a secure, non-root user (built into .NET 8+)
USER $APP_UID

WORKDIR /App
COPY --from=build /App/out .

ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ML.NET.Training/ML.NET.Training.csproj", "ML.NET.Training/"]
COPY ["ML.NET.API/ML.NET.API.csproj", "ML.NET.API/"]
RUN dotnet restore "./ML.NET.API/ML.NET.API.csproj"
COPY . .
WORKDIR "/src/DockerDemoWebApi"
RUN dotnet build "./ML.NET.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ML.NET.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ML.NET.API.dll"]