#!/usr/bin/env bash
docker build -f TaxonomyClient/Dockerfile -t txclient:1.0.1 .
docker build -f TaxonomyService/Dockerfile -t eeatcreg.azurecr.io/iwa/ttf/taxonomyservice:1.0.1 .
docker build -f envoy/Dockerfile -t eeatcreg.azurecr.io/iwa/ttf/envoy:1.0.1 .
docker build -f ../TTF-Web-UI/Dockerfile -t eeatcreg.azurecr.io/iwa/ttf/ui:1.0.1 ../TTF-Web-UI
docker build -f ../TTF-Printer/TTF-Printer/Dockerfile -t eeatcreg.azurecr.io/iwa/ttf/printer:1.0.1 ../TTF-Printer


docker push eeatcreg.azurecr.io/iwa/ttf/taxonomyservice:1.0.1
docker push eeatcreg.azurecr.io/iwa/ttf/envoy:1.0.1
docker push eeatcreg.azurecr.io/iwa/ttf/ui:1.0.1
docker push eeatcreg.azurecr.io/iwa/ttf/printer:1.0.1
