import "./App.css";
import Login from "./components/Login";
import useToken from "./hooks/useToken";

function App() {
  const { token, setToken } = useToken();

  if (!token) {
    return <Login setToken={setToken} />;
  }
  console.log(token);
  return (
    <main className="text-white bg-slate-800 min-h-screen">
      <div>
        {token !== undefined && (
          <>
            <div className="py-5 flex justify-center">
              <input
                type="text"
                className="rounded-full w-8/12 h-12 bg-slate-950 px-5"
                placeholder="Search anything..."
              />
            </div>
            <div>
              <h1>content</h1>
            </div>
          </>
        )}
      </div>
    </main>
  );
}

function setToken(userToken) {
  sessionStorage.setItem('token', JSON.stringify(userToken));
}

function getToken() {
  const tokenString = sessionStorage.getItem('token');
  const userToken = JSON.parse(tokenString);
  return userToken?.token;
}

export default App;
