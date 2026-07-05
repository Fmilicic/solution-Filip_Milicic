export const productKeys = {
  all: ['products'] as const,
  list: (page: number, pageSize: number) => [...productKeys.all, 'list', page, pageSize] as const,
  detail: (id: number) => [...productKeys.all, 'detail', id] as const,
  search: (query: string, page: number, pageSize: number) =>
    [...productKeys.all, 'search', query, page, pageSize] as const,
  filter: (
    category: string | undefined,
    minPrice: number | undefined,
    maxPrice: number | undefined,
    page: number,
    pageSize: number,
  ) => [...productKeys.all, 'filter', category, minPrice, maxPrice, page, pageSize] as const,
};

export const categoryKeys = {
  all: ['categories'] as const,
};
