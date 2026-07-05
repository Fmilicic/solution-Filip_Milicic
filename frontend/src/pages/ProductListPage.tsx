import { useCallback, useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { fetchProducts, productListDefaults } from '../api/products';
import { PagedProductResults } from '../components/PagedProductResults';
import type { PagedProducts } from '../types/api';
import './ProductListPage.css';

function parsePositiveInt(value: string | null, fallback: number): number {
  const parsed = Number(value);
  return Number.isInteger(parsed) && parsed > 0 ? parsed : fallback;
}

export function ProductListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const page = parsePositiveInt(searchParams.get('page'), productListDefaults.page);
  const pageSize = parsePositiveInt(searchParams.get('pageSize'), productListDefaults.pageSize);

  const [data, setData] = useState<PagedProducts | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [reloadKey, setReloadKey] = useState(0);

  const loadProducts = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      const result = await fetchProducts(page, pageSize);
      setData(result);
    } catch (loadError) {
      const message = loadError instanceof Error ? loadError.message : 'Failed to load products.';
      setData(null);
      setError(message);
    } finally {
      setLoading(false);
    }
  }, [page, pageSize]);

  useEffect(() => {
    void loadProducts();
  }, [loadProducts, reloadKey]);

  function handlePageChange(nextPage: number) {
    setSearchParams({
      page: String(nextPage),
      pageSize: String(pageSize),
    });
  }

  return (
    <section className="page product-list-page">
      <header className="product-list-page__header">
        <h1>Products</h1>
        <p>Browse the catalog from the middleware API.</p>
      </header>

      <PagedProductResults
        loading={loading}
        error={error}
        data={data}
        onRetry={() => setReloadKey((current) => current + 1)}
        onPageChange={handlePageChange}
      />
    </section>
  );
}
