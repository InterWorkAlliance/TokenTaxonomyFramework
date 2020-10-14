FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
# copy necessary files
COPY TTF-Printer TTF-Printer

# Restore all the dependencoes
WORKDIR /app/TTF-Printer
RUN dotnet restore
RUN dotnet publish -f netcoreapp3.1 -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
LABEL version=ttf1.0
WORKDIR /app

COPY --from=build /app/TTF-Printer/out /app
EXPOSE 8088/tcp
ENTRYPOINT ["dotnet", "TTF-Printer.dll"]