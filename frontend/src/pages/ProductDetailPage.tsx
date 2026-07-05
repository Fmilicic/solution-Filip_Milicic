import { useParams } from 'react-router-dom';

export function ProductDetailPage() {
  const { id } = useParams();

  return (
    <section className="page">
      <h1>Product detail</h1>
      <p>Full product view for id <code>{id}</code>. Implemented in Phase 5.</p>
      <p className="page__meta">Route: /products/:id</p>
    </section>
  );
}
