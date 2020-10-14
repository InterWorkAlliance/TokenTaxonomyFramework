import * as React from 'react';
import {Form} from "antd";
import ITemporaryStorageForHandleFields from "../../utils/temporary-storage-for-handle-fields/model";

interface IProps {
  temporaryStorage: ITemporaryStorageForHandleFields.Impl
  id: string;
  label: string;
  value: string;
  largeSize?: boolean
}

interface IState {
  value: string;
}

export default class AdvancedTextArea extends React.Component<IProps, IState> {
  state: IState = {
    value: this.props.value
  };

  componentDidMount(): void {
    const {id, value} = this.props;

    this.props.temporaryStorage.setFieldValue(id, value);
  }

  onChange = (e: any) => {
    const {id} = this.props;

    const newValue = e.target.value;

    this.props.temporaryStorage.updateFieldValue(id, newValue);

    this.setState({
      value: newValue
    })
  };

  render() {
    const {label} = this.props;

    return(
      <Form.Item label={label}>
        <textarea
          className={this.props.largeSize ? 'large' : ''}
          value={this.state.value}
          onChange={this.onChange}/>
      </Form.Item>
    )
  }
}