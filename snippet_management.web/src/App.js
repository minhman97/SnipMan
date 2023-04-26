import { useEffect, useState } from "react";
import Layout from "./components/Layout";
import useToken from "./hooks/useToken";
import { getSnippets, searchSnippet } from "./api/snippetApi";
import { SnippetTextArea } from "./components/SnippetTextArea";

function App() {
  const [token, setToken] = useToken();
  const [snippet, setSnippet] = useState({});
  const [snippets, setSnippets] = useState({
    data: [],
    totalRecords: 0,
  });
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
  const [filterKeyWord, setFilterKeyWord] = useState("");

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
              filterKeyWord,
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
      snippets={snippets}
      currentCursor={currentCursor}
      setCurrentCursor={setCurrentCursor}
      setSnippets={setSnippets}
      setSnippet={setSnippet}
      sortOrder={sortOrder}
      rangeObject={rangeObject}
      setRangeObject={setRangeObject}
      setFilterKeyWord={setFilterKeyWord}
      pageSize={pageSize}
      snippet={snippet}
      slidesPerView={slidesPerView}
    >
      <SnippetTextArea
        snippet={snippet}
        setSnippet={setSnippet}
        priviousSnippetContent={
          snippets.data.length > 0 ? snippets.data[currentCursor].content : snippet.content
        }
        token={token}
      ></SnippetTextArea>
    </Layout>
  );
}

export default App;
