import { apiGet } from './client';
import type { Category } from '../types/api';

export async function fetchCategories(): Promise<Category[]> {
  return apiGet<Category[]>('/api/categories');
}
