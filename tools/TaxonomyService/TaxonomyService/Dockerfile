#This is for a Local GitHub cloned instance.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
# copy necessary files
COPY TaxonomyModel TaxonomyModel
COPY TaxonomyService TaxonomyService

# Restore all the dependencoes
WORKDIR /app/TaxonomyModel
RUN dotnet restore
WORKDIR /app/TaxonomyService
RUN dotnet publish -f netcoreapp3.1 -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
LABEL version=ttf1.0
WORKDIR /app

COPY --from=build /app/TaxonomyService/out /app
EXPOSE 8086/tcp
ENTRYPOINT ["dotnet", "TaxonomyService.dll"]