import {Form, Input, Icon, Button} from 'antd';
import React from "react";
import {WrappedFormUtils} from "antd/lib/form/Form";

type KeyValueProps = {
  form: WrappedFormUtils;
  field: string;
  label: string;
  disabled?: boolean;
}

type KeyValuePair = {
  key: string;
  value: string;
}

export class KeyValue {
  props: KeyValueProps;
  constructor(props: KeyValueProps) {
    this.props = props;
  }

  remove(k: number) {
    const {form, field} = this.props;
    // can use data-binding to get
    const keyPairs = form.getFieldValue(field);
    const dataChanges = {};
    // @ts-ignore
    dataChanges[field] = keyPairs.filter((value: KeyValuePair, index: number) => {
      return index !== k;
    });

    // can use data-binding to set
    form.setFieldsValue(dataChanges);
  };

  public add = () => {
    const {form, field} = this.props;
    // can use data-binding to get
    const keypairs = form.getFieldValue(field);
    const nextKeys = keypairs.concat({key : "", value : ""});

    // can use data-binding to set
    // important! notify form to detect changes
    const dataChanges = {};
    // @ts-ignore
    dataChanges[field] = nextKeys;
    form.setFieldsValue(dataChanges);
  };

  render() {
    const {getFieldDecorator, getFieldValue} = this.props.form;
    const {field, label} = this.props;
    const formItemLayout = {
      labelCol: {
        xs: {span: 24},
        sm: {span: 4},
      },
      wrapperCol: {
        xs: {span: 24},
        sm: {span: 20},
      },
    };
    const formItemLayoutWithOutLabel = {
      wrapperCol: {
        xs: {span: 24, offset: 0},
        sm: {span: 20, offset: 4},
      },
    };
    getFieldDecorator(field, {initialValue: []});
    const keyValues = getFieldValue(field);
    const formItems = keyValues.map((kv: KeyValuePair, index: number) => {
      return <Form.Item
        {...(index === 0 ? formItemLayout : formItemLayoutWithOutLabel)}
        label={index === 0 ? label : ''}
        required={false}
        key={index}
      >
        {getFieldDecorator(`names[${index + "-k"}]`, {
          validateTrigger: ['onChange', 'onBlur'],
          rules: [
            {
              required: true,
              whitespace: true,
              message: "Please input value or delete this field.",
            },
          ],
        })(<Input placeholder="key" style={{width: '60%', marginRight: 8}}/>)}
        {getFieldDecorator(`names[${index + "-v"}]`, {
          validateTrigger: ['onChange', 'onBlur'],
          rules: [
            {
              required: true,
              whitespace: true,
              message: "Please input value or delete this field.",
            },
          ],
        })(<Input placeholder="value" style={{width: '60%', marginRight: 8}}/>)}
        <Icon
          className="dynamic-delete-button"
          type="minus-circle-o"
          onClick={() => this.remove(index)}
        />
      </Form.Item>;
    });
    return formItems.concat(
      <Form.Item {...formItemLayout} label={keyValues.length === 0 ? label : null} >
        <Button type="dashed" onClick={this.add} style={{width: '60%'}} disabled={this.props.disabled} >
          <Icon type="plus" /> Add field
        </Button>
      </Form.Item>
    );
  }
}

export default KeyValue;