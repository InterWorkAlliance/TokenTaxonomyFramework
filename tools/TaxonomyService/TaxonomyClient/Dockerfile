FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
# copy necessary files
COPY TaxonomyClient TaxonomyClient
COPY TaxonomyModel TaxonomyModel

WORKDIR /app/TaxonomyModel
RUN dotnet restore
WORKDIR /app/TaxonomyClient
RUN dotnet publish -f netcoreapp3.1 -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
LABEL version=ttf1.0
WORKDIR /app

COPY --from=build /app/TaxonomyClient/out /app
ENTRYPOINT ["dotnet", "TaxonomyClient.dll"]