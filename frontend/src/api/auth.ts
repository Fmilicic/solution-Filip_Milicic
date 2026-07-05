import { apiPost } from './client';
import type { LoginRequest, LoginResponse } from '../types/api';

export async function loginRequest(credentials: LoginRequest): Promise<LoginResponse> {
  return apiPost<LoginResponse, LoginRequest>('/api/auth/login', credentials);
}
