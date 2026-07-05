# Frontend

React + TypeScript + Vite. Phases 1-11 complete.

## Phases

| Phase | Done |
|-------|------|
| 1 | Vite + React + TypeScript |
| 2 | `.env`, types, API client |
| 3 | Router and app shell |
| 4 | Product list, pagination, URL sync |
| 5 | Product detail, back navigation |
| 6 | Login, auth context, protected routes |
| 7 | Search and filter with URL params |
| 8 | Admin CRUD (`/admin/products`) |
| 9 | TanStack Query caching |
| 10 | Vitest + React Testing Library |
| 11 | Full stack verification |

## Setup

```text
VITE_API_BASE_URL=http://localhost:5063
```

Copy from `.env.example` if needed.

## Run

```powershell
npm install
npm run dev
```

Backend must be on http://localhost:5063.

Test user: `emilys` / `emilyspass`

## Scripts

| Command | Purpose |
|---------|---------|
| `npm run dev` | Dev server (`:5173`) |
| `npm run build` | Production build |
| `npm test` | 5 unit/component tests |

## Routes

| Path | Auth |
|------|------|
| `/` | Public |
| `/products/:id` | Public |
| `/login` | Public |
| `/search` | Login required |
| `/filter` | Login required |
| `/admin/products` | Login required |

## AI usage (frontend and integration)

Example prompts per phase and aspect: [AI-USAGE.md](./AI-USAGE.md)
