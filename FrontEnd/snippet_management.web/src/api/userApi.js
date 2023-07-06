import { authUrl, externalAuthUrl } from "./apiEndpoint";

export const login = (credentials, isExternal) => {
  return fetch(isExternal ? externalAuthUrl : authUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(credentials),
  });
};
