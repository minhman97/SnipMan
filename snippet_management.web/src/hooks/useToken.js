import { useState } from "react";
import jwt_decode from 'jwt-decode'

export default function useToken() {
  const getToken = () => {
    const tokenString = localStorage.getItem("token");
    if(tokenString === undefined || tokenString === null) return undefined;
    const userToken = JSON.parse(tokenString);
    let expDate = jwt_decode(tokenString).exp
    let currentDate = Math.trunc(new Date().getTime()/1000);
    if(expDate < currentDate)
    {
      localStorage.removeItem("token");
      window.location = "/";
      return undefined; 
    }
    return userToken?.token;
  };

  const [token, setToken] = useState(getToken());

  const saveToken = (userToken) => {
    localStorage.setItem("token", JSON.stringify(userToken));
    setToken(userToken.token);
  };

  return [token, saveToken];
}
