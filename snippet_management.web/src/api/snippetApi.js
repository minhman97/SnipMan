const baseUrl = "https://localhost:44395/";

export const getSnippets = async (token, startIndex, endIndex, sortProperty, orderWay) => {
  var res = await fetch(`${baseUrl}Snippet/GetRange?startIndex=${startIndex}&endIndex=${endIndex}&property=${sortProperty}&orderWay=${orderWay}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  });

  return await res.json();
};

export const updateSnippet = async (token, snippet) => {
  var res = await fetch(baseUrl + "Snippet/" + snippet.id, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
    body: JSON.stringify(snippet),
  });

  return await res.json();
};

export const deleteSnippet = async (token, id) => {
  var res = await fetch(baseUrl + "Snippet?id=" + id, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  });

  return await res.status;
};
