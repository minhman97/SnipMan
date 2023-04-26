import React from "react";

const AddSnippetButton = () => {
  return (
    <div>
      <a
        href="/Snippet/Create"
        type="button"
        className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm"        
      >
        Create from scratch
      </a>
    </div>
  );
};

export default AddSnippetButton;
