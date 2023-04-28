import React, { createContext, useContext, useState } from "react";
import useToken from "../hooks/useToken";

const Context = createContext();

export const SnippetContext = ({ children }) => {
  const [token] = useToken();
  const [snippet, setSnippet] = useState({});
  const [snippets, setSnippets] = useState({
    data: [],
    totalRecords: 0,
  });
  const [currentCursor, setCurrentCursor] = useState(0);
  const [sortOrder, setSortOrder] = useState({
    sortProperty: "created",
    orderWay: "desc",
    clicked: false,
  });

  const [filterKeyWord, setFilterKeyWord] = useState("");


  const pageSize = 7;
  const slidesPerView = 7;
  
  const [rangeObject, setRangeObject] = useState({
    startIndex: 0,
    endIndex: pageSize - 1,
    filter: false,
  });

  return (
    <Context.Provider value={{ token, snippet, setSnippet, snippets, setSnippets, currentCursor, setCurrentCursor, sortOrder, setSortOrder, rangeObject, setRangeObject, filterKeyWord, setFilterKeyWord, pageSize, slidesPerView }}>
      {children}
    </Context.Provider>
  );
};

export const useSnippetContext = () => useContext(Context);
