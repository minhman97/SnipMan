const keyToken = "token";

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
