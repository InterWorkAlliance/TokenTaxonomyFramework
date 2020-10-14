import React, {Component} from "react";
import {Layout} from "antd";

import {TopbarWrapper} from "./topbar.style";
import {CSSProperties} from "styled-components";

const {Header} = Layout;

class Topbar extends Component {
  public render() {
    const styling: CSSProperties = {
      background: "#fff",
      height: 70
    };

    return (
      <Header style={styling}><h1><img src="/images/ttflogo.jpg" height="66px"/>Token Taxonomy Editor</h1></Header>
    );
  }
}

export default Topbar;