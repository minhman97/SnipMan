const csharp = {
  defaultToken: "",
  tokenPostfix: ".cs",

  brackets: [
    { open: "{", close: "}", token: "delimiter.curly" },
    { open: "[", close: "]", token: "delimiter.square" },
    { open: "(", close: ")", token: "delimiter.parenthesis" },
    { open: "<", close: ">", token: "delimiter.angle" },
  ],

  keywords: [
    "extern",
    "alias",
    "using",
    "bool",
    "decimal",
    "sbyte",
    "byte",
    "short",
    "ushort",
    "int",
    "uint",
    "long",
    "ulong",
    "char",
    "float",
    "double",
    "object",
    "dynamic",
    "string",
    "assembly",
    "is",
    "as",
    "ref",
    "out",
    "this",
    "base",
    "new",
    "typeof",
    "void",
    "checked",
    "unchecked",
    "default",
    "delegate",
    "var",
    "const",
    "if",
    "else",
    "switch",
    "case",
    "while",
    "do",
    "for",
    "foreach",
    "in",
    "break",
    "continue",
    "goto",
    "return",
    "throw",
    "try",
    "catch",
    "finally",
    "lock",
    "yield",
    "from",
    "let",
    "where",
    "join",
    "on",
    "equals",
    "into",
    "orderby",
    "ascending",
    "descending",
    "select",
    "group",
    "by",
    "namespace",
    "partial",
    "class",
    "field",
    "event",
    "method",
    "param",
    "public",
    "protected",
    "internal",
    "private",
    "abstract",
    "sealed",
    "static",
    "struct",
    "readonly",
    "volatile",
    "virtual",
    "override",
    "params",
    "get",
    "set",
    "add",
    "remove",
    "operator",
    "true",
    "false",
    "implicit",
    "explicit",
    "interface",
    "enum",
    "null",
    "async",
    "await",
    "fixed",
    "sizeof",
    "stackalloc",
    "unsafe",
    "nameof",
    "when",
  ],

  namespaceFollows: ["namespace", "using"],

  parenFollows: [
    "if",
    "for",
    "while",
    "switch",
    "foreach",
    "using",
    "catch",
    "when",
  ],

  operators: [
    "=",
    "??",
    "||",
    "&&",
    "|",
    "^",
    "&",
    "==",
    "!=",
    "<=",
    ">=",
    "<<",
    "+",
    "-",
    "*",
    "/",
    "%",
    "!",
    "~",
    "++",
    "--",
    "+=",
    "-=",
    "*=",
    "/=",
    "%=",
    "&=",
    "|=",
    "^=",
    "<<=",
    ">>=",
    ">>",
    "=>",
  ],

  symbols: /[=><!~?:&|+\-*\/\^%]+/,

  // escape sequences
  escapes:
    /\\(?:[abfnrtv\\"']|x[0-9A-Fa-f]{1,4}|u[0-9A-Fa-f]{4}|U[0-9A-Fa-f]{8})/,

    linqFunctions:['Set', 'Include', 'ThenInclude', 'SingleOrDefaultAsync'],

  // The main tokenizer for our languages
  tokenizer: {
    root: [
      // identifiers and keywords
      [
        /\@?[a-zA-Z_]\w*/,
        {
          cases: {
            "@namespaceFollows": {// matches ["using", "namespace"]
              token: "keyword.$0", //$0 subsequent to @namespace
              next: "@namespace",
            },
            "@keywords": {
              token: "keyword.$0",
              next: "@qualified",
            },
            "@default": { token: "identifier", next: "@qualified" }, 
          },
        },
      ],

      // Function declarations (including parameters)
			[/\s*[a-zA-Z_][\w]*(?=\()/, { token: 'function', next: '@type' }],
			[/\s*[A-Z][a-zA-Z_]\w*\s*/, { token: 'objectclass', next: '@type' }],
			[/\s*[a-zA-Z_][\w]*(?=\.)/, { token: 'identified', next: '@functions' }],

      // whitespace
      { include: "@whitespace" },

      // delimiters and operators
      [
        /}/,
        {
          cases: {
            "$S2==interpolatedstring": {
              token: "string.quote",
              next: "@pop",
            },
            "$S2==litinterpstring": {
              token: "string.quote",
              next: "@pop",
            },
            "@default": "@brackets",
          },
        },
      ],
      [/[{}()\[\]]/, "@brackets"],
      [/[<>](?!@symbols)/, "@brackets"],
      [
        /@symbols/,
        {
          cases: {
            "@operators": "delimiter",
            "@default": "",
          },
        },
      ],

      // numbers
      [/[0-9_]*\.[0-9_]+([eE][\-+]?\d+)?[fFdD]?/, "number.float"],
      [/0[xX][0-9a-fA-F_]+/, "number.hex"],
      [/0[bB][01_]+/, "number.hex"], // binary: use same theme style as hex
      [/[0-9_]+/, "number"],

      // delimiter: after number because of .\d floats
      [/[;,.]/, "delimiter"],

      // strings
      [/"([^"\\]|\\.)*$/, "string.invalid"], // non-teminated string
      [/"/, { token: "string.quote", next: "@string" }],
      [/\$\@"/, { token: "string.quote", next: "@litinterpstring" }],
      [/\@"/, { token: "string.quote", next: "@litstring" }],
      [/\$"/, { token: "string.quote", next: "@interpolatedstring" }],

      // characters
      [/'[^\\']'/, "string"],
      [/(')(@escapes)(')/, ["string", "string.escape", "string"]],
      [/'/, "string.invalid"],

      // Documentation comments (XML-style)
      [/\/\/\/\s*(?:(?:[^<]*<\/?[^>]*>\s*)+)/, "comment.doc"],

      // Attributes (assuming they are in square brackets [])
      [/\[[^\[\]]*\]/, "attribute"],

      //[/\b(?:static\s+)?[a-zA-Z_]\w*\s+([a-zA-Z_]\w*)\s*\(/, "function"], // Method or Function name

      // Class or Struct method and function declaration
      //[/(\b(?:class|struct)\s+([a-zA-Z_]\w*)\s*{.*?(\b(?:void|[a-zA-Z_]\w*)\s+([a-zA-Z_]\w*)\s*\())/, ['support.type', 'identifier', null, 'function']],

      // Other method and function references (method or function calls)
      [/\b([a-zA-Z_]\w*)\s*\(/, 'function'], // Method or Function name followed by '('
    ],

    type: [
			[/[a-zA-Z_]\w*((?=\?)|(?=>)|(?=\s))/, {
				cases: {
					"@keywords": {
						token: "keyword.$0",
						next: "@qualified",
					},
					'@default': { token: 'objectclass' }
				}
			}],
			[/[<>()]/, 'delimiter.parenthesis'],
			["", "", "@pop"],
		],

		functions: [
			[/[a-zA-Z]\w*/, { cases:{
				'@linqFunctions': { token: 'function', next: '@type' },
				'@default': { token: '', next: '' }
			} }],
			[/[.()]/, { token: 'delimiter.parenthesis', next: '' }],
			["", "", "@pop"],
		],

    qualified: [
      [
        /[a-zA-Z_][\w]*/,
        {
          cases: {
            "@keywords": { token: "keyword.$0" },
            "@default": "identifier",  
          },
        },
      ],
      [/\./, "delimiter"],
      ["", "", "@pop"],
    ],

    namespace: [
      { include: "@whitespace" },
      [/[A-Z]\w*/, "namespace"],
      [/[\.=]/, "delimiter"],
      ["", "", "@pop"],
    ],

    comment: [
      [/[^\/*]+/, "comment"],
      // [/\/\*/,    'comment', '@push' ],    // no nested comments :-(
      ["\\*/", "comment", "@pop"],
      [/[\/*]/, "comment"],
    ],

    string: [
      [/[^\\"]+/, "string"],
      [/@escapes/, "string.escape"],
      [/\\./, "string.escape.invalid"],
      [/"/, { token: "string.quote", next: "@pop" }],
    ],

    litstring: [
      [/[^"]+/, "string"],
      [/""/, "string.escape"],
      [/"/, { token: "string.quote", next: "@pop" }],
    ],

    litinterpstring: [
      [/[^"{]+/, "string"],
      [/""/, "string.escape"],
      [/{{/, "string.escape"],
      [/}}/, "string.escape"],
      [/{/, { token: "string.quote", next: "root.litinterpstring" }],
      [/"/, { token: "string.quote", next: "@pop" }],
    ],

    interpolatedstring: [
      [/[^\\"{]+/, "string"],
      [/@escapes/, "string.escape"],
      [/\\./, "string.escape.invalid"],
      [/{{/, "string.escape"],
      [/}}/, "string.escape"],
      [/{/, { token: "string.quote", next: "root.interpolatedstring" }],
      [/"/, { token: "string.quote", next: "@pop" }],
    ],

    whitespace: [
      [/^[ \t\v\f]*#((r)|(load))(?=\s)/, "directive.csx"],
      [/^[ \t\v\f]*#\w.*$/, "namespace.cpp"],
      [/[ \t\v\f\r\n]+/, ""],
      [/\/\*/, "comment", "@comment"],
      [/\/\/.*$/, "comment"],
    ],
  },
};

export default csharp;
