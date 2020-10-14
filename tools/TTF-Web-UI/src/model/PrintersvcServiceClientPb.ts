/* eslint-disable */
/**
 * @fileoverview gRPC-Web generated client stub for taxonomy.ttfprinter
 * @enhanceable
 * @public
 */

// GENERATED CODE -- DO NOT EDIT!


import * as grpcWeb from 'grpc-web';

import * as artifact_pb from './artifact_pb';

import {
  ArtifactToPrint,
  PrintResult,
  PrintTTFOptions} from './printersvc_pb';

export class PrinterServiceClient {
  client_: grpcWeb.AbstractClientBase;
  hostname_: string;
  credentials_: null | { [index: string]: string; };
  options_: null | { [index: string]: string; };

  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: string; }) {
    if (!options) options = {};
    if (!credentials) credentials = {};
    options['format'] = 'text';

    this.client_ = new grpcWeb.GrpcWebClientBase(options);
    this.hostname_ = hostname;
    this.credentials_ = credentials;
    this.options_ = options;
  }

  methodInfoPrintTTFArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    PrintResult,
    (request: ArtifactToPrint) => {
      return request.serializeBinary();
    },
    PrintResult.deserializeBinary
  );

  printTTFArtifact(
    request: ArtifactToPrint,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: PrintResult) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.ttfprinter.PrinterService/PrintTTFArtifact',
      request,
      metadata || {},
      this.methodInfoPrintTTFArtifact,
      callback);
  }

  methodInfoPrintTTF = new grpcWeb.AbstractClientBase.MethodInfo(
    PrintResult,
    (request: PrintTTFOptions) => {
      return request.serializeBinary();
    },
    PrintResult.deserializeBinary
  );

  printTTF(
    request: PrintTTFOptions,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: PrintResult) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.ttfprinter.PrinterService/PrintTTF',
      request,
      metadata || {},
      this.methodInfoPrintTTF,
      callback);
  }

}

