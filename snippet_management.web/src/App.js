import { useEffect, useState } from "react";
import Layout from "./components/Layout";
import useToken from "./hooks/useToken";
import SearchBar from "./components/SearchBar";
import MenuBar from "./components/MenuBar";
import { SnippetText } from "./components/SnippetText";
import { getSnippets, searchSnippet } from "./api/snippetApi";
import SnippetCommands from "./components/SnippetCommands";
import SnippetList from "./components/SnippetList";

function App() {
  const { token, setToken } = useToken();
  const [snippet, setSnippet] = useState({});
  const [snippets, setSnippets] = useState({
    data: [],
    totalRecords: 0,
  });
  const [popupObject, setPopupObject] = useState(undefined);
  const pageSize = 7;
  const slidesPerView = 7;

  const [currentCursor, setCurrentCursor] = useState(0);
  const [rangeObject, setRangeObject] = useState({
    startIndex: 0,
    endIndex: pageSize - 1,
    filter: false,
  });
  const [sortOrder, setSortOrder] = useState({
    sortProperty: "created",
    orderWay: "desc",
    clicked: false,
  });
  const [searKeyWord, setFilterKeyWork] = useState("");

  useEffect(() => {
    if (token) {
      (async () => {
        if (
          snippets.data.length === 0 ||
          (currentCursor + slidesPerView / 2 >= snippets.data.length &&
            currentCursor + slidesPerView / 2 < snippets.totalRecords) ||
          sortOrder.clicked
        ) {
          let rangeData = {};
          if (rangeObject.filter) {
            rangeData = await searchSnippet(
              token,
              rangeObject.startIndex,
              rangeObject.endIndex,
              searKeyWord,
              sortOrder.sortProperty,
              sortOrder.orderWay
            );
          } else {
            rangeData = await getSnippets(
              token,
              rangeObject.startIndex,
              rangeObject.endIndex,
              sortOrder.sortProperty,
              sortOrder.orderWay
            );
          }

          if (snippets.data.length === 0 || sortOrder.clicked) {
            setSnippets({
              ...snippets,
              data: rangeData.data,
              totalRecords: rangeData.totalRecords,
            });
            setSnippet(rangeData.data[currentCursor]);
          } else {
            setSnippets({
              ...snippets,
              data: snippets.data.concat(rangeData.data),
            });
          }
          setSortOrder({ ...sortOrder, clicked: false });
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
      setPopupObject={setPopupObject}
      snippetId={snippet !== undefined ? snippet.id : undefined}
      snippets={snippets}
      currentCursor={currentCursor}
      setCurrentCursor={setCurrentCursor}
      setSnippets={setSnippets}
      setSnippet={setSnippet}
    >
      <SearchBar
        token={token}
        sortOrder={sortOrder}
        snippets={snippets}
        setSnippets={setSnippets}
        setSnippet={setSnippet}
        rangeObject={rangeObject}
        setRangeObject={setRangeObject}
        setFilterKeyWork={setFilterKeyWork}
        setcurrentCurson={setCurrentCursor}
        pageSize={pageSize}
      />
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
        rangeObject={rangeObject}
        setRangeObject={setRangeObject}
        pageSize={pageSize}
        currentCursor={currentCursor}
        token={token}
      />
      <SnippetList
        snippet={snippet}
        snippets={snippets}
        rangeObject={rangeObject}
        setRangeObject={setRangeObject}
        pageSize={pageSize}
        currentCursor={currentCursor}
        setCurrentCursor={setCurrentCursor}
        setSnippet={setSnippet}
        slidesPerView={slidesPerView}
      />
    </Layout>
  );
}

export default App;
