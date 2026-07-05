import { EmptyState, ErrorState, LoadingState } from './PageStatus';
import { Pagination } from './Pagination';
import { ProductCard } from './ProductCard';
import type { PagedProducts } from '../types/api';
import './PagedProductResults.css';

interface PagedProductResultsProps {
  loading: boolean;
  error: string | null;
  data: PagedProducts | null;
  onRetry: () => void;
  onPageChange: (page: number) => void;
  emptyMessage?: string;
}

export function PagedProductResults({
  loading,
  error,
  data,
  onRetry,
  onPageChange,
  emptyMessage,
}: PagedProductResultsProps) {
  if (loading) {
    return <LoadingState />;
  }

  if (error) {
    return <ErrorState message={error} onRetry={onRetry} />;
  }

  if (!data || data.items.length === 0) {
    return <EmptyState message={emptyMessage} />;
  }

  return (
    <>
      <div className="paged-product-results__grid">
        {data.items.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>

      <Pagination
        page={data.page}
        pageSize={data.pageSize}
        total={data.total}
        onPageChange={onPageChange}
      />
    </>
  );
}
