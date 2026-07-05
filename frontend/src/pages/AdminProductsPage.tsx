import { useState, type FormEvent } from 'react';
import { Link } from 'react-router-dom';
import { ErrorState } from '../components/PageStatus';
import { useAuth } from '../context/AuthContext';
import {
  useCreateProductMutation,
  useDeleteProductMutation,
  useUpdateProductMutation,
} from '../hooks/useProductQueries';
import { isLocalProductId, LOCAL_PRODUCT_ID_START } from '../lib/productIds';
import type { CreateProductRequest } from '../types/api';
import './AdminProductsPage.css';

const emptyForm: CreateProductRequest = {
  name: '',
  price: 0,
  description: '',
  imageUrl: '',
  category: '',
};

export function AdminProductsPage() {
  const { token } = useAuth();
  const [form, setForm] = useState<CreateProductRequest>(emptyForm);
  const [updateId, setUpdateId] = useState('');
  const [deleteId, setDeleteId] = useState('');
  const [message, setMessage] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const createMutation = useCreateProductMutation();
  const updateMutation = useUpdateProductMutation();
  const deleteMutation = useDeleteProductMutation();

  if (!token) {
    return null;
  }

  async function handleCreate(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setMessage(null);
    setError(null);

    try {
      const product = await createMutation.mutateAsync(form);
      setMessage(`Created product ${product.id}.`);
      setForm(emptyForm);
    } catch (createError) {
      setError(createError instanceof Error ? createError.message : 'Create failed.');
    }
  }

  async function handleUpdate(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setMessage(null);
    setError(null);

    const id = Number(updateId);
    if (!isLocalProductId(id)) {
      setError(`Update requires a local product id (${LOCAL_PRODUCT_ID_START}+).`);
      return;
    }

    try {
      const product = await updateMutation.mutateAsync({ id, data: form });
      setMessage(`Updated product ${product.id}.`);
    } catch (updateError) {
      setError(updateError instanceof Error ? updateError.message : 'Update failed.');
    }
  }

  async function handleDelete(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setMessage(null);
    setError(null);

    const id = Number(deleteId);
    if (!isLocalProductId(id)) {
      setError(`Delete requires a local product id (${LOCAL_PRODUCT_ID_START}+).`);
      return;
    }

    try {
      await deleteMutation.mutateAsync(id);
      setMessage(`Deleted product ${id}.`);
      setDeleteId('');
    } catch (deleteError) {
      setError(deleteError instanceof Error ? deleteError.message : 'Delete failed.');
    }
  }

  return (
    <section className="page admin-page">
      <header className="admin-page__header">
        <h1>Admin products</h1>
        <p>Manage local SQLite products (ids from {LOCAL_PRODUCT_ID_START}).</p>
      </header>

      {message ? <p className="admin-page__message" role="status">{message}</p> : null}
      {error ? <ErrorState message={error} /> : null}

      <form className="admin-page__form" onSubmit={handleCreate}>
        <h2>Create product</h2>
        <AdminFields form={form} onChange={setForm} />
        <button type="submit" className="admin-page__submit" disabled={createMutation.isPending}>
          {createMutation.isPending ? 'Creating...' : 'Create'}
        </button>
      </form>

      <form className="admin-page__form" onSubmit={handleUpdate}>
        <h2>Update local product</h2>
        <label className="admin-page__field">
          Product id
          <input
            type="number"
            min={LOCAL_PRODUCT_ID_START}
            value={updateId}
            onChange={(event) => setUpdateId(event.target.value)}
            required
          />
        </label>
        <AdminFields form={form} onChange={setForm} />
        <button type="submit" className="admin-page__submit" disabled={updateMutation.isPending}>
          {updateMutation.isPending ? 'Updating...' : 'Update'}
        </button>
      </form>

      <form className="admin-page__form admin-page__form--compact" onSubmit={handleDelete}>
        <h2>Delete local product</h2>
        <label className="admin-page__field">
          Product id
          <input
            type="number"
            min={LOCAL_PRODUCT_ID_START}
            value={deleteId}
            onChange={(event) => setDeleteId(event.target.value)}
            required
          />
        </label>
        <button type="submit" className="admin-page__submit" disabled={deleteMutation.isPending}>
          {deleteMutation.isPending ? 'Deleting...' : 'Delete'}
        </button>
      </form>

      <p className="admin-page__hint">
        After create, open the product from the{' '}
        <Link to="/">product list</Link>.
      </p>
    </section>
  );
}

interface AdminFieldsProps {
  form: CreateProductRequest;
  onChange: (form: CreateProductRequest) => void;
}

function AdminFields({ form, onChange }: AdminFieldsProps) {
  return (
    <>
      <label className="admin-page__field">
        Name
        <input
          type="text"
          value={form.name}
          onChange={(event) => onChange({ ...form, name: event.target.value })}
          required
        />
      </label>

      <label className="admin-page__field">
        Price
        <input
          type="number"
          min="0"
          step="0.01"
          value={form.price}
          onChange={(event) => onChange({ ...form, price: Number(event.target.value) })}
          required
        />
      </label>

      <label className="admin-page__field">
        Description
        <textarea
          value={form.description}
          onChange={(event) => onChange({ ...form, description: event.target.value })}
          required
        />
      </label>

      <label className="admin-page__field">
        Image URL
        <input
          type="url"
          value={form.imageUrl}
          onChange={(event) => onChange({ ...form, imageUrl: event.target.value })}
          required
        />
      </label>

      <label className="admin-page__field">
        Category
        <input
          type="text"
          value={form.category}
          onChange={(event) => onChange({ ...form, category: event.target.value })}
          required
        />
      </label>
    </>
  );
}
