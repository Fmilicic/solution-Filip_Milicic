import { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { productListDefaults } from '../api/products';
import { PagedProductResults } from '../components/PagedProductResults';
import { useDebouncedValue } from '../hooks/useDebouncedValue';
import { useSearchProductsQuery } from '../hooks/useProductQueries';
import './SearchPage.css';

function parsePositiveInt(value: string | null, fallback: number): number {
  const parsed = Number(value);
  return Number.isInteger(parsed) && parsed > 0 ? parsed : fallback;
}

export function SearchPage() {
  const [searchParams, setSearchParams] = useSearchParams();

  const queryFromUrl = searchParams.get('query') ?? '';
  const page = parsePositiveInt(searchParams.get('page'), productListDefaults.page);
  const pageSize = parsePositiveInt(searchParams.get('pageSize'), productListDefaults.pageSize);

  const [queryInput, setQueryInput] = useState(queryFromUrl);
  const debouncedQuery = useDebouncedValue(queryInput, 300);
  const activeQuery = searchParams.get('query')?.trim() ?? '';
  const validationError = activeQuery ? null : 'Enter a search query.';

  useEffect(() => {
    setQueryInput(queryFromUrl);
  }, [queryFromUrl]);

  useEffect(() => {
    const trimmed = debouncedQuery.trim();
    const currentQuery = searchParams.get('query') ?? '';

    if (!trimmed || trimmed === currentQuery) {
      return;
    }

    setSearchParams({
      query: trimmed,
      page: '1',
      pageSize: String(pageSize),
    });
  }, [debouncedQuery, pageSize, searchParams, setSearchParams]);

  const { data, isLoading, error, refetch } = useSearchProductsQuery(
    activeQuery,
    page,
    pageSize,
    Boolean(activeQuery),
  );

  function handlePageChange(nextPage: number) {
    setSearchParams({
      query: activeQuery,
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
        loading={isLoading}
        error={error instanceof Error ? error.message : error ? 'Search failed.' : null}
        data={validationError ? null : (data ?? null)}
        onRetry={() => void refetch()}
        onPageChange={handlePageChange}
        emptyMessage="No products matched your search."
      />
    </section>
  );
}
