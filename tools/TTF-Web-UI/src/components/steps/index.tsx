import * as React from 'react';

interface IProps {
  steps: number[],
  activeStep: number
}

interface IState {

}

class Steps extends React.Component<IProps, IState> {

  render() {
    const {steps, activeStep} = this.props;

    return(
      <div className={`road-wrapper rs-${activeStep}`}>
        {
          steps.map((stepId: number, index: number) => {
            const stepForView = index + 1;

            return(
              <div
                key={stepId}
                className={`road-step step-${stepForView}`}>
                <span>{stepForView}</span>
              </div>
            )
          })
        }
        <div className="road-line"/>
      </div>
    );
  }
}

export default Steps;