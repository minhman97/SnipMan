import React from "react";

export const RectangleShape = ({name, ...props}) => {
  return (
    <div {...props}>
      <div>{name}</div>
      <div></div>
    </div>
  );
};
