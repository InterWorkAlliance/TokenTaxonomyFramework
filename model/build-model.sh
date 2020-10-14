#!/bin/bash
#requires protoc be installed and in your path.
#For Go, install the go plugin: https://github.com/golang/protobuf
#For ts, install the ts plugin: https://github.com/improbable-eng/ts-protoc-gen
PROTO_PATH="${PROTO_PATH:-../../../.nuget/packages/google.protobuf.tools/3.13.0/tools}"
CSHARP_PLUGIN="${CSHARP_PLUGIN:-../../../.nuget/packages/grpc.tools/2.31.0/tools/macosx_x64/grpc_csharp_plugin}"
echo "paths:"
echo $PROTO_PATH
echo $CSHARP_PLUGIN

mkdir -p ../tools/TaxonomyObjectModel/out/csharp
mkdir -p ../tools/TaxonomyObjectModel/out/java
mkdir -p ../tools/TaxonomyObjectModel/out/go
mkdir -p ../tools/TaxonomyObjectModel/out/js
mkdir -p ../tools/TaxonomyObjectModel/out/ts

#you will need to adjust the relative path to the protoc and grpc tools.

protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/ts --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --grpc-web_out=import_style=typescript,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/ts --proto_path=./protos --proto_path=$PROTO_PATH ./protos/artifact.proto --plugin=protoc-gen-web
protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/ts --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --grpc-web_out=import_style=typescript,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/ts --proto_path=./protos --proto_path=$PROTO_PATH ./protos/core.proto --plugin=protoc-gen-web
protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/ts --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --grpc-web_out=import_style=typescript,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/ts --proto_path=./protos --proto_path=$PROTO_PATH ./protos/taxonomy.proto --plugin=protoc-gen-web
protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/ts --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --grpc-web_out=import_style=typescript,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/ts --proto_path=./protos --proto_path=$PROTO_PATH  --grpc_out ../tools/TaxonomyService/TaxonomyService ./protos/service.proto --plugin=protoc-gen-grpc=$CSHARP_PLUGIN --plugin=protoc-gen-web
protoc --proto_path=./protos --proto_path=$PROTO_PATH  --grpc_out=no_server:../tools/TaxonomyService/TaxonomyClient ./protos/service.proto --plugin=protoc-gen-grpc=$CSHARP_PLUGIN
protoc --proto_path=./protos --proto_path=$PROTO_PATH  --grpc_out=no_server:../tools/TTF-Printer/TTF-Printer/Model ./protos/service.proto --plugin=protoc-gen-grpc=$CSHARP_PLUGIN

#printersvc
protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --grpc_out=no_server:../tools/TaxonomyService/TaxonomyClient --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/ts --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --grpc-web_out=import_style=typescript,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/ts --proto_path=./protos --proto_path=$PROTO_PATH  --grpc_out=../tools/TTF-Printer/TTF-Printer ./protos/printersvc.proto --plugin=protoc-gen-grpc=$CSHARP_PLUGIN --plugin=protoc-gen-web

#protoc --proto_path=./protos --proto_path=$PROTO_PATH  --grpc_out=no_server:../tools/UI-Sandbox/UI-Sandbox ./protos/service.proto --plugin=protoc-gen-grpc=$CSHARP_PLUGIN
#protoc --proto_path=./protos --proto_path=$PROTO_PATH  --grpc_out=no_server:../tools/UI-Sandbox/UI-Sandbox ./protos/printersvc.proto --plugin=protoc-gen-grpc=$CSHARP_PLUGIN

cp ../tools/TaxonomyObjectModel/out/csharp/* ../tools/ArtifactGenerator/ArtifactGenerator/model

cp ../tools/TaxonomyObjectModel/out/csharp/* ../tools/TaxonomyService/TaxonomyModel
cp ../tools/TaxonomyObjectModel/out/csharp/* ../tools/TTF-Printer/TTF-Printer/Model
#cp ../tools/TaxonomyObjectModel/out/csharp/* ../tools/TTF-Win/Model

cp ../tools/TaxonomyObjectModel/out/ts/* ../tools/TTF-Web-UI/src/model
for f in `ls ../tools/TTF-Web-UI/src/model`
do
  echo '/* eslint-disable */' | cat - ../tools/TTF-Web-UI/src/model/$f > ../tools/TTF-Web-UI/src/model/$f.new; mv ../tools/TTF-Web-UI/src/model/$f.new ../tools/TTF-Web-UI/src/model/$f
done

