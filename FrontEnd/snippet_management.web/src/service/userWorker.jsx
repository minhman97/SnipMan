import * as monaco from "monaco-editor";
import editorWorker from "monaco-editor/esm/vs/editor/editor.worker?worker";
import jsonWorker from "monaco-editor/esm/vs/language/json/json.worker?worker";
import cssWorker from "monaco-editor/esm/vs/language/css/css.worker?worker";
import htmlWorker from "monaco-editor/esm/vs/language/html/html.worker?worker";
import tsWorker from "monaco-editor/esm/vs/language/typescript/ts.worker?worker";
import csharp from "../utils/EditorLanguages/csharp";
// @ts-ignore
self.MonacoEnvironment = {
  getWorker(_, label) {
    if (label === "json") {
      return new jsonWorker();
    }
    if (label === "css" || label === "scss" || label === "less") {
      return new cssWorker();
    }
    if (label === "html" || label === "handlebars" || label === "razor") {
      return new htmlWorker();
    }
    if (label === "typescript" || label === "javascript") {
      return new tsWorker();
    }
    return new editorWorker();
  },
};
monaco.languages.typescript.typescriptDefaults.setEagerModelSync(true);

monaco.languages.register({ id: "csharp" });

// Register a tokens provider for the language
monaco.languages.setMonarchTokensProvider("csharp", csharp);

// Define a new theme that contains only rules that match this language
monaco.editor.defineTheme("myCustomTheme", {
  base: "vs-dark", // Inherit from 'vs-dark' theme
  inherit: true,
  rules: [   
    // Add more custom rules or modifications here

    { token: 'comment.doc', foreground: '408080' }, // Documentation comments (dark teal)
    { token: 'attribute', foreground: '800000' }, // Attributes (maroon)

    { token: 'namespace', foreground: '11f22b' }, // Function names (blue)
		{ token: 'objectclass', foreground: '11f22b' }, // Function names (blue)
    { token: 'function', foreground: 'e8b315' }, // Method and Function names (blue)
    { token: 'support.type', foreground: '800000' }, // Class or Struct names (maroon)
    // { token: 'identifier', foreground: '408080' }, // Other identifiers (black)    
    // { token: 'namespace', foreground: '0000ff' },
  ],
  colors: {},
});
