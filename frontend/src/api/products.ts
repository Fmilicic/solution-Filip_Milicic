import { apiGet, apiGetAuth } from './client';
import type { PagedProducts, ProductDetail } from '../types/api';

const DEFAULT_PAGE = 1;
const DEFAULT_PAGE_SIZE = 12;

export function getProductsQuery(page = DEFAULT_PAGE, pageSize = DEFAULT_PAGE_SIZE): string {
  const params = new URLSearchParams({
    page: String(page),
    pageSize: String(pageSize),
  });

  return `/api/products?${params.toString()}`;
}

export async function fetchProducts(page: number, pageSize: number): Promise<PagedProducts> {
  return apiGet<PagedProducts>(getProductsQuery(page, pageSize));
}

export async function fetchProductById(id: number): Promise<ProductDetail> {
  return apiGet<ProductDetail>(`/api/products/${id}`);
}

export async function searchProducts(
  query: string,
  page: number,
  pageSize: number,
  token: string,
): Promise<PagedProducts> {
  const params = new URLSearchParams({
    query,
    page: String(page),
    pageSize: String(pageSize),
  });

  return apiGetAuth<PagedProducts>(`/api/products/search?${params.toString()}`, token);
}

export async function filterProducts(
  category: string | undefined,
  minPrice: number | undefined,
  maxPrice: number | undefined,
  page: number,
  pageSize: number,
  token: string,
): Promise<PagedProducts> {
  const params = new URLSearchParams({
    page: String(page),
    pageSize: String(pageSize),
  });

  if (category) {
    params.set('category', category);
  }

  if (minPrice !== undefined) {
    params.set('minPrice', String(minPrice));
  }

  if (maxPrice !== undefined) {
    params.set('maxPrice', String(maxPrice));
  }

  return apiGetAuth<PagedProducts>(`/api/products/filter?${params.toString()}`, token);
}

export const productListDefaults = {
  page: DEFAULT_PAGE,
  pageSize: DEFAULT_PAGE_SIZE,
} as const;

export function validateFilterInput(
  category: string,
  minPrice: string,
  maxPrice: string,
): string | null {
  const hasCategory = category.trim().length > 0;
  const parsedMin = minPrice.trim() === '' ? undefined : Number(minPrice);
  const parsedMax = maxPrice.trim() === '' ? undefined : Number(maxPrice);

  if (!hasCategory && parsedMin === undefined && parsedMax === undefined) {
    return 'At least one filter parameter is required.';
  }

  if (parsedMin !== undefined && Number.isNaN(parsedMin)) {
    return 'minPrice must be a number.';
  }

  if (parsedMax !== undefined && Number.isNaN(parsedMax)) {
    return 'maxPrice must be a number.';
  }

  if (parsedMin !== undefined && parsedMin < 0) {
    return 'minPrice cannot be negative.';
  }

  if (parsedMax !== undefined && parsedMax < 0) {
    return 'maxPrice cannot be negative.';
  }

  if (parsedMin !== undefined && parsedMax !== undefined && parsedMin > parsedMax) {
    return 'minPrice cannot exceed maxPrice.';
  }

  return null;
}
