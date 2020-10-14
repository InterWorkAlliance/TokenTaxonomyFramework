import * as React from 'react';
import {EventEmitter} from 'events';
import {connect} from 'react-redux';
import {Dispatch} from "redux";
import IBehaviourForm from "../../../reducers/behaviour-form-reducer/model";
import {AppState} from "../../../reducers";
import TemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields";
import ITemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields/model";
import AdvancedInput from "../../../components/advanced-input";
import {createBehaviourFormSaveStep1} from "../../../actions/create-behaviour-form";
import AdvancedTextArea from "../../../components/advanced-textarea";

interface IProps {
  dispatch: Dispatch;
  step1: IBehaviourForm.Step1;
  eventEmitter: EventEmitter
}

interface IState {

}

class CreateBehaviourFormStep1 extends React.Component<IProps, IState> {
  private temporaryStorage: ITemporaryStorageForHandleFields.Impl = new TemporaryStorageForHandleFields();

  componentDidMount(): void {
    this.props.eventEmitter.on('NEXT_STEP', this.saveData);
    this.props.eventEmitter.on('PREV_STEP', this.saveData);
  }

  private saveData = () => {
    this.props.dispatch(createBehaviourFormSaveStep1(this.temporaryStorage.getFields()))

    this.props.eventEmitter.removeListener('NEXT_STEP', this.saveData);
    this.props.eventEmitter.removeListener('PREV_STEP', this.saveData);
  };

  render() {
    const {behaviorName,symbol, description } = this.props.step1;

    return (
      <div className="inputs-wrapper">
        <AdvancedInput
          temporaryStorage={this.temporaryStorage}
          id={'behaviorName'}
          label={'Behavior Name'}
          value={behaviorName}
        />
        <AdvancedInput
          temporaryStorage={this.temporaryStorage}
          id={'symbol'}
          label={'Symbol'}
          value={symbol}
        />
        <div className="add-area-wrapper">
          <p>Other Aliases</p>
          <div className="add-area" data-name="Known Alias"/>
        </div>
        <AdvancedTextArea
          temporaryStorage={this.temporaryStorage}
          id={'description'}
          label={'Description'}
          value={description}
          largeSize={true}/>
      </div>
    )
  }
}

const mapStateToProps = ({ forms: {behaviourForm: {step1}} }: AppState) => ({ step1 });

export default connect(mapStateToProps)(CreateBehaviourFormStep1);

