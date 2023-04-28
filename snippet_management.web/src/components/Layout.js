import Popup from "./Popup/Popup";
import { deleteSnippet } from "../api/SnippetApi";
import { ToastContainer, toast } from "react-toastify";
import AddSnippetButton from "./Popup/Contents/AddSnippetButton";
import { useState } from "react";
import SearchBar from "./SearchBar";
import SideBar from "./SideBar";
import SnippetList from "./SnippetList";
import Button from "./Elements/Button";
import { GetErrorMessage } from "../api/StatusCode";
import { useSnippetContext } from "../context/SnippetContext";

const Layout = ({ children }) => {
  const {
    token,
    snippet,
    setSnippet,
    snippets,
    setSnippets,
    currentCursor,
    setCurrentCursor,
    sortOrder,
    setSortOrder,
    rangeObject,
    setRangeObject,
    pageSize,
    slidesPerView,
  } = useSnippetContext();
  let [isOpen, setIsOpen] = useState(false);

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
            let res = await deleteSnippet(token, snippet.id);
            if (res.status && res.status === 200) {
              let tempData = snippets.data.filter(
                (item) => item.id !== snippet.id
              );
              let cursor = currentCursor - 1 < 0 ? 0 : currentCursor - 1;
              setSnippets({ ...snippets, data: tempData });
              setSnippet(tempData[cursor]);
              setCurrentCursor(cursor);
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
                `${GetErrorMessage(res.status)}. Can't delete snippet: ${
                  snippet.name
                }`,
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
        <SearchBar />
        <div className="flex flex-col sm:flex-row w-full">
          <div className="w-full">
            <div className="mx-5 ">{children}</div>
            <div className="flex justify-center">
              <Button
                className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-700"
                onClick={(e) => {
                  setIsOpen(true);
                }}
                type="button"
              >
                Add
              </Button>
              <Button
                type="button"
                className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-700"
              >
                Discover
              </Button>
              <Button
                type="button"
                className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-700"
              >
                Paste
              </Button>
              <Button
                type="button"
                className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-700"
                onClick={async (e) => {
                  setRangeObject({
                    ...rangeObject,
                    startIndex: 0,
                    endIndex: currentCursor + pageSize,
                  });
                  setSortOrder({
                    ...sortOrder,
                    orderWay: sortOrder.orderWay === "asc" ? "desc" : "asc",
                    sortProperty: "created",
                    clicked: true,
                  });
                }}
                title="Sort by Last Added"
              >
                Sort
              </Button>
            </div>
            <SnippetList />
          </div>
          <div>
            <SideBar></SideBar>
          </div>
        </div>
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
