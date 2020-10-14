FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /
# copy necessary files

COPY ArtifactGenerator app
WORKDIR /app
RUN dotnet restore  && \
    dotnet publish -c Release -o out
    
FROM microsoft/dotnet:2.2-runtime 
WORKDIR /app
COPY --from=build /app/out /app

ENTRYPOINT ["dotnet", "FactGen.dll"]