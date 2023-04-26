import LoginForm from "./LoginForm";
import Popup from "./Popup/Popup";
import { deleteSnippet } from "../api/snippetApi";
import { ToastContainer, toast } from "react-toastify";
import AddSnippetButton from "./Popup/Contents/AddSnippetButton";
import { useState } from "react";
import SearchBar from "./SearchBar";
import SideBar from "./SideBar";
import { SnippetTextArea } from "./SnippetTextArea";
import SnippetCommands from "./SnippetCommands";
import SnippetList from "./SnippetList";

const Layout = ({
  children,
  token,
  setToken,
  snippets,
  currentCursor,
  setCurrentCursor,
  setSnippets,
  setSnippet,
  sortOrder,
  setSortOrder,
  rangeObject,
  setRangeObject,
  setFilterKeyWord,
  pageSize,
  snippet,
  slidesPerView,
}) => {
  let [isOpen, setIsOpen] = useState(false);

  if (!token) {
    return <LoginForm setToken={setToken} />;
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
            snippet.id
          ) {
            let status = await deleteSnippet(token, snippet.id);
            let tempData = snippets.data.filter(
              (item) => item.id !== snippet.id
            );
            let cursor = currentCursor - 1 < 0 ? 0 : currentCursor - 1;
            setSnippets({ ...snippets, data: tempData });
            setSnippet(tempData[cursor]);
            setCurrentCursor(cursor);
            if (status === 200) {
              toast.success("Snippet deleted successfully", {
                position: "top-center",
                autoClose: 2000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: false,
                theme: "light",
              });
            } else {
              toast.error(
                `Something wrong can't delete snippet id: ${snippet.id}`,
                {
                  position: "top-center",
                  autoClose: 2000,
                  hideProgressBar: false,
                  closeOnClick: true,
                  pauseOnHover: true,
                  draggable: false,
                  theme: "light",
                }
              );
            }
          }
        }}
      >
        <SearchBar
          token={token}
          sortOrder={sortOrder}
          snippets={snippets}
          setSnippets={setSnippets}
          setSnippet={setSnippet}
          rangeObject={rangeObject}
          setRangeObject={setRangeObject}
          setFilterKeyWork={setFilterKeyWord}
          setcurrentCurson={setCurrentCursor}
          pageSize={pageSize}
        />
        <div className="flex justify-center mx-10">
          <div className="w-full mx-5 ">{children}</div>
          <div>
            <SideBar></SideBar>
          </div>
        </div>
        <SnippetCommands
          sortOrder={sortOrder}
          setSortOrder={setSortOrder}
          rangeObject={rangeObject}
          setRangeObject={setRangeObject}
          pageSize={pageSize}
          currentCursor={currentCursor}
          setIsOpen={setIsOpen}
        />
        <SnippetList
          snippet={snippet}
          snippets={snippets}
          rangeObject={rangeObject}
          setRangeObject={setRangeObject}
          pageSize={pageSize}
          currentCursor={currentCursor}
          setCurrentCursor={setCurrentCursor}
          setSnippet={setSnippet}
          slidesPerView={slidesPerView}
        />
      </main>
      <Popup
        title={"Add Code Snippets & Developer Materials"}
        isOpen={isOpen}
        setIsOpen={setIsOpen}
      >
        <AddSnippetButton></AddSnippetButton>
      </Popup>
      <ToastContainer />
    </>
  );
};

export default Layout;
