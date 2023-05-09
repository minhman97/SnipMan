import React, { createContext, useContext, useState } from "react";

const Context = createContext();

export const SnippetContext = ({ children }) => {
  const [snippet, setSnippet] = useState({});
  const [snippets, setSnippets] = useState({
    data: [],
    totalRecords: 0,
  });
  const [currentCursor, setCurrentCursor] = useState(0);

  const [filterKeyWord, setFilterKeyWord] = useState("");

  return (
    <Context.Provider
      value={{
        snippet,
        setSnippet,
        snippets,
        setSnippets,
        currentCursor,
        setCurrentCursor,
        filterKeyWord,
        setFilterKeyWord,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export const useSnippetContext = () => useContext(Context);
