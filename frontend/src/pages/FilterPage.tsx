import { useCallback, useEffect, useState, type FormEvent } from 'react';
import { useSearchParams } from 'react-router-dom';
import { fetchCategories } from '../api/categories';
import {
  filterProducts,
  productListDefaults,
  validateFilterInput,
} from '../api/products';
import { PagedProductResults } from '../components/PagedProductResults';
import { ErrorState, LoadingState } from '../components/PageStatus';
import { useAuth } from '../context/AuthContext';
import type { Category, PagedProducts } from '../types/api';
import './FilterPage.css';

function parsePositiveInt(value: string | null, fallback: number): number {
  const parsed = Number(value);
  return Number.isInteger(parsed) && parsed > 0 ? parsed : fallback;
}

export function FilterPage() {
  const { token } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();

  const page = parsePositiveInt(searchParams.get('page'), productListDefaults.page);
  const pageSize = parsePositiveInt(searchParams.get('pageSize'), productListDefaults.pageSize);

  const [category, setCategory] = useState(searchParams.get('category') ?? '');
  const [minPrice, setMinPrice] = useState(searchParams.get('minPrice') ?? '');
  const [maxPrice, setMaxPrice] = useState(searchParams.get('maxPrice') ?? '');

  const [categories, setCategories] = useState<Category[]>([]);
  const [categoriesLoading, setCategoriesLoading] = useState(true);
  const [categoriesError, setCategoriesError] = useState<string | null>(null);

  const [data, setData] = useState<PagedProducts | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [validationError, setValidationError] = useState<string | null>(null);
  const [reloadKey, setReloadKey] = useState(0);
  const [submitted, setSubmitted] = useState(() => {
    return Boolean(
      searchParams.get('category') ||
        searchParams.get('minPrice') ||
        searchParams.get('maxPrice'),
    );
  });

  useEffect(() => {
    let cancelled = false;

    async function loadCategories() {
      setCategoriesLoading(true);
      setCategoriesError(null);

      try {
        const result = await fetchCategories();
        if (!cancelled) {
          setCategories(result);
        }
      } catch (loadError) {
        if (!cancelled) {
          const message =
            loadError instanceof Error ? loadError.message : 'Failed to load categories.';
          setCategoriesError(message);
        }
      } finally {
        if (!cancelled) {
          setCategoriesLoading(false);
        }
      }
    }

    void loadCategories();

    return () => {
      cancelled = true;
    };
  }, []);

  const runFilter = useCallback(async () => {
    if (!token) {
      return;
    }

    const validationMessage = validateFilterInput(category, minPrice, maxPrice);
    if (validationMessage) {
      setValidationError(validationMessage);
      setData(null);
      setError(null);
      setLoading(false);
      return;
    }

    setValidationError(null);
    setLoading(true);
    setError(null);

    const parsedMin = minPrice.trim() === '' ? undefined : Number(minPrice);
    const parsedMax = maxPrice.trim() === '' ? undefined : Number(maxPrice);
    const normalizedCategory = category.trim() || undefined;

    try {
      const result = await filterProducts(
        normalizedCategory,
        parsedMin,
        parsedMax,
        page,
        pageSize,
        token,
      );
      setData(result);
    } catch (loadError) {
      const message = loadError instanceof Error ? loadError.message : 'Filter failed.';
      setData(null);
      setError(message);
    } finally {
      setLoading(false);
    }
  }, [category, maxPrice, minPrice, page, pageSize, token]);

  useEffect(() => {
    if (submitted) {
      void runFilter();
    }
  }, [runFilter, reloadKey, submitted, page, pageSize]);

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const nextParams: Record<string, string> = {
      page: '1',
      pageSize: String(pageSize),
    };

    if (category.trim()) {
      nextParams.category = category.trim();
    }

    if (minPrice.trim()) {
      nextParams.minPrice = minPrice.trim();
    }

    if (maxPrice.trim()) {
      nextParams.maxPrice = maxPrice.trim();
    }

    setSearchParams(nextParams);
    setSubmitted(true);
  }

  function handlePageChange(nextPage: number) {
    const nextParams: Record<string, string> = {
      page: String(nextPage),
      pageSize: String(pageSize),
    };

    if (category.trim()) {
      nextParams.category = category.trim();
    }

    if (minPrice.trim()) {
      nextParams.minPrice = minPrice.trim();
    }

    if (maxPrice.trim()) {
      nextParams.maxPrice = maxPrice.trim();
    }

    setSearchParams(nextParams);
  }

  return (
    <section className="page filter-page">
      <header className="filter-page__header">
        <h1>Filter</h1>
        <p>Filter by category and price range. Results sync to the URL.</p>
      </header>

      {categoriesLoading ? <LoadingState message="Loading categories..." /> : null}

      {!categoriesLoading && categoriesError ? (
        <ErrorState message={categoriesError} onRetry={() => window.location.reload()} />
      ) : null}

      {!categoriesLoading && !categoriesError ? (
        <form className="filter-page__form" onSubmit={handleSubmit}>
          <label className="filter-page__field">
            Category
            <select value={category} onChange={(event) => setCategory(event.target.value)}>
              <option value="">Any category</option>
              {categories.map((item) => (
                <option key={item.slug} value={item.slug}>
                  {item.name}
                </option>
              ))}
            </select>
          </label>

          <label className="filter-page__field">
            Min price
            <input
              type="number"
              min="0"
              step="0.01"
              value={minPrice}
              onChange={(event) => setMinPrice(event.target.value)}
            />
          </label>

          <label className="filter-page__field">
            Max price
            <input
              type="number"
              min="0"
              step="0.01"
              value={maxPrice}
              onChange={(event) => setMaxPrice(event.target.value)}
            />
          </label>

          {validationError ? <p className="filter-page__validation">{validationError}</p> : null}

          <button type="submit" className="filter-page__submit">
            Apply filter
          </button>
        </form>
      ) : null}

      {submitted ? (
        <PagedProductResults
          loading={loading}
          error={error}
          data={data}
          onRetry={() => setReloadKey((current) => current + 1)}
          onPageChange={handlePageChange}
          emptyMessage="No products matched your filter."
        />
      ) : null}
    </section>
  );
}
