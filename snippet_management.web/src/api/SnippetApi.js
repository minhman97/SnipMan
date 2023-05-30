import { GetErrorMessage, baseUrl } from "./StatusCode";
import { getUserToken } from "./UserApi";

export const getSnippets = async (
  filterKeyWord,
  startIndex,
  endIndex,
  sortProperty,
  orderWay
) => {
  let url =
    filterKeyWord.trim() === ""
      ? `${baseUrl}Snippet?startIndex=${startIndex}&endIndex=${endIndex}&property=${sortProperty}&orderWay=${orderWay}`
      : `${baseUrl}Snippet/Search?keyWord=${filterKeyWord.trim()}&startIndex=${startIndex}&endIndex=${endIndex}&property=${sortProperty}&orderWay=${orderWay}`;
  let token = getUserToken();
  return await fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  }).then((res) => {
    if (res.ok) return res.json();

    throw new Error(
      `StatusCode: ${res.status}. ErrorMessage: ${GetErrorMessage(res.status)}`,
      { cause: { status: res.status } }
    );
  });
};

export const createSnippet = async (snippet) => {
  let token = getUserToken();
  return await fetch(`${baseUrl}Snippet`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
    body: JSON.stringify(snippet),
  })
    .then((res) => {
      if (res.ok) return res.json();

      throw new Error(
        `StatusCode:${res.status}. ErrorMessage:${GetErrorMessage(res.status)}`,
        { cause: { status: res.status } }
      );
    })
    .catch((err) => {
      console.error(err);
      return err.cause;
    });
};

export const updateSnippet = async ({ snippet }) => {
  let token = getUserToken();
  return await fetch(baseUrl + "Snippet/" + snippet.id, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
    body: JSON.stringify(snippet),
  }).then((res) => {
    if (res.ok) return res.json();
    throw new Error(
      `StatusCode: ${res.status}. ErrorMessage: ${GetErrorMessage(res.status)}`,
      { cause: { status: res.status } }
    );
  });
};

export const deleteSnippet = async ({ id }) => {
  let token = getUserToken();
  return await fetch(baseUrl + "Snippet?id=" + id, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  }).then((res) => {
    if (res.ok) return res.json();
    throw new Error(
      `StatusCode: ${res.status}. ErrorMessage: ${GetErrorMessage(res.status)}`,
      { cause: { status: res.status } }
    );
  });
};
