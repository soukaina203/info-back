FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copier tous les fichiers de la racine (incluant .csproj)
COPY *.csproj ./
RUN dotnet restore


COPY ./ ./
# Build et publish en Release
RUN dotnet publish -c Release --output ./publish /p:PublishReadyToRun=false --no-self-contained

# Image runtime plus légère
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000

# Copier les fichiers publiés de l'étape build-env
COPY --from=build-env /app/publish .


# Lancer l'application (remplace "coreApi.dll" par le nom de ton dll généré)
ENTRYPOINT ["dotnet", "info-backend.dll"]


