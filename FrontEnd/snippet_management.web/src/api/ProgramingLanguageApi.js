import { programingLanguageUrl } from "./apiEndpoint";
import { HandleStatuscode } from "../helper/statusCodeHelper";
import { getUserToken } from "../service/userService";

export const getProgramingLanguages = async () => {
  const token = getUserToken();
  return await fetch(programingLanguageUrl, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  })
    .then((res) => {
      if (!res.ok)
        throw new Error(
          `StatusCode:${res.status}. ErrorMessage:${HandleStatuscode(
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
