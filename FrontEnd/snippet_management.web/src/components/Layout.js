import Popup from "./Popup/Popup";
import AddSnippetButton from "./Popup/Contents/AddSnippetButton";
import SearchBar from "./SearchBar";
import SideBar from "./SideBar";
import SnippetList from "./SnippetList";
import Button from "./Elements/Button";
import { useSnippetContext } from "../context/SnippetContext";
import { usePopupContext } from "../context/PopupContext";
import { Toaster } from "react-hot-toast";

const Layout = ({
  children,
  pages,
  fetchNextPage,
  refetch,
  handleUpdateSnippet,
  handleDeleteSnippet,
  handleSortSnippets,
}) => {
  const { snippet } = useSnippetContext();
  const { popup, setPopup } = usePopupContext();

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
            handleDeleteSnippet(snippet.id);
          }
        }}
      >
        <SearchBar refetch={refetch} />
        <div className="flex flex-col sm:flex-row">
          <div className="w-full overflow-hidden">
            <div className="mx-5 ">{children}</div>
            <div className="flex justify-center">
              <Button
                className="px-4 py-2 font-semibold text-sm bg-slate-700 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-500"
                onClick={(e) => {
                  setPopup({
                    ...popup,
                    isOpen: true,
                    title: "Add Code Snippets & Developer Materials",
                    content: <AddSnippetButton></AddSnippetButton>,
                  });
                }}
                type="button"
              >
                Add
              </Button>
              <Button
                type="button"
                className="px-4 py-2 font-semibold text-sm bg-slate-700 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-500 disabled:opacity-50 cursor-not-allowed"
                disabled
              >
                Discover
              </Button>
              <Button
                type="button"
                className="px-4 py-2 font-semibold text-sm bg-slate-700 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-500 disabled:opacity-50 cursor-not-allowed"
                disabled
              >
                Paste
              </Button>
              <Button
                type="button"
                className="px-4 py-2 font-semibold text-sm bg-slate-700 text-white rounded-full shadow-sm mt-5 mr-5 hover:bg-slate-500"
                onClick={handleSortSnippets}
                title="Sort by Last Added"
              >
                Sort
              </Button>
            </div>
            <SnippetList
              pages={pages}
              fetchNextPage={fetchNextPage}
              handleUpdateSnippet={handleUpdateSnippet}
            />
          </div>
          <div>
            <SideBar></SideBar>
          </div>
        </div>
      </main>
      <Popup></Popup>
      <Toaster />
    </>
  );
};

export default Layout;
