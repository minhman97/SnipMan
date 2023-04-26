const baseUrl = "https://localhost:44395/";

export const getSnippets = async (token, startIndex, endIndex, sortProperty, orderWay) => {
  let res = await fetch(`${baseUrl}Snippet?startIndex=${startIndex}&endIndex=${endIndex}&property=${sortProperty}&orderWay=${orderWay}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  });

  return await res.json();
};

export const createSnippet = async (token, snippet) =>{
  return await fetch("https://localhost:44395/Snippet", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
      body: JSON.stringify(snippet),
    }).catch((err) => {
      console.log(err.message);
   });
  };


export const updateSnippet = async (token, snippet) => {
  let res = await fetch(baseUrl + "Snippet/" + snippet.id, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
    body: JSON.stringify(snippet),
  });

  return await res;
};

export const deleteSnippet = async (token, id) => {
  let res = await fetch(baseUrl + "Snippet?id=" + id, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  });

  return await res.status;
};

export const searchSnippet = async (token, startIndex, endIndex, keyWord, sortProperty, orderWay) => {
  let url = `${baseUrl}Snippet/SearchRange?startIndex=${startIndex}&endIndex=${endIndex}&keyWord=${keyWord}&property=${sortProperty}&orderWay=${orderWay}`;
  let res = await fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
    },
  });

  return await res.json();
};
