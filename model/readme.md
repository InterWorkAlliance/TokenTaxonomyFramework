# TTF Model

The Taxonomy Object Model (TOM) is a Protocol Buffers (Protobuf) based object model for the artifacts, metadata and content, that make up the TTF. The TOM should be the primary means for interacting with artifacts and the TTF using applications build for web, model and desktop based applications.

The TTF's artifacts are stored in the [artifacts](../artifacts) folder as JSON files. The TOM serializes and de-serializes to these files, using the [Taxonomy Service](../tools/TaxonomyService) that can perform all create, read, update and delete operations on the TOM and saving it to the artifacts folder structure.

You can modify an artifact's JSON file directly, but references and symbols to other artifacts will need to be manually checked.

- Documentation of the [TOM](tom.md)
- Documentation for the gRpc [Taxonomy Services](taxonomyServices.md)

## Prerequisites

- install the protoc [gRpcWeb](https://github.com/grpc/grpc-web/releases) extension and [install](https://github.com/grpc/grpc-web).
