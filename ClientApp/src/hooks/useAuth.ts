import { useContext } from "react";
import type { AuthContextType } from "../contexts/authContext.types";
import { AuthContext } from "../contexts/authContext.types";

export function useAuth(): AuthContextType {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
