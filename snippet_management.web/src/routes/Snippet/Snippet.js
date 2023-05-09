import React, { useEffect } from "react";

import { SnippetTextArea } from "../../components/SnippetTextArea";
import { toast } from "react-toastify";
import { GetErrorMessage } from "../../api/StatusCode";
import Layout from "../../components/Layout";
import { getSnippets, searchSnippet } from "../../api/SnippetApi";
import { useSnippetContext } from "../../context/SnippetContext";
import { usePaginationContext } from "../../context/PaginationContext";
import useToken from "../../hooks/useToken";

const Snippet = () => {
  const {
    snippet,
    setSnippet,
    snippets,
    setSnippets,
    currentCursor,
    filterKeyWord,
  } = useSnippetContext();

  const { sortOrder, setSortOrder, rangeObject, slidesPerView } =
    usePaginationContext();
  const [token] = useToken();

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
    <Layout>
      <SnippetTextArea
        priviousSnippetContent={
          snippets.data.length > 0
            ? snippets.data[currentCursor].content
            : snippet.content
        }
      ></SnippetTextArea>
    </Layout>
  );
};

export default Snippet;
