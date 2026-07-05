import { describe, expect, it } from 'vitest';
import { formatPrice } from '../lib/formatPrice';

describe('formatPrice', () => {
  it('formats a number as USD currency', () => {
    const formatted = formatPrice(19.99);
    expect(formatted).toContain('19.99');
  });
});
