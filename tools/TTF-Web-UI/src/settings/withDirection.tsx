import React, {Component} from "react";

const rtl = document.getElementsByTagName('html')[0].getAttribute('dir');
const withDirection = (component : Component, props: any) => {
  return <Component {...props} rtl={rtl} />;
};

export default withDirection;
export { rtl };
