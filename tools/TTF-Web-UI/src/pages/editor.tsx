import React from "react";
import {connect} from 'react-redux';
import {
  Form,
  Input,
  Button, Tabs
} from 'antd';
import KeyValue from "../components/form/keyvalue";
import {WrappedFormUtils} from "antd/lib/form/Form";
import {FormComponentProps} from "antd/es/form";
import {IStoreState} from "../store/IStoreState";
import TextArea from "antd/es/input/TextArea";
import {Artifact, ArtifactType, UpdateArtifactRequest} from "../model/artifact_pb";
import {Any} from "google-protobuf/google/protobuf/any_pb";
import {BinaryWriter} from "google-protobuf";
import {client} from "../state";
import '../shared/styles/global.scss';

const editable = document.cookie.replace(/(?:(?:^|.*;\s*)EDITABLE\s*\=\s*([^;]*).*$)|^.*$/, "$1") === 'true';

const formItemLayout = {
  labelCol: {
    xs: {span: 3},
    sm: {span: 8},
  },
  wrapperCol: {
    xs: {span: 24},
    sm: {span: 16},
  },
};

let countAliasesFields = 0;
let countDependenciesFields = 0;
let countIncompatibleSymbols = 0;
let countInfluencedSymbols = 0;
let countArtifactFiles = 0;
let isBasesArtifact = false;

interface BaseFormProps extends FormComponentProps {
  state: IStoreState;
  entityType: string | null;
  currentEntity: string | null;
}

function getCurrentArtifact(artifact: Artifact, id: string)  {
  if (artifact) {
    const symbol = artifact.getArtifactSymbol();
    if (symbol && symbol.getId() === id) {
      return true;
    }
  } else {
    return false;
  }
}

class BaseForm extends React.Component<BaseFormProps, any> {

  constructor(props: BaseFormProps) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  public handleSubmit() {
    const form = this.props.form;
    const entityType = this.props.entityType;
    switch (entityType) {
      case "behavior":
        const behavior = this.props.state.ui.sidebarUI.behaviors
          .find((el) => getCurrentArtifact(el.getArtifact()!!, this.props.currentEntity!!));

        if (!behavior) {
          return;
        }

        // edit

        const request = new UpdateArtifactRequest();
        request.setType(ArtifactType.BEHAVIOR);

        const serialized = behavior.serializeBinary();

        const any = new Any();
        any.pack(serialized, "proto.taxonomy.model.core.Behavior");

        request.setArtifactTypeObject(any);

        client.updateArtifact(request, null, (err, response) => {
          console.log('err - ', err);
          console.log('resp - ', response);
        });
    }
  }

  public render() {
    const form: WrappedFormUtils = this.props.form;
    const state = this.props.state;
    const currentID = this.props.currentEntity;
    const entityType = this.props.entityType;
    const selected = currentID === null ? undefined : state.entities.bases.byId.get(currentID) ||
      state.entities.behaviors.byId.get(currentID) ||
      state.entities.behaviorGroups.byId.get(currentID) ||
      state.entities.propertySets.byId.get(currentID) ||
      state.entities.templateDefinitions.byId.get(currentID);

    if (selected === undefined || entityType === null) {
      return <div/>;
    } else {
      const formName = selected.getName();
      const {TabPane} = Tabs;

      return <div className={"form"}>
        <Form {...formItemLayout}>
          <h2>{formName}</h2>
          <div className="form-wrapper">
            <h3 dangerouslySetInnerHTML={{
              __html: "Edit " +
                selected.getArtifactSymbol()!.getVisual()
            }}/>
            <Tabs defaultActiveKey="1">
              <TabPane tab="General" key="1">
                {this.renderForm()}
              </TabPane>
              <TabPane tab="Artifact Definition" key="2">
                {this.renderArtifact()}
              </TabPane>
            </Tabs>

            {editable ?
              <div className="submit-wrapper">
                <Form.Item
                  wrapperCol={{
                    xs: {span: 24, offset: 0},
                    sm: {span: 16, offset: 8},
                  }}
                >
                  <Button type="primary" htmlType="submit" onClick={this.handleSubmit} className="submit">Submit</Button>
                </Form.Item>
              </div> : null}
          </div>
        </Form>
      </div>;
    }
  }

  private renderArtifact() {
    const form = this.props.form;
    const {getFieldDecorator} = this.props.form;
    const entityType = this.props.entityType;
    return (
      <React.Fragment>
        <div className="inputs-wrapper">
          <Form.Item label="Business Description">
            {getFieldDecorator('businessDescription', {
              rules: [
                {
                  required: false,
                },
              ],
            })(<TextArea disabled={!editable}/>)}
          </Form.Item>
          <Form.Item label="Business Example">
            {getFieldDecorator('businessExample', {
              rules: [
                {
                  required: false,
                },
              ],
            })(<TextArea disabled={!editable}/>)}
          </Form.Item>
          {// TODO add analogies list
          }
          <Form.Item label="Comments">
            {getFieldDecorator('comments', {
              rules: [
                {
                  required: false,
                },
              ],
            })(<TextArea disabled={!editable}/>)}
          </Form.Item>
        </div>
      </React.Fragment>
    );
  }


  private renderForm() {
    const {getFieldDecorator} = this.props.form;

    const arrayAliasesFields = [];
    const arrayDescriptionFields = [];
    const arrayIncompatibleFields = [];
    const arrayInfluencedFields = [];
    const arrayArtifactFiles = [];
    const arrayBaseAdditionalFields = [];

    if (countAliasesFields) {
      for (let i = 0; i < countAliasesFields; i++){
        const form =
          <Form.Item label="Alias">
            {getFieldDecorator(`aliases${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayAliasesFields.push(form);
      }
    }

    if (countDependenciesFields) {
      for (let i = 0; i < countDependenciesFields; i++){
        const formDescription =
          <Form.Item label="Description">
            {getFieldDecorator(`description${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayDescriptionFields.push(formDescription);

        const formSymbol =
          <Form.Item label="Symbol">
            {getFieldDecorator(`symbol${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayDescriptionFields.push(formSymbol);
      }
    }

    if (countIncompatibleSymbols) {
      for (let i = 0; i < countIncompatibleSymbols; i++){
        const formId =
          <Form.Item label="Id">
            {getFieldDecorator(`id${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayIncompatibleFields.push(formId);

        const formTooling =
          <Form.Item label="Tooling">
            {getFieldDecorator(`tooling${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayIncompatibleFields.push(formTooling);

        const formVersion =
          <Form.Item label="Version">
            {getFieldDecorator(`version${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayIncompatibleFields.push(formVersion);

        const formVisual =
          <Form.Item label="Visual">
            {getFieldDecorator(`visual${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayIncompatibleFields.push(formVisual);
      }
    }

    if (countInfluencedSymbols) {
      for (let i = 0; i < countInfluencedSymbols; i++){
        const influencedDescription =
          <Form.Item label="InfluencedDescription">
            {getFieldDecorator(`influencedDescription${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayInfluencedFields.push(influencedDescription);
      }
    }

    if (countArtifactFiles) {
      for (let i = 0; i < countArtifactFiles; i++){
        const fileName =
          <Form.Item label="File Name">
            {getFieldDecorator(`fileName${i}`, {
              rules: [
                {
                  required: false,
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>;

        arrayArtifactFiles.push(fileName);
      }
    }

    if (isBasesArtifact) {
      const tokenType =
        <Form.Item label="Token Type">
          {getFieldDecorator(`tokenType`, {
            rules: [
              {
                required: false,
              },
            ],
          })(<Input disabled={!editable}/>)}
        </Form.Item>;

      const tokenUnit =
        <Form.Item label="Token Unit">
          {getFieldDecorator(`tokenUnit`, {
            rules: [
              {
                required: false,
              },
            ],
          })(<Input disabled={!editable}/>)}
        </Form.Item>;

      const representationType =
        <Form.Item label="Representation Type">
          {getFieldDecorator(`representationType`, {
            rules: [
              {
                required: false,
              },
            ],
          })(<Input disabled={!editable}/>)}
        </Form.Item>;

      const valueType =
        <Form.Item label="Value Type">
          {getFieldDecorator(`valueType`, {
            rules: [
              {
                required: false,
              },
            ],
          })(<Input disabled={!editable}/>)}
        </Form.Item>;

      arrayBaseAdditionalFields.push(tokenType, tokenUnit, representationType, valueType);
    }

    return (
      <React.Fragment>
        <div className="inputs-wrapper">

          <Form.Item label="Name">
            {getFieldDecorator('name', {
              rules: [
                {
                  required: false,
                  message: 'Please input a token name',
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>

          <Form.Item label="Symbol">
            {getFieldDecorator('symbol', {
              rules: [
                {
                  required: false,
                  message: 'Please input a token symbol',
                },
              ],
            })(<Input disabled={!editable}/>)}
          </Form.Item>

          {
            arrayBaseAdditionalFields.length !== 0 && arrayBaseAdditionalFields.map((field: any, index: number) => {
              return <li className={'form-item'} key={index}>{field}</li>;
            })
          }

          {
            arrayAliasesFields.length !== 0 && <h1>Aliases</h1>
          }

          {
            arrayAliasesFields.length !== 0 && arrayAliasesFields.map((field: any, index: number) => {
              return <li className={'form-item'} key={index}>{field}</li>;
            })
          }

          {
            arrayDescriptionFields.length !== 0 && <h1>Description</h1>
          }

          {
            arrayDescriptionFields.length !== 0 && arrayDescriptionFields.map((field: any, index: number) => {
              return <li className={'form-item'} key={index}>{field}</li>;
            })
          }

          {
            arrayIncompatibleFields.length !== 0 && <h1>Incompatible With Symbols</h1>
          }

          {
            arrayIncompatibleFields.length !== 0 && arrayIncompatibleFields.map((field: any, index: number) => {
              return <li className={'form-item'} key={index}>{field}</li>;
            })
          }

          {
            arrayInfluencedFields.length !== 0 && <h1>Influenced By Symbols</h1>
          }

          {
            arrayInfluencedFields.length !== 0 && arrayInfluencedFields.map((field: any, index: number) => {
              return <li className={'form-item'} key={index}>{field}</li>;
            })
          }

          {
            arrayArtifactFiles.length !== 0 && <h1>Artifact Files</h1>
          }

          {
            arrayArtifactFiles.length !== 0 && arrayArtifactFiles.map((field: any, index: number) => {
              return <li className={'form-item'} key={index}>{field}</li>;
            })
          }

        </div>
      </React.Fragment>
    );
  }
}

const Editor = Form.create<BaseFormProps>({
  mapPropsToFields: (props: BaseFormProps) => {
    const state = props.state;
    const currentID = props.currentEntity;
    const entityType = props.entityType;
    const selected = currentID === null ? undefined : state.entities.bases.byId.get(currentID) ||
      state.entities.behaviors.byId.get(currentID) ||
      state.entities.behaviorGroups.byId.get(currentID) ||
      state.entities.propertySets.byId.get(currentID) ||
      state.entities.templateDefinitions.byId.get(currentID);
    if (selected === undefined) {
      return {};
    } else {
      const symbol = selected.getArtifactSymbol()!.getVisual();
      const name = selected.getName();
      const aliases = selected.getAliasesList();
      const dependencies = selected.getDependenciesList();
      const incompatibleSymbols = selected.getIncompatibleWithSymbolsList();
      const influencedSymbols = selected.getInfluencedBySymbolsList();
      const artifactFiles = selected.getArtifactFilesList();
      //const maps = selected.getMaps();

      countAliasesFields = aliases.length;
      countDependenciesFields = dependencies.length;
      countIncompatibleSymbols = incompatibleSymbols.length;
      countInfluencedSymbols = influencedSymbols.length;
      countArtifactFiles = artifactFiles.length;

      let aliasesFields = {};
      let dependenciesFields = {};
      let incompatibleFields = {};
      let influencedSymbolsFields = {};
      let artifactFilesFields = {};
      let baseAdditionalFields = {};

      isBasesArtifact = false;

      if (entityType === 'base') {
        const baseArray = props.state.ui.sidebarUI.bases;
        const currentArtifactName = selected.getName();

        const baseArtifact = baseArray.find((artifact: any) => {
          return artifact.getArtifact()!.getName() === currentArtifactName;
        });

        if (baseArtifact) {
          isBasesArtifact = true;

          const tokenType = baseArtifact.getTokenType();
          const tokenUnit = baseArtifact.getTokenUnit();
          const representationType = baseArtifact.getRepresentationType();
          const valueType = baseArtifact.getValueType();

          baseAdditionalFields = {
            tokenType: Form.createFormField({
              name: tokenType,
              value: tokenType,
            }),
            tokenUnit: Form.createFormField({
              name: tokenUnit,
              value: tokenUnit,
            }),
            representationType: Form.createFormField({
              name: representationType,
              value: representationType,
            }),
            valueType: Form.createFormField({
              name: valueType,
              value: valueType,
            }),
          };
        }
      }


      aliases.forEach((name: string, index: number) => {
        aliasesFields = {
          ...aliasesFields,
          [`aliases${index}`]: Form.createFormField({
            name,
            value: name,
          })
        };
      });

      dependencies.forEach((dependence: any, index: number) => {
        const symbol = dependence.getSymbol()!.getVisual();
        const description = dependence.getDescription();

        dependenciesFields = {
          ...dependenciesFields,
          [`symbol${index}`]: Form.createFormField({
            name: symbol,
            value: symbol,
          }),
          [`description${index}`]: Form.createFormField({
            name: description,
            value: description,
          }),
        };
      });

      incompatibleSymbols.forEach((incompatible: any, index: number) => {
        const id = incompatible.getId();
        const tooling = incompatible.getTooling();
        const version = incompatible.getVersion();
        const visual = incompatible.getVisual();

        incompatibleFields = {
          ...incompatibleFields,
          [`id${index}`]: Form.createFormField({
            name: id,
            value: id,
          }),
          [`tooling${index}`]: Form.createFormField({
            name: tooling,
            value: tooling,
          }),
          [`version${index}`]: Form.createFormField({
            name: version,
            value: version,
          }),
          [`visual${index}`]: Form.createFormField({
            name: visual,
            value: visual,
          }),
        };
      });

      influencedSymbols.forEach((influenced: any, index: number) => {
        // TODO: create getAppliesToList
        const description = influenced.getDescription();

        influencedSymbolsFields = {
          ...influencedSymbolsFields,
          [`influencedDescription${index}`]: Form.createFormField({
            name: description,
            value: description,
          }),
        };
      });

      artifactFiles.forEach((file: any, index: number) => {
        // TODO: create getFileData_asB64 (?)
        const fileName = file.getFileName();

        artifactFilesFields = {
          ...artifactFilesFields,
          [`fileName${index}`]: Form.createFormField({
            name: fileName,
            value: fileName,
          }),
        };
      });

      const generalValues = {
        name: Form.createFormField({
          name: name,
          value: name,
        }),
        symbol: Form.createFormField({
          name: symbol,
          value: symbol,
        }),
        ...aliasesFields,
        ...dependenciesFields,
        ...incompatibleFields,
        ...influencedSymbolsFields,
        ...artifactFilesFields,
        ...baseAdditionalFields
      };
      const artifactValues = {
        businessDescription: Form.createFormField({
          name: 'BusinessDescription',
          value: selected.getArtifactDefinition()!.getBusinessDescription(),
        }),
        businessExample: Form.createFormField({
          name: 'BusinessExample',
          value: selected.getArtifactDefinition()!.getBusinessExample(),
        }),
        // TODO use a list editor
        analogies: Form.createFormField({
          name: 'Analogies',
          value: selected.getArtifactDefinition()!.getAnalogiesList(),
        }),
        comments: Form.createFormField({
          name: 'Comments',
          value: selected.getArtifactDefinition()!.getComments(),
        })
      };

      return {
        ...generalValues,
        ...artifactValues
      };
    }
  }
})(BaseForm);

export default connect(
  (state: IStoreState) => {
    const urlSegments = window.location.href.split('/');
    if (urlSegments.length < 2) {
      return {
        state: state,
        currentEntity: null,
        entityType: null,
      };
    } else {
      return {
        entityType: urlSegments[urlSegments.length -2],
        currentEntity: urlSegments[urlSegments.length -1],
        state: state,
      };
    }
  },
  { },
  null,
  {
    pure: false
  }
)(Editor);