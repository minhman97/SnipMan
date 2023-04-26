import { Menu, Transition } from "@headlessui/react";
import {
  ChevronDownIcon,
  MagnifyingGlassIcon,
  RectangleStackIcon,
  UserCircleIcon,
} from "@heroicons/react/24/solid";
import React, { Fragment, useState } from "react";
import { getSnippets, searchSnippet } from "../api/snippetApi";
import { searchTypes } from "../model/searchTypes";

const SearchBar = ({
  token,
  sortOrder,
  snippets,
  setSnippets,
  setSnippet,
  rangeObject,
  setRangeObject,
  setFilterKeyWork,
  setcurrentCurson,
  pageSize
}) => {
  const [searchType, setSearchType] = useState(searchTypes[0]);

  return (
    <div className="py-5 flex justify-center items-center">
      <button
        className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full relative h-10 left-10 shadow-sm hover:bg-slate-700 flex items-center"
        onClick={() => {}}
        title="Select search mode - Current: Blended"
      >
        <RectangleStackIcon className="h-6 w-6 text-white" />
        <ChevronDownIcon className="h-4 w-4 text-white" />
      </button>
      <div className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full relative h-10 left-16 shadow-sm hover:bg-slate-700 flex items-center">
        <Menu as="div" className="relative inline-block text-left">
          <div>
            <Menu.Button className="inline-flex justify-center items-center text-white ">
              <MagnifyingGlassIcon className="h-6 w-6 text-white" />
              <ChevronDownIcon className="h-4 w-4 text-white" />
            </Menu.Button>
          </div>
          <Transition
            as={Fragment}
            enter="transition ease-out duration-100"
            enterFrom="transform opacity-0 scale-95"
            enterTo="transform opacity-100 scale-100"
            leave="transition ease-in duration-75"
            leaveFrom="transform opacity-100 scale-100"
            leaveTo="transform opacity-0 scale-95"
          >
            <Menu.Items className="absolute w-[50rem] mt-2 origin-top-right divide-y divide-gray-100 rounded-md bg-slate-700 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none z-10">
              <div className="px-1 py-1 ">
                {searchTypes.map((item, i) => {
                  return (
                    <Menu.Item key={item.id}>
                      {({ active }) => (
                        <button
                          className={`${
                            active
                              ? "bg-gray-500"
                              : searchType.id === item.id
                              ? "bg-gray-600"
                              : ""
                          } text-white group flex w-full items-center justify-between rounded-md px-2 py-2 text-sm text-left`}
                          onClick={() => {
                            setSearchType(item);
                          }}
                        >
                          <div className="flex items-center">
                            <MagnifyingGlassIcon className="h-6 w-6 text-white" />
                            <div className="m-2">
                              <p>{item.name}</p>
                              <p className="text-gray-400">{item.detail}</p>
                            </div>
                          </div>
                          {i === 0 && (
                            <div className="bg-gray-900 rounded-md p-1">
                              <p>DEFAULT</p>
                            </div>
                          )}
                        </button>
                      )}
                    </Menu.Item>
                  );
                })}
              </div>
            </Menu.Items>
          </Transition>
        </Menu>
      </div>
      <input
        type="text"
        className="rounded-full w-8/12 h-12 bg-slate-950 px-20"
        placeholder={searchType.placeholder}
        onChange={async (e) => {
          let rangeData = {};
          if (e.target.value !== "") {
            rangeData = await searchSnippet(
              token,
              0,
              pageSize,
              e.target.value,
              sortOrder.sortProperty,
              sortOrder.orderWay
            );
            setRangeObject({
              ...rangeObject,
              startIndex: 0,
              endIndex: pageSize,
              filter: true,
            });
          } else {
            rangeData = await getSnippets(
              token,
              0,
              pageSize,
              sortOrder.sortProperty,
              sortOrder.orderWay
            );
            setRangeObject({
              ...rangeObject,
              startIndex: 0,
              endIndex: pageSize,
              filter: false,
            });
          }

          setSnippets({
            ...snippets,
            data: rangeData.data,
            totalRecords: rangeData.totalRecords,
          });
          setSnippet(rangeData.data.length > 0 ? rangeData.data[0] : {});
          setcurrentCurson(0);
          setFilterKeyWork(e.target.value);
          
        }}
      />
      <button
        className="p-2 font-semibold text-sm bg-cyan-500 text-white rounded-full relative right-12 h-10 shadow-sm hover:bg-slate-700"
        onClick={() => {}}
        title="Settings [ , ]"
      >
        <UserCircleIcon className="h-6 w-6 text-white" />
      </button>
    </div>
  );
};

export default SearchBar;
