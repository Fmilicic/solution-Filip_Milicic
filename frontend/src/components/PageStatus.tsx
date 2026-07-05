import './PageStatus.css';

interface LoadingStateProps {
  message?: string;
}

export function LoadingState({ message = 'Loading products...' }: LoadingStateProps) {
  return (
    <div className="page-status" role="status" aria-live="polite">
      <div className="page-status__skeleton-grid" aria-hidden="true">
        {Array.from({ length: 6 }, (_, index) => (
          <div key={index} className="page-status__skeleton-card" />
        ))}
      </div>
      <p className="page-status__message">{message}</p>
    </div>
  );
}

interface EmptyStateProps {
  message?: string;
}

export function EmptyState({ message = 'No products found.' }: EmptyStateProps) {
  return (
    <div className="page-status" role="status">
      <p className="page-status__message">{message}</p>
    </div>
  );
}

interface ErrorStateProps {
  message: string;
  onRetry?: () => void;
}

export function ErrorState({ message, onRetry }: ErrorStateProps) {
  return (
    <div className="page-status page-status--error" role="alert">
      <p className="page-status__message">{message}</p>
      {onRetry ? (
        <button type="button" className="page-status__button" onClick={onRetry}>
          Try again
        </button>
      ) : null}
    </div>
  );
}
