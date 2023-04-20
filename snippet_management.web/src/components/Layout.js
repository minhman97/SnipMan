import Login from "./Login";
import Popup from "./Popup/Popup";
import { deleteSnippet } from "../api/snippetApi";
import { ToastContainer, toast } from "react-toastify";

const Layout = ({ children, token, setToken, popupObject, snippetId }) => {

  if (!token) {
    return <Login setToken={setToken} />;
  }

  return (
    <>
      <main
        tabIndex={0}
        className="text-white bg-slate-800 min-h-screen"
        onKeyDown={async (e) => {
          if (
            (e.key === "Delete" || e.key === "Backspace") &&
            e.target.localName === "main" && snippetId
          ) {
            await deleteSnippet(token, snippetId);
            toast.success("Snippet deleted successfully", {
              position: "top-center",
              autoClose: 2000,
              hideProgressBar: false,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: false,
              theme: "light",
            });
          }
        }}
      >
        {token !== undefined && (
          <>
            {children}
          </>
        )}
      </main>
      <Popup popupObject={popupObject} />
      <ToastContainer />
    </>
  );
};

export default Layout;
