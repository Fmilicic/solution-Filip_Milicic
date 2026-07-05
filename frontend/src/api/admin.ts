import { apiDelete, apiPost, apiPut } from './client';
import type { CreateProductRequest, ProductDetail, UpdateProductRequest } from '../types/api';

export async function createProduct(
  data: CreateProductRequest,
  token: string,
): Promise<ProductDetail> {
  return apiPost<ProductDetail, CreateProductRequest>('/api/admin/products', data, token);
}

export async function updateProduct(
  id: number,
  data: UpdateProductRequest,
  token: string,
): Promise<ProductDetail> {
  return apiPut<ProductDetail, UpdateProductRequest>(`/api/admin/products/${id}`, data, token);
}

export async function deleteProduct(id: number, token: string): Promise<void> {
  return apiDelete(`/api/admin/products/${id}`, token);
}
