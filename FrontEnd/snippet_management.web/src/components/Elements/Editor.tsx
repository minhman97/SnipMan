import { useRef, useState, useEffect } from "react";
import * as monaco from "monaco-editor/esm/vs/editor/editor.api";
import React from "react";
import { useSnippetContext } from "../../context/SnippetContext";
import {
  Bars3BottomLeftIcon,
  DocumentArrowDownIcon,
  PencilIcon,
  XMarkIcon,
} from "@heroicons/react/24/solid";
import "../../service/userWorker";

export function Editor() {
  const { snippet, setSnippet, handleUpdateSnippet } = useSnippetContext();
  const [editor, setEditor] =
    useState<monaco.editor.IStandaloneCodeEditor | null>(null);
  const monacoEl = useRef(null);
  const [isEditable, setIsEditable] = useState(0);
  const [value, setValue] = useState("");

  useEffect(() => {
    if (monacoEl) {
      setEditor((editor) => {
        if (editor) {
          let model = editor.getModel();
          if (model) {
            monaco.editor.setModelLanguage(
              model,
              getLanguageMatchingEditor(snippet?.language)
            );
            if (isEditable == 0 && snippet) model.setValue(snippet.content);

            editor.updateOptions({
              readOnly: !isEditable,
            });            
          }
          return editor;
        }

        let editorCreated = monaco.editor.create(monacoEl.current!, {
          value: getCodes(),
          language: "csharp",
          theme: "myCustomTheme",
        });

        editorCreated.onDidChangeModelContent(() => {
          setValue(editorCreated.getValue());
        });

        return editorCreated;
      });
    }

    return () => {};
  }, [snippet, isEditable, value]);

  const getLanguageMatchingEditor = (snippetLanguage) => {
    switch (snippetLanguage) {
      case "c-sharp":
        return "csharp";
      default:
        return "javascript";
    }
  };

  const getBtnControlSnippetTextArea = () => {
    switch (isEditable) {
      case 1:
        if (value != snippet.content)
          return (
            <button
              className="absolute right-8 top-8 rounded-full  bg-slate-700 p-2 text-sm font-semibold text-white shadow-sm hover:bg-slate-700"
              onClick={async () => {
                setIsEditable(0);
                handleUpdateSnippet({ ...snippet, content: value });
              }}
            >
              <DocumentArrowDownIcon className="text-white-500 h-6 w-6" />
            </button>
          );
        return (
          <button
            className="absolute right-8 top-8 rounded-full  bg-slate-700 p-2 text-sm font-semibold text-white shadow-sm hover:bg-slate-700"
            onClick={() => {
              setIsEditable(0);
            }}
          >
            <XMarkIcon className="text-white-500 h-6 w-6" />
          </button>
        );
      default:
        return (
          <button
            className="absolute right-8 top-8 rounded-full  bg-slate-700 p-2 text-sm font-semibold text-white shadow-sm hover:bg-slate-700"
            onClick={() => {
              setIsEditable(1);
            }}
          >
            <PencilIcon className="text-white-500 h-6 w-6" />
          </button>
        );
    }
  };


  const getCodes = () => {
    return [
      "public class UserController: ControllerBase\r\n{\r\n    private readonly IUnitOfWork _unitOfWork;\r\n    public UserController(IUnitOfWork unitOfWork)\r\n    {\r\n        _unitOfWork = unitOfWork;\r\n    }\r\n    [HttpPost]\r\n    public async Task<IActionResult> Create(UserViewModel model)\r\n    {\r\n        if (!ModelState.IsValid)\r\n            return BadRequest(ModelState);\r\n            \r\n        return Ok(await _unitOfWork.UserRepository.Create(new CreateUserRequest(model.Email, model.Password, null)));\r\n    }\r\n}"
    ].join("\n");
  }


  return (
    <div className="relative">
      <div className="h-[calc(100vh-20rem)] rounded-lg">
        <div className="h-full w-full" ref={monacoEl}></div>
      </div>
      {getBtnControlSnippetTextArea()}
      <button className="absolute bottom-14 right-8 rounded-full  bg-slate-700 p-2 text-sm font-semibold text-white shadow-sm hover:bg-slate-700">
        <Bars3BottomLeftIcon className="text-white-500 h-6 w-6" />
      </button>
    </div>
  );
}
