import React, { useState } from "react";
import useToken from "../../hooks/useToken";

const Snippet = () => {
  const { token } = useToken();
  const [name, setName] = useState("");
  const [content, setContent] = useState("");
  const handleCreate = async (e) => {
    e.preventDefault();

    let snippet = {
      name: name,
      content: content,
      description: "",
      origin: "",
      tags:[]
    };

    var res = await fetch("https://localhost:44395/Snippet", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
      body: JSON.stringify(snippet),
    }).catch((err) => {
      console.log(err.message);
   });
    if(res.status === 200)
      return window.location.href = "/"
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
            onClick={(e) => {
              handleCreate(e);
            }}
          >
            Create
          </button>
        </div>
      </div>
    </main>
  );
};

export default Snippet;
