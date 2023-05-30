import { baseUrl } from "./StatusCode";

export const login = (credentials, isExternal) => {
  let url = baseUrl.concat("Authentication");
  if (isExternal) url = url.concat("/External");
  return fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(credentials),
  }).then((data) => data.json());
};

export const getUserToken = () => {
  const tokenString = localStorage.getItem("token");
  if (tokenString) {
    const userToken = JSON.parse(tokenString);
    return userToken?.token;
  }

  return undefined;
};

export const removeToken = () => {
  window.location = "/";
  localStorage.removeItem("token");
};
