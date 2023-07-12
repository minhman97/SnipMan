import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getShareableSnippet } from "../../api/snippetApi";
import { RectangleShape } from "../../components/Elements/RectangleShape";

const ShareableSnippet = () => {
  const { userId, shareableId } = useParams();
  const [shareableSnippet, setShareableSnippet] = useState(undefined);

  useEffect(() => {
    (async () => {
      setShareableSnippet(await getShareableSnippet(shareableId, userId));
    })();
  }, []);

  if (shareableSnippet === undefined) return <div>Loading...</div>;
  return (
    <div className="bg-slate-800 text-white min-h-screen">
      <div className="bg-slate-900 flex items-center h-12">
        <span>Snippet management</span>
      </div>
      <div className="py-8 px-40">
        <div className="text-3xl">{shareableSnippet.name}</div>
        <div className="mt-4">Shared by: {shareableSnippet.user.email}</div>
        <div className="mt-4 bg-slate-900 rounded-md p-6">
          {shareableSnippet.content}
        </div>
        <div className="mt-4 flex">
          <RectangleShape className= "w-1/2 bg-slate-900 rounded-md p-2" name="related link"></RectangleShape>
          <RectangleShape className= "w-1/2 bg-slate-900 rounded-md p-2 ml-4"></RectangleShape>
        </div>
      </div>
    </div>
  );
};

export default ShareableSnippet;
