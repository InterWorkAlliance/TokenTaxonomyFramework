#!/bin/bash
#requires protoc be installed and in your path. ex. export PATH=$PATH:~/.nuget/packages/google.protobuf.tools/3.20.1/tools/macosx_x64
#requires protoc-gen-web be installed - https://github.com/grpc/grpc-web/releases 
#For Go, install the go plugin: https://github.com/golang/protobuf
#For ts, install the ts plugin: https://github.com/improbable-eng/ts-protoc-gen
PROTO_PATH="${PROTO_PATH:-../../../../../.nuget/packages/google.protobuf.tools/3.29.1/tools}"
CSHARP_PLUGIN="${CSHARP_PLUGIN:-../../../../../.nuget/packages/grpc.tools/2.29.1/tools/macosx_x64/grpc_csharp_plugin}"
#PROTOC_GEN_TS_PATH="${PROTOC_GEN_TS_PATH:-./node_modules/.bin/protoc-gen-ts}"
echo "paths:"
echo $PROTO_PATH
echo $CSHARP_PLUGIN
#echo $PROTOC_GEN_TS_PATH

mkdir -p ../../tools/dmrv/out/csharp
mkdir -p ../../tools/dmrv/out/java
#mkdir -p ../../tools/dmrv/out/go
#mkdir -p ../../tools/dmrv/out/js
#mkdir -p ../../tools/dmrv/out/ts


#you will need to adjust the relative path to the protoc and grpc tools.

protoc --csharp_out=../../tools/dmrv/out/csharp --java_out=../../tools/dmrv/out/java --proto_path=./ --proto_path=$PROTO_PATH ./common.proto
protoc --csharp_out=../../tools/dmrv/out/csharp --java_out=../../tools/dmrv/out/java --proto_path=./ --proto_path=$PROTO_PATH ./dmrv.proto
protoc --csharp_out=../../tools/dmrv/out/csharp --java_out=../../tools/dmrv/out/java --proto_path=./  --proto_path=. --proto_path=$PROTO_PATH ./sustainability.proto



cp ../../tools/dmrv/out/csharp/* ../../tools/DmrvApi/Model




