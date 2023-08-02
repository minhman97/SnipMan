import Layout from "../../components/Layout";
import { getSnippets } from "../../api/snippetApi";
import { useSnippetContext } from "../../context/SnippetContext";
import {
  pageSize,
  usePaginationContext,
} from "../../context/PaginationContext";
import { useDeleteSnippet, useUpdateSnippet } from "../../hooks/snippetHooks";
import { useInfiniteQuery } from "@tanstack/react-query";
import { Editor } from "../../components/Elements/Editor";

const Snippet = () => {
  const {
    currentCursor,
    setCurrentCursor,
    filterKeyWord,
  } = useSnippetContext();

  const { sortOrder, setSortOrder } = usePaginationContext();

  const { mutate: mutateDeleteSnippet } = useDeleteSnippet();

  const handleDeleteSnippet = (id) => {
    let cursor = currentCursor - 1 < 0 ? 0 : currentCursor - 1;

    mutateDeleteSnippet({ id });
    setCurrentCursor(cursor);
  };

  const handleSortSnippets = () => {
    setSortOrder({
      ...sortOrder,
      orderWay: sortOrder.orderWay === "asc" ? "desc" : "asc",
      sortProperty: "created",
    });
    setCurrentCursor(0);
  };

  const { data, error, fetchNextPage, refetch } = useInfiniteQuery(
    ["list-snippet", sortOrder, filterKeyWord],
    async ({ pageParam = 0 }) => {
      const res = await getSnippets(
        filterKeyWord,
        pageParam,
        pageParam + pageSize,
        sortOrder.sortProperty,
        sortOrder.orderWay
      );

      return res;
    },
    {
      getNextPageParam: (lastPage) => {
        return lastPage.endIndex < lastPage.totalRecords
          ? lastPage?.endIndex + 1
          : undefined;
      },
      refetchOnWindowFocus: false,
    }
  );

  if (error) {
    return (
      <div className="mt-5 flex justify-center">{`An error has occured: ${error.message}`}</div>
    );
  }
  return (
    <Layout
      pages={data?.pages}
      fetchNextPage={fetchNextPage}
      refetch={refetch}
      handleDeleteSnippet={handleDeleteSnippet}
      handleSortSnippets={handleSortSnippets}
    >
      <Editor></Editor>
    </Layout>
  );
};

export default Snippet;
