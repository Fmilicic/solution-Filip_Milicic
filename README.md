# Product Middleware

Fullstack monorepo. Backend complete. Frontend phases 4-7 complete.

## Layout

```text
ProductMiddleware.slnx
backend/     .NET 8 API
frontend/    React SPA
```

## Backend

From repo root:

```powershell
dotnet build
dotnet test
dotnet run --project backend/ProductMiddleware.Api
```

| Item | Value |
|------|-------|
| API | http://localhost:5063 |
| Swagger | http://localhost:5063/swagger |
| Test user | `emilys` / `emilyspass` |
| Tests | 13 (integration tests call live DummyJSON) |

Open `ProductMiddleware.slnx` in Visual Studio 2026. Startup project: `ProductMiddleware.Api`, profile: `http`.

## Frontend

Phases 4-7 implemented:

- **4:** product list, pagination, URL sync, loading/empty/error
- **5:** product detail, back navigation with list state
- **6:** login, auth context, protected search/filter routes
- **7:** search (debounce + URL), filter (category + price + URL)

```powershell
cd frontend
npm install
npm run dev
```

Dev: http://localhost:5173

```text
VITE_API_BASE_URL=http://localhost:5063
```

## Manual check

1. `GET /api/products` via product list page
2. Open a product detail; back returns to the same list page
3. Login, then search and filter
4. Copy search/filter URL; refresh keeps results
5. Admin create on backend still works via Swagger if needed
