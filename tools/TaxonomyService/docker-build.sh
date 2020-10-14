#!/usr/bin/env bash
docker build -f TaxonomyClient/Dockerfile -t txclient .
docker build -f TaxonomyService/Dockerfile -t iwa/ttf/taxonomyservice .
#docker build -f envoy/Dockerfile -t iwa/ttf/envoy .
#docker build -f ../TTF-Web-UI/Dockerfile -t iwa/ttf/ui ../TTF-Web-UI
docker build -f ../TTF-Printer/TTF-Printer/Dockerfile -t iwa/ttf/printer ../TTF-Printer
docker-compose up
