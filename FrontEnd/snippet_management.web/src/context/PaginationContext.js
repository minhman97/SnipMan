import React, { createContext, useContext, useState } from "react";

const Context = createContext();

export const pageSize = 7;
export const slidesPerView = 7;

export const PaginationContext = ({ children }) => {
  const [sortOrder, setSortOrder] = useState({
    sortProperty: "created",
    orderWay: "desc",
  });


  return (
    <Context.Provider
      value={{
        sortOrder,
        setSortOrder,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export const usePaginationContext = () => useContext(Context);
