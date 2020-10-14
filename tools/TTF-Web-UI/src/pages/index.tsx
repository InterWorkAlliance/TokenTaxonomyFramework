import React from "react";
import {FormComponentProps} from "antd/es/form";
import {connect} from "react-redux";
import {IStoreState} from "../store/IStoreState";

interface IndexPageProps {
}

class IndexPage extends React.Component<IndexPageProps, any> {

  public render() {
    return <div>
      <h1>Welcome to the Token Taxonomy Editor!</h1>
      <p>Pick an existing token element in the sidebar or create a new element with the "Create Artifact" wizard.</p>
    </div>
  }
}

export default connect(
  (state: IStoreState) => {
  },
  { },
  null,
  {
    pure: false
  }
)(IndexPage);