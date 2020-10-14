#!/usr/bin/env bash
docker run --name ttfRegistry -v pathToArtifactsFolder:path/artifacts -p 8086:8086 taxonomyService