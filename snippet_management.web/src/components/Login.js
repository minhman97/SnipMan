import { React, useState } from "react";

const Login = ({ setToken }) => {
  const [email, setEmail] = useState(null);
  const [password, setPassword] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = await loginUser({
      email,
      password,
    });
    setToken(token);
  };

  return (
    <>
      <div>Login</div>
      <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 flex flex-col">
        <form onSubmit={(e) => handleSubmit(e)}>
          <div className="mb-4">
            <label
              className="block text-grey-darker text-sm font-bold mb-2"
              htmlFor="email"
            >
              Email
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-grey-darker"
              id="email"
              type="text"
            //   value={"a@a.vn"}
              placeholder="Email"
              onChange={(e) => {
                setEmail(e.target.value);
              }}
            />
          </div>
          <div className="mb-6">
            <label
              className="block text-grey-darker text-sm font-bold mb-2"
              htmlFor="password"
            >
              Password
            </label>
            <input
              className="shadow appearance-none border border-red rounded w-full py-2 px-3 text-grey-darker mb-3"
              id="password"
              type="password"
              placeholder="******************"
            //   value={"a"}
              onChange={(e) => {
                setPassword(e.target.value);
              }}
            />
            <p className="text-red text-xs italic">Please choose a password.</p>
          </div>
          <div className="flex items-center justify-between">
            <button
              className="bg-blue-400 hover:bg-blue-800 text-white font-bold py-2 px-4 rounded"
              type="submit"
            >
              Sign In
            </button>
            <a
              className="inline-block align-baseline font-bold text-sm text-blue hover:text-blue-darker"
              href="/"
            >
              Forgot Password?
            </a>
          </div>
        </form>
      </div>
    </>
  );
};

async function loginUser(credentials) {
  return fetch("https://localhost:44395/Authentication", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(credentials),
  }).then((data) => data.json());
}

export default Login;
