/* eslint-disable */
import * as jspb from "google-protobuf"

import * as google_protobuf_any_pb from 'google-protobuf/google/protobuf/any_pb';

export class Classification extends jspb.Message {
  getTemplateType(): TemplateType;
  setTemplateType(value: TemplateType): void;

  getTokenType(): TokenType;
  setTokenType(value: TokenType): void;

  getTokenUnit(): TokenUnit;
  setTokenUnit(value: TokenUnit): void;

  getRepresentationType(): RepresentationType;
  setRepresentationType(value: RepresentationType): void;

  getValueType(): ValueType;
  setValueType(value: ValueType): void;

  getSupply(): Supply;
  setSupply(value: Supply): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Classification.AsObject;
  static toObject(includeInstance: boolean, msg: Classification): Classification.AsObject;
  static serializeBinaryToWriter(message: Classification, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Classification;
  static deserializeBinaryFromReader(message: Classification, reader: jspb.BinaryReader): Classification;
}

export namespace Classification {
  export type AsObject = {
    templateType: TemplateType,
    tokenType: TokenType,
    tokenUnit: TokenUnit,
    representationType: RepresentationType,
    valueType: ValueType,
    supply: Supply,
  }
}

export class ArtifactSymbol extends jspb.Message {
  getId(): string;
  setId(value: string): void;

  getType(): ArtifactType;
  setType(value: ArtifactType): void;

  getVisual(): string;
  setVisual(value: string): void;

  getTooling(): string;
  setTooling(value: string): void;

  getVersion(): string;
  setVersion(value: string): void;

  getTemplateValidated(): boolean;
  setTemplateValidated(value: boolean): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ArtifactSymbol.AsObject;
  static toObject(includeInstance: boolean, msg: ArtifactSymbol): ArtifactSymbol.AsObject;
  static serializeBinaryToWriter(message: ArtifactSymbol, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ArtifactSymbol;
  static deserializeBinaryFromReader(message: ArtifactSymbol, reader: jspb.BinaryReader): ArtifactSymbol;
}

export namespace ArtifactSymbol {
  export type AsObject = {
    id: string,
    type: ArtifactType,
    visual: string,
    tooling: string,
    version: string,
    templateValidated: boolean,
  }
}

export class Artifact extends jspb.Message {
  getArtifactSymbol(): ArtifactSymbol | undefined;
  setArtifactSymbol(value?: ArtifactSymbol): void;
  hasArtifactSymbol(): boolean;
  clearArtifactSymbol(): void;

  getName(): string;
  setName(value: string): void;

  getAliasesList(): Array<string>;
  setAliasesList(value: Array<string>): void;
  clearAliasesList(): void;
  addAliases(value: string, index?: number): void;

  getArtifactDefinition(): ArtifactDefinition | undefined;
  setArtifactDefinition(value?: ArtifactDefinition): void;
  hasArtifactDefinition(): boolean;
  clearArtifactDefinition(): void;

  getDependenciesList(): Array<SymbolDependency>;
  setDependenciesList(value: Array<SymbolDependency>): void;
  clearDependenciesList(): void;
  addDependencies(value?: SymbolDependency, index?: number): SymbolDependency;

  getIncompatibleWithSymbolsList(): Array<ArtifactSymbol>;
  setIncompatibleWithSymbolsList(value: Array<ArtifactSymbol>): void;
  clearIncompatibleWithSymbolsList(): void;
  addIncompatibleWithSymbols(value?: ArtifactSymbol, index?: number): ArtifactSymbol;

  getInfluencedBySymbolsList(): Array<SymbolInfluence>;
  setInfluencedBySymbolsList(value: Array<SymbolInfluence>): void;
  clearInfluencedBySymbolsList(): void;
  addInfluencedBySymbols(value?: SymbolInfluence, index?: number): SymbolInfluence;

  getControlUri(): string;
  setControlUri(value: string): void;

  getArtifactFilesList(): Array<ArtifactFile>;
  setArtifactFilesList(value: Array<ArtifactFile>): void;
  clearArtifactFilesList(): void;
  addArtifactFiles(value?: ArtifactFile, index?: number): ArtifactFile;

  getMaps(): Maps | undefined;
  setMaps(value?: Maps): void;
  hasMaps(): boolean;
  clearMaps(): void;

  getContributorsList(): Array<Contributor>;
  setContributorsList(value: Array<Contributor>): void;
  clearContributorsList(): void;
  addContributors(value?: Contributor, index?: number): Contributor;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Artifact.AsObject;
  static toObject(includeInstance: boolean, msg: Artifact): Artifact.AsObject;
  static serializeBinaryToWriter(message: Artifact, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Artifact;
  static deserializeBinaryFromReader(message: Artifact, reader: jspb.BinaryReader): Artifact;
}

export namespace Artifact {
  export type AsObject = {
    artifactSymbol?: ArtifactSymbol.AsObject,
    name: string,
    aliasesList: Array<string>,
    artifactDefinition?: ArtifactDefinition.AsObject,
    dependenciesList: Array<SymbolDependency.AsObject>,
    incompatibleWithSymbolsList: Array<ArtifactSymbol.AsObject>,
    influencedBySymbolsList: Array<SymbolInfluence.AsObject>,
    controlUri: string,
    artifactFilesList: Array<ArtifactFile.AsObject>,
    maps?: Maps.AsObject,
    contributorsList: Array<Contributor.AsObject>,
  }
}

export class ArtifactReferenceValues extends jspb.Message {
  getControlUri(): string;
  setControlUri(value: string): void;

  getArtifactFilesList(): Array<ArtifactFile>;
  setArtifactFilesList(value: Array<ArtifactFile>): void;
  clearArtifactFilesList(): void;
  addArtifactFiles(value?: ArtifactFile, index?: number): ArtifactFile;

  getMaps(): Maps | undefined;
  setMaps(value?: Maps): void;
  hasMaps(): boolean;
  clearMaps(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ArtifactReferenceValues.AsObject;
  static toObject(includeInstance: boolean, msg: ArtifactReferenceValues): ArtifactReferenceValues.AsObject;
  static serializeBinaryToWriter(message: ArtifactReferenceValues, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ArtifactReferenceValues;
  static deserializeBinaryFromReader(message: ArtifactReferenceValues, reader: jspb.BinaryReader): ArtifactReferenceValues;
}

export namespace ArtifactReferenceValues {
  export type AsObject = {
    controlUri: string,
    artifactFilesList: Array<ArtifactFile.AsObject>,
    maps?: Maps.AsObject,
  }
}

export class ArtifactReference extends jspb.Message {
  getId(): string;
  setId(value: string): void;

  getType(): ArtifactType;
  setType(value: ArtifactType): void;

  getReferenceNotes(): string;
  setReferenceNotes(value: string): void;

  getValues(): ArtifactReferenceValues | undefined;
  setValues(value?: ArtifactReferenceValues): void;
  hasValues(): boolean;
  clearValues(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ArtifactReference.AsObject;
  static toObject(includeInstance: boolean, msg: ArtifactReference): ArtifactReference.AsObject;
  static serializeBinaryToWriter(message: ArtifactReference, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ArtifactReference;
  static deserializeBinaryFromReader(message: ArtifactReference, reader: jspb.BinaryReader): ArtifactReference;
}

export namespace ArtifactReference {
  export type AsObject = {
    id: string,
    type: ArtifactType,
    referenceNotes: string,
    values?: ArtifactReferenceValues.AsObject,
  }
}

export class SymbolInfluence extends jspb.Message {
  getDescription(): string;
  setDescription(value: string): void;

  getSymbol(): ArtifactSymbol | undefined;
  setSymbol(value?: ArtifactSymbol): void;
  hasSymbol(): boolean;
  clearSymbol(): void;

  getAppliesToList(): Array<ArtifactSymbol>;
  setAppliesToList(value: Array<ArtifactSymbol>): void;
  clearAppliesToList(): void;
  addAppliesTo(value?: ArtifactSymbol, index?: number): ArtifactSymbol;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): SymbolInfluence.AsObject;
  static toObject(includeInstance: boolean, msg: SymbolInfluence): SymbolInfluence.AsObject;
  static serializeBinaryToWriter(message: SymbolInfluence, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): SymbolInfluence;
  static deserializeBinaryFromReader(message: SymbolInfluence, reader: jspb.BinaryReader): SymbolInfluence;
}

export namespace SymbolInfluence {
  export type AsObject = {
    description: string,
    symbol?: ArtifactSymbol.AsObject,
    appliesToList: Array<ArtifactSymbol.AsObject>,
  }
}

export class Contributor extends jspb.Message {
  getName(): string;
  setName(value: string): void;

  getOrganization(): string;
  setOrganization(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Contributor.AsObject;
  static toObject(includeInstance: boolean, msg: Contributor): Contributor.AsObject;
  static serializeBinaryToWriter(message: Contributor, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Contributor;
  static deserializeBinaryFromReader(message: Contributor, reader: jspb.BinaryReader): Contributor;
}

export namespace Contributor {
  export type AsObject = {
    name: string,
    organization: string,
  }
}

export class SymbolDependency extends jspb.Message {
  getDescription(): string;
  setDescription(value: string): void;

  getSymbol(): ArtifactSymbol | undefined;
  setSymbol(value?: ArtifactSymbol): void;
  hasSymbol(): boolean;
  clearSymbol(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): SymbolDependency.AsObject;
  static toObject(includeInstance: boolean, msg: SymbolDependency): SymbolDependency.AsObject;
  static serializeBinaryToWriter(message: SymbolDependency, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): SymbolDependency;
  static deserializeBinaryFromReader(message: SymbolDependency, reader: jspb.BinaryReader): SymbolDependency;
}

export namespace SymbolDependency {
  export type AsObject = {
    description: string,
    symbol?: ArtifactSymbol.AsObject,
  }
}

export class ArtifactDefinition extends jspb.Message {
  getBusinessDescription(): string;
  setBusinessDescription(value: string): void;

  getBusinessExample(): string;
  setBusinessExample(value: string): void;

  getAnalogiesList(): Array<ArtifactAnalogy>;
  setAnalogiesList(value: Array<ArtifactAnalogy>): void;
  clearAnalogiesList(): void;
  addAnalogies(value?: ArtifactAnalogy, index?: number): ArtifactAnalogy;

  getComments(): string;
  setComments(value: string): void;

  getArtifact(): Artifact | undefined;
  setArtifact(value?: Artifact): void;
  hasArtifact(): boolean;
  clearArtifact(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ArtifactDefinition.AsObject;
  static toObject(includeInstance: boolean, msg: ArtifactDefinition): ArtifactDefinition.AsObject;
  static serializeBinaryToWriter(message: ArtifactDefinition, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ArtifactDefinition;
  static deserializeBinaryFromReader(message: ArtifactDefinition, reader: jspb.BinaryReader): ArtifactDefinition;
}

export namespace ArtifactDefinition {
  export type AsObject = {
    businessDescription: string,
    businessExample: string,
    analogiesList: Array<ArtifactAnalogy.AsObject>,
    comments: string,
    artifact?: Artifact.AsObject,
  }
}

export class ArtifactAnalogy extends jspb.Message {
  getName(): string;
  setName(value: string): void;

  getDescription(): string;
  setDescription(value: string): void;

  getArtifactDefinition(): ArtifactDefinition | undefined;
  setArtifactDefinition(value?: ArtifactDefinition): void;
  hasArtifactDefinition(): boolean;
  clearArtifactDefinition(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ArtifactAnalogy.AsObject;
  static toObject(includeInstance: boolean, msg: ArtifactAnalogy): ArtifactAnalogy.AsObject;
  static serializeBinaryToWriter(message: ArtifactAnalogy, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ArtifactAnalogy;
  static deserializeBinaryFromReader(message: ArtifactAnalogy, reader: jspb.BinaryReader): ArtifactAnalogy;
}

export namespace ArtifactAnalogy {
  export type AsObject = {
    name: string,
    description: string,
    artifactDefinition?: ArtifactDefinition.AsObject,
  }
}

export class ArtifactFile extends jspb.Message {
  getContent(): ArtifactContent;
  setContent(value: ArtifactContent): void;

  getFileName(): string;
  setFileName(value: string): void;

  getFileData(): Uint8Array | string;
  getFileData_asU8(): Uint8Array;
  getFileData_asB64(): string;
  setFileData(value: Uint8Array | string): void;

  getArtifact(): Artifact | undefined;
  setArtifact(value?: Artifact): void;
  hasArtifact(): boolean;
  clearArtifact(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ArtifactFile.AsObject;
  static toObject(includeInstance: boolean, msg: ArtifactFile): ArtifactFile.AsObject;
  static serializeBinaryToWriter(message: ArtifactFile, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ArtifactFile;
  static deserializeBinaryFromReader(message: ArtifactFile, reader: jspb.BinaryReader): ArtifactFile;
}

export namespace ArtifactFile {
  export type AsObject = {
    content: ArtifactContent,
    fileName: string,
    fileData: Uint8Array | string,
    artifact?: Artifact.AsObject,
  }
}

export class Maps extends jspb.Message {
  getCodeReferencesList(): Array<MapReference>;
  setCodeReferencesList(value: Array<MapReference>): void;
  clearCodeReferencesList(): void;
  addCodeReferences(value?: MapReference, index?: number): MapReference;

  getImplementationReferencesList(): Array<MapReference>;
  setImplementationReferencesList(value: Array<MapReference>): void;
  clearImplementationReferencesList(): void;
  addImplementationReferences(value?: MapReference, index?: number): MapReference;

  getResourcesList(): Array<MapResourceReference>;
  setResourcesList(value: Array<MapResourceReference>): void;
  clearResourcesList(): void;
  addResources(value?: MapResourceReference, index?: number): MapResourceReference;

  getArtifact(): Artifact | undefined;
  setArtifact(value?: Artifact): void;
  hasArtifact(): boolean;
  clearArtifact(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Maps.AsObject;
  static toObject(includeInstance: boolean, msg: Maps): Maps.AsObject;
  static serializeBinaryToWriter(message: Maps, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Maps;
  static deserializeBinaryFromReader(message: Maps, reader: jspb.BinaryReader): Maps;
}

export namespace Maps {
  export type AsObject = {
    codeReferencesList: Array<MapReference.AsObject>,
    implementationReferencesList: Array<MapReference.AsObject>,
    resourcesList: Array<MapResourceReference.AsObject>,
    artifact?: Artifact.AsObject,
  }
}

export class MapReference extends jspb.Message {
  getMappingType(): MappingType;
  setMappingType(value: MappingType): void;

  getName(): string;
  setName(value: string): void;

  getPlatform(): TargetPlatform;
  setPlatform(value: TargetPlatform): void;

  getReferencePath(): string;
  setReferencePath(value: string): void;

  getMaps(): Maps | undefined;
  setMaps(value?: Maps): void;
  hasMaps(): boolean;
  clearMaps(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): MapReference.AsObject;
  static toObject(includeInstance: boolean, msg: MapReference): MapReference.AsObject;
  static serializeBinaryToWriter(message: MapReference, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): MapReference;
  static deserializeBinaryFromReader(message: MapReference, reader: jspb.BinaryReader): MapReference;
}

export namespace MapReference {
  export type AsObject = {
    mappingType: MappingType,
    name: string,
    platform: TargetPlatform,
    referencePath: string,
    maps?: Maps.AsObject,
  }
}

export class MapResourceReference extends jspb.Message {
  getMappingType(): MappingType;
  setMappingType(value: MappingType): void;

  getName(): string;
  setName(value: string): void;

  getDescription(): string;
  setDescription(value: string): void;

  getResourcePath(): string;
  setResourcePath(value: string): void;

  getMaps(): Maps | undefined;
  setMaps(value?: Maps): void;
  hasMaps(): boolean;
  clearMaps(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): MapResourceReference.AsObject;
  static toObject(includeInstance: boolean, msg: MapResourceReference): MapResourceReference.AsObject;
  static serializeBinaryToWriter(message: MapResourceReference, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): MapResourceReference;
  static deserializeBinaryFromReader(message: MapResourceReference, reader: jspb.BinaryReader): MapResourceReference;
}

export namespace MapResourceReference {
  export type AsObject = {
    mappingType: MappingType,
    name: string,
    description: string,
    resourcePath: string,
    maps?: Maps.AsObject,
  }
}

export class NewArtifactRequest extends jspb.Message {
  getType(): ArtifactType;
  setType(value: ArtifactType): void;

  getArtifact(): google_protobuf_any_pb.Any | undefined;
  setArtifact(value?: google_protobuf_any_pb.Any): void;
  hasArtifact(): boolean;
  clearArtifact(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): NewArtifactRequest.AsObject;
  static toObject(includeInstance: boolean, msg: NewArtifactRequest): NewArtifactRequest.AsObject;
  static serializeBinaryToWriter(message: NewArtifactRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): NewArtifactRequest;
  static deserializeBinaryFromReader(message: NewArtifactRequest, reader: jspb.BinaryReader): NewArtifactRequest;
}

export namespace NewArtifactRequest {
  export type AsObject = {
    type: ArtifactType,
    artifact?: google_protobuf_any_pb.Any.AsObject,
  }
}

export class NewArtifactResponse extends jspb.Message {
  getType(): ArtifactType;
  setType(value: ArtifactType): void;

  getArtifactTypeObject(): google_protobuf_any_pb.Any | undefined;
  setArtifactTypeObject(value?: google_protobuf_any_pb.Any): void;
  hasArtifactTypeObject(): boolean;
  clearArtifactTypeObject(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): NewArtifactResponse.AsObject;
  static toObject(includeInstance: boolean, msg: NewArtifactResponse): NewArtifactResponse.AsObject;
  static serializeBinaryToWriter(message: NewArtifactResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): NewArtifactResponse;
  static deserializeBinaryFromReader(message: NewArtifactResponse, reader: jspb.BinaryReader): NewArtifactResponse;
}

export namespace NewArtifactResponse {
  export type AsObject = {
    type: ArtifactType,
    artifactTypeObject?: google_protobuf_any_pb.Any.AsObject,
  }
}

export class UpdateArtifactRequest extends jspb.Message {
  getType(): ArtifactType;
  setType(value: ArtifactType): void;

  getArtifactTypeObject(): google_protobuf_any_pb.Any | undefined;
  setArtifactTypeObject(value?: google_protobuf_any_pb.Any): void;
  hasArtifactTypeObject(): boolean;
  clearArtifactTypeObject(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): UpdateArtifactRequest.AsObject;
  static toObject(includeInstance: boolean, msg: UpdateArtifactRequest): UpdateArtifactRequest.AsObject;
  static serializeBinaryToWriter(message: UpdateArtifactRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): UpdateArtifactRequest;
  static deserializeBinaryFromReader(message: UpdateArtifactRequest, reader: jspb.BinaryReader): UpdateArtifactRequest;
}

export namespace UpdateArtifactRequest {
  export type AsObject = {
    type: ArtifactType,
    artifactTypeObject?: google_protobuf_any_pb.Any.AsObject,
  }
}

export class UpdateArtifactResponse extends jspb.Message {
  getType(): ArtifactType;
  setType(value: ArtifactType): void;

  getUpdated(): boolean;
  setUpdated(value: boolean): void;

  getArtifactTypeObject(): google_protobuf_any_pb.Any | undefined;
  setArtifactTypeObject(value?: google_protobuf_any_pb.Any): void;
  hasArtifactTypeObject(): boolean;
  clearArtifactTypeObject(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): UpdateArtifactResponse.AsObject;
  static toObject(includeInstance: boolean, msg: UpdateArtifactResponse): UpdateArtifactResponse.AsObject;
  static serializeBinaryToWriter(message: UpdateArtifactResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): UpdateArtifactResponse;
  static deserializeBinaryFromReader(message: UpdateArtifactResponse, reader: jspb.BinaryReader): UpdateArtifactResponse;
}

export namespace UpdateArtifactResponse {
  export type AsObject = {
    type: ArtifactType,
    updated: boolean,
    artifactTypeObject?: google_protobuf_any_pb.Any.AsObject,
  }
}

export class DeleteArtifactRequest extends jspb.Message {
  getArtifactSymbol(): ArtifactSymbol | undefined;
  setArtifactSymbol(value?: ArtifactSymbol): void;
  hasArtifactSymbol(): boolean;
  clearArtifactSymbol(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): DeleteArtifactRequest.AsObject;
  static toObject(includeInstance: boolean, msg: DeleteArtifactRequest): DeleteArtifactRequest.AsObject;
  static serializeBinaryToWriter(message: DeleteArtifactRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): DeleteArtifactRequest;
  static deserializeBinaryFromReader(message: DeleteArtifactRequest, reader: jspb.BinaryReader): DeleteArtifactRequest;
}

export namespace DeleteArtifactRequest {
  export type AsObject = {
    artifactSymbol?: ArtifactSymbol.AsObject,
  }
}

export class DeleteArtifactResponse extends jspb.Message {
  getDeleted(): boolean;
  setDeleted(value: boolean): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): DeleteArtifactResponse.AsObject;
  static toObject(includeInstance: boolean, msg: DeleteArtifactResponse): DeleteArtifactResponse.AsObject;
  static serializeBinaryToWriter(message: DeleteArtifactResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): DeleteArtifactResponse;
  static deserializeBinaryFromReader(message: DeleteArtifactResponse, reader: jspb.BinaryReader): DeleteArtifactResponse;
}

export namespace DeleteArtifactResponse {
  export type AsObject = {
    deleted: boolean,
  }
}

export class TokenTemplateId extends jspb.Message {
  getDefinitionId(): string;
  setDefinitionId(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): TokenTemplateId.AsObject;
  static toObject(includeInstance: boolean, msg: TokenTemplateId): TokenTemplateId.AsObject;
  static serializeBinaryToWriter(message: TokenTemplateId, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): TokenTemplateId;
  static deserializeBinaryFromReader(message: TokenTemplateId, reader: jspb.BinaryReader): TokenTemplateId;
}

export namespace TokenTemplateId {
  export type AsObject = {
    definitionId: string,
  }
}

export class Identifier extends jspb.Message {
  getId(): string;
  setId(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Identifier.AsObject;
  static toObject(includeInstance: boolean, msg: Identifier): Identifier.AsObject;
  static serializeBinaryToWriter(message: Identifier, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Identifier;
  static deserializeBinaryFromReader(message: Identifier, reader: jspb.BinaryReader): Identifier;
}

export namespace Identifier {
  export type AsObject = {
    id: string,
  }
}

export class NewTemplateDefinition extends jspb.Message {
  getTemplateFormulaId(): string;
  setTemplateFormulaId(value: string): void;

  getTokenName(): string;
  setTokenName(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): NewTemplateDefinition.AsObject;
  static toObject(includeInstance: boolean, msg: NewTemplateDefinition): NewTemplateDefinition.AsObject;
  static serializeBinaryToWriter(message: NewTemplateDefinition, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): NewTemplateDefinition;
  static deserializeBinaryFromReader(message: NewTemplateDefinition, reader: jspb.BinaryReader): NewTemplateDefinition;
}

export namespace NewTemplateDefinition {
  export type AsObject = {
    templateFormulaId: string,
    tokenName: string,
  }
}

export class InitializeNewArtifactRequest extends jspb.Message {
  getArtifactType(): ArtifactType;
  setArtifactType(value: ArtifactType): void;

  getName(): string;
  setName(value: string): void;

  getSymbol(): string;
  setSymbol(value: string): void;

  getTemplateType(): TemplateType;
  setTemplateType(value: TemplateType): void;

  getTokenType(): TokenType;
  setTokenType(value: TokenType): void;

  getTokenUnit(): TokenUnit;
  setTokenUnit(value: TokenUnit): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): InitializeNewArtifactRequest.AsObject;
  static toObject(includeInstance: boolean, msg: InitializeNewArtifactRequest): InitializeNewArtifactRequest.AsObject;
  static serializeBinaryToWriter(message: InitializeNewArtifactRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): InitializeNewArtifactRequest;
  static deserializeBinaryFromReader(message: InitializeNewArtifactRequest, reader: jspb.BinaryReader): InitializeNewArtifactRequest;
}

export namespace InitializeNewArtifactRequest {
  export type AsObject = {
    artifactType: ArtifactType,
    name: string,
    symbol: string,
    templateType: TemplateType,
    tokenType: TokenType,
    tokenUnit: TokenUnit,
  }
}

export class InitializeNewArtifactResponse extends jspb.Message {
  getArtifactType(): ArtifactType;
  setArtifactType(value: ArtifactType): void;

  getArtifact(): google_protobuf_any_pb.Any | undefined;
  setArtifact(value?: google_protobuf_any_pb.Any): void;
  hasArtifact(): boolean;
  clearArtifact(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): InitializeNewArtifactResponse.AsObject;
  static toObject(includeInstance: boolean, msg: InitializeNewArtifactResponse): InitializeNewArtifactResponse.AsObject;
  static serializeBinaryToWriter(message: InitializeNewArtifactResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): InitializeNewArtifactResponse;
  static deserializeBinaryFromReader(message: InitializeNewArtifactResponse, reader: jspb.BinaryReader): InitializeNewArtifactResponse;
}

export namespace InitializeNewArtifactResponse {
  export type AsObject = {
    artifactType: ArtifactType,
    artifact?: google_protobuf_any_pb.Any.AsObject,
  }
}

export class CommitUpdatesRequest extends jspb.Message {
  getCommitMessage(): string;
  setCommitMessage(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): CommitUpdatesRequest.AsObject;
  static toObject(includeInstance: boolean, msg: CommitUpdatesRequest): CommitUpdatesRequest.AsObject;
  static serializeBinaryToWriter(message: CommitUpdatesRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): CommitUpdatesRequest;
  static deserializeBinaryFromReader(message: CommitUpdatesRequest, reader: jspb.BinaryReader): CommitUpdatesRequest;
}

export namespace CommitUpdatesRequest {
  export type AsObject = {
    commitMessage: string,
  }
}

export class CommitUpdatesResponse extends jspb.Message {
  getResult(): string;
  setResult(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): CommitUpdatesResponse.AsObject;
  static toObject(includeInstance: boolean, msg: CommitUpdatesResponse): CommitUpdatesResponse.AsObject;
  static serializeBinaryToWriter(message: CommitUpdatesResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): CommitUpdatesResponse;
  static deserializeBinaryFromReader(message: CommitUpdatesResponse, reader: jspb.BinaryReader): CommitUpdatesResponse;
}

export namespace CommitUpdatesResponse {
  export type AsObject = {
    result: string,
  }
}

export class IssuePullRequest extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): IssuePullRequest.AsObject;
  static toObject(includeInstance: boolean, msg: IssuePullRequest): IssuePullRequest.AsObject;
  static serializeBinaryToWriter(message: IssuePullRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): IssuePullRequest;
  static deserializeBinaryFromReader(message: IssuePullRequest, reader: jspb.BinaryReader): IssuePullRequest;
}

export namespace IssuePullRequest {
  export type AsObject = {
  }
}

export class IssuePullResponse extends jspb.Message {
  getResponse(): string;
  setResponse(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): IssuePullResponse.AsObject;
  static toObject(includeInstance: boolean, msg: IssuePullResponse): IssuePullResponse.AsObject;
  static serializeBinaryToWriter(message: IssuePullResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): IssuePullResponse;
  static deserializeBinaryFromReader(message: IssuePullResponse, reader: jspb.BinaryReader): IssuePullResponse;
}

export namespace IssuePullResponse {
  export type AsObject = {
    response: string,
  }
}

export class ConfigurationRequest extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ConfigurationRequest.AsObject;
  static toObject(includeInstance: boolean, msg: ConfigurationRequest): ConfigurationRequest.AsObject;
  static serializeBinaryToWriter(message: ConfigurationRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ConfigurationRequest;
  static deserializeBinaryFromReader(message: ConfigurationRequest, reader: jspb.BinaryReader): ConfigurationRequest;
}

export namespace ConfigurationRequest {
  export type AsObject = {
  }
}

export class ServiceConfiguration extends jspb.Message {
  getReadOnly(): boolean;
  setReadOnly(value: boolean): void;

  getGitId(): string;
  setGitId(value: string): void;

  getGitBranch(): string;
  setGitBranch(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ServiceConfiguration.AsObject;
  static toObject(includeInstance: boolean, msg: ServiceConfiguration): ServiceConfiguration.AsObject;
  static serializeBinaryToWriter(message: ServiceConfiguration, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ServiceConfiguration;
  static deserializeBinaryFromReader(message: ServiceConfiguration, reader: jspb.BinaryReader): ServiceConfiguration;
}

export namespace ServiceConfiguration {
  export type AsObject = {
    readOnly: boolean,
    gitId: string,
    gitBranch: string,
  }
}

export class QueryOptions extends jspb.Message {
  getArtifactType(): ArtifactType;
  setArtifactType(value: ArtifactType): void;

  getMaxItemReturn(): number;
  setMaxItemReturn(value: number): void;

  getLastItemIndex(): number;
  setLastItemIndex(value: number): void;

  getByClassification(): boolean;
  setByClassification(value: boolean): void;

  getClassification(): Classification | undefined;
  setClassification(value?: Classification): void;
  hasClassification(): boolean;
  clearClassification(): void;

  getIncludeHybrids(): boolean;
  setIncludeHybrids(value: boolean): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): QueryOptions.AsObject;
  static toObject(includeInstance: boolean, msg: QueryOptions): QueryOptions.AsObject;
  static serializeBinaryToWriter(message: QueryOptions, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): QueryOptions;
  static deserializeBinaryFromReader(message: QueryOptions, reader: jspb.BinaryReader): QueryOptions;
}

export namespace QueryOptions {
  export type AsObject = {
    artifactType: ArtifactType,
    maxItemReturn: number,
    lastItemIndex: number,
    byClassification: boolean,
    classification?: Classification.AsObject,
    includeHybrids: boolean,
  }
}

export class QueryResult extends jspb.Message {
  getArtifactType(): ArtifactType;
  setArtifactType(value: ArtifactType): void;

  getFirstItemIndex(): number;
  setFirstItemIndex(value: number): void;

  getLastItemIndex(): number;
  setLastItemIndex(value: number): void;

  getTotalItemsInCollection(): number;
  setTotalItemsInCollection(value: number): void;

  getArtifactCollection(): google_protobuf_any_pb.Any | undefined;
  setArtifactCollection(value?: google_protobuf_any_pb.Any): void;
  hasArtifactCollection(): boolean;
  clearArtifactCollection(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): QueryResult.AsObject;
  static toObject(includeInstance: boolean, msg: QueryResult): QueryResult.AsObject;
  static serializeBinaryToWriter(message: QueryResult, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): QueryResult;
  static deserializeBinaryFromReader(message: QueryResult, reader: jspb.BinaryReader): QueryResult;
}

export namespace QueryResult {
  export type AsObject = {
    artifactType: ArtifactType,
    firstItemIndex: number,
    lastItemIndex: number,
    totalItemsInCollection: number,
    artifactCollection?: google_protobuf_any_pb.Any.AsObject,
  }
}

export class FormulaGrammar extends jspb.Message {
  getSingleTokenGrammar(): SingleTokenGrammar | undefined;
  setSingleTokenGrammar(value?: SingleTokenGrammar): void;
  hasSingleTokenGrammar(): boolean;
  clearSingleTokenGrammar(): void;

  getHybridGrammar(): HybridTokenGrammar | undefined;
  setHybridGrammar(value?: HybridTokenGrammar): void;
  hasHybridGrammar(): boolean;
  clearHybridGrammar(): void;

  getHybridWithHybridsGrammar(): HybridTokenWithHybridChildrenGrammar | undefined;
  setHybridWithHybridsGrammar(value?: HybridTokenWithHybridChildrenGrammar): void;
  hasHybridWithHybridsGrammar(): boolean;
  clearHybridWithHybridsGrammar(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): FormulaGrammar.AsObject;
  static toObject(includeInstance: boolean, msg: FormulaGrammar): FormulaGrammar.AsObject;
  static serializeBinaryToWriter(message: FormulaGrammar, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): FormulaGrammar;
  static deserializeBinaryFromReader(message: FormulaGrammar, reader: jspb.BinaryReader): FormulaGrammar;
}

export namespace FormulaGrammar {
  export type AsObject = {
    singleTokenGrammar?: SingleTokenGrammar.AsObject,
    hybridGrammar?: HybridTokenGrammar.AsObject,
    hybridWithHybridsGrammar?: HybridTokenWithHybridChildrenGrammar.AsObject,
  }
}

export class HybridTokenGrammar extends jspb.Message {
  getParent(): SingleTokenGrammar | undefined;
  setParent(value?: SingleTokenGrammar): void;
  hasParent(): boolean;
  clearParent(): void;

  getChildrenStart(): string;
  setChildrenStart(value: string): void;

  getChildTokensList(): Array<SingleTokenGrammar>;
  setChildTokensList(value: Array<SingleTokenGrammar>): void;
  clearChildTokensList(): void;
  addChildTokens(value?: SingleTokenGrammar, index?: number): SingleTokenGrammar;

  getChildrenEnd(): string;
  setChildrenEnd(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): HybridTokenGrammar.AsObject;
  static toObject(includeInstance: boolean, msg: HybridTokenGrammar): HybridTokenGrammar.AsObject;
  static serializeBinaryToWriter(message: HybridTokenGrammar, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): HybridTokenGrammar;
  static deserializeBinaryFromReader(message: HybridTokenGrammar, reader: jspb.BinaryReader): HybridTokenGrammar;
}

export namespace HybridTokenGrammar {
  export type AsObject = {
    parent?: SingleTokenGrammar.AsObject,
    childrenStart: string,
    childTokensList: Array<SingleTokenGrammar.AsObject>,
    childrenEnd: string,
  }
}

export class HybridTokenWithHybridChildrenGrammar extends jspb.Message {
  getParent(): SingleTokenGrammar | undefined;
  setParent(value?: SingleTokenGrammar): void;
  hasParent(): boolean;
  clearParent(): void;

  getHybridChildrenStart(): string;
  setHybridChildrenStart(value: string): void;

  getHybridChildTokensList(): Array<HybridTokenGrammar>;
  setHybridChildTokensList(value: Array<HybridTokenGrammar>): void;
  clearHybridChildTokensList(): void;
  addHybridChildTokens(value?: HybridTokenGrammar, index?: number): HybridTokenGrammar;

  getHybridChildrenEnd(): string;
  setHybridChildrenEnd(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): HybridTokenWithHybridChildrenGrammar.AsObject;
  static toObject(includeInstance: boolean, msg: HybridTokenWithHybridChildrenGrammar): HybridTokenWithHybridChildrenGrammar.AsObject;
  static serializeBinaryToWriter(message: HybridTokenWithHybridChildrenGrammar, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): HybridTokenWithHybridChildrenGrammar;
  static deserializeBinaryFromReader(message: HybridTokenWithHybridChildrenGrammar, reader: jspb.BinaryReader): HybridTokenWithHybridChildrenGrammar;
}

export namespace HybridTokenWithHybridChildrenGrammar {
  export type AsObject = {
    parent?: SingleTokenGrammar.AsObject,
    hybridChildrenStart: string,
    hybridChildTokensList: Array<HybridTokenGrammar.AsObject>,
    hybridChildrenEnd: string,
  }
}

export class SingleTokenGrammar extends jspb.Message {
  getGroupStart(): string;
  setGroupStart(value: string): void;

  getBaseTokenToolingSymbol(): string;
  setBaseTokenToolingSymbol(value: string): void;

  getBehaviors(): BehaviorList | undefined;
  setBehaviors(value?: BehaviorList): void;
  hasBehaviors(): boolean;
  clearBehaviors(): void;

  getPropertySets(): PropertySetList | undefined;
  setPropertySets(value?: PropertySetList): void;
  hasPropertySets(): boolean;
  clearPropertySets(): void;

  getGroupEnd(): string;
  setGroupEnd(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): SingleTokenGrammar.AsObject;
  static toObject(includeInstance: boolean, msg: SingleTokenGrammar): SingleTokenGrammar.AsObject;
  static serializeBinaryToWriter(message: SingleTokenGrammar, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): SingleTokenGrammar;
  static deserializeBinaryFromReader(message: SingleTokenGrammar, reader: jspb.BinaryReader): SingleTokenGrammar;
}

export namespace SingleTokenGrammar {
  export type AsObject = {
    groupStart: string,
    baseTokenToolingSymbol: string,
    behaviors?: BehaviorList.AsObject,
    propertySets?: PropertySetList.AsObject,
    groupEnd: string,
  }
}

export class BehaviorList extends jspb.Message {
  getListStart(): string;
  setListStart(value: string): void;

  getBehaviorToolingSymbolsList(): Array<string>;
  setBehaviorToolingSymbolsList(value: Array<string>): void;
  clearBehaviorToolingSymbolsList(): void;
  addBehaviorToolingSymbols(value: string, index?: number): void;

  getListEnd(): string;
  setListEnd(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): BehaviorList.AsObject;
  static toObject(includeInstance: boolean, msg: BehaviorList): BehaviorList.AsObject;
  static serializeBinaryToWriter(message: BehaviorList, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): BehaviorList;
  static deserializeBinaryFromReader(message: BehaviorList, reader: jspb.BinaryReader): BehaviorList;
}

export namespace BehaviorList {
  export type AsObject = {
    listStart: string,
    behaviorToolingSymbolsList: Array<string>,
    listEnd: string,
  }
}

export class PropertySetList extends jspb.Message {
  getListStart(): string;
  setListStart(value: string): void;

  getPropertySetsList(): Array<PropertySetListItem>;
  setPropertySetsList(value: Array<PropertySetListItem>): void;
  clearPropertySetsList(): void;
  addPropertySets(value?: PropertySetListItem, index?: number): PropertySetListItem;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): PropertySetList.AsObject;
  static toObject(includeInstance: boolean, msg: PropertySetList): PropertySetList.AsObject;
  static serializeBinaryToWriter(message: PropertySetList, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): PropertySetList;
  static deserializeBinaryFromReader(message: PropertySetList, reader: jspb.BinaryReader): PropertySetList;
}

export namespace PropertySetList {
  export type AsObject = {
    listStart: string,
    propertySetsList: Array<PropertySetListItem.AsObject>,
  }
}

export class PropertySetListItem extends jspb.Message {
  getPropertySetSymbol(): string;
  setPropertySetSymbol(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): PropertySetListItem.AsObject;
  static toObject(includeInstance: boolean, msg: PropertySetListItem): PropertySetListItem.AsObject;
  static serializeBinaryToWriter(message: PropertySetListItem, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): PropertySetListItem;
  static deserializeBinaryFromReader(message: PropertySetListItem, reader: jspb.BinaryReader): PropertySetListItem;
}

export namespace PropertySetListItem {
  export type AsObject = {
    propertySetSymbol: string,
  }
}

export enum TemplateType { 
  SINGLE_TOKEN = 0,
  HYBRID = 1,
}
export enum TokenType { 
  FUNGIBLE = 0,
  NON_FUNGIBLE = 1,
}
export enum RepresentationType { 
  COMMON = 0,
  UNIQUE = 1,
}
export enum ValueType { 
  INTRINSIC = 0,
  REFERENCE = 1,
}
export enum TokenUnit { 
  FRACTIONAL = 0,
  WHOLE = 1,
  SINGLETON = 2,
}
export enum Supply { 
  FIXED = 0,
  CAPPED_VARIABLE = 1,
  GATED = 2,
  INFINITE = 3,
}
export enum ArtifactType { 
  BASE = 0,
  BEHAVIOR = 1,
  BEHAVIOR_GROUP = 2,
  PROPERTY_SET = 3,
  TEMPLATE_FORMULA = 4,
  TEMPLATE_DEFINITION = 5,
  TOKEN_TEMPLATE = 6,
}
export enum ArtifactContent { 
  DEFINITION = 0,
  CONTROL = 1,
  UML = 2,
  OTHER = 3,
}
export enum MappingType { 
  SOURCE_CODE = 0,
  IMPLEMENTATION = 1,
  RESOURCE = 2,
}
export enum TargetPlatform { 
  ETHEREUM_SOLIDITY = 0,
  CHAINCODE_GO = 1,
  CHAINCODE_JAVA = 2,
  CHAINCODE_NODE = 3,
  CORDA = 4,
  DAML = 5,
  OTHER_PLATFORM = 6,
}
