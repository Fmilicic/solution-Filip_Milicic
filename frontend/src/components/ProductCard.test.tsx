import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { describe, expect, it } from 'vitest';
import { ProductCard } from '../components/ProductCard';

describe('ProductCard', () => {
  it('renders product name and price', () => {
    render(
      <MemoryRouter>
        <ProductCard
          product={{
            id: 1,
            name: 'Test Phone',
            price: 499,
            image: 'https://example.com/phone.jpg',
            shortDescription: 'A short description',
          }}
        />
      </MemoryRouter>,
    );

    expect(screen.getByRole('heading', { name: 'Test Phone' })).toBeInTheDocument();
    expect(screen.getByText(/\$499\.00/)).toBeInTheDocument();
  });
});
