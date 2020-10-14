/* eslint-disable */
import * as jspb from "google-protobuf"

import * as artifact_pb from './artifact_pb';

export class ArtifactToPrint extends jspb.Message {
  getType(): artifact_pb.ArtifactType;
  setType(value: artifact_pb.ArtifactType): void;

  getId(): string;
  setId(value: string): void;

  getDraft(): boolean;
  setDraft(value: boolean): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ArtifactToPrint.AsObject;
  static toObject(includeInstance: boolean, msg: ArtifactToPrint): ArtifactToPrint.AsObject;
  static serializeBinaryToWriter(message: ArtifactToPrint, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ArtifactToPrint;
  static deserializeBinaryFromReader(message: ArtifactToPrint, reader: jspb.BinaryReader): ArtifactToPrint;
}

export namespace ArtifactToPrint {
  export type AsObject = {
    type: artifact_pb.ArtifactType,
    id: string,
    draft: boolean,
  }
}

export class PrintTTFOptions extends jspb.Message {
  getBook(): boolean;
  setBook(value: boolean): void;

  getDraft(): boolean;
  setDraft(value: boolean): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): PrintTTFOptions.AsObject;
  static toObject(includeInstance: boolean, msg: PrintTTFOptions): PrintTTFOptions.AsObject;
  static serializeBinaryToWriter(message: PrintTTFOptions, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): PrintTTFOptions;
  static deserializeBinaryFromReader(message: PrintTTFOptions, reader: jspb.BinaryReader): PrintTTFOptions;
}

export namespace PrintTTFOptions {
  export type AsObject = {
    book: boolean,
    draft: boolean,
  }
}

export class PrintResult extends jspb.Message {
  getOpenXmlDocument(): string;
  setOpenXmlDocument(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): PrintResult.AsObject;
  static toObject(includeInstance: boolean, msg: PrintResult): PrintResult.AsObject;
  static serializeBinaryToWriter(message: PrintResult, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): PrintResult;
  static deserializeBinaryFromReader(message: PrintResult, reader: jspb.BinaryReader): PrintResult;
}

export namespace PrintResult {
  export type AsObject = {
    openXmlDocument: string,
  }
}

