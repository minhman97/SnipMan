import { authUrl, externalAuthUrl } from "./ApiEndpoint";
const keyToken = "token";

export const login = (credentials, isExternal) => {
  return fetch(isExternal ? externalAuthUrl : authUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(credentials),
  }).then((data) => data.json());
};

export const getUserToken = () => {
  const tokenString = localStorage.getItem(keyToken);
  if (tokenString) {
    const userToken = JSON.parse(tokenString);
    return userToken?.token;
  }

  return undefined;
};

export const removeToken = () => {
  window.location = "/";
  localStorage.removeItem(keyToken);
};
