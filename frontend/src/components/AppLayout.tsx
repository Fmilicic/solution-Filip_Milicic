import { NavLink, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './AppLayout.css';

export function AppLayout() {
  const { isAuthenticated, username, logout } = useAuth();

  return (
    <div className="app-shell">
      <header className="app-header">
        <div className="app-header__brand">
          <NavLink to="/" className="app-header__title">
            Product Middleware
          </NavLink>
          <p className="app-header__subtitle">Product catalog SPA</p>
        </div>

        <nav className="app-nav" aria-label="Main navigation">
          <NavLink to="/" end className="app-nav__link">
            Products
          </NavLink>
          <NavLink to="/search" className="app-nav__link">
            Search
          </NavLink>
          <NavLink to="/filter" className="app-nav__link">
            Filter
          </NavLink>
          {!isAuthenticated ? (
            <NavLink to="/login" className="app-nav__link">
              Login
            </NavLink>
          ) : (
            <div className="app-nav__account">
              <span className="app-nav__username">{username}</span>
              <button type="button" className="app-nav__button" onClick={logout}>
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
        <p>API: {import.meta.env.VITE_API_BASE_URL}</p>
      </footer>
    </div>
  );
}
