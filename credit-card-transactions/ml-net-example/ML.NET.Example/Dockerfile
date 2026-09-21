FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ML.NET.Training/ML.NET.Training.csproj", "ML.NET.Training/"]
COPY ["ML.NET.API/ML.NET.API.csproj", "ML.NET.API/"]
RUN dotnet restore "./ML.NET.API/ML.NET.API.csproj"
COPY . .
WORKDIR "/src/ML.NET.API"
RUN dotnet build "./ML.NET.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ML.NET.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Run the application as a secure, non-root user (built into .NET 8+)
USER 1001

HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ML.NET.API.dll"]