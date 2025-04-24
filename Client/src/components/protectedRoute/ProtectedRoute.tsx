import { Navigate, useParams } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { JSX } from "react";

interface ProtectedRouteProps {
  children: JSX.Element;
  guestOnly?: boolean;
  requireRole?: string;
  allowedToUserOrAdmin?: boolean;
}

const ProtectedRoute = ({
  children,
  guestOnly,
  requireRole,
  allowedToUserOrAdmin,
}: ProtectedRouteProps) => {
  const { user, loading, hasRole } = useAuth();
  const { userName } = useParams<{ userName: string }>();

  const isThisUser = () => {
    return user?.username === userName;
  };

  if (loading) return <div>Loading...</div>;

  // If guestOnly and use is logged in => redirect to home page
  if (guestOnly && user) {
    return <Navigate to="/" />;
  }

  // If route requires a role and user does not have it => redirect to /unauthorized
  if (requireRole && !hasRole(requireRole)) {
    return <Navigate to="/unauthorized" />;
  }

  // If allowedToUserOrAdmin is true and user is not an admin or the user is not the same as the one in the URL => redirect to /unauthorized
  if (allowedToUserOrAdmin && !(isThisUser() || hasRole("Administrator"))) {
    return <Navigate to="/unauthorized" />;
  }

  // If it's a protected route and user is not logged in => redirect to login page
  if (!guestOnly && !user) {
    return <Navigate to="/login" />;
  }

  // If all checks pass, render the children
  return children;
};

export default ProtectedRoute;
