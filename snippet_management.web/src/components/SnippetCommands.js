import React from "react";

const SnippetCommands = ({
  sortOrder,
  setSortOrder,
  rangeObject,
  setRangeObject,
  pageSize,
  currentCursor,
  setIsOpen
}) => {
  return (
    <div className="flex justify-center">
      <button
        type="button"
        className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-700"
        onClick={(e) => {
          setIsOpen(true);
        }}
      >
        Add
      </button>
      <button
        type="button"
        className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-700"
      >
        Discover
      </button>
      <button
        type="button"
        className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-700"
      >
        Paste
      </button>
      <button
        type="button"
        className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-700"
        onClick={async (e) => {
          setRangeObject({
            ...rangeObject,
            startIndex: 0,
            endIndex: currentCursor + pageSize,
          });
          setSortOrder({
            ...sortOrder,
            orderWay: sortOrder.orderWay === "asc" ? "desc" : "asc",
            sortProperty: "created",
            clicked: true,
          });
        }}
        title="Sort by Last Added"
      >
        Sort
      </button>
    </div>
  );
};

export default SnippetCommands;
