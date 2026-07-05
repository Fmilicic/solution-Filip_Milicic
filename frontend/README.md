# Frontend

React + TypeScript + Vite.

## Current implementation: phases 4-10

| Phase | Done |
|-------|------|
| 4 | Product list, pagination, URL params, loading/empty/error |
| 5 | Product detail, back link preserves list state |
| 6 | Login, auth context, protected routes |
| 7 | Search (debounce + URL), filter (category/price + URL) |
| 8 | Admin CRUD at `/admin/products` (local ids 10000+) |
| 9 | TanStack Query caching and cache invalidation after admin writes |
| 10 | Vitest + React Testing Library tests |

## Setup

```text
VITE_API_BASE_URL=http://localhost:5063
```

## Run

```powershell
cd frontend
npm install
npm run dev
```

Test user: `emilys` / `emilyspass`

## Scripts

| Command | Purpose |
|---------|---------|
| `npm run dev` | Dev server |
| `npm run build` | Production build |
| `npm test` | Unit/component tests |
