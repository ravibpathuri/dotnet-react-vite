import { useEffect, useState } from "react";
import type { ReactNode } from "react";
import type { AuthContextType } from "./authContext.types";
import { AuthContext } from "./authContext.types";
import axios from "axios";
import { useLocation } from "react-router-dom";

export function AuthProvider({ children }: { children: ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [user, setUser] = useState<AuthContextType["user"]>(null);
  const location = useLocation();

  const checkAuthStatus = async () => {
    try {
      const response = await axios.get("/api/auth/user", {
        withCredentials: true,
      });
      setUser(response.data);
      setIsAuthenticated(true);
    } catch (error) {
      console.error("Error checking authentication status:", error);
      setUser(null);
      setIsAuthenticated(false);
    } finally {
      setIsLoading(false);
    }
  };

  // Check auth status on mount and when location changes
  useEffect(() => {
    checkAuthStatus();
  }, [location.pathname]);

  const login = () => {
    // Store the current location to redirect back after login
    const returnUrl = location.pathname === "/login" ? "/" : location.pathname;
    window.location.href = `/api/auth/login?returnUrl=${returnUrl}`;
  };

  const logout = () => {
    window.location.href = "/api/auth/logout";
  };

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated,
        isLoading,
        user,
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
