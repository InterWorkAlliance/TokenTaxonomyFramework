FROM node:10.15.1-stretch

RUN apt-get update && apt-get install protobuf-compiler nuget -y && apt-get clean
RUN npm i grpc grpc-web ts-protoc-gen
RUN nuget install google.protobuf.tools -version 3.8.0
RUN nuget install grpc.tools -version 1.22.0
RUN wget -q https://github.com/grpc/grpc-web/releases/download/1.0.6/protoc-gen-grpc-web-1.0.6-linux-x86_64 -O /usr/local/bin/protoc-gen-grpc-web && chmod +x /usr/local/bin/protoc-gen-grpc-web
RUN chmod +x /Grpc.Tools.1.22.0/tools/linux_x64/grpc_csharp_plugin
ENTRYPOINT ["/bin/bash", "-c", "export PROTO_PATH=\"/Google.Protobuf.Tools.3.8.0/tools\"; export CSHARP_PLUGIN=\"/Grpc.Tools.1.22.0/tools/linux_x64/grpc_csharp_plugin\"; cd /var/iwa/model; ./build-model.sh"]