export const login = (credentials) =>{
    return fetch("https://localhost:44395/Authentication", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(credentials),
  }).then((data) => data.json());
}