import { createContext, useCallback, useContext, useMemo, useState, type ReactNode } from 'react';
import { loginRequest } from '../api/auth';
import {
  clearAuthSession,
  getAccessToken,
  getUsername,
  setAuthSession,
} from '../lib/authStorage';

interface AuthContextValue {
  token: string | null;
  username: string | null;
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(() => getAccessToken());
  const [username, setUsername] = useState<string | null>(() => getUsername());

  const login = useCallback(async (loginUsername: string, password: string) => {
    const result = await loginRequest({ username: loginUsername, password });
    setAuthSession(result.accessToken, result.username);
    setToken(result.accessToken);
    setUsername(result.username);
  }, []);

  const logout = useCallback(() => {
    clearAuthSession();
    setToken(null);
    setUsername(null);
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({
      token,
      username,
      isAuthenticated: Boolean(token),
      login,
      logout,
    }),
    [token, username, login, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error('useAuth must be used within AuthProvider.');
  }

  return context;
}
