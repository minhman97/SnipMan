import * as React from "react";

interface Props extends React.HTMLAttributes<HTMLButtonElement>, React.ButtonHTMLAttributes<HTMLButtonElement> {
  //other property
}

const Button = ({ ...props }: Props) => {
  return (<button {...props}>{props.children}</button>);
};

export default Button;
