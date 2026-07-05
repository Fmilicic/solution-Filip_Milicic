import { useCallback, useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { productListDefaults, searchProducts } from '../api/products';
import { PagedProductResults } from '../components/PagedProductResults';
import { useAuth } from '../context/AuthContext';
import { useDebouncedValue } from '../hooks/useDebouncedValue';
import type { PagedProducts } from '../types/api';
import './SearchPage.css';

function parsePositiveInt(value: string | null, fallback: number): number {
  const parsed = Number(value);
  return Number.isInteger(parsed) && parsed > 0 ? parsed : fallback;
}

export function SearchPage() {
  const { token } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();

  const queryFromUrl = searchParams.get('query') ?? '';
  const page = parsePositiveInt(searchParams.get('page'), productListDefaults.page);
  const pageSize = parsePositiveInt(searchParams.get('pageSize'), productListDefaults.pageSize);

  const [queryInput, setQueryInput] = useState(queryFromUrl);
  const debouncedQuery = useDebouncedValue(queryInput, 300);

  const [data, setData] = useState<PagedProducts | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [validationError, setValidationError] = useState<string | null>(null);
  const [reloadKey, setReloadKey] = useState(0);

  useEffect(() => {
    setQueryInput(queryFromUrl);
  }, [queryFromUrl]);

  useEffect(() => {
    const trimmed = debouncedQuery.trim();
    const currentQuery = searchParams.get('query') ?? '';

    if (!trimmed) {
      return;
    }

    if (trimmed !== currentQuery) {
      setSearchParams({
        query: trimmed,
        page: '1',
        pageSize: String(pageSize),
      });
    }
  }, [debouncedQuery, pageSize, searchParams, setSearchParams]);

  const loadSearch = useCallback(async () => {
    const query = searchParams.get('query')?.trim() ?? '';

    if (!query) {
      setValidationError('Enter a search query.');
      setData(null);
      setError(null);
      setLoading(false);
      return;
    }

    if (!token) {
      return;
    }

    setValidationError(null);
    setLoading(true);
    setError(null);

    try {
      const result = await searchProducts(query, page, pageSize, token);
      setData(result);
    } catch (loadError) {
      const message = loadError instanceof Error ? loadError.message : 'Search failed.';
      setData(null);
      setError(message);
    } finally {
      setLoading(false);
    }
  }, [page, pageSize, searchParams, token]);

  useEffect(() => {
    void loadSearch();
  }, [loadSearch, reloadKey]);

  function handlePageChange(nextPage: number) {
    const query = searchParams.get('query')?.trim() ?? '';

    setSearchParams({
      query,
      page: String(nextPage),
      pageSize: String(pageSize),
    });
  }

  return (
    <section className="page search-page">
      <header className="search-page__header">
        <h1>Search</h1>
        <p>Find products by name. Results sync to the URL.</p>
      </header>

      <label className="search-page__field">
        Search query
        <input
          type="search"
          value={queryInput}
          onChange={(event) => setQueryInput(event.target.value)}
          placeholder="e.g. phone"
        />
      </label>

      {validationError ? <p className="search-page__validation">{validationError}</p> : null}

      <PagedProductResults
        loading={loading}
        error={error}
        data={validationError ? null : data}
        onRetry={() => setReloadKey((current) => current + 1)}
        onPageChange={handlePageChange}
        emptyMessage="No products matched your search."
      />
    </section>
  );
}
