import { ChevronDownIcon,MagnifyingGlassIcon, RectangleStackIcon, UserCircleIcon } from "@heroicons/react/24/solid";
import React from "react";

const SearchBar = () => {
  return (
    <div className="py-5 flex justify-center items-center">
        <button
        className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full relative h-10 left-10 shadow-sm hover:bg-slate-700 flex items-center"
        onClick={() => {}}
        title="Select search mode - Current: Blended"
      >
        <RectangleStackIcon className="h-6 w-6 text-white"/>
        <ChevronDownIcon className="h-4 w-4 text-white"/>
      </button>
        <button
        className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full relative h-10 left-16 shadow-sm hover:bg-slate-700 flex items-center"
        onClick={() => {}}
        title="Select search mode - Current: Blended"
      >
        <MagnifyingGlassIcon className="h-6 w-6 text-white"/>
        <ChevronDownIcon className="h-4 w-4 text-white"/>
      </button>
      <input
        type="text"
        className="rounded-full w-8/12 h-12 bg-slate-950 px-20"
        placeholder="Search anything..."
      />
      <button
        className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full relative right-12 h-10 shadow-sm hover:bg-slate-700"
        onClick={() => {}}
        title="Settings [ , ]"
      >
        <UserCircleIcon className="h-6 w-6 text-white"/>
      </button>
    </div>
  );
};

export default SearchBar;
