import { useCallback, useEffect, useState } from 'react';
import { Link, useLocation, useParams } from 'react-router-dom';
import { fetchProductById } from '../api/products';
import { ErrorState, LoadingState } from '../components/PageStatus';
import type { ProductDetail } from '../types/api';
import './ProductDetailPage.css';

function formatPrice(price: number): string {
  return new Intl.NumberFormat(undefined, {
    style: 'currency',
    currency: 'USD',
  }).format(price);
}

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

  const [product, setProduct] = useState<ProductDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [reloadKey, setReloadKey] = useState(0);

  const loadProduct = useCallback(async () => {
    if (!Number.isInteger(productId) || productId < 1) {
      setError('Invalid product id.');
      setProduct(null);
      setLoading(false);
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const result = await fetchProductById(productId);
      setProduct(result);
    } catch (loadError) {
      const message = loadError instanceof Error ? loadError.message : 'Failed to load product.';
      setProduct(null);
      setError(message);
    } finally {
      setLoading(false);
    }
  }, [productId]);

  useEffect(() => {
    void loadProduct();
  }, [loadProduct, reloadKey]);

  return (
    <section className="page product-detail-page">
      <Link to={backTarget} className="product-detail-page__back">
        Back to list
      </Link>

      {loading ? <LoadingState message="Loading product..." /> : null}

      {!loading && error ? (
        <ErrorState message={error} onRetry={() => setReloadKey((current) => current + 1)} />
      ) : null}

      {!loading && !error && product ? (
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
