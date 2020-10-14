import * as React from 'react';
import {EventEmitter} from 'events';
import {connect} from 'react-redux';
import {Dispatch} from "redux";
import {AppState} from "../../../reducers";
import IBehaviourForm from "../../../reducers/behaviour-form-reducer/model";
import {Button, Form} from "antd";
import Steps from "../../../components/steps";
import CreateBehaviourFormStep1 from "../create-behaviour-form-step-1";
import CreateBehaviourFormStep4 from "../create-behaviour-form-step-4";
import CreateBehaviourFormStep3 from "../create-behaviour-form-step-3";
import CreateBehaviourFormStep2 from "../create-behaviour-form-step-2";
import {createBehaviourFormNextStep, createBehaviourFormPrevStep} from "../../../actions/create-behaviour-form";

const formItemLayout = {
  labelCol: {
    xs: { span: 3 },
    sm: { span: 8 },
  },
  wrapperCol: {
    xs: { span: 24 },
    sm: { span: 16 },
  },
};

interface IProps {
  behaviourForm: IBehaviourForm.ModelState;
  dispatch: Dispatch;
}

interface IState {

}

class CreateBehaviourForm extends React.Component<IProps, IState> {
  private eventEmitter: EventEmitter = new EventEmitter();

  handleNextStep = () => {
    this.eventEmitter.emit('NEXT_STEP');
    this.props.dispatch(createBehaviourFormNextStep())
  };

  handlePrevStep = () => {
    this.eventEmitter.emit('PREV_STEP');
    this.props.dispatch(createBehaviourFormPrevStep())
  };

  handleSubmit = () => {
    this.eventEmitter.emit('SUBMIT');


    //TODO: refactor setTimeout async
    setTimeout(() => {
      const formState = this.props.behaviourForm;

      const formAnalogies = formState.step2.analogies.map((analogy) => {
        return {
          analogy: analogy.analogy,
          description: analogy.description
        }
      });

      const formInvocations = formState.step3.invocations.map((invocation) => {
        const requestParameters = invocation.requestInfo.parameters.map((parameter) => {
          return {
            name: parameter.name,
            description: parameter.description
          }
        });

        const responseParameters = invocation.responseInfo.parameters.map((parameter) => {
          return {
            name: parameter.name,
            description: parameter.description
          }
        });

        return {
          invocation: invocation.invocation,
          description: invocation.description,
          requestInfo: {
            request: invocation.requestInfo.request,
            parameters: requestParameters
          },
          responseInfo: {
            response: invocation.responseInfo.response,
            parameters: responseParameters
          }
        }
      });

      const formProperties = formState.step4.properties.map((propertyMain) => {

        const secondProperties = propertyMain.properties.map((propertySecond) => {

          const properties = propertySecond.properties.map((property) => {
            return {
              property: property.property,
              description: property.description
            }
          });

          return {
            property: propertySecond.property,
            description: propertySecond.description,
            properties: properties
          }
        });

        return {
          property: propertyMain.property,
          description: propertyMain.description,
          properties: secondProperties
        }
      });

      const jsonModel = {
        behaviorName: formState.step1.behaviorName,
        symbol: formState.step1.symbol,
        otherAliases: formState.step1.otherAliases,
        description: formState.step1.description,

        analogies: formAnalogies,
        dependencies: formState.step2.dependencies,
        incompatibilities: formState.step2.incompatibilities,
        influencedBy: formState.step2.influencedBy,

        invocations: formInvocations,

        properties: formProperties
      };

      console.log(jsonModel)
    }, 0);
  };

  render() {
    const {behaviourForm} = this.props;

    const {steps, activeStep} = behaviourForm;

    return (
      <div className={"form creating-form"}>
        <Form {...formItemLayout}>
          <h2>New Behavior</h2>
          <p className="subtitle">Welcome to the TTF Behavior Wizard</p>

          <div className="form-wrapper">
            <Steps
              steps={steps}
              activeStep={activeStep}
            />
            {activeStep === 1 ? <CreateBehaviourFormStep1
              eventEmitter={this.eventEmitter}
            /> : null}
            {activeStep === 2 ? <CreateBehaviourFormStep2
              eventEmitter={this.eventEmitter}
            /> : null}
            {activeStep === 3 ? <CreateBehaviourFormStep3
              eventEmitter={this.eventEmitter}
            /> : null}
            {activeStep === 4 ? <CreateBehaviourFormStep4
              eventEmitter={this.eventEmitter}
            /> : null}

            <div className="submit-wrapper">
              <Form.Item
                wrapperCol={{
                  xs: {span: 24, offset: 0},
                  sm: {span: 16, offset: 8},
                }}
              >
                {activeStep < 4 ?
                  <Button type="primary" htmlType="submit" className="submit" onClick={this.handleNextStep}>Next</Button> :
                  <Button type="primary" htmlType="submit" className="submit" onClick={this.handleSubmit}>Create</Button>}
                {activeStep > 1 ?
                  <Button type="primary" htmlType="button" className="cancel" onClick={this.handlePrevStep}>Back</Button> : null}
              </Form.Item>
            </div>
          </div>
        </Form>
      </div>
    );
  }
}

const mapStateToProps = ({ forms: {behaviourForm} }: AppState) => ({ behaviourForm });

export default connect(mapStateToProps)(CreateBehaviourForm);

