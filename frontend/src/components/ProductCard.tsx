import { Link, useLocation } from 'react-router-dom';
import type { ProductListItem } from '../types/api';
import { formatPrice } from '../lib/formatPrice';
import './ProductCard.css';

interface ProductCardProps {
  product: ProductListItem;
}

export function ProductCard({ product }: ProductCardProps) {
  const location = useLocation();

  return (
    <article className="product-card">
      <Link
        to={`/products/${product.id}`}
        state={{ from: `${location.pathname}${location.search}` }}
        className="product-card__link"
      >
        <img
          src={product.image}
          alt={product.name}
          className="product-card__image"
          loading="lazy"
        />
        <div className="product-card__body">
          <h2 className="product-card__name">{product.name}</h2>
          <p className="product-card__price">{formatPrice(product.price)}</p>
          <p className="product-card__description">{product.shortDescription}</p>
        </div>
      </Link>
    </article>
  );
}
