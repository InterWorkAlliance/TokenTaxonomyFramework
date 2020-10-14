import * as React from 'react';
import {connect} from 'react-redux';
import {Dispatch} from "redux";
import IBehaviourForm from "../../../reducers/behaviour-form-reducer/model";
import {AppState} from "../../../reducers";
import {
  createBehaviourFormAddInvocation, createBehaviourFormAddParameterToRequest,
  createBehaviourFormAddParameterToResponse, createBehaviourFormSaveStep3
} from "../../../actions/create-behaviour-form";
import CreateBehaviourFormInvocation from "../create-behaviour-form-invocation";
import {EventEmitter} from "events";
import ITemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields/model";
import TemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields";

interface IProps {
  dispatch: Dispatch;
  step3: IBehaviourForm.Step3;
  eventEmitter: EventEmitter
}

interface IState {

}

class CreateBehaviourFormStep3 extends React.Component<IProps, IState> {
  private temporaryStorage: ITemporaryStorageForHandleFields.Impl = new TemporaryStorageForHandleFields();

  componentDidMount(): void {
    this.props.eventEmitter.on('NEXT_STEP', this.saveData);
    this.props.eventEmitter.on('PREV_STEP', this.saveData);
  }

  private saveData = () => {
    this.props.dispatch(createBehaviourFormSaveStep3(this.temporaryStorage.getFields()))

    this.props.eventEmitter.removeListener('NEXT_STEP', this.saveData);
    this.props.eventEmitter.removeListener('PREV_STEP', this.saveData);
  };

  handleAddInvocation = () => {
    this.props.dispatch(createBehaviourFormAddInvocation())
  };

  addParameterToResponse = (id: string) => {
    this.props.dispatch(createBehaviourFormAddParameterToResponse(id));
  };

  addParameterToRequest = (id: string) => {
    this.props.dispatch(createBehaviourFormAddParameterToRequest(id));
  };

  render() {
    const {invocations} = this.props.step3;
    return (
      <div className="inputs-wrapper">
        <div className="add-area-wrapper">
          <p>Invocations</p>
          <div className="add-area" data-name="Add" onClick={this.handleAddInvocation}/>
        </div>
        {
          invocations.map((invocation: IBehaviourForm.Invocation) => {
            return (
              <CreateBehaviourFormInvocation
                temporaryStorage={this.temporaryStorage}
                invocation={invocation}
                addParameterToResponse={this.addParameterToResponse}
                addParameterToRequest={this.addParameterToRequest}
                key={invocation.id}/>
            )
          })
        }
      </div>
    )
  }
}

const mapStateToProps = ({ forms: {behaviourForm: {step3}} }: AppState) => ({ step3 });

export default connect(mapStateToProps)(CreateBehaviourFormStep3);