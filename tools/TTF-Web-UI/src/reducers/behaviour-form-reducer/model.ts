import {Maybe} from "../../custom-types";

namespace IBehaviourForm {

  export interface Step1 {
    id: number,
    behaviorName: string,
    symbol: string,
    otherAliases: any, //?????
    description: string
  }

  export interface Analogy {
    id: string,
    analogy: string;
    description: string
  }

  export interface Step2 {
    id: number;
    analogies: Analogy[],
    dependencies: string,
    incompatibilities: string,
    influencedBy: string,
  }

  export interface Invocation {
    id: string,
    invocation: string,
    description: string,
    requestInfo: Request
    responseInfo: Response
  }

  export interface Request {
    request: string,
    parameters: Parameter[]
  }

  export interface Response {
    response: string,
    parameters: Parameter[]
  }

  export interface Parameter {
    id: string,
    name: string,
    description: string
  }

  export interface Step3 {
    id: number,
    invocations: Invocation[]
  }

  export interface Property {
    id: string,
    property: string,
    description: string
  }

  export  interface PropertySecond{
    id: string,
    property: string,
    description: string
    properties: Property[]
  }

  export  interface PropertyMain{
    id: string,
    property: string,
    description: string
    properties: PropertySecond[]
  }

  export interface Step4 {
    id: number
    properties: PropertyMain[]
  }

  export interface ModelState {
    steps: number[];
    activeStep: number;
    step1: Step1,
    step2: Step2,
    step3: Step3,
    step4: Step4
  }
}

export default IBehaviourForm;