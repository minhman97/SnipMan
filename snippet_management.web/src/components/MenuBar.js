import { DocumentDuplicateIcon } from "@heroicons/react/24/outline";
import { AdjustmentsHorizontalIcon, DocumentTextIcon, EllipsisVerticalIcon, UserIcon } from "@heroicons/react/24/solid";
import React from "react";

const MenuBar = ({ children }) => {
  return (
    <div className="flex flex-row mx-10 min-h-[65vh]">
      <div className="w-full mx-5 relative">{children}</div>
      <div className="bg-slate-950 mx-5 rounded-full flex flex-col">
        <button
          className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
          onClick={() => {}}
          title="Settings [ , ]"
        >
          <UserIcon className="h-6 w-6 text-white" />
        </button>
        <button
          className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
          onClick={() => {}}
          title="Settings [ , ]"
        >
          <DocumentDuplicateIcon className="h-6 w-6 text-white" />
        </button>
        <button
          className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
          onClick={() => {}}
          title="Settings [ , ]"
        >
          <AdjustmentsHorizontalIcon className="h-6 w-6 text-white" />
        </button>
        <button
          className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
          onClick={() => {}}
          title="Settings [ , ]"
        >
          <DocumentTextIcon className="h-6 w-6 text-white" />
        </button>
        <button
          className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
          onClick={() => {}}
          title="Settings [ , ]"
        >
          <EllipsisVerticalIcon className="h-6 w-6 text-white" />
        </button>
      </div>
    </div>
  );
};

export default MenuBar;
