import * as React from 'react';

interface IProps {
  label?: string
}

interface IState {
  isChecked: boolean
}

class Checkbox extends React.Component<IProps, IState> {

  state: IState = {
    isChecked: true
  };

  private checkboxHandler = () => {
    this.setState({
      isChecked: !this.state.isChecked
    })
  };

  render() {
    return(
      <div className={`checkbox-component ${this.state.isChecked ? "checked" : ""}`} onClick={this.checkboxHandler}>
        <div className="checkbox"/>
        {this.props.label ? <span>{this.props.label}</span> : null}
      </div>
    );
  }
}

export default Checkbox;