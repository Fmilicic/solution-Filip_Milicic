import './Pagination.css';

interface PaginationProps {
  page: number;
  pageSize: number;
  total: number;
  onPageChange: (page: number) => void;
}

export function Pagination({ page, pageSize, total, onPageChange }: PaginationProps) {
  const totalPages = Math.max(1, Math.ceil(total / pageSize));
  const canGoBack = page > 1;
  const canGoForward = page < totalPages;

  return (
    <nav className="pagination" aria-label="Product pagination">
      <button
        type="button"
        className="pagination__button"
        onClick={() => onPageChange(page - 1)}
        disabled={!canGoBack}
      >
        Previous
      </button>

      <p className="pagination__info">
        Page {page} of {totalPages} ({total} products)
      </p>

      <button
        type="button"
        className="pagination__button"
        onClick={() => onPageChange(page + 1)}
        disabled={!canGoForward}
      >
        Next
      </button>
    </nav>
  );
}
