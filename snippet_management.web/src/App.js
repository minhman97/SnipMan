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
import CreateSnippet from "./routes/Snippet/CreateSnippet";
import ErrorPage from "./ErrorPage";
import { SnippetContext } from "./context/SnippetContext";
import { PaginationContext } from "./context/PaginationContext";
function App() {
  const [token, saveToken] = useToken();

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
              <SnippetContext>
                <PaginationContext>
                  <Snippet />
                </PaginationContext>
              </SnippetContext>
            ) : (
              <Navigate to={`/`} />
            )
          }
        />
        <Route
          path="/snippet/create"
          element={token ? <CreateSnippet /> : <Navigate to={`/`} />}
        />
      </>
    )
  );
  return <RouterProvider router={router} />;
}

export default App;
