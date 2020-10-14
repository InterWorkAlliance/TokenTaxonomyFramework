import * as React from 'react';
import {connect} from 'react-redux';
import {Dispatch} from "redux";
import IBehaviourForm from "../../../reducers/behaviour-form-reducer/model";
import {AppState} from "../../../reducers";
import {createBehaviourFormAddProperty
} from "../../../actions/create-behaviour-form";
import CreateBehaviourFormPropertiesSecond from "../create-behaviour-form-properties-second";
import ITemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields/model";
import AdvancedInput from "../../../components/advanced-input";
import AdvancedTextArea from "../../../components/advanced-textarea";

interface IProps {
  temporaryStorage: ITemporaryStorageForHandleFields.Impl;
  dispatch: Dispatch;
  property: IBehaviourForm.PropertyMain;
  propertyMainId: string;
  handleAddPropertySecond(id: string): void;
}

interface IState {

}

class CreateBehaviourFormPropertiesMain extends React.Component<IProps, IState> {
  handleAddPropertySecond = () => {
    this.props.handleAddPropertySecond(this.props.property.id);
  };

  handleAddProperty = (propertyMainId: string, propertySecondId: string) => {
    this.props.dispatch(createBehaviourFormAddProperty(propertyMainId, propertySecondId));
  };

  render() {
    const {property, temporaryStorage} = this.props;

    return(
      <div className="property-line-wrapper">
        <div className="properties-first-layer" key={property.id}>
          <AdvancedInput
            temporaryStorage={temporaryStorage}
            id={`property_${property.id}`}
            label={"Property"}
            value={property.property}/>
          <AdvancedTextArea
            temporaryStorage={temporaryStorage}
            id={`description_${property.id}`}
            label={"Describe what data the property holds"}
            value={property.description}/>
          <div className="add-area-wrapper">
            <p>Properties</p>
            <div className="add-area" data-name="Add" onClick={this.handleAddPropertySecond}/>
          </div>
        {
          property.properties.map((property: IBehaviourForm.PropertySecond) => {
            return(
              <CreateBehaviourFormPropertiesSecond
                temporaryStorage={this.props.temporaryStorage}
                property={property}
                propertyMainId={this.props.propertyMainId}
                handleAddProperty={this.handleAddProperty}
                key={property.id}/>
            )
          })
        }
        </div>
      </div>
    )
  }
}
const mapStateToProps = ({ forms: {behaviourForm: {step4}} }: AppState) => ({ step4 });

export default connect(mapStateToProps)(CreateBehaviourFormPropertiesMain);