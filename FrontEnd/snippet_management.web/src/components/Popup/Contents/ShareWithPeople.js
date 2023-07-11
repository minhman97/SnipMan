import React, { useEffect, useState } from "react";
import { getShareableId } from "../../../api/snippetApi";
import { useSnippetContext } from "../../../context/SnippetContext";
import { getUserId } from "../../../service/userService";
import { DocumentDuplicateIcon } from "@heroicons/react/24/outline";
import { useLocation } from "react-router-dom";

const ShareWithPeople = () => {
  const { snippet, shareableLink, setShareableLink } = useSnippetContext();

  useEffect(() => {
    (async () => {
      let res = await getShareableId(
        snippet.id,
        getUserId()
      );
      setShareableLink(
        `${process.env.REACT_APP_WEB_BASE_URL}/${res.shareableSnippet.userId}/${res.shareableSnippet.shareableId}`
      );
    })();
  }, []);

  return (
    <div className="bg-slate-950 px-4 py-2 rounded-full flex sm:my-0 text-white">
      <p>{shareableLink != undefined ? `${shareableLink.substring(0,45)}...`: `Loading...`}</p>
      <DocumentDuplicateIcon
        className="ml-2 h-6 w-6 text-white cursor-pointer"
        title="Copy link"
        onClick={() => {
          navigator.clipboard.writeText(shareableLink);
        }}
      />
    </div>
  );
};

export default ShareWithPeople;
