import Login from "./Login";
import Popup from "./Popup/Popup";
import { deleteSnippet } from "../api/snippetApi";
import { ToastContainer, toast } from "react-toastify";

const Layout = ({
  children,
  token,
  setToken,
  popupObject,
  setPopupObject,
  snippetId,
  snippets,
  currentCursor,
  setCurrentCursor,
  setSnippets,
  setSnippet,
}) => {
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
            e.target.localName === "main" &&
            snippetId
          ) {
            let status = await deleteSnippet(token, snippetId);
            let tempData = snippets.data.filter(item => item.id !== snippetId);
            let cursor = currentCursor - 1 < 0 ? 0 : currentCursor - 1;
            setSnippets({ ...snippets, data: tempData });
            setSnippet(tempData[cursor]);
            setCurrentCursor(cursor);
            if(status === 200)
            {
              toast.success("Snippet deleted successfully", {
                position: "top-center",
                autoClose: 2000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: false,
                theme: "light",
              });
            }else{
              toast.error(`Something wrong can't delete snippet id: ${snippetId}`, {
                position: "top-center",
                autoClose: 2000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: false,
                theme: "light",
              });
            }
            
          }
        }}
      >
        {token !== undefined && <>{children}</>}
      </main>
      <Popup popupObject={popupObject} setPopupObject={setPopupObject} />
      <ToastContainer />
    </>
  );
};

export default Layout;
