# Product Middleware

Fullstack monorepo. Backend and frontend complete (phases 1-11).

## Layout

```text
ProductMiddleware.slnx
backend/     .NET 8 API
frontend/    React SPA
```

## Prerequisites

- .NET 8 SDK
- Node.js 20+
- Internet (DummyJSON for live product data)

## Run full stack

Terminal 1 (API):

```powershell
cd REPO_ROOT
dotnet run --project backend/ProductMiddleware.Api
```

Terminal 2 (SPA):

```powershell
cd REPO_ROOT/frontend
npm install (on first use)
npm run dev
```

| Service | URL |
|---------|-----|
| API | http://localhost:5063 |
| Swagger | http://localhost:5063/swagger |
| SPA | http://localhost:5173 |

Frontend config (`frontend/.env`):

```text
VITE_API_BASE_URL=http://localhost:5063
```

Test user: `emilys` / `emilyspass`

## Tests and build

From repo root:

```powershell
dotnet build
dotnet test
```

From `frontend/`:

```powershell
npm test
npm run build
```

| Suite | Count |
|-------|-------|
| Backend (`dotnet test`) | 13 |
| Frontend (`npm test`) | 5 |

## Backend

Public: product list/detail, categories, login.

Protected: search, filter, admin CRUD.

SQLite local products (ids from 10000), DummyJSON aggregation, JWT auth, 3 min search/filter cache.

Open `ProductMiddleware.slnx` in Visual Studio 2026. Startup: `ProductMiddleware.Api`, profile `http`.

### Example AI prompts (backend)

**Layered API (Application + Infrastructure)**

```text
Scaffold a .NET 8 Web API with Application, Infrastructure, Api, and Tests projects.
Put domain models and IProductSource in Application; no HttpClient or EF in controllers.
Map DummyJSON title/thumbnail to domain Name/ImageUrl in Infrastructure only.
```

**DummyJSON + JWT**

```text
Implement IProductSource using HttpClient against dummyjson.com.
Add POST /api/auth/login that validates via dummyjson auth/login then issues a local JWT.
Protect GET /api/products/search and /api/products/filter with [Authorize].
Test user: emilys / emilyspass.
```

**SQLite, aggregation, CORS**

```text
Add EF Core SQLite for admin products (ids from 10000).
Merge DatabaseProductSource and DummyJsonProductSource in AggregatingProductSource; DB wins on GetById.
Enable CORS for http://localhost:5173 before UseAuthentication so the React SPA can call the API.
```

More frontend and integration prompts: `frontend/AI-USAGE.md`.

## Frontend

| Phase | Done |
|-------|------|
| 1 | Vite + React + TypeScript scaffold |
| 2 | `.env`, API types, fetch client |
| 3 | Router, layout, placeholder routes |
| 4 | Product list, pagination, URL params |
| 5 | Product detail, back link |
| 6 | Login, auth context, protected routes |
| 7 | Search (debounce + URL), filter (category/price + URL) |
| 8 | Admin CRUD at `/admin/products` |
| 9 | TanStack Query caching |
| 10 | Vitest + React Testing Library |
| 11 | Full stack verification |

## Manual E2E checklist

1. `GET /api/products` via product list; pagination updates URL
2. Open product detail; **Back to list** keeps list query params
3. `/search` without login redirects to `/login`
4. Login; search returns results; URL copy/refresh works
5. Filter by category and/or price; URL copy/refresh works
6. Admin create (id ≥ 10000); product appears on list
7. Restart API; local admin product still in list
8. Browser console: no CORS errors from `localhost:5173`
9. Narrow viewport (~375px): layout usable
10. `dotnet test` and `npm test` pass
