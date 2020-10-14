import React, {Component} from 'react';
import './shared/styles/global.scss';
import { Layout } from 'antd';
import Sidebar from './components/sidebar/sidebar';
import AppRouter from './AppRouter';
import {BrowserRouter} from "react-router-dom";


import { connect } from 'react-redux';
import {Base} from "./model/core_pb";
import { IStoreState } from './store/IStoreState'
import { actionFetchServerState } from './actions';

import 'antd/dist/antd.css';


const { Content } = Layout;

class App extends Component<AppProps> {

  public constructor(props: AppProps) {
    super(props);
  }

  public componentDidMount() {
    if(this.props.state === 'INIT') {
      this.props.loadData();
    }
  }

  public render = () => {
    return (
      <BrowserRouter>
        <Layout>
          <Layout style={{ flexDirection: 'row', overflowX: 'hidden' }} >
            <Sidebar />
            <Layout style={{ padding: '2px 2px 2px' }}>
              <Content
                style={{
                  background: '#fff',
                  padding: 24,
                  margin: 0,
                  minHeight: 280,
                  height: "calc(100vh - 70px)"
                }}
              >
                <AppRouter style={{}}/>
              </Content>
            </Layout>
          </Layout>
        </Layout>
      </BrowserRouter>
    );
  };
}

interface AppProps {
  loadData: () => () => void;
  bases: Base[];
  state: string;
}

const mapStateToProps = (state: IStoreState) => {
  return {
    bases: state.ui.sidebarUI.bases,
    state: state.ui.sidebarUI.state,
    selectedEntity: {entity: null}
  };
};

const mapDispatchToProps = (dispatch: any) => {
  return {
    loadData: () => dispatch(actionFetchServerState())
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(App);
