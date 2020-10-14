import * as React from 'react';
import {connect} from 'react-redux';
import {Dispatch} from "redux";
import IBehaviourForm from "../../../reducers/behaviour-form-reducer/model";
import {AppState} from "../../../reducers";
import {
  createBehaviourFormAddPropertyMain,
  createBehaviourFormAddPropertySecond, createBehaviourFormSaveStep4
} from "../../../actions/create-behaviour-form";
import CreateBehaviourFormPropertiesMain from "../create-behaviour-form-properties-main";
import {EventEmitter} from "events";
import ITemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields/model";
import TemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields";

interface IProps {
  dispatch: Dispatch;
  step4: IBehaviourForm.Step4;
  eventEmitter: EventEmitter;
}

interface IState {

}

class CreateBehaviourFormStep4 extends React.Component<IProps, IState> {
  private temporaryStorage: ITemporaryStorageForHandleFields.Impl = new TemporaryStorageForHandleFields();

  componentDidMount(): void {
    this.props.eventEmitter.on('NEXT_STEP', this.saveData);
    this.props.eventEmitter.on('PREV_STEP', this.saveData);
    this.props.eventEmitter.on('SUBMIT', this.saveData);
  }

  private saveData = () => {
    this.props.dispatch(createBehaviourFormSaveStep4(this.temporaryStorage.getFields()));

    this.props.eventEmitter.removeListener('NEXT_STEP', this.saveData);
    this.props.eventEmitter.removeListener('PREV_STEP', this.saveData);
    this.props.eventEmitter.removeListener('SUBMIT', this.saveData);
  };

  handleAddPropertyMain = () => {
    this.props.dispatch(createBehaviourFormAddPropertyMain())
  };

  handleAddPropertySecond = (propertyMainId: string) => {
    this.props.dispatch(createBehaviourFormAddPropertySecond(propertyMainId));
  };

  render() {
    const {properties} = this.props.step4;
    return (
      <div className="inputs-wrapper">
        <div className="add-area-wrapper">
          <p>Properties</p>
          <div className="add-area" data-name="Add" onClick={this.handleAddPropertyMain}/>
        </div>
        {
          properties.map((property: IBehaviourForm.PropertyMain) => {
            return(
              <CreateBehaviourFormPropertiesMain
                temporaryStorage={this.temporaryStorage}
                property={property}
                propertyMainId={property.id}
                handleAddPropertySecond={this.handleAddPropertySecond}
                key={property.id}/>
            )
          })
        }
      </div>
    )
  }
}

const mapStateToProps = ({ forms: {behaviourForm: {step4}} }: AppState) => ({ step4 });

export default connect(mapStateToProps)(CreateBehaviourFormStep4);