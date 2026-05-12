import { Navigate } from "react-router-dom";

// Protects routes by validating JWT authentication status
export default function PrivateRoute({ children }) {
  const token = localStorage.getItem("accessToken");

  if (!token) {
    return <Navigate to="/" replace />;
  }

  return children;
}