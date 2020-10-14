import * as grpcWeb from 'grpc-web';
import {ServiceClient} from './model/ServiceServiceClientPb';
import {Taxonomy} from "./model/taxonomy_pb";
import {ArtifactType, QueryOptions, QueryResult} from "./model/artifact_pb";
import {
  Base,
  Bases,
  Behavior,
  BehaviorGroup,
  BehaviorGroups,
  Behaviors,
  PropertySet,
  PropertySets, TemplateDefinition, TemplateDefinitions
} from "./model/core_pb";
import {Any} from "google-protobuf/google/protobuf/any_pb";
import {TaxonomyVersion} from "./model/taxonomy_pb";

let backendHost = document.cookie.replace(/(?:(?:^|.*;\s*)BACKEND_HOST\s*\=\s*([^;]*).*$)|^.*$/, "$1");

if (backendHost === undefined || backendHost === "") {
  console.log("Backend default value used");
  backendHost = "http://0.0.0.0:9080";
} else {
  console.log(`Backend set to ${backendHost}`);
}

export const client = new ServiceClient(backendHost, null, null);

export const getFullTaxonomy = async (): Promise<Taxonomy> => {
  return new Promise<Taxonomy>((resolve, reject) => {
    const query = new TaxonomyVersion();
    query.setVersion("1.0");
    client.getFullTaxonomy(query, null, (err: grpcWeb.Error, response: Taxonomy) => {
      if (err !== null) {
        reject(err);
      } else {
        resolve(response);
      }
    });
  });
};

export const getAllBases = async (): Promise<Base[]> => {
  return new Promise<Base[]>((resolve, reject) => {
    const query = new QueryOptions();
    query.setArtifactType(ArtifactType.BASE);
    client.getArtifactsOfType(query, null, (err: grpcWeb.Error, response: QueryResult) => {
      const artifactCollection: Any | undefined = response.getArtifactCollection();
      if (artifactCollection != null) {

        const bases: Bases | null = artifactCollection.unpack<Bases>(Bases.deserializeBinary, 'taxonomy.model.core.Bases');
        if (bases != null) {
          if (err !== null) {
            reject(err);
          } else {
            resolve(bases.getBaseList());
          }
        }
      }
    });
  });
};

export const getAllBehaviors = async (): Promise<Behavior[]> => {
  return new Promise<Behavior[]>((resolve, reject) => {
    const query = new QueryOptions();
    query.setArtifactType(ArtifactType.BEHAVIOR);
    query.setMaxItemReturn(10000);
    client.getArtifactsOfType(query, null, (err: grpcWeb.Error, response: QueryResult) => {
      const artifactCollection: Any | undefined = response.getArtifactCollection();
      if (artifactCollection != null) {
        const behaviors: Behaviors | null = artifactCollection.unpack<Behaviors>(Behaviors.deserializeBinary, 'taxonomy.model.core.Behaviors');
        if (behaviors != null) {
          if (err !== null) {
            reject(err);
          } else {
            resolve(behaviors.getBehaviorList());
          }
        }
      }
    });
  });
};

export const getAllBehaviorGroups = async (): Promise<BehaviorGroup[]> => {
  return new Promise<BehaviorGroup[]>((resolve, reject) => {
    const query = new QueryOptions();
    query.setArtifactType(ArtifactType.BEHAVIOR_GROUP);
    query.setMaxItemReturn(10000);
    client.getArtifactsOfType(query, null, (err: grpcWeb.Error, response: QueryResult) => {
      const artifactCollection: Any | undefined = response.getArtifactCollection();
      if (artifactCollection != null) {
        const behaviorGroups: BehaviorGroups | null = artifactCollection.unpack<BehaviorGroups>(BehaviorGroups.deserializeBinary, 'taxonomy.model.core.BehaviorGroups');
        if (behaviorGroups != null) {
          if (err !== null) {
            reject(err);
          } else {
            resolve(behaviorGroups.getBehaviorGroupList());
          }
        }
      }
    });
  });
};

export const getAllPropertySets = async (): Promise<PropertySet[]> => {
  return new Promise<PropertySet[]>((resolve, reject) => {
    const query = new QueryOptions();
    query.setArtifactType(ArtifactType.PROPERTY_SET);
    query.setMaxItemReturn(10000);
    client.getArtifactsOfType(query, null, (err: grpcWeb.Error, response: QueryResult) => {
      const artifactCollection: Any | undefined = response.getArtifactCollection();
      if (artifactCollection != null) {
        const propertySets: PropertySets | null = artifactCollection.unpack<PropertySets>(PropertySets.deserializeBinary, 'taxonomy.model.core.PropertySets');
        if (propertySets != null) {
          if (err !== null) {
            reject(err);
          } else {
            resolve(propertySets.getPropertySetList());
          }
        }
      }
    });
  });
};

export const getAllTemplateDefinitions = async (): Promise<TemplateDefinition[]> => {
  return new Promise<TemplateDefinition[]>((resolve, reject) => {
    const query = new QueryOptions();
    query.setArtifactType(ArtifactType.TEMPLATE_DEFINITION);
    query.setMaxItemReturn(10000);
    client.getArtifactsOfType(query, null, (err: grpcWeb.Error, response: QueryResult) => {
      const artifactCollection: Any | undefined = response.getArtifactCollection();
      if (artifactCollection != null) {
        const propertySets: TemplateDefinitions | null = artifactCollection.unpack<TemplateDefinitions>(TemplateDefinitions.deserializeBinary, 'taxonomy.model.core.TemplateDefinitions');
        if (propertySets != null) {
          if (err !== null) {
            reject(err);
          } else {
            resolve(propertySets.getDefinitionsList());
          }
        }
      }
    });
  });
};
