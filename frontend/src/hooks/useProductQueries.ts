import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { createProduct, deleteProduct, updateProduct } from '../api/admin';
import { fetchCategories } from '../api/categories';
import {
  fetchProductById,
  fetchProducts,
  filterProducts,
  searchProducts,
} from '../api/products';
import { useAuth } from '../context/AuthContext';
import type { CreateProductRequest, UpdateProductRequest } from '../types/api';
import { categoryKeys, productKeys } from '../query/queryKeys';

export function useProductsQuery(page: number, pageSize: number) {
  return useQuery({
    queryKey: productKeys.list(page, pageSize),
    queryFn: () => fetchProducts(page, pageSize),
  });
}

export function useProductQuery(productId: number) {
  return useQuery({
    queryKey: productKeys.detail(productId),
    queryFn: () => fetchProductById(productId),
    enabled: Number.isInteger(productId) && productId > 0,
  });
}

export function useCategoriesQuery() {
  return useQuery({
    queryKey: categoryKeys.all,
    queryFn: fetchCategories,
    staleTime: 5 * 60_000,
  });
}

export function useSearchProductsQuery(
  query: string,
  page: number,
  pageSize: number,
  enabled: boolean,
) {
  const { token } = useAuth();

  return useQuery({
    queryKey: productKeys.search(query, page, pageSize),
    queryFn: () => searchProducts(query, page, pageSize, token!),
    enabled: enabled && Boolean(query && token),
  });
}

export function useFilterProductsQuery(
  category: string | undefined,
  minPrice: number | undefined,
  maxPrice: number | undefined,
  page: number,
  pageSize: number,
  enabled: boolean,
) {
  const { token } = useAuth();

  return useQuery({
    queryKey: productKeys.filter(category, minPrice, maxPrice, page, pageSize),
    queryFn: () => filterProducts(category, minPrice, maxPrice, page, pageSize, token!),
    enabled: enabled && Boolean(token),
  });
}

function invalidateProductCaches(queryClient: ReturnType<typeof useQueryClient>) {
  void queryClient.invalidateQueries({ queryKey: productKeys.all });
}

export function useCreateProductMutation() {
  const { token } = useAuth();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateProductRequest) => createProduct(data, token!),
    onSuccess: () => invalidateProductCaches(queryClient),
  });
}

export function useUpdateProductMutation() {
  const { token } = useAuth();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateProductRequest }) =>
      updateProduct(id, data, token!),
    onSuccess: () => invalidateProductCaches(queryClient),
  });
}

export function useDeleteProductMutation() {
  const { token } = useAuth();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => deleteProduct(id, token!),
    onSuccess: () => invalidateProductCaches(queryClient),
  });
}
