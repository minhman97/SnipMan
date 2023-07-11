const keyToken = "token";

export const getUserToken = () => {
  const tokenString = localStorage.getItem(keyToken);
  if (tokenString) {
    const userToken = JSON.parse(tokenString);
    return userToken?.token;
  }

  return null;
};
export const getUserId = () => {
  const tokenString = localStorage.getItem(keyToken);
  const userToken = JSON.parse(tokenString);
  return userToken?.userId;
};

export const removeToken = () => {
  window.location = "/";
  localStorage.removeItem(keyToken);
};
