import {
  Navigate,
  Route,
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
} from "react-router-dom";
import useToken from "./hooks/useToken";
import LoginForm from "./components/LoginForm";
import Snippet from "./routes/Snippet/Snippet";
import ShareableSnippet from "./routes/Snippet/ShareableSnippet";
import CreateSnippet from "./routes/Snippet/CreateSnippet";
import ErrorPage from "./ErrorPage";
import { SnippetContext } from "./context/SnippetContext";
import { PaginationContext } from "./context/PaginationContext";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { PopupContext } from "./context/PopupContext";
function App() {
  const [token, saveToken] = useToken();

  const queryClient = new QueryClient();
  queryClient.invalidateQueries({ queryKey: ["list-snippet"] });
  const router = createBrowserRouter(
    createRoutesFromElements(
      <>
        <Route
          path="/"
          element={
            token ? (
              <Navigate to={`/snippet`} />
            ) : (
              <LoginForm token={token} setToken={saveToken} />
            )
          }
          errorElement={<ErrorPage />}
        />
        <Route
          path="/snippet"
          element={
            token ? (
              <QueryClientProvider client={queryClient}>
                <SnippetContext>
                  <PopupContext>
                    <PaginationContext>
                      <Snippet />
                    </PaginationContext>
                  </PopupContext>
                </SnippetContext>
                <ReactQueryDevtools
                  initialIsOpen={false}
                  position="bottom-right"
                />
              </QueryClientProvider>
            ) : (
              <Navigate to={`/`} />
            )
          }
        />
        <Route
          path="/snippet/create"
          element={token ? <CreateSnippet /> : <Navigate to={`/`} />}
        />
        <Route
          path="/:userId/:shareableId"
          element={<ShareableSnippet />}
        ></Route>
      </>
    )
  );
  return <RouterProvider router={router} />;
}

export default App;
