import * as React from 'react';

import IBehaviourForm from "../../../reducers/behaviour-form-reducer/model";

import ITemporaryStorageForHandleFields from "../../../utils/temporary-storage-for-handle-fields/model";
import AdvancedInput from "../../../components/advanced-input";
import AdvancedTextArea from "../../../components/advanced-textarea";

interface IProps {
  temporaryStorage: ITemporaryStorageForHandleFields.Impl;
  property: IBehaviourForm.PropertySecond;
  propertyMainId: string;
  handleAddProperty(propertyMainId: string, propertySecondId: string): void;
}

interface IState {

}

export default class CreateBehaviourFormPropertiesSecond extends React.Component<IProps, IState> {
  handleAddProperty = () => {
      this.props.handleAddProperty(this.props.propertyMainId, this.props.property.id);
  };

  render() {
    const {property, temporaryStorage} = this.props;

    return(
      <div className="properties-second-layer" key={property.id}>
        <AdvancedInput
          temporaryStorage={temporaryStorage}
          id={`sub_property_${property.id}`}
          label={"Sub-property Name"}
          value={property.property}/>
        <AdvancedTextArea
          temporaryStorage={temporaryStorage}
          id={`sub_description_${property.id}`}
          label={"Describe what data the property holds"}
          value={property.description}/>

        <div className="add-area-wrapper">
          <p>Properties</p>
          <div className="add-area" data-name="Add" onClick={this.handleAddProperty}/>
        </div>
        {
          property.properties.map((propertyDefault: IBehaviourForm.Property) => {
            return (
              <div className="properties-third-layer" key={propertyDefault.id}>
                <div className="property-fields-wrapper">
                  <AdvancedInput
                    temporaryStorage={temporaryStorage}
                    id={`sub_sub_property_${propertyDefault.id}`}
                    label={"Sub-sub-property Name"}
                    value={propertyDefault.property}/>
                  <AdvancedTextArea
                    temporaryStorage={temporaryStorage}
                    id={`sub_sub_description_${propertyDefault.id}`}
                    label={"Describe what data the property holds"}
                    value={propertyDefault.description}/>
                </div>
              </div>
            )
          })
        }
      </div>
    )
  }
}