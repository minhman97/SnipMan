import React, { useEffect, useState } from "react";
import { createSnippet } from "../../api/snippetApi";
import { Listbox, Transition } from "@headlessui/react";
import { Fragment } from "react";
import { CheckIcon, ChevronUpDownIcon } from "@heroicons/react/20/solid";
import { HandleStatuscode } from "../../helper/statusCodeHelper";
import { toast } from "react-hot-toast";
import { getProgramingLanguages } from "../../api/programingLanguageApi";
import { baseUrl } from "../../api/apiEndpoint";

const CreateSnippet = () => {
  const [name, setName] = useState("");
  const [content, setContent] = useState("");
  const [languages, setLanguages] = useState([]);
  const [language, setLanguage] = useState();

  const handleCreate = async (e) => {
    e.preventDefault();

    let snippet = {
      name: name,
      content: content,
      description: "",
      language: language.name,
      origin: "",
      tags: [],
    };

    var res = await createSnippet(snippet);

    if (res.status && res.status !== 200)
      return toast.error(HandleStatuscode(res.status));
    if (res) window.location.href = "/";
  };

  useEffect(() => {
    (async () => {
      let languages = await getProgramingLanguages();
      if (languages.status && languages.status !== 200)
        return toast.error(HandleStatuscode(languages.status));

      setLanguages(languages);
      setLanguage(languages[0]);
    })();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

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
            {languages.length === 0 ? (
              <div className="flex justify-center mt-5">Loading...</div>
            ) : (
              <Listbox value={language} onChange={setLanguage}>
                <div className="relative mt-1">
                  <Listbox.Button className="relative w-full cursor-default rounded-full h-12 bg-slate-950 text-white py-2 pl-3 pr-10 text-left shadow-md focus:outline-none focus-visible:border-indigo-500 focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75 focus-visible:ring-offset-2 focus-visible:ring-offset-orange-300 sm:text-sm">
                    <div className="flex items-center">
                      <img
                        alt="test"
                        src={`${baseUrl}/${language.url}`}
                        className="w-7 h-7 mr-2"
                      />
                      <span className="block truncate text-center">
                        {language.name}
                      </span>
                    </div>
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
                      {languages.map((language, i) => (
                        <Listbox.Option
                          key={i}
                          className={({ active }) =>
                            `relative cursor-default select-none py-2 pl-10 pr-4 flex items-center ${
                              active
                                ? "bg-slate-400 text-amber-900"
                                : "text-white"
                            }`
                          }
                          value={language}
                        >
                          {({ selected }) => (
                            <>
                              <img
                                alt="test"
                                src={`${baseUrl + language.url}`}
                                className="w-7 h-7 mr-2"
                              />
                              <span
                                className={`block truncate ${
                                  selected ? "font-medium" : "font-normal"
                                }`}
                              >
                                {language.name}
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
            )}
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
            className="px-4 py-2 font-semibold text-sm bg-slate-700  text-white rounded-full shadow-sm"
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
