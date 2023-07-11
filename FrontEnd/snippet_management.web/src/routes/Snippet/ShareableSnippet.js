import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom';
import { getShareableSnippet } from '../../api/snippetApi';

const ShareableSnippet = () => {
  const { userId, shareableId } = useParams();
  const [shareableSnippet, setShareableSnippet] = useState(undefined);
  
  useEffect(()=>{
    (async ()=>{
      setShareableSnippet(await getShareableSnippet(shareableId, userId));
    })()
  },[]);

  if(shareableSnippet === undefined)
    return (<div>Loading...</div>)
  return (
    <div>Shareable Snippet: {shareableSnippet.name}</div>
  )
}

export default ShareableSnippet;
