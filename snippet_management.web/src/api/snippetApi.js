import { GetErrorMessage } from "./StatusCode";
const baseUrl = "https://localhost:44395/";

export const getSnippets = async (
  token,
  startIndex,
  endIndex,
  sortProperty,
  orderWay
) => {
  return await fetch(
    `${baseUrl}Snippet?startIndex=${startIndex}&endIndex=${endIndex}&property=${sortProperty}&orderWay=${orderWay}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
    }
  )
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

export const searchSnippet = async (
  token,
  startIndex,
  endIndex,
  keyWord,
  sortProperty,
  orderWay
) => {
  let url = `${baseUrl}Snippet/Search?startIndex=${startIndex}&endIndex=${endIndex}&keyWord=${keyWord}&property=${sortProperty}&orderWay=${orderWay}`;
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
