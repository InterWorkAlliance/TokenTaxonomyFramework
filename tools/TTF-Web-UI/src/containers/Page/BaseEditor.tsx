import React, {Component} from 'react';
import LayoutContentWrapper from '../../components/util/layoutWrapper';
import LayoutContent from '../../components/util/layoutContent';
import {connect} from 'react-redux';
import {} from '../../actions';

import {
  Table, Button, Modal, Form,
} from 'antd';

import Base from '../../pages/editor';
import {IStoreState} from "../../store/IStoreState";
import {FormComponentProps} from "antd/es/form";



class BaseEditor extends Component {
    render() {
        return (
            <LayoutContentWrapper>
                <LayoutContent>
                    hi
                </LayoutContent>
            </LayoutContentWrapper>
        );
    }
}
export default connect(
  (state: IStoreState) => {
    return {
      bases: state.entities.bases
    }
  },
  {  }
)(BaseEditor);