import React, {Component} from "react";
import {connect} from 'react-redux';
import {
  Form,
  Input,
  Tooltip,
  Icon,
  Cascader,
  Select,
  Row,
  Col,
  Checkbox,
  Button,
  AutoComplete,
} from 'antd';
import {render} from "react-dom";
import {Taxonomy} from "../model/taxonomy_pb";
import {FormProps} from "antd/lib/form";
import KeyValue from "../components/form/keyvalue";
import {WrappedFormUtils} from "antd/lib/form/Form";

const formItemLayout = {
  labelCol: {
    xs: {span: 24},
    sm: {span: 8},
  },
  wrapperCol: {
    xs: {span: 24},
    sm: {span: 16},
  },
};

const BaseForm = (formProps: FormProps) => {
  // @ts-ignore
  const {getFieldDecorator} = formProps.form;
  // @ts-ignore
  const form: WrappedFormUtils = formProps.form;
  return <div>
    <Form {...formItemLayout}>
      <Form.Item label="Name">
        {getFieldDecorator('name', {
          rules: [
            {
              required: true,
              message: 'Please input a token name',
            },
          ],
        })(<Input />)}
      </Form.Item>
      <Form.Item label="Symbol">
        {getFieldDecorator('symbol', {
          rules: [
            {
              required: true,
              message: 'Please input a token symbol',
            },
          ],
        })(<Input />)}
      </Form.Item>
      <Form.Item label="Owner">
        {getFieldDecorator('owner', {
          rules: [
            {
              required: true,
              message: 'Please input a token owner',
            },
          ],
        })(<Input />)}
      </Form.Item>
      <Form.Item label="Quantity">
        {getFieldDecorator('quantity', {
          rules: [
            {
              required: true,
              message: 'Please enter a quantity',
            },
          ],
        })(<Input />)}
      </Form.Item>
      <Form.Item label="Decimals">
        {getFieldDecorator('decimals', {
          rules: [
            {
              required: true,
              message: 'Please enter the number of decimals allowed',
            },
          ],
        })(<Input />)}
      </Form.Item>
      <Form.Item label="Constructor name">
        {getFieldDecorator('constructor_name', {
          rules: [
            {
              required: true,
              message: 'Please enter the name of the constructor',
            },
          ],
        })(<Input />)}
      </Form.Item>
      {new KeyValue({field : "token_properties", form : form, label : "Token properties"}).render()}
    </Form>
  </div>;
};

export const createdForm = Form.create({name: 'token'})(BaseForm);
export default createdForm;

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