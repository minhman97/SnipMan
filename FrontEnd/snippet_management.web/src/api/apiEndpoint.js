export const baseUrl = import.meta.env.VITE_BASE_URL;
export const authUrl = `${baseUrl}/Authentication`;
export const externalAuthUrl = `${authUrl}/External`;

export const programingLanguageUrl = `${baseUrl}/ProgramingLanguage`;

export const getSnippetUrl = (
  filterKeyWord,
  startIndex,
  endIndex,
  sortProperty,
  orderWay
) => {
  return `${baseUrl}/Snippet${ filterKeyWord.trim() === "" ? "?" : `/Search?keyWord=${filterKeyWord.trim()}&` }startIndex=${startIndex}&endIndex=${endIndex}&property=${sortProperty}&orderWay=${orderWay}`;
};

export const createSnippetUrl = `${baseUrl}/Snippet`;

export const updateSnippetUrl = (id) => {
  return `${baseUrl}/Snippet/${id}`;
};

export const deleteSnippetUrl = (id) => {
  return `${baseUrl}/Snippet?id=${id}`;
};

export const getIconUrl = (url, language) => {
  if(language)
  {
    return `${baseUrl}/${url}/${language === "" ? "text": language}.png`;
  }
  
  return `${baseUrl}/${url}`;
};
