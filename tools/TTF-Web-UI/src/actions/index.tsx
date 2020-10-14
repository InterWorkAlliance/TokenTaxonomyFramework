import {Action, Dispatch} from 'redux';
import {Base, Behavior, BehaviorGroup, PropertySet, TemplateDefinition} from "../model/core_pb";
import * as jspb from "google-protobuf";
import {
  getFullTaxonomy
} from '../state';
import {IEntity} from "../store/IStoreState";
import {Taxonomy} from "../model/taxonomy_pb";
import {Message} from "google-protobuf";


export const FETCH_SERVER_STATE = 'FETCH_SERVER_STATE';
export const FETCH_SERVER_STATE_SUCCESS = 'FETCH_SERVER_STATE_SUCCESS';
export const FETCH_SERVER_STATE_ERROR = 'FETCH_SERVER_STATE_ERROR';
export const SELECT_ENTITY = "SELECT_ENTITY";

export interface ISelectEntity extends Action {
  type: 'SELECT_ENTITY';
  payload: IEntity;
}

export interface IFetchServerState extends Action {
  type: 'FETCH_SERVER_STATE';
}

export interface IFetchServerStateSuccess extends Action {
  type: 'FETCH_SERVER_STATE_SUCCESS';
  bases: Base[];
  behaviors: Behavior[];
  behaviorGroups: BehaviorGroup[];
  propertySets: PropertySet[];
  templateDefinitions: TemplateDefinition[];
}

export interface IFetchServerStateError extends Action {
  type: 'FETCH_SERVER_STATE_ERROR';
  errorMsg: string;
}

export type AppActions = ISelectEntity | IFetchServerState | IFetchServerStateSuccess | IFetchServerStateError;

// Dispatchers
function dispatchSelectEntity(selected: IEntity): ISelectEntity {
  return {
    type: SELECT_ENTITY,
    payload: selected
  };
}
function dispatchFetchServerStateProgress(): IFetchServerState {
  return {
    type: FETCH_SERVER_STATE
  };
}

function dispatchFetchServerStateSuccess(bases: Base[], behaviors: Behavior[], behaviorGroups: BehaviorGroup[], propertySets: PropertySet[], templateDefinitions: TemplateDefinition[]): IFetchServerStateSuccess {
  return {
    type: FETCH_SERVER_STATE_SUCCESS,
    bases: bases,
    behaviors: behaviors,
    behaviorGroups: behaviorGroups,
    propertySets: propertySets,
    templateDefinitions: templateDefinitions
  };
}

function dispatchFetchServerStateError(e: Error): IFetchServerStateError {
  return {
    type: FETCH_SERVER_STATE_ERROR,
    errorMsg: e.message
  };
}

// Action creator 
export function actionSelectEntity(selected: IEntity) {
  return (dispatch: Dispatch) => {
    return dispatch(dispatchSelectEntity(selected));
  }
}

export function actionFetchServerState() {
  return (dispatch: Dispatch) => {
    dispatch(dispatchFetchServerStateProgress());

    return Promise.all([getFullTaxonomy()])
      .then((artifacts) => {
        const taxonomy: Taxonomy = artifacts[0];
        const bases: Base[] = mapToArray<Base>(taxonomy.getBaseTokenTypesMap());
        const behaviors: Behavior[] = mapToArray<Behavior>(taxonomy.getBehaviorsMap());
        const behaviorGroups: BehaviorGroup[] = mapToArray<BehaviorGroup>(taxonomy.getBehaviorGroupsMap());
        const propertySets: PropertySet[] = mapToArray<PropertySet>(taxonomy.getPropertySetsMap());
        const templateDefinitions: TemplateDefinition[] = mapToArray<TemplateDefinition>(taxonomy.getTemplateDefinitionsMap());

        return dispatch(dispatchFetchServerStateSuccess(bases, behaviors, behaviorGroups, propertySets, templateDefinitions));
      })
      .catch((e: Error) => {
        return dispatch(dispatchFetchServerStateError(e));
      });
  };
}

function mapToArray<T extends Message>(map: jspb.Map<string, T>): Array<T> {
  const it = map.entries();
  let entry = it.next();
  const result = Array<T>();
  do {
    const value: T = entry.value[1];
    result.push(value);
    entry = it.next();
  } while(!entry.done);
  return result;
}
