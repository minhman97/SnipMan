import React, { useRef, useState } from "react";
import {
  Bars3BottomLeftIcon,
  DocumentArrowDownIcon,
  PencilIcon,
  XMarkIcon,
} from "@heroicons/react/24/solid";
import { useSnippetContext } from "../context/SnippetContext";

export const SnippetTextArea = ({ handleUpdateSnippet }) => {
  const { snippet, setSnippet } = useSnippetContext();
  const [statusSnippetTextArea, setStatusSnippetTextArea] =
    useState("edit_status");
  const refTextArea = useRef();
  const refEditor = useRef();
  const getBtnControlSnippetTextArea = (status) => {
    let btn = "";
    switch (status) {
      case "exit_status":
        btn = (
          <button
            className="p-2 font-semibold text-sm bg-slate-700  text-white rounded-full shadow-sm absolute right-8 top-8 hover:bg-slate-700"
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
            className="p-2 font-semibold text-sm bg-slate-700  text-white rounded-full shadow-sm absolute right-8 top-8 hover:bg-slate-700"
            onClick={async () => {
              setStatusSnippetTextArea("edit_status");
              handleUpdateSnippet();
            }}
          >
            <DocumentArrowDownIcon className="h-6 w-6 text-white-500" />
          </button>
        );
        break;
      default:
        btn = (
          <button
            className="p-2 font-semibold text-sm bg-slate-700  text-white rounded-full shadow-sm absolute right-8 top-8 hover:bg-slate-700"
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
      <div className="h-[65vh] overflow-auto">
        <div ref={refEditor} className="textarea-container">
          <div className="line-numbers">
            {snippet !== undefined &&
              snippet.content !== undefined &&
              snippet.content.split("\n").map((v, i) => {
                return <span key={i}></span>;
              })}
          </div>
          <textarea
            ref={refTextArea}
            className="bg-slate-950 rounded-lg w-full"
            rows={28}
            value={snippet.content}
            {...(statusSnippetTextArea !== "exit_status" &&
            statusSnippetTextArea !== "save_status"
              ? { readOnly: true }
              : {})}
            onChange={(e) => {
              setSnippet({ ...snippet, content: e.target.value });
              setStatusSnippetTextArea("save_status");
            }}
            onKeyUp={(event) => {
              if (event.key === "Tab") {
                const start = event.target.selectionStart;
                const end = event.target.selectionEnd;

                event.target.value =
                  event.target.value.substring(0, start) +
                  "\t" +
                  event.target.value.substring(end);

                event.preventDefault();
              }
            }}
          ></textarea>
        </div>
      </div>
      {getBtnControlSnippetTextArea(statusSnippetTextArea)}
      <button className="p-2 font-semibold text-sm bg-slate-700  text-white rounded-full shadow-sm absolute right-8 bottom-14 hover:bg-slate-700">
        <Bars3BottomLeftIcon className="h-6 w-6 text-white-500" />
      </button>
    </div>
  );
};
