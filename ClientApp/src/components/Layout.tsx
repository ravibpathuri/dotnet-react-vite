import { Outlet } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export function Layout() {
  const { isAuthenticated, user, logout } = useAuth();

  return (
    <div className="app-container">
      <header className="app-header">
        <nav>
          <div className="logo">ReferBuddy</div>
          {isAuthenticated && (
            <div className="user-info">
              <span>Welcome, {user?.name}</span>
              <button onClick={logout} className="logout-button">
                Logout
              </button>
            </div>
          )}
        </nav>
      </header>

      <main className="app-main">
        <Outlet />
      </main>

      <footer className="app-footer">
        <p>© {new Date().getFullYear()} ReferBuddy. All rights reserved.</p>
      </footer>
    </div>
  );
}
