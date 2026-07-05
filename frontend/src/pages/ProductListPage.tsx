import { useSearchParams } from 'react-router-dom';
import { productListDefaults } from '../api/products';
import { PagedProductResults } from '../components/PagedProductResults';
import { useProductsQuery } from '../hooks/useProductQueries';
import './ProductListPage.css';

function parsePositiveInt(value: string | null, fallback: number): number {
  const parsed = Number(value);
  return Number.isInteger(parsed) && parsed > 0 ? parsed : fallback;
}

export function ProductListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const page = parsePositiveInt(searchParams.get('page'), productListDefaults.page);
  const pageSize = parsePositiveInt(searchParams.get('pageSize'), productListDefaults.pageSize);

  const { data, isLoading, error, refetch } = useProductsQuery(page, pageSize);

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
        loading={isLoading}
        error={error instanceof Error ? error.message : error ? 'Failed to load products.' : null}
        data={data ?? null}
        onRetry={() => void refetch()}
        onPageChange={handlePageChange}
      />
    </section>
  );
}
