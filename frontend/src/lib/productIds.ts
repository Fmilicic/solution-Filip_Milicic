export const LOCAL_PRODUCT_ID_START = 10000;

export function isLocalProductId(id: number): boolean {
  return Number.isInteger(id) && id >= LOCAL_PRODUCT_ID_START;
}
