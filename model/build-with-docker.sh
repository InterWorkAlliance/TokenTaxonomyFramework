#!/bin/bash

docker build -t iwa-model .
docker run -it -v `pwd`/..:/var/iwa iwa-model