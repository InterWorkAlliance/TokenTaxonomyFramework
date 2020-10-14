// @ts-ignore
import React, {Component} from "react";
import { connect } from 'react-redux';
import {Button, Form, Input} from "antd";
import TextArea from "antd/es/input/TextArea";


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

interface State {
  step: number
  value: string
  jsonModel: {
    behaviorName: string,
    symbol: string,
    otherAliases: string, //???
    description: string,
    analogies: [
      {
        analogy: string,
        description: string
      }
    ],
    dependencies: string, //???
    incompatibilities: string, //???
    influencedBy: string,
    invocations: [
      {
        invocation: string,
        description: string,
        requestInfo: {
          request: string,
          parameters: [
            {
              name: string,
              description: string
            }
          ]
        },
        responseInfo: {
          response: string,
          parameters: [
            {
              name: string,
              description: string
            }
          ]
        }
      }
    ],
    properties: [
      {
        property: string,
        description: string,
        properties: [
          {
            property: string,
            description: string,
            properties: [
              {
                property: string,
                description: string
              }
            ]
          }
        ]
      }
    ]
  }

  thirstPropertyFieldsCount: number
  secondPropertyFieldsCount: number
  thirdPropertyFieldsCount: number
}

class CreateBehaviour extends React.Component<any> {
  state: State = {
    step: 1,
    value: '',
    jsonModel: {
      behaviorName: "",
      symbol: "",
      otherAliases: "", //???
      description: "",
      analogies: [
        {
          analogy: "",
          description: ""
        }
      ],
      dependencies: "", //???
      incompatibilities: "", //???
      influencedBy: "",
      invocations: [
        {
          invocation: "",
          description: "",
          requestInfo: {
            request: "",
            parameters: [
              {
                name: "",
                description: ""
              }
            ]
          },
          responseInfo: {
            response: "",
            parameters: [
              {
                name: "",
                description: ""
              }
            ]
          }
        }
      ],
      properties: [
        {
          property: "",
          description: "",
          properties: [
            {
              property: "",
              description: "",
              properties: [
                {
                  property: "",
                  description: ""
                }
              ]
            }
          ]
        }
      ]
    },

    thirstPropertyFieldsCount: 1,
    secondPropertyFieldsCount: 1,
    thirdPropertyFieldsCount: 1,
  };

  private nextStep = () => {
    const currentStep = this.state.step;
    if (currentStep < 4) {
      this.setState({
        step: currentStep + 1
      })
    }
  };

  private prevStep = () => {
    const currentStep = this.state.step;
    if (currentStep > 1) {
      this.setState({
        step: currentStep - 1
      })
    }
  };

  private createJSON = () => {
    console.log('JSON')
  };

  private handleChange = (e: any) => {
    this.setState({
      value: e.target.value
    })
  };

  private renderStep1 = () => {
    return (
      <div className="inputs-wrapper">
        <Form.Item label="Behavior Name">
          <Input/>
        </Form.Item>
        <Form.Item label="Symbol">
          <Input/>
        </Form.Item>
        <div className="add-area-wrapper">
          <p>Other Aliases</p>
          <div className="add-area" data-name="Known Alias"/>
        </div>
        <Form.Item label="Description">
          <textarea className="large"/>
        </Form.Item>
      </div>
    )
  };

  private renderStep2 = () => {
    return (
      <div className="inputs-wrapper">
        <Form.Item label="Analogy">
          <Input/>
        </Form.Item>
        <Form.Item label="Description">
          <textarea/>
        </Form.Item>
        <div className="add-area-wrapper">
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
  };
  private renderStep3 = () => {
    return (
      <div className="inputs-wrapper">
        <div className="add-area-wrapper">
          <p>Invocations</p>
          <div className="add-area" data-name="Add"/>
        </div>
        <div className="invocation-wrapper">
          <div className="invocation-line-wrapper">
            <Form.Item label="Invocation">
              <Input/>
            </Form.Item>
            <Form.Item label="Describe what invoking this behavior will do, not how it will do it, but what should be the outcome">
              <textarea/>
            </Form.Item>
            <div className="request-wrapper">
              <Form.Item label="Request">
                <Input/>
              </Form.Item>
              <div className="add-area-wrapper">
                <p>Parameters</p>
                <div className="add-area" data-name="Add"/>
              </div>
              <Form.Item label="Parameter Name">
                <Input/>
              </Form.Item>
              <Form.Item label="Value Description: type, defaults, etc. ">
                <textarea/>
              </Form.Item>
            </div>
          </div>
          <div className="response-wrapper">
            <Form.Item label="Response">
              <Input/>
            </Form.Item>
            <div className="add-area-wrapper">
              <p>Parameters</p>
              <div className="add-area" data-name="Add"/>
            </div>
            <Form.Item label="Parameter Name">
              <Input/>
            </Form.Item>
            <Form.Item label="Value Description: type, defaults, etc. ">
              <textarea/>
            </Form.Item>
          </div>
        </div>
      </div>
    )
  };


  private renderStep4 = () => {
    return (
      <div className="inputs-wrapper">
        <div className="add-area-wrapper">
          <p>Properties</p>
          <div className="add-area" data-name="Add"/>
        </div>
        <div className="property-line-wrapper">

          <div className="properties-first-layer">
            <Form.Item label="Property">
              <Input/>
            </Form.Item>
            <Form.Item label="Describe what data the property holds">
              <textarea/>
            </Form.Item>
            <div className="add-area-wrapper">
              <p>Properties</p>
              <div className="add-area" data-name="Add"/>
            </div>

            <div className="properties-second-layer">
              <Form.Item label="Sub-property Name">
                <Input/>
              </Form.Item>
              <Form.Item label="Describe what data the property holds">
                <textarea/>
              </Form.Item>
              <div className="add-area-wrapper">
                <p>Properties</p>
                <div className="add-area" data-name="Add"/>
              </div>
              <div className="properties-third-layer">
                <div className="property-fields-wrapper">
                  <Form.Item label="Sub-sub-property Name">
                    <Input/>
                  </Form.Item>
                  <Form.Item label="Describe what data the property holds">
                    <textarea/>
                  </Form.Item>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  };

  public render() {
     return (
      <div className={"form creating-form"}>
        <Form {...formItemLayout}>
          <h2>New Behavior</h2>
          <p className="subtitle">Welcome to the TTF Behavior Wizard</p>
          <div className="form-wrapper">
            <div className={`road-wrapper rs-${this.state.step}`}>
              <div className="road-step step-1">
                <span>1</span>
              </div>
              <div className="road-step step-2">
                <span>2</span>
              </div>
              <div className="road-step step-3">
                <span>3</span>
              </div>
              <div className="road-step step-4">
                <span>4</span>
              </div>
              <div className="road-line"/>
            </div>
            {this.state.step === 1 ? this.renderStep1() : null}
            {this.state.step === 2 ? this.renderStep2() : null}
            {this.state.step === 3 ? this.renderStep3() : null}
            {this.state.step === 4 ? this.renderStep4() : null}
            <div className="submit-wrapper">
              <Form.Item
                wrapperCol={{
                  xs: {span: 24, offset: 0},
                  sm: {span: 16, offset: 8},
                }}
              >
                {this.state.step < 4 ?
                  <Button type="primary" htmlType="submit" className="submit" onClick={this.nextStep}>Next</Button> :
                  <Button type="primary" htmlType="submit" className="submit" onClick={this.createJSON}>Create</Button>}
                {this.state.step > 1 ?
                  <Button type="primary" htmlType="button" className="cancel" onClick={this.prevStep}>Back</Button> :
                  <Button type="primary" htmlType="button" className="cancel" >Cancel</Button>}
              </Form.Item>
            </div>
          </div>
        </Form>
      </div>
    );
  }
}

export default CreateBehaviour;

// export default connect(
//   function () {
//     return {
//     }
//   },
//   { },
//   null,
//   {
//     pure: false
//   }
// )(Home);