import React, { createContext, useContext, useState } from "react";

const Context = createContext();

export const PopupContext = ({ children }) => {
    let [popup, setPopup] = useState({
        isOpen: false,
        title: "",
        content: ""
    });

  return (
    <Context.Provider
      value={{
        popup,
        setPopup,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export const usePopupContext = () => useContext(Context);
