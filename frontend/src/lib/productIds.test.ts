import { describe, expect, it } from 'vitest';
import { isLocalProductId, LOCAL_PRODUCT_ID_START } from '../lib/productIds';

describe('isLocalProductId', () => {
  it('returns true for ids at or above the local threshold', () => {
    expect(isLocalProductId(LOCAL_PRODUCT_ID_START)).toBe(true);
    expect(isLocalProductId(LOCAL_PRODUCT_ID_START + 1)).toBe(true);
  });

  it('returns false for dummyjson ids', () => {
    expect(isLocalProductId(1)).toBe(false);
  });
});
