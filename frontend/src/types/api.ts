export interface ProductListItem {
  id: number;
  image: string;
  name: string;
  price: number;
  shortDescription: string;
}

export interface PagedProducts {
  items: ProductListItem[];
  page: number;
  pageSize: number;
  total: number;
}

export interface ProductDetail {
  id: number;
  image: string;
  name: string;
  price: number;
  description: string;
  category: string;
}

export interface Category {
  slug: string;
  name: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresAt: string;
  username: string;
}

export interface ApiError {
  message: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface CreateProductRequest {
  name: string;
  price: number;
  description: string;
  imageUrl: string;
  category: string;
}

export interface UpdateProductRequest {
  name: string;
  price: number;
  description: string;
  imageUrl: string;
  category: string;
}
