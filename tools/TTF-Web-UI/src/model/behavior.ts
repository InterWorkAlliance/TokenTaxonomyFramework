/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
/* eslint-disable */
import {Maybe} from "../custom-types";

namespace BehaviorJSON {
  
  export interface jsonModel {
    behaviorName: string,
    symbol: string,
    otherAliases: string, //???
    description: string,
    analogies: Maybe<Analogy[]>,
    dependencies: string, //???
    incompatibilities: string, //???
    influencedBy: string,
    invocations: Maybe<Invocation[]>,
    properties: PropertyMain
  }

  export interface Analogy {
    analogy: string,
    description: string
  }

  export interface Invocation {
    invocation: string,
    description: string,
    requestInfo: Maybe<Request>
    responseInfo: Maybe<Response>
  }

  export interface Request {
    request: string,
    parameters: Maybe<Parameter[]>
  }

  export interface Response {
    response: string,
    parameters: Maybe<Parameter[]>
  }
  
  export interface Parameter {
    name: string,
    description: string
  }
  
  export interface Property {
    property: string,
    description: string
  }
  
  export  interface PropertySecond extends Property{
    properties: Maybe<Property[]>
  }

  export interface PropertyMain extends Property{
    properties: Maybe<PropertySecond[]>
  }

  export interface ModelState {
    jsonModel: Maybe<jsonModel>
  }
}

export default BehaviorJSON;
