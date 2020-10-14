import IBehaviourForm from "./model";
import {
  CREATE_BEHAVIOUR_FORM_ADD_ANALOGY,
  CREATE_BEHAVIOUR_FORM_ADD_INVOCATION,
  CREATE_BEHAVIOUR_FORM_ADD_PARAMETER_TO_REQUEST,
  CREATE_BEHAVIOUR_FORM_ADD_PARAMETER_TO_RESPONSE,
  CREATE_BEHAVIOUR_FORM_ADD_PROPERTY,
  CREATE_BEHAVIOUR_FORM_ADD_PROPERTY_MAIN,
  CREATE_BEHAVIOUR_FORM_ADD_PROPERTY_SECOND,
  CREATE_BEHAVIOUR_FORM_NEXT_STEP,
  CREATE_BEHAVIOUR_FORM_PREV_STEP,
  CREATE_BEHAVIOUR_SAVE_STEP_1,
  CREATE_BEHAVIOUR_SAVE_STEP_2,
  CREATE_BEHAVIOUR_SAVE_STEP_3,
  CREATE_BEHAVIOUR_SAVE_STEP_4
} from "../../actions/types";
import ITemporaryStorageForHandleFields from "../../utils/temporary-storage-for-handle-fields/model";
import {shapeFieldValue} from "../../helpers/shape-field-value";

export const defaultBehaviourFormState = (): IBehaviourForm.ModelState => {
  return {
    steps: [1, 2, 3, 4],
    activeStep: 1,
    step1: {
      id: 1,
      behaviorName: '',
      symbol: '',
      otherAliases: null,
      description: ''
    },
    step2: {
      id: 2,
      analogies: [
        {
          id: 'analogy_1',
          analogy: '',
          description: ''
        }
      ],
      dependencies: '',
      incompatibilities: '',
      influencedBy: '',
    },
    step3: {
      id: 3,
      invocations: [ shapeDefaultInvocation('invocation_1') ]
    },
    step4: {
      id: 4,
      properties: [ shapeDefaultMainProperty('property_main_1') ]
    }
  }
};

export const behaviourFormReducer = (state: IBehaviourForm.ModelState , action: any) => {
  switch (action.type) {
    case CREATE_BEHAVIOUR_SAVE_STEP_4: {
      const fields: ITemporaryStorageForHandleFields.Fields = action.payload;

      const {step4} = state;
      const propertiesMain = step4.properties;

      const newPropertiesMain = propertiesMain.map((propertyMain: IBehaviourForm.PropertyMain) => {
        const newPropertyMainValue = shapeFieldValue(fields[`property_${propertyMain.id}`].value, propertyMain.property);
        const newDescriptionMainValue = shapeFieldValue(fields[`description_${propertyMain.id}`].value, propertyMain.description);

        const newPropertiesSecond  = propertyMain.properties.map((propertySecond: IBehaviourForm.PropertySecond) => {
          const newPropertySecondValue = shapeFieldValue(fields[`sub_property_${propertySecond.id}`].value, propertySecond.property);
          const newDescriptionSecondValue = shapeFieldValue(fields[`sub_description_${propertySecond.id}`].value, propertySecond.description);

          const newProperties = propertySecond.properties.map((property: IBehaviourForm.Property) => {
            const newPropertyValue = shapeFieldValue(fields[`sub_sub_property_${property.id}`].value, property.property);
            const newDescriptionValue = shapeFieldValue(fields[`sub_sub_description_${property.id}`].value, property.description);

            return {
              ...property,
              property: newPropertyValue,
              description: newDescriptionValue
            }
          });

          return {
            ...propertySecond,
            property: newPropertySecondValue,
            description: newDescriptionSecondValue,
            properties: newProperties
          }
        });

        return {
          ...propertyMain,
          property: newPropertyMainValue,
          description: newDescriptionMainValue,
          properties: newPropertiesSecond
        }
      });

      return {
        ...state,
        step4: {
          ...step4,
          properties: newPropertiesMain
        }
      }
    }

    case CREATE_BEHAVIOUR_SAVE_STEP_3: {
      const fields: ITemporaryStorageForHandleFields.Fields = action.payload;

      const {step3} = state;
      const {invocations} = step3;

      const newInvocations = invocations.map((invocation: IBehaviourForm.Invocation) => {
        const newInvocationValue = shapeFieldValue(fields[`invocation_${invocation.id}`].value, invocation.invocation);
        const newDescriptionValue = shapeFieldValue(fields[`description_${invocation.id}`].value, invocation.description);

        const newRequestInfoValue = shapeFieldValue(fields[`request_${invocation.id}`].value, invocation.requestInfo.request);
        const newResponseInfoValue = shapeFieldValue(fields[`response_${invocation.id}`].value, invocation.responseInfo.response);

        const newRequestInfoParameters = invocation.requestInfo.parameters.map((parameter: IBehaviourForm.Parameter) => {
          const newNameValue = shapeFieldValue(fields[`name_${parameter.id}`].value, parameter.name);
          const newDescriptionValue = shapeFieldValue(fields[`description_${parameter.id}`].value, parameter.description);

          return {
            ...parameter,
            name: newNameValue,
            description: newDescriptionValue
          }
        });

        const newRequestInfo = {
          ...invocation.requestInfo,
          request: newRequestInfoValue,
          parameters: newRequestInfoParameters
        };

        const newResponseInfoParameters = invocation.responseInfo.parameters.map((parameter: IBehaviourForm.Parameter) => {
          const newNameValue = shapeFieldValue(fields[`name_${parameter.id}`].value, parameter.name);
          const newDescriptionValue = shapeFieldValue(fields[`description_${parameter.id}`].value, parameter.description);

          return {
            ...parameter,
            name: newNameValue,
            description: newDescriptionValue
          }
        });

        const newResponseInfo = {
          ...invocation.responseInfo,
          response: newResponseInfoValue,
          parameters: newResponseInfoParameters
        };

        return {
          ...invocation,
          invocation: newInvocationValue,
          description: newDescriptionValue,
          requestInfo: newRequestInfo,
          responseInfo: newResponseInfo
        }
      });

      return {
        ...state,
        step3: {
          ...step3,
          invocations: newInvocations
        }
      }
    }
    case CREATE_BEHAVIOUR_SAVE_STEP_2: {
      const fields: ITemporaryStorageForHandleFields.Fields = action.payload;
      const {step2} = state;
      const {analogies} = step2;

      const newAnalogies = analogies.map((analogy: IBehaviourForm.Analogy) => {
        const newAnalogyValue = shapeFieldValue(fields[`analogy_${analogy.id}`].value, analogy.analogy);
        const newDescriptionValue = shapeFieldValue(fields[`description_${analogy.id}`].value, analogy.description);

        return {
          ...analogy,
          analogy: newAnalogyValue,
          description: newDescriptionValue
        }
      });

      return {
        ...state,
        step2: {
          ...step2,
          analogies: newAnalogies
        }
      }
    }
    case CREATE_BEHAVIOUR_SAVE_STEP_1: {
      const fields: ITemporaryStorageForHandleFields.Fields = action.payload;

      const {step1} = state;

      const {behaviorName, description, symbol} = step1;

      const newBehaviourNameValue = shapeFieldValue(fields['behaviorName'].value, behaviorName);
      const newSymbolValue = shapeFieldValue(fields['symbol'].value, symbol);
      const newDescriptionValue = shapeFieldValue(fields['description'].value, description);

      return {
        ...state,
        step1: {
          ...step1,
          behaviorName: newBehaviourNameValue,
          symbol: newSymbolValue,
          description: newDescriptionValue
        },
      };
    }


    case CREATE_BEHAVIOUR_FORM_ADD_PROPERTY_MAIN: {
      const propertiesMain = state.step4.properties;

      const newMainPropertyId = `property_main_${propertiesMain.length+1}`;

      const newMainProperty = shapeDefaultMainProperty(newMainPropertyId);

      return {
        ...state,
        step4: {
          ...state.step4,
          properties: [
            ...state.step4.properties,
            newMainProperty
          ]
        }
      }
    }
    case CREATE_BEHAVIOUR_FORM_ADD_PROPERTY_SECOND: {
      const propertiesMain = state.step4.properties;
      const propertyMainId = action.payload;

      const newPropertiesMain = propertiesMain.map((property: IBehaviourForm.PropertyMain) => {
        if (property.id === propertyMainId) {
          const propertiesSecond = property.properties;

          const newPropertySecondId = `property_second_${propertiesSecond.length+1}_${propertyMainId}`;

          const newPropertySecond = shapeDefaultSecondProperty(newPropertySecondId);

          return {
            ...property,
            properties: [
              ...property.properties,
              newPropertySecond
            ]
          }
        }

        return property
      });

      return {
        ...state,
        step4: {
          ...state.step4,
          properties: newPropertiesMain
        }
      }
    }
    case CREATE_BEHAVIOUR_FORM_ADD_PROPERTY: {
      const propertiesMain = state.step4.properties;

      const propertyMainId = action.payload.propertyMainId;
      const propertySecondId = action.payload.propertySecondId;

      const newPropertiesMain = propertiesMain.map((propertyMain: IBehaviourForm.PropertyMain) => {
        if(propertyMain.id === propertyMainId) {

          const newPropertiesForMain = propertyMain.properties.map((propertySecond: IBehaviourForm.PropertySecond) => {
            if(propertySecond.id === propertySecondId) {
              const newPropertyId = `property_${propertySecond.properties.length+1}_${propertySecondId}_${propertyMainId}`;

              const newProperty = shapeDefaultProperty(newPropertyId);

              return {
                ...propertySecond,
                properties: [
                  ...propertySecond.properties,
                  newProperty
                ]
              }
            }

            return propertySecond;
          });

          return {
            ...propertyMain,
            properties: newPropertiesForMain
          }
        }

        return propertyMain;
      });


      return {
        ...state,
        step4: {
          ...state.step4,
          properties: newPropertiesMain
        }
      }
    }
    case CREATE_BEHAVIOUR_FORM_ADD_PARAMETER_TO_REQUEST: {
      const {invocations} = state.step3;

      const newInvocations = invocations.map((invocation: IBehaviourForm.Invocation) => {
        if(invocation.id === action.payload) {
          const {requestInfo} = invocation;

          const newParameterId = `parameter_${requestInfo.parameters.length+1}_request_${invocation.id}`;

          const newParameter = shapeDefaultParameter(newParameterId);

          return {
            ...invocation,
            requestInfo: {
              ...invocation.requestInfo,
              parameters: [
                ...invocation.requestInfo.parameters,
                newParameter
              ]
            }
          }
        }

        return invocation;
      });

      return {
        ...state,
        step3: {
          ...state.step3,
          invocations: newInvocations
        }
      }
    }
    case CREATE_BEHAVIOUR_FORM_ADD_PARAMETER_TO_RESPONSE: {
      const {invocations} = state.step3;

      const newInvocations = invocations.map((invocation: IBehaviourForm.Invocation) => {
        if(invocation.id === action.payload) {
          const {responseInfo} = invocation;

          const newParameterId = `parameter_${responseInfo.parameters.length+1}_response_${invocation.id}`;

          const newParameter = shapeDefaultParameter(newParameterId);

          return {
            ...invocation,
            responseInfo: {
              ...invocation.responseInfo,
              parameters: [
                ...invocation.responseInfo.parameters,
                newParameter
              ]
            }
          }
        }

        return invocation;
      });

      return {
        ...state,
        step3: {
          ...state.step3,
          invocations: newInvocations
        }
      }
    }
    case CREATE_BEHAVIOUR_FORM_ADD_INVOCATION: {
      const {invocations} = state.step3;

      const newInvocationId = `invocation_${invocations.length+1}`;

      const newInvocation = shapeDefaultInvocation(newInvocationId);

      return {
        ...state,
        step3: {
          ...state.step3,
          invocations: [
            ...state.step3.invocations,
            newInvocation
          ]
        }
      }
    }
    case CREATE_BEHAVIOUR_FORM_ADD_ANALOGY: {
      const {analogies} = state.step2;
      const newAnalogyId = `analogy_${analogies.length+1}`;

      const newAnalogy: IBehaviourForm.Analogy = {
        id: newAnalogyId,
        analogy: '',
        description: ''
      };

      const newAnalogies = [
        ...analogies,
        newAnalogy
      ];

      return {
        ...state,
        step2: {
          ...state.step2,
          analogies: newAnalogies
        }
      }
    }
    case CREATE_BEHAVIOUR_FORM_NEXT_STEP: {
      return {
        ...state,
        activeStep: state.activeStep + 1
      }
    }
    case CREATE_BEHAVIOUR_FORM_PREV_STEP: {
      return {
        ...state,
        activeStep: state.activeStep - 1
      }
    }
    default: {
      return state;
    }
  }
};

const shapeDefaultMainProperty = (id: string): IBehaviourForm.PropertyMain => {
  return {
    id,
    property: '',
    description: '',
    properties: [ shapeDefaultSecondProperty(`property_second_1_${id}`) ]
  }
};

const shapeDefaultSecondProperty = (id: string): IBehaviourForm.PropertySecond => {
  return {
    id,
    property: '',
    description: '',
    properties: [ shapeDefaultProperty(`property_1_${id}`) ]
  }
};

const shapeDefaultProperty = (id: string): IBehaviourForm.Property => {
  return {
    id,
    property: '',
    description: '',
  }
};

const shapeDefaultInvocation = (id: string): IBehaviourForm.Invocation => {
  return {
    id,
    invocation: '',
    description: '',
    requestInfo: shapeDefaultRequest(`request_${id}`),
    responseInfo: shapeDefaultResponse(`response_${id}`)
  }
};

const shapeDefaultRequest = (id: string): IBehaviourForm.Request => {
  return {
    request: '',
    parameters: [ shapeDefaultParameter(`parameter_1_${id}`) ]
  }
};

const shapeDefaultResponse = (id: string): IBehaviourForm.Response => {
  return {
    response: '',
    parameters: [ shapeDefaultParameter(`parameter_1_${id}`) ]
  }
};

const shapeDefaultParameter = (id: string): IBehaviourForm.Parameter => {
  return {
    id,
    name: '',
    description: ''
  }
};