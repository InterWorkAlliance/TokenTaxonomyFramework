import * as React from 'react';
import IBehaviourForm from "../../../reducers/behaviour-form-reducer/model";
import ITemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields/model";
import AdvancedInput from "../../../components/advanced-input";
import AdvancedTextArea from "../../../components/advanced-textarea";

interface IProps {
  temporaryStorage: ITemporaryStorageForHandleFields.Impl;
  invocation: IBehaviourForm.Invocation;
  addParameterToResponse(id: string): void;
  addParameterToRequest(id: string): void;
}

interface IState {

}

export default class CreateBehaviourFormInvocation extends React.Component<IProps, IState> {
  handleAddParameterToResponse = () => {
    this.props.addParameterToResponse(this.props.invocation.id);
  };

  handleAddParameterToRequest = () => {
    this.props.addParameterToRequest(this.props.invocation.id);
  };

  render() {
    const {invocation, temporaryStorage} = this.props;

    return(
      <div className="invocation-wrapper">
        <div className="invocation-line-wrapper">
          <AdvancedInput
            temporaryStorage={temporaryStorage}
            id={`invocation_${invocation.id}`}
            label={"Invocation"}
            value={invocation.invocation}/>
          <AdvancedTextArea
            temporaryStorage={temporaryStorage}
            id={`description_${invocation.id}`}
            label={"Describe what invoking this behavior will do, not how it will do it, but what should be the outcome"}
            value={invocation.description}/>
          <div className="request-wrapper">
            <AdvancedInput
              temporaryStorage={temporaryStorage}
              id={`request_${invocation.id}`}
              label={"Request"}
              value={invocation.requestInfo.request}/>
            <div className="add-area-wrapper">
              <p>Parameters</p>
              <div className="add-area" data-name="Add" onClick={this.handleAddParameterToRequest}/>
            </div>
            {
              invocation.requestInfo.parameters.map((parameter: IBehaviourForm.Parameter) => {
                return(
                  <React.Fragment key={parameter.id}>
                    <AdvancedInput
                      temporaryStorage={temporaryStorage}
                      id={`name_${parameter.id}`}
                      label={"Parameter Name"}
                      value={parameter.name}/>
                    <AdvancedTextArea
                      temporaryStorage={temporaryStorage}
                      id={`description_${parameter.id}`}
                      label={"Value Description: type, defaults, etc."}
                      value={parameter.description}/>
                  </React.Fragment>
                )
              })
            }
          </div>
        </div>
        <div className="response-wrapper">
          <AdvancedInput
            temporaryStorage={temporaryStorage}
            id={`response_${invocation.id}`}
            label={"Response"}
            value={invocation.responseInfo.response}/>
          <div className="add-area-wrapper">
            <p>Parameters</p>
            <div className="add-area" data-name="Add" onClick={this.handleAddParameterToResponse}/>
          </div>
          {
            invocation.responseInfo.parameters.map((parameter: IBehaviourForm.Parameter) => {
              return(
                <React.Fragment key={parameter.id}>
                  <AdvancedInput
                    temporaryStorage={temporaryStorage}
                    id={`name_${parameter.id}`}
                    label={"Parameter Name"}
                    value={parameter.name}/>
                  <AdvancedTextArea
                    temporaryStorage={temporaryStorage}
                    id={`description_${parameter.id}`}
                    label={"Value Description: type, defaults, etc."}
                    value={parameter.description}/>
                </React.Fragment>
              )
            })
          }
        </div>
      </div>
    )
  }
}