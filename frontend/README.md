# Frontend

React + TypeScript + Vite.

## Current implementation: phases 4-7

| Phase | Done |
|-------|------|
| 4 | Product list with pagination, loading/empty/error, URL `page` and `pageSize` |
| 5 | Product detail page, back link preserves list state |
| 6 | Login form, auth context, token in sessionStorage, protected routes |
| 7 | Search with debounce and URL `query`; filter by category/price with URL params |

## Setup

```text
VITE_API_BASE_URL=http://localhost:5063
```

Copy `.env.example` to `.env` if needed.

## Run

```powershell
cd frontend
npm install
npm run dev
```

Dev server: http://localhost:5173

Backend must be running on http://localhost:5063.

Test user: `emilys` / `emilyspass`

## Scripts

| Command | Purpose |
|---------|---------|
| `npm run dev` | Dev server |
| `npm run build` | Production build |
