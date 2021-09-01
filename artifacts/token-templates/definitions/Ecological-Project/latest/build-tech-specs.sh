#!/bin/bash
#requires protoc be installed and in your path.
#For Go, install the go plugin: https://github.com/golang/protobuf
#For ts, install the ts plugin: https://github.com/improbable-eng/ts-protoc-gen
PROTO_PATH="${PROTO_PATH:-../../../../../../../.nuget/packages/google.protobuf.tools/3.17.0/tools}"
CSHARP_PLUGIN="${CSHARP_PLUGIN:-../../../../../../../.nuget/packages/grpc.tools/2.37.1/tools/macosx_x64/grpc_csharp_plugin}"
echo "paths:"
echo $PROTO_PATH
echo $CSHARP_PLUGIN
rm -rf ./out
mkdir -p ./out/csharp
mkdir -p ./out/java
mkdir -p ./out/go
mkdir -p ./out/js
mkdir -p ./out/ts
mkdir -p ./out/api

#you will need to adjust the relative path to the protoc and grpc tools.

protoc --csharp_out=./out/csharp --java_out=./out/java --js_out=import_style=commonjs:./out/js --js_out=import_style=commonjs:./out/ts --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./out/js --grpc-web_out=import_style=typescript,mode=grpcwebtext:./out/ts --proto_path=./ --proto_path=../../../../base --proto_path=../../Ecological-Claim/latest/ --proto_path=../../common --proto_path=$PROTO_PATH --grpc_out ./out/api ./Ecological-Project.proto  --plugin=protoc-gen-grpc=$CSHARP_PLUGIN --plugin=protoc-gen-web


