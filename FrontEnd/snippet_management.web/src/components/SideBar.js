import { DocumentDuplicateIcon } from "@heroicons/react/24/outline";
import {
  AdjustmentsHorizontalIcon,
  DocumentTextIcon,
  EllipsisVerticalIcon,
} from "@heroicons/react/24/solid";
import React from "react";
import { usePopupContext } from "../context/PopupContext";
import ShareWithPeople from "./Popup/Contents/ShareWithPeople";
import { ShareIconOutline } from "./Icons/IconsOutline";

const SideBar = () => {
  const { popup, setPopup } = usePopupContext();

  return (
    <div className="bg-slate-950 m-5 rounded-full flex flex-row sm:flex-col sm:my-0">
      <button
        className="p-2 font-semibold text-sm bg-slate-900  text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
        title="Generate Shareable Link"
        onClick={async () => {
          setPopup({
            ...popup,
            isOpen: true,
            title: "Share with people",
            content: <ShareWithPeople></ShareWithPeople>,
          });
        }}
      >
        <ShareIconOutline className="h-6 w-6" />
      </button>
      <button
        className="p-2 font-semibold text-sm bg-slate-900  text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
        title="Settings [ , ]"
      >
        <DocumentDuplicateIcon className="h-6 w-6 text-white" />
      </button>
      <button
        className="p-2 font-semibold text-sm bg-slate-900  text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
        title="Settings [ , ]"
      >
        <AdjustmentsHorizontalIcon className="h-6 w-6 text-white" />
      </button>
      <button
        className="p-2 font-semibold text-sm bg-slate-900  text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
        title="Settings [ , ]"
      >
        <DocumentTextIcon className="h-6 w-6 text-white" />
      </button>
      <button
        className="p-2 font-semibold text-sm bg-slate-900  text-white rounded-full m-2 h-10 shadow-sm hover:bg-slate-700"
        title="Settings [ , ]"
      >
        <EllipsisVerticalIcon className="h-6 w-6 text-white" />
      </button>
    </div>
  );
};

export default SideBar;
