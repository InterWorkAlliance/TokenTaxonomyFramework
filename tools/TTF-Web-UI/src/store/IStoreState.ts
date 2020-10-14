import {Base, Behavior, BehaviorGroup, PropertySet, TemplateDefinition} from "../model/core_pb";
import {ISelectEntity} from "../actions";
import {ArtifactDict} from "../reducers/baseList";

export interface IEntity {
  name: string;
  symbol: string;
  owner: string;
  quantity: number;
  decimals: number;
}

export interface IStoreState {
  readonly ui: IUI;
  readonly entities: IEntities;
  selectedEntity: IEntity | undefined;
}
export interface IEntities {
  bases: ArtifactDict;
  behaviors: ArtifactDict;
  behaviorGroups: ArtifactDict;
  propertySets: ArtifactDict;
  templateDefinitions: ArtifactDict;
}
export interface IUI {
  readonly sidebarUI: ISidebarUI;
}

export const SELECT_ITEM = 'SELECT_ITEM';

interface SelectItemAction {
  type: typeof SELECT_ITEM;
  payload: string;
}
  
export interface ISidebarUI {
  readonly bases: Base[];
  readonly behaviors: Behavior[];
  readonly behaviorGroups: BehaviorGroup[];
  readonly propertySets: PropertySet[];
  readonly templateDefinitions: TemplateDefinition[];
  state: string;
  errorMsg: string;
  selectEntity: (entity: IEntity) => ISelectEntity;
}
