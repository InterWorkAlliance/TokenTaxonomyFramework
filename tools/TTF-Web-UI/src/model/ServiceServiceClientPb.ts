/* eslint-disable */
/**
 * @fileoverview gRPC-Web generated client stub for taxonomy
 * @enhanceable
 * @public
 */

// GENERATED CODE -- DO NOT EDIT!


import * as grpcWeb from 'grpc-web';

import * as taxonomy_pb from './taxonomy_pb';
import * as core_pb from './core_pb';
import * as artifact_pb from './artifact_pb';

export class ServiceClient {
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

  methodInfoGetFullTaxonomy = new grpcWeb.AbstractClientBase.MethodInfo(
    taxonomy_pb.Taxonomy,
    (request: taxonomy_pb.TaxonomyVersion) => {
      return request.serializeBinary();
    },
    taxonomy_pb.Taxonomy.deserializeBinary
  );

  getFullTaxonomy(
    request: taxonomy_pb.TaxonomyVersion,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: taxonomy_pb.Taxonomy) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetFullTaxonomy',
      request,
      metadata || {},
      this.methodInfoGetFullTaxonomy,
      callback);
  }

  methodInfoGetLiteTaxonomy = new grpcWeb.AbstractClientBase.MethodInfo(
    taxonomy_pb.Taxonomy,
    (request: taxonomy_pb.TaxonomyVersion) => {
      return request.serializeBinary();
    },
    taxonomy_pb.Taxonomy.deserializeBinary
  );

  getLiteTaxonomy(
    request: taxonomy_pb.TaxonomyVersion,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: taxonomy_pb.Taxonomy) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetLiteTaxonomy',
      request,
      metadata || {},
      this.methodInfoGetLiteTaxonomy,
      callback);
  }

  methodInfoGetBaseArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.Base,
    (request: artifact_pb.ArtifactSymbol) => {
      return request.serializeBinary();
    },
    core_pb.Base.deserializeBinary
  );

  getBaseArtifact(
    request: artifact_pb.ArtifactSymbol,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.Base) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetBaseArtifact',
      request,
      metadata || {},
      this.methodInfoGetBaseArtifact,
      callback);
  }

  methodInfoGetBehaviorArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.Behavior,
    (request: artifact_pb.ArtifactSymbol) => {
      return request.serializeBinary();
    },
    core_pb.Behavior.deserializeBinary
  );

  getBehaviorArtifact(
    request: artifact_pb.ArtifactSymbol,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.Behavior) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetBehaviorArtifact',
      request,
      metadata || {},
      this.methodInfoGetBehaviorArtifact,
      callback);
  }

  methodInfoGetBehaviorGroupArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.BehaviorGroup,
    (request: artifact_pb.ArtifactSymbol) => {
      return request.serializeBinary();
    },
    core_pb.BehaviorGroup.deserializeBinary
  );

  getBehaviorGroupArtifact(
    request: artifact_pb.ArtifactSymbol,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.BehaviorGroup) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetBehaviorGroupArtifact',
      request,
      metadata || {},
      this.methodInfoGetBehaviorGroupArtifact,
      callback);
  }

  methodInfoGetPropertySetArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.PropertySet,
    (request: artifact_pb.ArtifactSymbol) => {
      return request.serializeBinary();
    },
    core_pb.PropertySet.deserializeBinary
  );

  getPropertySetArtifact(
    request: artifact_pb.ArtifactSymbol,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.PropertySet) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetPropertySetArtifact',
      request,
      metadata || {},
      this.methodInfoGetPropertySetArtifact,
      callback);
  }

  methodInfoGetTemplateFormulaArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.TemplateFormula,
    (request: artifact_pb.ArtifactSymbol) => {
      return request.serializeBinary();
    },
    core_pb.TemplateFormula.deserializeBinary
  );

  getTemplateFormulaArtifact(
    request: artifact_pb.ArtifactSymbol,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.TemplateFormula) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetTemplateFormulaArtifact',
      request,
      metadata || {},
      this.methodInfoGetTemplateFormulaArtifact,
      callback);
  }

  methodInfoGetTemplateDefinitionArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.TemplateDefinition,
    (request: artifact_pb.ArtifactSymbol) => {
      return request.serializeBinary();
    },
    core_pb.TemplateDefinition.deserializeBinary
  );

  getTemplateDefinitionArtifact(
    request: artifact_pb.ArtifactSymbol,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.TemplateDefinition) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetTemplateDefinitionArtifact',
      request,
      metadata || {},
      this.methodInfoGetTemplateDefinitionArtifact,
      callback);
  }

  methodInfoGetTokenTemplate = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.TokenTemplate,
    (request: artifact_pb.TokenTemplateId) => {
      return request.serializeBinary();
    },
    core_pb.TokenTemplate.deserializeBinary
  );

  getTokenTemplate(
    request: artifact_pb.TokenTemplateId,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.TokenTemplate) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetTokenTemplate',
      request,
      metadata || {},
      this.methodInfoGetTokenTemplate,
      callback);
  }

  methodInfoGetTokenSpecification = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.TokenSpecification,
    (request: artifact_pb.TokenTemplateId) => {
      return request.serializeBinary();
    },
    core_pb.TokenSpecification.deserializeBinary
  );

  getTokenSpecification(
    request: artifact_pb.TokenTemplateId,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.TokenSpecification) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetTokenSpecification',
      request,
      metadata || {},
      this.methodInfoGetTokenSpecification,
      callback);
  }

  methodInfoGetArtifactsOfType = new grpcWeb.AbstractClientBase.MethodInfo(
    artifact_pb.QueryResult,
    (request: artifact_pb.QueryOptions) => {
      return request.serializeBinary();
    },
    artifact_pb.QueryResult.deserializeBinary
  );

  getArtifactsOfType(
    request: artifact_pb.QueryOptions,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: artifact_pb.QueryResult) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetArtifactsOfType',
      request,
      metadata || {},
      this.methodInfoGetArtifactsOfType,
      callback);
  }

  methodInfoInitializeNewArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    artifact_pb.InitializeNewArtifactResponse,
    (request: artifact_pb.InitializeNewArtifactRequest) => {
      return request.serializeBinary();
    },
    artifact_pb.InitializeNewArtifactResponse.deserializeBinary
  );

  initializeNewArtifact(
    request: artifact_pb.InitializeNewArtifactRequest,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: artifact_pb.InitializeNewArtifactResponse) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/InitializeNewArtifact',
      request,
      metadata || {},
      this.methodInfoInitializeNewArtifact,
      callback);
  }

  methodInfoCreateArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    artifact_pb.NewArtifactResponse,
    (request: artifact_pb.NewArtifactRequest) => {
      return request.serializeBinary();
    },
    artifact_pb.NewArtifactResponse.deserializeBinary
  );

  createArtifact(
    request: artifact_pb.NewArtifactRequest,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: artifact_pb.NewArtifactResponse) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/CreateArtifact',
      request,
      metadata || {},
      this.methodInfoCreateArtifact,
      callback);
  }

  methodInfoUpdateArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    artifact_pb.UpdateArtifactResponse,
    (request: artifact_pb.UpdateArtifactRequest) => {
      return request.serializeBinary();
    },
    artifact_pb.UpdateArtifactResponse.deserializeBinary
  );

  updateArtifact(
    request: artifact_pb.UpdateArtifactRequest,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: artifact_pb.UpdateArtifactResponse) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/UpdateArtifact',
      request,
      metadata || {},
      this.methodInfoUpdateArtifact,
      callback);
  }

  methodInfoDeleteArtifact = new grpcWeb.AbstractClientBase.MethodInfo(
    artifact_pb.DeleteArtifactResponse,
    (request: artifact_pb.DeleteArtifactRequest) => {
      return request.serializeBinary();
    },
    artifact_pb.DeleteArtifactResponse.deserializeBinary
  );

  deleteArtifact(
    request: artifact_pb.DeleteArtifactRequest,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: artifact_pb.DeleteArtifactResponse) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/DeleteArtifact',
      request,
      metadata || {},
      this.methodInfoDeleteArtifact,
      callback);
  }

  methodInfoCreateTemplateDefinition = new grpcWeb.AbstractClientBase.MethodInfo(
    core_pb.TemplateDefinition,
    (request: artifact_pb.NewTemplateDefinition) => {
      return request.serializeBinary();
    },
    core_pb.TemplateDefinition.deserializeBinary
  );

  createTemplateDefinition(
    request: artifact_pb.NewTemplateDefinition,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: core_pb.TemplateDefinition) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/CreateTemplateDefinition',
      request,
      metadata || {},
      this.methodInfoCreateTemplateDefinition,
      callback);
  }

  methodInfoCommitLocalUpdates = new grpcWeb.AbstractClientBase.MethodInfo(
    artifact_pb.CommitUpdatesResponse,
    (request: artifact_pb.CommitUpdatesRequest) => {
      return request.serializeBinary();
    },
    artifact_pb.CommitUpdatesResponse.deserializeBinary
  );

  commitLocalUpdates(
    request: artifact_pb.CommitUpdatesRequest,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: artifact_pb.CommitUpdatesResponse) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/CommitLocalUpdates',
      request,
      metadata || {},
      this.methodInfoCommitLocalUpdates,
      callback);
  }

  methodInfoPullRequest = new grpcWeb.AbstractClientBase.MethodInfo(
    artifact_pb.IssuePullResponse,
    (request: artifact_pb.IssuePullRequest) => {
      return request.serializeBinary();
    },
    artifact_pb.IssuePullResponse.deserializeBinary
  );

  pullRequest(
    request: artifact_pb.IssuePullRequest,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: artifact_pb.IssuePullResponse) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/PullRequest',
      request,
      metadata || {},
      this.methodInfoPullRequest,
      callback);
  }

  methodInfoGetConfig = new grpcWeb.AbstractClientBase.MethodInfo(
    artifact_pb.ServiceConfiguration,
    (request: artifact_pb.ConfigurationRequest) => {
      return request.serializeBinary();
    },
    artifact_pb.ServiceConfiguration.deserializeBinary
  );

  getConfig(
    request: artifact_pb.ConfigurationRequest,
    metadata: grpcWeb.Metadata | null,
    callback: (err: grpcWeb.Error,
               response: artifact_pb.ServiceConfiguration) => void) {
    return this.client_.rpcCall(
      this.hostname_ +
        '/taxonomy.Service/GetConfig',
      request,
      metadata || {},
      this.methodInfoGetConfig,
      callback);
  }

}

