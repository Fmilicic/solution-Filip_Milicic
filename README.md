# Product Middleware

Fullstack monorepo. Backend complete. Frontend Phase 2 complete (API client).


To build & run:
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

Phase 2 done. API client in `src/api/client.ts`, types in `src/types/api.ts`.
```powershell
cd frontend
npm install
npm run dev
```

Dev: http://localhost:5173

## Manual check (backend)

1. `GET /api/products` (list items have `shortDescription` max 100 chars)
2. `GET /api/products/1`
3. `GET /api/categories`
4. Login, then search and filter with Bearer token
5. Admin create product, confirm in list, restart API, product still there