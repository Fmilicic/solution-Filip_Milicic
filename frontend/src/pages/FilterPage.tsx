import { useEffect, useState, type FormEvent } from 'react';
import { useSearchParams } from 'react-router-dom';
import { productListDefaults, validateFilterInput } from '../api/products';
import { PagedProductResults } from '../components/PagedProductResults';
import { ErrorState, LoadingState } from '../components/PageStatus';
import { useCategoriesQuery, useFilterProductsQuery } from '../hooks/useProductQueries';
import './FilterPage.css';

function parsePositiveInt(value: string | null, fallback: number): number {
  const parsed = Number(value);
  return Number.isInteger(parsed) && parsed > 0 ? parsed : fallback;
}

function parseOptionalPrice(value: string): number | undefined {
  return value.trim() === '' ? undefined : Number(value);
}

export function FilterPage() {
  const [searchParams, setSearchParams] = useSearchParams();

  const page = parsePositiveInt(searchParams.get('page'), productListDefaults.page);
  const pageSize = parsePositiveInt(searchParams.get('pageSize'), productListDefaults.pageSize);

  const [category, setCategory] = useState(searchParams.get('category') ?? '');
  const [minPrice, setMinPrice] = useState(searchParams.get('minPrice') ?? '');
  const [maxPrice, setMaxPrice] = useState(searchParams.get('maxPrice') ?? '');
  const [validationError, setValidationError] = useState<string | null>(null);
  const [submitted, setSubmitted] = useState(() => {
    return Boolean(
      searchParams.get('category') ||
        searchParams.get('minPrice') ||
        searchParams.get('maxPrice'),
    );
  });

  const {
    data: categories,
    isLoading: categoriesLoading,
    error: categoriesError,
    refetch: refetchCategories,
  } = useCategoriesQuery();

  const parsedMin = parseOptionalPrice(minPrice);
  const parsedMax = parseOptionalPrice(maxPrice);
  const normalizedCategory = category.trim() || undefined;

  const {
    data,
    isLoading,
    error,
    refetch,
  } = useFilterProductsQuery(
    normalizedCategory,
    parsedMin,
    parsedMax,
    page,
    pageSize,
    submitted && !validationError,
  );

  useEffect(() => {
    if (!submitted) {
      return;
    }

    setValidationError(validateFilterInput(category, minPrice, maxPrice));
  }, [category, maxPrice, minPrice, submitted]);

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const validationMessage = validateFilterInput(category, minPrice, maxPrice);
    if (validationMessage) {
      setValidationError(validationMessage);
      return;
    }

    setValidationError(null);

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
        <ErrorState
          message={
            categoriesError instanceof Error
              ? categoriesError.message
              : 'Failed to load categories.'
          }
          onRetry={() => void refetchCategories()}
        />
      ) : null}

      {!categoriesLoading && !categoriesError ? (
        <form className="filter-page__form" onSubmit={handleSubmit}>
          <label className="filter-page__field">
            Category
            <select value={category} onChange={(event) => setCategory(event.target.value)}>
              <option value="">Any category</option>
              {(categories ?? []).map((item) => (
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
          loading={isLoading}
          error={error instanceof Error ? error.message : error ? 'Filter failed.' : null}
          data={validationError ? null : (data ?? null)}
          onRetry={() => void refetch()}
          onPageChange={handlePageChange}
          emptyMessage="No products matched your filter."
        />
      ) : null}
    </section>
  );
}
