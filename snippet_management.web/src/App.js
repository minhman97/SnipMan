import { useEffect, useState } from "react";
import Layout from "./components/Layout";
import useToken from "./hooks/useToken";
import SearchBar from "./components/SearchBar";
import MenuBar from "./components/MenuBar";
import { SnippetText } from "./components/SnippetText";
import { getSnippets } from "./api/snippetApi";
import SnippetCommands from "./components/SnippetCommands";
import SnippetList from "./components/SnippetList";

function App() {
  const { token, setToken } = useToken();
  const [snippet, setSnippet] = useState({});
  const [snippets, setSnippets] = useState([]);
  const [popupObject, setPopupObject] = useState(undefined);
  const pageSize = 7;

  const [currentCursor, setCurrentCursor] = useState(0);
  const [rangeObject, setRangeObject] = useState({
    startIndex: 0,
    endIndex: pageSize - 1,
  });
  const [sortOrder, setSortOrder] = useState({
    sortProperty: "created",
    orderWay: "desc",
    clicked: false,
  });

  useEffect(() => {
    if (token) {
      (async () => {
        if (snippets.length === 0 || currentCursor + 3 >= snippets.length || sortOrder.clicked) {
          let rangeData = await getSnippets(
            token,
            rangeObject.startIndex,
            rangeObject.endIndex,
            sortOrder.sortProperty,
            sortOrder.orderWay
          );
          if (snippets.length === 0) {
            setSnippets(rangeData.data);
          } else {
            let tempData = snippets;
            setSnippets(tempData.concat(rangeData.data));
          }

          setSnippet(rangeData.data[currentCursor]);
        }
      })();
      return () => {};
    }
  }, [currentCursor, sortOrder]);
  return (
    <Layout
      token={token}
      setToken={setToken}
      popupObject={popupObject}
      snippetId={snippet !== undefined ? snippet.id : undefined}
    >
      <SearchBar />
      <MenuBar>
        <SnippetText
          snippet={snippet}
          setSnippet={setSnippet}
          token={token}
        ></SnippetText>
      </MenuBar>
      <SnippetCommands
        setPopupObject={setPopupObject}
        sortOrder={sortOrder}
        setSortOrder={setSortOrder}
        setSnippets={setSnippets}
        setSnippet={setSnippet}
        setRangeObject={setRangeObject}
        pageSize={pageSize}
        setCurrentCursor={setCurrentCursor}
        currentCursor={currentCursor}
        token={token}
      />
      <SnippetList
        snippet={snippet}
        snippets={snippets}
        setRangeObject={setRangeObject}
        pageSize={pageSize}
        currentCursor={currentCursor}
        setCurrentCursor={setCurrentCursor}
        setSnippet={setSnippet}
      />
    </Layout>
  );
}

export default App;
