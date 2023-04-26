import React, { useState } from "react";
import { updateSnippet } from "../api/SnippetApi";
import { toast } from "react-toastify";
import {
  Bars3BottomLeftIcon,
  DocumentArrowDownIcon,
  PencilIcon,
  XMarkIcon,
} from "@heroicons/react/24/solid";

export const SnippetTextArea = ({ snippet, setSnippet, priviousSnippetContent,token }) => {
  const [statusSnippetTextArea, setStatusSnippetTextArea] =
    useState("edit_status");
  const getBtnControlSnippetTextArea = (status) => {
    let btn = "";
    switch (status) {
      case "exit_status":
        btn = (
          <button
            className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm absolute mt-5 mr-5 right-0 hover:bg-slate-700"
            onClick={() => {
              setStatusSnippetTextArea("edit_status");
            }}
          >
            <XMarkIcon className="h-6 w-6 text-white-500" />
          </button>
        );
        break;
      case "save_status":
        btn = (
          <button
            className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm absolute mt-5 mr-5 right-0 hover:bg-slate-700"
            onClick={async () => {
              setStatusSnippetTextArea("edit_status");
              var res = await updateSnippet(token, snippet);
              if (res.status === 200) {
                toast.success("Snippet updated successfully", {
                  position: "top-center",
                  autoClose: 2000,
                  hideProgressBar: false,
                  closeOnClick: true,
                  pauseOnHover: true,
                  draggable: false,
                  theme: "light",
                });
              } else {
                setSnippet({ ...snippet, content: priviousSnippetContent });
                toast.error(
                  `Something wrong cannot save snippet. Error status: ${res.status}`,
                  {
                    position: "top-center",
                    autoClose: 2000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: false,
                    theme: "light",
                  }
                );
              }
            }}
          >
            <DocumentArrowDownIcon className="h-6 w-6 text-white-500" />
          </button>
        );
        break;
      default:
        btn = (
          <button
            className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm absolute right-6 top-6 hover:bg-slate-700"
            onClick={() => {
              setStatusSnippetTextArea("exit_status");
            }}
          >
            <PencilIcon className="h-6 w-6 text-white-500" />
          </button>
        );
        break;
    }
    return btn;
  };

  return (
    <div className="relative">
      <textarea
        className="bg-slate-950 w-full min-h-[65vh] rounded-lg"
        value={snippet.content}
        {...(statusSnippetTextArea !== "exit_status" &&
        statusSnippetTextArea !== "save_status"
          ? { readOnly: true }
          : {})}
        onChange={(e) => {
          setSnippet({ ...snippet, content: e.target.value });
          setStatusSnippetTextArea("save_status");
        }}
      ></textarea>
      {getBtnControlSnippetTextArea(statusSnippetTextArea)}
      <button
        className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm absolute right-6 bottom-6 hover:bg-slate-700"
        onClick={() => {}}
      >
        <Bars3BottomLeftIcon className="h-6 w-6 text-white-500" />
      </button>
    </div>
  );
};
