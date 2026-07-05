import { NavLink, Outlet } from 'react-router-dom';
import { getAccessToken } from '../lib/authStorage';
import './AppLayout.css';

export function AppLayout() {
  const isAuthenticated = Boolean(getAccessToken());

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
          <NavLink to="/login" className="app-nav__link">
            {isAuthenticated ? 'Account' : 'Login'}
          </NavLink>
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
