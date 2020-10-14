import {
  CREATE_BEHAVIOUR_FORM_ADD_ANALOGY,
  CREATE_BEHAVIOUR_FORM_ADD_INVOCATION,
  CREATE_BEHAVIOUR_FORM_ADD_PARAMETER_TO_REQUEST,
  CREATE_BEHAVIOUR_FORM_ADD_PARAMETER_TO_RESPONSE, CREATE_BEHAVIOUR_FORM_ADD_PROPERTY,
  CREATE_BEHAVIOUR_FORM_ADD_PROPERTY_MAIN,
  CREATE_BEHAVIOUR_FORM_ADD_PROPERTY_SECOND,
  CREATE_BEHAVIOUR_FORM_NEXT_STEP,
  CREATE_BEHAVIOUR_FORM_PREV_STEP, CREATE_BEHAVIOUR_SAVE_STEP_1,
  CREATE_BEHAVIOUR_SAVE_STEP_2, CREATE_BEHAVIOUR_SAVE_STEP_3, CREATE_BEHAVIOUR_SAVE_STEP_4
} from "./types";
import ITemporaryStorageForHandleFields from "../utils/temporary-storage-for-handle-fields/model";

export const createBehaviourFormNextStep = () => {
  return {
    type: CREATE_BEHAVIOUR_FORM_NEXT_STEP,
  }
};

export const createBehaviourFormPrevStep = () => {
  return {
    type: CREATE_BEHAVIOUR_FORM_PREV_STEP,
  }
};

export const createBehaviourFormAddAnalogy = () => {
  return {
    type: CREATE_BEHAVIOUR_FORM_ADD_ANALOGY,
  }
};

export const createBehaviourFormAddInvocation = () => {
  return {
    type: CREATE_BEHAVIOUR_FORM_ADD_INVOCATION
  }
};

export const createBehaviourFormAddParameterToResponse = (invocationId: string) => {
  return {
    type: CREATE_BEHAVIOUR_FORM_ADD_PARAMETER_TO_RESPONSE,
    payload: invocationId
  }
};

export const createBehaviourFormAddParameterToRequest = (invocationId: string) => {
  return {
    type: CREATE_BEHAVIOUR_FORM_ADD_PARAMETER_TO_REQUEST,
    payload: invocationId
  }
};

export const createBehaviourFormAddPropertyMain = () => {
  return {
    type: CREATE_BEHAVIOUR_FORM_ADD_PROPERTY_MAIN,
  }
};

export const createBehaviourFormAddPropertySecond = (propertyMainId: string) => {
  return {
    type: CREATE_BEHAVIOUR_FORM_ADD_PROPERTY_SECOND,
    payload: propertyMainId
  }
};

export const createBehaviourFormAddProperty = (propertyMainId: string, propertySecondId: string) => {
  return {
    type: CREATE_BEHAVIOUR_FORM_ADD_PROPERTY,
    payload: { propertyMainId, propertySecondId }
  }
};

export const createBehaviourFormSaveStep1 = (payload: ITemporaryStorageForHandleFields.Fields) => {
  return {
    type: CREATE_BEHAVIOUR_SAVE_STEP_1,
    payload
  }
};

export const createBehaviourFormSaveStep2 = (payload: ITemporaryStorageForHandleFields.Fields) => {
  return {
    type: CREATE_BEHAVIOUR_SAVE_STEP_2,
    payload
  }
};

export const createBehaviourFormSaveStep3 = (payload: ITemporaryStorageForHandleFields.Fields) => {
  return {
    type: CREATE_BEHAVIOUR_SAVE_STEP_3,
    payload
  }
};

export const createBehaviourFormSaveStep4 = (payload: ITemporaryStorageForHandleFields.Fields) => {
  return {
    type: CREATE_BEHAVIOUR_SAVE_STEP_4,
    payload
  }
};