# Allocation Run History and Exception Review

Sign in, browse saved allocation runs (newest first), and review which replenishment gaps are filled, partially filled, or unfilled.

**Stack:** Angular 21 · ASP.NET Core (.NET 10) with EF Core, Identity and JWT · MySQL 8

## Prerequisites

.NET SDK 10 · Node.js 20.19+ or 22.12+ · MySQL 8

## Quick start (clean checkout)

**1. Database**

```sql
CREATE DATABASE IF NOT EXISTS allocation_review;
CREATE USER IF NOT EXISTS 'allocation_app'@'localhost' IDENTIFIED BY 'CHOOSE_A_DB_PASSWORD';
GRANT ALL PRIVILEGES ON allocation_review.* TO 'allocation_app'@'localhost';
```

**2. Backend secrets** (stored in user-secrets, never in the repo; use the plain values, no angle brackets)

```bash
cd backend
dotnet user-secrets set 'ConnectionStrings:DefaultConnection' 'Server=localhost;Port=3306;Database=allocation_review;User=allocation_app;Password=CHOOSE_A_DB_PASSWORD;'
dotnet user-secrets set 'Jwt:Key' 'A_RANDOM_STRING_OF_AT_LEAST_32_CHARACTERS'
dotnet user-secrets set 'SeedUsers:Admin:Password'  'CHOOSE_AN_ADMIN_PASSWORD'
dotnet user-secrets set 'SeedUsers:Viewer:Password' 'CHOOSE_A_VIEWER_PASSWORD'
```

**3. Run the backend** (in Development it applies migrations and seeds data on startup)

```bash
cd backend
dotnet run            # http://localhost:5216
```

To migrate manually: `dotnet tool restore && dotnet ef database update`.

**4. Run the frontend**

```bash
cd frontend
npm i
npm start             # http://localhost:4200
```

## Demo users

| User | Role | Behaviour |
|---|---|---|
| `admin` | Admin | Can view history and run review |
| `viewer` | Viewer | Can sign in; review endpoints return `403`, UI shows "Access denied" |

Passwords are the ones set in step 2. To recreate users, delete them from `AspNetUsers` / `AspNetUserRoles` and restart the API.

## Seed data

| Run | Date | Gap | Store • Brand • Type | Shortfall | Batches |
|---|---|---|---|---|---|
| R-102 | 14 Sep 2026 | G-01 | Store A • Brand X • Frames | 5 | B-401 (2), B-402 (1) |
| R-102 | 14 Sep 2026 | G-02 | Store B • Brand X • Frames | 2 | B-501 (2) |
| R-102 | 14 Sep 2026 | G-03 | Store C • Brand Y • Sunglasses | 4 | none |
| R-101 | 13 Sep 2026 | G-01 | Store D • Brand Z • Frames | 1 | B-601 (1) |

Expected for R-102: 3 gaps, 5 allocated units → G-01 Partially filled (3 allocated, 2 remaining), G-02 Filled, G-03 Unfilled (4 remaining). The exceptions filter shows G-01 and G-03.

Seeding is idempotent and runs only in Development.

## API

All `/api/runs` endpoints need `Authorization: Bearer TOKEN` and the Admin role.

| Method | Path | Notes |
|---|---|---|
| `POST` | `/api/auth/login` | `{ username, password }` → `{ token, username, role, expiresAt }` |
| `GET` | `/api/runs?page=1&pageSize=10` | Newest first (date desc, then id desc); `pageSize` 1–50 |
| `GET` | `/api/runs/{runIdentifier}?exceptionsOnly=false` | Run review; the filter affects gap rows only, not totals |

Status codes: `400` invalid pagination or login body · `401` missing, invalid or expired token, or bad credentials · `403` non-Admin · `404` unknown run.

## Calculation rules

Per gap (`AllocationCalculationService`): shortfall is counted once and rounded up; allocated = sum of its batch quantities; remaining = `max(0, shortfall − allocated)`. Status is **Filled** when remaining is 0, **Unfilled** when none were allocated, otherwise **Partially filled**. The run total is the sum of allocated units across its gaps.

## Configuration

Standard ASP.NET Core order: `appsettings.json` → `appsettings.{Environment}.json` → user-secrets (Development) → environment variables. In environment variables, use `__` instead of `:` (for example `Jwt__Key`).

| Setting | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | MySQL connection string |
| `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpiryMinutes` | JWT signing and validation (key 32+ characters) |
| `Cors:AllowedOrigins` | Allowed browser origins |
| `SeedUsers:*` | Demo users (Development only) |

- **Development:** `appsettings.Development.json` for non-secret values, user-secrets for the rest.
- **Test:** the test host overrides settings in code (test database, fixed test key). No client systems or real data.
- **Production:** environment variables or a secret manager only. No demo data. Run migrations as a deployment step (`dotnet ef database update`, or `dotnet ef migrations script --idempotent`) with a schema-rights user; the runtime user needs only SELECT, INSERT and UPDATE.

The API refuses to start if the connection string or JWT settings are missing or invalid.
