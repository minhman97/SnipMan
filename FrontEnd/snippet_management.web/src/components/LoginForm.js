import { React, useEffect, useState } from "react";
import { login } from "../api/userApi";
import { useForm } from "react-hook-form";
import { HandleStatuscode } from "../helper/statusCodeHelper";

const LoginForm = ({ setToken }) => {
  const [auth, setAuth] = useState({
    isAuth: false,
    token: null,
    message: null,
  });
  const { register, handleSubmit, formState: { errors } } = useForm();

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

  const onSubmit = async (data) => {
    console.log(data);

    let response = await login(data);
    if (response.isFailed) {
      setAuth({ ...auth, message: response.message.message });
      return;
    }
    await response.json().then((data) => {
      setToken(data);
    });
  };

  const handleCredentialResponse = async (response) => {
    const res = await login(response.credential, true);
    if (!res.ok) {
      throw new Error(
        `StatusCode:${res.status}. ErrorMessage:${HandleStatuscode(
          res.status
        )}`,
        { cause: { status: res.status } }
      );
    }
    await res.json().then((data) => {
      setToken(data);
    });
  };

  return (
    <>
      <div className="h-screen bg-slate-800 from-blue-600 to-indigo-600 flex justify-center items-center w-full">
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="bg-white px-10 py-8 rounded-xl w-screen shadow-md max-w-sm">
            <div className="space-y-4">
              <h1 className="text-center text-2xl font-semibold text-gray-600">
                Login
              </h1>
              {!auth.isAuth && auth.message !== null && (
                <div
                  class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative"
                  role="alert"
                >
                  <span class="block sm:inline">{auth.message}</span>
                </div>
              )}
              <div>
                <label
                  htmlFor="email"
                  className="block mb-1 text-gray-600 font-semibold"
                >
                  Email
                </label>
                <input
                  type="text"
                  className="bg-indigo-50 px-4 py-2 outline-none rounded-md w-full"
                  //   value={"a@a.vn"}
                  placeholder="Input your Email"
                  {...register("email", { required: true, maxLength: 150 })}
                />
                {errors?.email?.type === "required" && <p className="text-red-700">Please input your Email</p>}
              </div>
              <div>
                <label
                  htmlFor="email"
                  className="block mb-1 text-gray-600 font-semibold"
                >
                  Password
                </label>
                <input
                  className="bg-indigo-50 px-4 py-2 outline-none rounded-md w-full"
                  type="password"
                  placeholder="Input your Password"
                  {...register("password", { required: true })}                  
                />
                {errors?.email?.type === "required" && <p className="text-red-700">Please input your Password</p>}
              </div>
            </div>
            <button type="submit" className="mt-4 w-full bg-slate-700  text-white py-2 rounded-md text-lg tracking-wide">
              Sign in
            </button>
            <div className="mt-4 flex justify-center" id="btn-signin-gg"></div>
          </div>
        </form>
      </div>
    </>
  );
};

export default LoginForm;
