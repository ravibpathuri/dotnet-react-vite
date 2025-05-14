import { Routes, Route, Navigate } from "react-router-dom";
import type { ReactElement } from "react";
import { WeatherForecast } from "./components/WeatherForecast";
import { Login } from "./components/Login";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { Layout } from "./components/Layout";

// Define protected route wrapper
const withProtection = (element: ReactElement): ReactElement => (
  <ProtectedRoute>{element}</ProtectedRoute>
);

// Create and export the router
export function Router() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={withProtection(<WeatherForecast />)} />
        <Route path="login" element={<Login />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Route>
    </Routes>
  );
}
