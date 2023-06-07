import { removeToken } from "./UserApi";

export const GetErrorMessage = (statusCode) => {
  switch (statusCode) {
    case 400:
      return "Bad request";
    case 404:
      return "Not found";
    case 501:
      return "Not implemented";
    case 401:
        removeToken();
        return "Unauthorized"
    default:
      return "Interner server error";
  }
};