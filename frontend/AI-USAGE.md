# AI usage (frontend and integration)

The fullstack task allows AI tools. This file lists some prompts for various aspects of the frontend build.

Backend contract reference: API at `http://localhost:5063`, test user `emilys` / `emilyspass`, CORS allows `http://localhost:5173`.

---

## Phase 1: Vite scaffold

**Aspect:** project setup

```text
Create a Vite React TypeScript app in frontend/ for a product catalog SPA.
Use strict TypeScript, no any.
Add react-router-dom.
Default dev server mustr remain on port 5173.
Do not call DummyJSON from the browser; all data goes through our backend.
```

---

## Phase 2: Env and API client

**Aspect:** environment variable

```text
Add VITE_API_BASE_URL=http://localhost:5063 in .env.example.
Add vite-env.d.ts so import.meta.env.VITE_API_BASE_URL is typed.
Explain that Vite only exposes vars prefixed with VITE_.
```

**Aspect:** TypeScript shapes (integration)

```text
Create src/types/api.ts mirroring ASP.NET API JSON:
a) PagedProducts: items, page, pageSize, total
b) ProductListItem: id, image, name, price, shortDescription
c) ProductDetail: id, image, name, price, description, category
d) LoginResponse: accessToken, expiresAt, username
e) ApiError: message
Do not use any.
```

**Aspect:** fetch wrapper

```text
Create src/api/client.ts with:
a) getBaseUrl() from VITE_API_BASE_URL
b) apiGet, apiGetAuth (Bearer header), apiPost, apiPut, apiDelete
On non-OK responses, parse { message } from the backend and throw a typed error.
No direct calls to dummyjson.com.
```

---

## Phase 3: Router and shell

**Aspect:** routes

```text
Provide examples of React Router routes:
/ list, /products/:id detail, /login, /search and /filter protected.
Use a shared AppLayout with nav links and an Outlet.
Protected routes redirect to /login when no token is in sessionStorage.
```

---

## Phase 4: Product list (integration)

**Aspect:** public list endpoint

```text
Build ProductListPage that calls GET /api/products?page=&pageSize=.
Sync page and pageSize to URL search params so refresh keeps state.
Show loading skeleton, empty state, and error with retry.
Render a responsive card grid: image, name, price, shortDescription (max 100 chars from API).
Each card links to /products/{id} and passes location state so Back can restore the list URL.
```

**Aspect:** list item id (backend alignment)

```text
Update ProductListItem type and ProductCard to use product.id in the link.
```

---

## Phase 7: Search and filter (integration)

**Aspect:** search endpoint

```text
Build SearchPage that calls GET /api/products/search?query=&page=&pageSize= with Bearer token.
Debounce the input ~300ms before updating the URL query param.
Do not call the API with an empty query (backend returns 400).
Reuse the same paged card grid and pagination as the list page.
```

**Aspect:** filter + categories

```text
Build FilterPage:
a) Load categories from public GET /api/categories for the dropdown
b) On submit call GET /api/products/filter?category=&minPrice=&maxPrice=&page=&pageSize= with Bearer token
c) Validate at least one filter param and minPrice <= maxPrice before calling the API (match backend rules)
d) Persist filter values in the URL so refresh and share links work
```

**Aspect:** CORS debugging

```text
The React app runs on localhost:5173 and the API on localhost:5063.
If fetch fails with a CORS error in the browser but curl works, what should I check in the backend Cors config and middleware order?
```

---

## Phase 9: TanStack Query (integration)

**Aspect:** caching and invalidation

```text
Refactor product list, detail, search, filter, and categories to use TanStack Query.
Use stable query keys that include page, pageSize, query, and filter params.
Set staleTime ~60s for product lists; longer for categories.
After admin create/update/delete invalidate all product queries so list and search show new SQLite items without a full page reload.
Keep fetch logic in src/api/; hooks should wrap the client.
```

---

## Phase 10: Tests

**Aspect:** component tests without live API

```text
Add Vitest and React Testing Library.
Test ProductCard renders name and formatted price with MemoryRouter.
Test LoginPage calls auth context login on submit; mock useAuth, do not hit localhost:5063.
Test isLocalProductId for ids below and above 10000.
```

---

## Phase 11: Full stack verification

**Aspect:** integration checklist

```text
Give me a manual E2E checklist for:
frontend on :5173, backend on :5063,
public list/detail, login, search, filter, admin create, CORS, URL state on refresh.
Include dotnet test and npm test commands.
```
