import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export function Login() {
  const { isAuthenticated, login } = useAuth();
  const location = useLocation();
  const from = location.state?.from?.pathname || "/";

  if (isAuthenticated) {
    return <Navigate to={from} replace />;
  }

  return (
    <div className="login-container">
      <h1>Welcome to ReferBuddy</h1>
      <p>Please log in to continue</p>
      <button onClick={login} className="login-button">
        Log In
      </button>
    </div>
  );
}
