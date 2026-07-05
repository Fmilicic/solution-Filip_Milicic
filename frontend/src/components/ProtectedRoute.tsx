import { Navigate, Outlet } from 'react-router-dom';
import { getAccessToken } from '../lib/authStorage';

export function ProtectedRoute() {
  if (!getAccessToken()) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
}
