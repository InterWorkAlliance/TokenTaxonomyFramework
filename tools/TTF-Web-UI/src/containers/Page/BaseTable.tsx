import React, {Component} from 'react';
import LayoutContentWrapper from '../../components/util/layoutWrapper';
import LayoutContent from '../../components/util/layoutContent';
import {connect} from 'react-redux';
import {} from '../../actions';

import {
  Table, 
} from 'antd';

import {IStoreState} from "../../store/IStoreState";

class BaseTable extends Component {

  public render () {
    const columns = [
      {
        title: 'Name',
        dataIndex: 'name',
        key: 'name',
        width: "15%"
      }, {
        title: 'Symbol',
        dataIndex: 'symbol',
        key: 'symbol', 
        width: "10%"
      }, {
        title: 'Owner',
        dataIndex: 'owner',
        key: 'owner', 
        width: "15%"
      }, {
        title: 'Quantity',
        dataIndex: 'quantity',
        key: 'quantity', 
        width: "10%"
      }, {
        title: 'Decimals',
        dataIndex: 'decimals',
        key: 'decimals', 
        width: "10%"
      }, {
        title: 'Constructor',
        dataIndex: 'constructor',
        key: 'constructor', 
        width: "15%"
      }, {
      },
    ];

    const mockData = [
      {
        name: 'Base 1',
        symbol: 'Symbol 1',
        owner: 'Owner 1',
        quantity: 5,
        decimals: 3,
        constructor: 'Constructor 1',
        tokenProperties: 'token property mapping 1'
      },
      {
        name: 'Base 2',
        symbol: 'Symbol 2',
        owner: 'Owner 2',
        quantity: 3,
        decimals: 6,
        constructor: 'Constructor 2',
        tokenProperties: 'token property mapping 2'
      },
      {
        name: 'Base 3',
        symbol: 'Symbol 3',
        owner: 'Owner 3',
        quantity: 1,
        decimals: 5,
        constructor: 'Constructor 3',
        tokenProperties: 'token property mapping 3'
      },
    ];

    return (
      <LayoutContentWrapper  >
        <LayoutContent>
          <h1>Fungible Bases</h1>
          <Table
            columns={columns}
            rowKey={record => record.name}
            dataSource={mockData}
            expandedRowRender={record => <p style={{margin: 0}}>{record.tokenProperties}</p>}
          />
        </LayoutContent>
      </LayoutContentWrapper>
    );
  }
}
export default connect(
  (state: IStoreState) => {
    return {
      bases: state.entities.bases
    };
  },
  {  }
)(BaseTable);