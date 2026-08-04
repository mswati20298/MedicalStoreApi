# ============================
# Build Stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy all source files
COPY . .

# Restore NuGet packages
RUN dotnet restore "MedStoreAPI.sln"

# Publish API
RUN dotnet publish "MedStoreAPI.API/MedStoreAPI.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ============================
# Runtime Stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "MedStoreAPI.API.dll"]