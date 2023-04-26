import React, { useState } from "react";
import useToken from "../../hooks/useToken";
import { createSnippet } from "../../api/snippetApi";
import { Listbox, Transition } from "@headlessui/react";
import { langueges } from "../../data_model/languages";
import { Fragment } from "react";
import { CheckIcon, ChevronUpDownIcon } from "@heroicons/react/20/solid";

const CreateSnippet = () => {
  const [token] = useToken();
  const [name, setName] = useState("");
  const [content, setContent] = useState("");
  const [language, setLanguage] = useState(langueges[0]);
  const handleCreate = async (e) => {
    e.preventDefault();

    let snippet = {
      name: name,
      content: content,
      description: "",
      language: language,
      origin: "",
      tags: [],
    };

    var res = createSnippet(token, snippet);
    if (res.status === 200) window.location.href = "/";
  };

  return (
    <main className="text-white bg-slate-800 min-h-screen">
      <div className="mx-8">
        <h1 className="text-2xl">Create a snippet</h1>
        <div>
          <textarea
            id="content"
            name="content"
            className="bg-slate-950 h-96 w-full"
            onChange={(e) => setContent(e.target.value)}
          ></textarea>
        </div>
        <div className="flex justify-center mt-5">
          <div className="w-52">
            <Listbox value={language} onChange={setLanguage}>
              <div className="relative mt-1">
                <Listbox.Button className="relative w-full cursor-default rounded-full h-12 bg-slate-950 text-white py-2 pl-3 pr-10 text-left shadow-md focus:outline-none focus-visible:border-indigo-500 focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75 focus-visible:ring-offset-2 focus-visible:ring-offset-orange-300 sm:text-sm">
                  <span className="block truncate text-center">{language}</span>
                  <span className="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-2">
                    <ChevronUpDownIcon
                      className="h-5 w-5 text-gray-400"
                      aria-hidden="true"
                    />
                  </span>
                </Listbox.Button>
                <Transition
                  as={Fragment}
                  leave="transition ease-in duration-100"
                  leaveFrom="opacity-100"
                  leaveTo="opacity-0"
                >
                  <Listbox.Options className="absolute mt-1 max-h-60 w-full overflow-auto rounded-md bg-slate-700  py-1 text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm">
                    {langueges.map((languege, i) => (
                      <Listbox.Option
                        key={i}
                        className={({ active }) =>
                          `relative cursor-default select-none py-2 pl-10 pr-4 ${
                            active
                              ? "bg-slate-400 text-amber-900"
                              : "text-white"
                          }`
                        }
                        value={languege}
                      >
                        {({ selected }) => (
                          <>
                            <span
                              className={`block truncate ${
                                selected ? "font-medium" : "font-normal"
                              }`}
                            >
                              {languege}
                            </span>
                            {selected ? (
                              <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-amber-600">
                                <CheckIcon
                                  className="h-5 w-5"
                                  aria-hidden="true"
                                />
                              </span>
                            ) : null}
                          </>
                        )}
                      </Listbox.Option>
                    ))}
                  </Listbox.Options>
                </Transition>
              </div>
            </Listbox>
          </div>
        </div>
        <div className="flex justify-center mt-5">
          <input
            id="name"
            name="name"
            placeholder="Name your snippet..."
            className="rounded-full h-12 bg-slate-950 px-5"
            onChange={(e) => setName(e.target.value)}
          />
        </div>
        <div className="mt-5">
          <button
            type="button"
            className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm"
            onClick={handleCreate}
          >
            Create
          </button>
        </div>
      </div>
    </main>
  );
};

export default CreateSnippet;
