import { React, useEffect, useState } from "react";
import { login } from "../api/userApi";

const LoginForm = ({ setToken }) => {
  const [email, setEmail] = useState(null);
  const [password, setPassword] = useState(null);
  const [auth, setAuth] = useState({ isAuth: false, token: null });

  useEffect(() => {
    const script = document.createElement("script");
    script.src = "https://accounts.google.com/gsi/client";
    script.async = true;
    script.defer = true;
    script.onload = () => {
      if (window.google) {
        /*global google*/
        var a = {
          client_id: process.env.REACT_APP_GOOGLE_CLIENT_ID,
          callback: handleCredentialResponse,
        };
        google.accounts.id.initialize(a);
        google.accounts.id.renderButton(
          document.getElementById("btn-signin-gg"),
          { theme: "outline", size: "large" } // customization attributes
        );
        google.accounts.id.prompt(); // also display the One Tap dialog
      }
    };
    document.body.appendChild(script);
    return () => {
      document.body.removeChild(script);
    };
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    let response = await login({
      email,
      password,
    });
    if (response.isFailed) {
      setAuth({ ...auth, message: response.message });
      return;
    }
    setToken(response);
  };

  const handleCredentialResponse = async (response) => {
    const token = await login(response.credential, true);
    setToken(token);
  };

  return (
    <>
      <div>Login</div>
      <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 flex flex-col">
        {!auth.isAuth && auth.message === null && (
          <div
            class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative"
            role="alert"
          >
            <span class="block sm:inline">{auth.message}</span>
          </div>
        )}
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
            <div id="btn-signin-gg"></div>
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

export default LoginForm;
