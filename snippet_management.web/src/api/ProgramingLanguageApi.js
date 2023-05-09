import { GetErrorMessage, baseUrl } from "./StatusCode";

export const getProgramingLanguages = async (token) => {
    return await fetch(
        `${baseUrl}ProgramingLanguage/GetLanguage`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          },
        }
      )
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
}