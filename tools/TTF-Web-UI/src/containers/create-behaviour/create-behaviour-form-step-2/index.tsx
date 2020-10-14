import * as React from 'react';
import {connect} from 'react-redux';
import {Dispatch} from "redux";
import IBehaviourForm from "../../../reducers/behaviour-form-reducer/model";
import {AppState} from "../../../reducers";
import {
  createBehaviourFormAddAnalogy,
  createBehaviourFormSaveStep2
} from "../../../actions/create-behaviour-form";
import {EventEmitter} from "events";
import ITemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields/model";
import TemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields";
import AdvancedInput from "../../../components/advanced-input";
import AdvancedTextArea from "../../../components/advanced-textarea";

interface IProps {
  dispatch: Dispatch;
  step2: IBehaviourForm.Step2;
  eventEmitter: EventEmitter
}

interface IState {

}

class CreateBehaviourFormStep2 extends React.Component<IProps, IState> {
  private temporaryStorage: ITemporaryStorageForHandleFields.Impl = new TemporaryStorageForHandleFields();

  componentDidMount(): void {
    this.props.eventEmitter.on('NEXT_STEP', this.saveData);
    this.props.eventEmitter.on('PREV_STEP', this.saveData);
  }

  private saveData = () => {
    this.props.dispatch(createBehaviourFormSaveStep2(this.temporaryStorage.getFields()))

    this.props.eventEmitter.removeListener('NEXT_STEP', this.saveData);
    this.props.eventEmitter.removeListener('PREV_STEP', this.saveData);
  };

  handleAddAnalogy = () => {
    this.props.dispatch(createBehaviourFormAddAnalogy())
  }

  render() {
    const {analogies} = this.props.step2;

    return (
      <div className="inputs-wrapper">
        {
          analogies.map((analogy: IBehaviourForm.Analogy) => {
            return(
              <React.Fragment key={analogy.id}>
                <AdvancedInput
                  temporaryStorage={this.temporaryStorage}
                  id={`analogy_${analogy.id}`}
                  label={'Analogy'}
                  value={analogy.analogy}/>
                <AdvancedTextArea
                  temporaryStorage={this.temporaryStorage}
                  id={`description_${analogy.id}`}
                  label={"Description"}
                  value={analogy.description}/>
              </React.Fragment>
            )
          })
        }

        <div className="add-area-wrapper" onClick={this.handleAddAnalogy}>
          <div className="add-area" data-name="Add"/>
        </div>
        <div className="add-area-wrapper">
          <p>Dependencies</p>
          <div className="add-area" data-name="Other Artifacts"/>
        </div>
        <div className="add-area-wrapper">
          <p>Incompatibilities</p>
          <div className="add-area" data-name="Other Artifacts"/>
        </div>
        <div className="add-area-wrapper">
          <p>Influenced By</p>
          <div className="add-area" data-name="Other Artifacts"/>
        </div>
      </div>
    )
  }
}

const mapStateToProps = ({ forms: {behaviourForm: {step2}} }: AppState) => ({ step2 });

export default connect(mapStateToProps)(CreateBehaviourFormStep2);