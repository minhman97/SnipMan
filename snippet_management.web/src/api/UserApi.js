export const login = (credentials, isExternal) => {
  let url = "https://localhost:44395/Authentication";
  if (isExternal) url = url.concat("/External");
  return fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(credentials),
  }).then((data) => data.json());
};
