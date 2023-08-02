import {
  createSnippetUrl,
  deleteSnippetUrl,
  getSnippetUrl,
  updateSnippetUrl,
} from "./apiEndpoint";
import { HandleStatuscode } from "../helper/statusCodeHelper";
import { getUserToken } from "../service/userService";

export const getSnippets = async (
  filterKeyWord,
  startIndex,
  endIndex,
  sortProperty,
  orderWay
) => {
  const token = getUserToken();
  return await fetch(
    getSnippetUrl(filterKeyWord, startIndex, endIndex, sortProperty, orderWay),
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    }
  ).then((res) => {
    if (res.ok) return res.json();

    throw new Error(
      `StatusCode: ${res.status}. ErrorMessage: ${HandleStatuscode(
        res.status
      )}`,
      { cause: { status: res.status } }
    );
  });
};

export const createSnippet = async (snippet) => {
  const token = getUserToken();
  return await fetch(createSnippetUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(snippet),
  })
    .then((res) => {
      if (res.ok) return res.json();

      throw new Error(
        `StatusCode:${res.status}. ErrorMessage:${HandleStatuscode(
          res.status
        )}`,
        { cause: { status: res.status } }
      );
    })
    .catch((err) => {
      console.error(err);
      return err.cause;
    });
};

export const updateSnippet = async ({ updatingSnippet }) => {
  const token = getUserToken();
  return await fetch(updateSnippetUrl(updatingSnippet.id), {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(updatingSnippet),
  }).then((res) => {
    if (res.ok) return res.json();
    throw new Error(
      `StatusCode: ${res.status}. ErrorMessage: ${HandleStatuscode(
        res.status
      )}`,
      { cause: { status: res.status } }
    );
  });
};

export const deleteSnippet = async ({ id }) => {
  const token = getUserToken();
  return await fetch(deleteSnippetUrl(id), {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  }).then((res) => {
    if (res.ok) return res.json();
    throw new Error(
      `StatusCode: ${res.status}. ErrorMessage: ${HandleStatuscode(
        res.status
      )}`,
      { cause: { status: res.status } }
    );
  });
};
