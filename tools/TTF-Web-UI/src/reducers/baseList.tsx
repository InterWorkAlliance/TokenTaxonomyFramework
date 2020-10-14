import {
  AppActions
} from '../actions';
import {Base, Behavior, BehaviorGroup, PropertySet, TemplateDefinition} from "../model/core_pb";
import {Artifact} from "../model/artifact_pb";

export interface BaseListState {
  state: string; // 'INIT', 'LOADING' | 'LOADED' | 'ERROR',
  bases: Base[];
  behaviors: Behavior[];
  behaviorGroups: BehaviorGroup[];
  propertySets: PropertySet[];
  templateDefinitions: TemplateDefinition[];
  errorMsg?: string;
}

export function defaultBaseListState() {
  return {
    state: 'INIT',
    bases: [],
    behaviors: [],
    behaviorGroups: [],
    propertySets: [],
    templateDefinitions: []
  };
}

export function baseListReducer(state: BaseListState, action: AppActions): BaseListState {
  // TODO: reducer
  switch (action.type) {
    case 'FETCH_SERVER_STATE':
      return {
        ...state,
        state: 'LOADING',
        bases: [],
      };
    case 'FETCH_SERVER_STATE_SUCCESS':
      return {
        ...state,
        state: 'LOADED',
        bases: action.bases,
        behaviors: action.behaviors,
        behaviorGroups: action.behaviorGroups,
        propertySets: action.propertySets,
        templateDefinitions: action.templateDefinitions
      };
    case 'FETCH_SERVER_STATE_ERROR':
      return {
        ...state,
        state: 'ERROR',
        bases: [],
        errorMsg: action.errorMsg
      };
    default:
      return state;
  }
}

// Define normalized state 

export type ArtifactDict = {
  byId: Map<string,Artifact>;
  allIds: number[];
  errorMsg?: string;
};

export function defaultArtifactDict() {
  return {
    byId: new Map<string,Artifact>(),
    allIds: [],
  };
}

export function basesDictReducer(state: ArtifactDict, action: AppActions, field: string): ArtifactDict {
  switch (action.type) {
    case 'FETCH_SERVER_STATE_SUCCESS':
      const iAmAMap = new Map<string, Artifact>(
        // @ts-ignore
        action[field].map(obj => [obj.toObject().artifact.artifactSymbol.id, obj.getArtifact()] as [string, Artifact])
      );
      // @ts-ignore
      const allIds = action[field].map((base, baseId) => baseId);
      return {
        ...state,
        byId: iAmAMap,
        allIds: allIds
      };
    case 'FETCH_SERVER_STATE_ERROR':
      return {
        ...state,
        errorMsg: action.errorMsg
      };
    default:
      return state;
  }    
}
