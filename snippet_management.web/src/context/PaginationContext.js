import React, { createContext, useContext, useState } from "react";

const Context = createContext();

export const PaginationContext = ({ children }) => {
  const [sortOrder, setSortOrder] = useState({
    sortProperty: "created",
    orderWay: "desc",
    clicked: false,
  });

  const pageSize = 7;
  const slidesPerView = 7;

  const [rangeObject, setRangeObject] = useState({
    startIndex: 0,
    endIndex: pageSize - 1,
    filter: false,
  });

  return (
    <Context.Provider
      value={{
        sortOrder,
        setSortOrder,
        rangeObject,
        setRangeObject,
        pageSize,
        slidesPerView,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export const usePaginationContext = () => useContext(Context);
