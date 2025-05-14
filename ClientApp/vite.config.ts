import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    strictPort: true,
    proxy: {
      "/api": {
        target: "https://localhost:7126",
        secure: false, // Accept self-signed certificate
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, "/api"),
      },
      "/swagger": {
        target: "https://localhost:7126",
        secure: false, // Accept self-signed certificate
        changeOrigin: true,
      },
    },
  },
});
