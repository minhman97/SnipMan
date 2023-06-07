import { baseUrl } from "./ApiEndpoint";
import { GetErrorMessage } from "./StatusCode";
import { getUserToken } from "./UserApi";

export const getProgramingLanguages = async () => {
  const token = getUserToken();
  return await fetch(`${baseUrl}ProgramingLanguage/GetLanguage`, {
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
