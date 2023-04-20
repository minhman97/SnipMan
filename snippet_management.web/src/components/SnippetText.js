import React, { useState } from "react";
import { updateSnippet } from "../api/snippetApi";
import { toast } from "react-toastify";
import {
    Bars3BottomLeftIcon,
  DocumentArrowDownIcon,
  PencilIcon,
  XMarkIcon,
} from "@heroicons/react/24/solid";

export const SnippetText = ({ snippet, setSnippet, token }) => {
  const [statusSnippetText, setStatusSnippetText] = useState("edit");

  const getBtnControlSnippetText = (status) => {
    let btn = "";
    switch (status) {
      case "exit":
        btn = (
          <button
            className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm absolute mt-5 mr-5 right-0 hover:bg-slate-700"
            onClick={() => {
              setStatusSnippetText("edit");
            }}
          >
            <XMarkIcon className="h-6 w-6 text-white-500" />
          </button>
        );
        break;
      case "save":
        btn = (
          <button
            className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm absolute mt-5 mr-5 right-0 hover:bg-slate-700"
            onClick={async () => {
              setStatusSnippetText("edit");
              await updateSnippet(token, snippet);
              toast.success("Snippet updated successfully", {
                position: "top-center",
                autoClose: 2000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: false,
                theme: "light",
              });
            }}
          >
            <DocumentArrowDownIcon className="h-6 w-6 text-white-500" />
          </button>
        );
        break;
      default:
        btn = (
          <button
            className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm absolute mt-5 mr-5 right-0 hover:bg-slate-700"
            onClick={() => {
              setStatusSnippetText("exit");
            }}
          >
            <PencilIcon className="h-6 w-6 text-white-500" />
          </button>
        );
        break;
    }
    return btn;
  };

  var btnControlSnippetText = getBtnControlSnippetText(statusSnippetText);

//   console.log(snippet);
  return (
    <>
      {btnControlSnippetText}
      <textarea
        className="bg-slate-950  w-full min-h-full rounded-lg"
        defaultValue={snippet.content}
        {...(statusSnippetText !== "exit" && statusSnippetText !== "save"
          ? { readOnly: true }
          : {})}
        onChange={(e) => {
          setSnippet({ ...snippet, content: e.target.value });
          setStatusSnippetText("save");
        }}
      ></textarea>
      <button
        className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm absolute mt-5 mr-5 right-0 bottom-3 hover:bg-slate-700"
        onClick={() => {
        }}
      >
        <Bars3BottomLeftIcon className="h-6 w-6 text-white-500" />
      </button>
    </>
  );
};
