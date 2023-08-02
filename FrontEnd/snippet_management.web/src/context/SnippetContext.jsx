import React, { createContext, useContext, useState } from "react";
import { useUpdateSnippet } from "../hooks/snippetHooks";
const Context = createContext();

export const SnippetContext = ({ children }) => {
  const [snippet, setSnippet] = useState(undefined);
  const [currentCursor, setCurrentCursor] = useState(0);

  const [filterKeyWord, setFilterKeyWord] = useState("");
  const { mutate: mutateUpdateSnippet } = useUpdateSnippet();

  const handleUpdateSnippet = (updatingSnippet) => {
    setSnippet(updatingSnippet);
    mutateUpdateSnippet({ updatingSnippet });
  };

  return (
    <Context.Provider
      value={{
        snippet,
        setSnippet,
        currentCursor,
        setCurrentCursor,
        filterKeyWord,
        setFilterKeyWord,
        handleUpdateSnippet
      }}
    >
      {children}
    </Context.Provider>
  );
};

export const useSnippetContext = () => useContext(Context);
