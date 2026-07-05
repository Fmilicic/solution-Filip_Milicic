# Product Middleware

Fullstack monorepo. Backend complete. Frontend phases 4-10 complete.

## Backend

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
| Tests | 13 |

## Frontend

Phases 4-10:

- **4:** product list, pagination, URL sync
- **5:** product detail, back navigation
- **6:** login, auth context, protected routes
- **7:** search and filter with URL params
- **8:** admin create/update/delete for local products
- **9:** TanStack Query client caching
- **10:** frontend tests (`npm test`)

```powershell
cd frontend
npm install
npm run dev
npm test
```

Dev: http://localhost:5173

## Manual check

1. Product list and detail
2. Login; search and filter
3. Admin create; confirm on list after cache refresh
4. `npm test` and `dotnet test` both pass
