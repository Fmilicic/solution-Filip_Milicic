import { Link, useLocation, useParams } from 'react-router-dom';
import { ErrorState, LoadingState } from '../components/PageStatus';
import { useProductQuery } from '../hooks/useProductQueries';
import { formatPrice } from '../lib/formatPrice';
import './ProductDetailPage.css';

export function ProductDetailPage() {
  const { id } = useParams();
  const location = useLocation();
  const productId = Number(id);
  const backTarget =
    typeof location.state === 'object' &&
    location.state !== null &&
    'from' in location.state &&
    typeof location.state.from === 'string'
      ? location.state.from
      : '/';

  const { data: product, isLoading, error, refetch } = useProductQuery(productId);

  if (!Number.isInteger(productId) || productId < 1) {
    return (
      <section className="page product-detail-page">
        <ErrorState message="Invalid product id." />
      </section>
    );
  }

  return (
    <section className="page product-detail-page">
      <Link to={backTarget} className="product-detail-page__back">
        Back to list
      </Link>

      {isLoading ? <LoadingState message="Loading product..." /> : null}

      {!isLoading && error ? (
        <ErrorState
          message={error instanceof Error ? error.message : 'Failed to load product.'}
          onRetry={() => void refetch()}
        />
      ) : null}

      {!isLoading && !error && product ? (
        <article className="product-detail-page__content">
          <img
            src={product.image}
            alt={product.name}
            className="product-detail-page__image"
          />
          <div className="product-detail-page__info">
            <h1>{product.name}</h1>
            <p className="product-detail-page__price">{formatPrice(product.price)}</p>
            <p className="product-detail-page__category">Category: {product.category}</p>
            <p className="product-detail-page__description">{product.description}</p>
          </div>
        </article>
      ) : null}
    </section>
  );
}
