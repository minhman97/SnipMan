export const baseUrl = process.env.REACT_APP_BASE_URL;

export const authUrl = `${baseUrl}/Authentication`;
export const externalAuthUrl = `${authUrl}/External`;

export const programingLanguageUrl = `${baseUrl}/ProgramingLanguage`;

export const getSnippetUrl = (startIndex, endIndex, sortProperty, orderWay) => {
  return `${baseUrl}/Snippet?startIndex=${startIndex}&endIndex=${endIndex}&property=${sortProperty}&orderWay=${orderWay}`;
};

export const searchSnippetUrl = (
  filterKeyWord,
  startIndex,
  endIndex,
  sortProperty,
  orderWay
) => {
  return `${baseUrl}/Snippet/Search?keyWord=${filterKeyWord.trim()}&startIndex=${startIndex}&endIndex=${endIndex}&property=${sortProperty}&orderWay=${orderWay}`;
};

export const createSnippetUrl = `${baseUrl}/Snippet`;

export const updateSnippetUrl = (id) => {
  return `${baseUrl}/Snippet/${id}`;
};

export const deleteSnippetUrl = (id) => {
  return `${baseUrl}/Snippet?id=${id}`;
};
