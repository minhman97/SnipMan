import React, { useEffect, useState } from "react";

import { SnippetTextArea } from "../../components/SnippetTextArea";
import { toast } from "react-toastify";
import { GetErrorMessage } from "../../api/StatusCode";
import Layout from "../../components/Layout";
import useToken from "../../hooks/useToken";
import { getSnippets, searchSnippet } from "../../api/SnippetApi";

const Snippet = () => {
  const [token] = useToken();
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

          if (rangeData.status && rangeData.status !== 200)
            return toast.error(GetErrorMessage(rangeData.status));

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
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentCursor, sortOrder]);
  return (
      <Layout
        token={token}
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
            snippets.data.length > 0
              ? snippets.data[currentCursor].content
              : snippet.content
          }
          token={token}
        ></SnippetTextArea>
      </Layout>
  );
};

export default Snippet;
