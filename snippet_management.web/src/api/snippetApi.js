import { GetErrorMessage, baseUrl } from "./StatusCode";
import { pageSize } from "../context/PaginationContext";

export const getSnippets = async (
  token,
  filterKeyWord,
  cursorIndex,
  sortProperty,
  orderWay
) => {
  let url =
    filterKeyWord.trim() === ""
      ? `${baseUrl}Snippet?startIndex=${cursorIndex}&endIndex=${
          cursorIndex + pageSize
        }&property=${sortProperty}&orderWay=${orderWay}`
      : `${baseUrl}Snippet/Search?keyWord=${filterKeyWord.trim()}&startIndex=${cursorIndex}&endIndex=${
          cursorIndex + pageSize
        }&property=${sortProperty}&orderWay=${orderWay}`;
  return await fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  })
    .then((res) => {
      if (!res.ok)
        throw new Error(
          `StatusCode:${res.status}. ErrorMessage:${GetErrorMessage(
            res.status
          )}`,
          { cause: { status: res.status } }
        );
      return res.json();
    })
    .catch((err) => {
      console.error(err);
      return err.cause;
    });
};

export const createSnippet = async (token, snippet) => {
  return await fetch(`${baseUrl}Snippet`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
    body: JSON.stringify(snippet),
  })
    .then((res) => {
      if (!res.ok)
        throw new Error(
          `StatusCode:${res.status}. ErrorMessage:${GetErrorMessage(
            res.status
          )}`,
          { cause: { status: res.status } }
        );
      return res.json();
    })
    .catch((err) => {
      console.error(err);
      return err.cause;
    });
};

export const updateSnippet = async (token, snippet) => {
  return await fetch(baseUrl + "Snippet/" + snippet.id, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
    body: JSON.stringify(snippet),
  })
    .then((res) => {
      if (!res.ok)
        throw new Error(
          `StatusCode:${res.status}. ErrorMessage:${GetErrorMessage(
            res.status
          )}`,
          { cause: { status: res.status } }
        );
      return res.json();
    })
    .catch((err) => {
      console.error(err);
      return err.cause;
    });
};

export const deleteSnippet = async (token, id) => {
  return await fetch(baseUrl + "Snippet?id=" + id, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  })
    .then((res) => {
      if (!res.ok)
        throw new Error(
          `StatusCode:${res.status}. ErrorMessage:${GetErrorMessage(
            res.status
          )}`,
          { cause: { status: res.status } }
        );
      return res;
    })
    .catch((err) => {
      console.error(err);
      return err.cause;
    });
};
