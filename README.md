# DoohClick - Digital out-of-home Ad Management System

## Summary
DoohClick is a Digital out-of-home advertisement management system that allows administartors to manage ads, campaigns, and screens, and enables screens to retrieve their scheduled ad playlists dynamically.

The system consist of:

- A backend REST API built with ASP.NET Core
- A frontend application built with Angular 17
- A SQL Server database

<hr>

## Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 8)
- **Frontend:** Angular CLI 17, PrimeNG
- **Database:** Microsoft SQL Server (Database-first approach)
- **ORM / DB:** EF Core + Dapper + Stored Procedures
- **Authentication:** JWT
- **API Documentation:** Swagger

<hr>

## Setup & Running Locally

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js + npm](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
- SQL Server (local or remote)

---

### 1. Clone the Repository

```bash
git clone 
cd DoohClick
```

---

### 2. Backend Setup

#### 2a. Configure User Secrets

Navigate to the API project folder, then run:

```bash
cd DoohClick.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Local" "Server=;Database=DoohClick;MultipleActiveResultSets=true;user id=;password=;TrustServerCertificate=True"
dotnet user-secrets set "Jwt:Secret" ""
```

> To generate a secure JWT secret key:
> ```bash
> openssl rand -base64 32
> ```

#### 2b. Scaffold the Database (DB-First)

```bash
dotnet ef dbcontext scaffold "" Microsoft.EntityFrameworkCore.SqlServer \
  --output-dir Entity \
  --context-dir Data \
  --context AppDbContext \
  --namespace DoohClick.DataAccess.Entity \
  --context-namespace DoohClick.DataAccess.Data \
  --force
```

#### 2c. Run the API

```bash
dotnet run --launch-profile DoohClick.API.Local
```

API runs at: `http://localhost:5200`  
Swagger UI at: `http://localhost:5200/swagger`

---

### 3. Frontend Setup

#### 3a. Install Dependencies

```bash
cd DoohClick.Web
npm install
```

#### 3b. Configure Environment

Create the file `src/assets/env/environment.json`:

```json
{
  "production": false,
  "apiUrl": "http://localhost:5200/",
  "secretKey": "",
  "storageKey": ""
}
```

> To generate keys:
> ```bash
> openssl rand -base64 32
> ```

#### 3c. Run the Frontend

```bash
ng serve
```

Frontend runs at: `http://localhost:4200`


## API Documentation
 
> All endpoints except `/account/login` require a valid JWT token.
> 
> All responses are wrapped in:
> ```json
> {
>   "type": "Success",
>   "message": "string",
>   "data": {}
> }
> ```
 
---
 
### Auth
 
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/account/login` | Login with tenant credentials |
| POST | `/account/refresh-token` | Refresh expired access token |
 
**POST** `/account/login`
```json
{
  "userName": "string",
  "password": "string",
  "tenantCode": "string"
}
```
 
**POST** `/account/refresh-token`
```json
{
  "accessToken": "string",
  "refreshToken": "string",
  "userId": 0,
  "refreshTokenExpiry": "2026-01-01T00:00:00Z"
}
```
 
---
 
### Inventory — Screens
 
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/inv/screen/grid` | Paginated screen list |
| POST | `/inv/screens` | Create or update a screen |
| DELETE | `/inv/screen/{id}` | Delete a screen |
| GET | `/inv/screen/ddl` | Screen dropdown list |
 
---
 
### CRM — Advertisers
 
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/crm/advertiser/grid` | Paginated advertiser list |
| GET | `/crm/advertiser/ddl` | Advertiser dropdown list |
 
---
 
### CMS — Media Library
 
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/cms/media/grid` | Paginated media list |
| POST | `/cms/media` | Upload and save media metadata |
| DELETE | `/cms/media` | Delete media (pass `id` in request body) |
| GET | `/cms/media/ddl` | Media dropdown list |
 
---
 
### File
 
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/file/upload` | Upload a file to the server |
 
---
 
### Commercial — Campaigns
 
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/commercial/campaign/grid` | Paginated campaign list |
| POST | `/commercial/campaign` | Create or update a campaign |
| DELETE | `/commercial/campaign/{id}` | Delete a campaign |
| POST | `/commercial/campaign/approve/{id}` | Approve a campaign |
| GET | `/commercial/campaign/schedules?Id={campaignId}` | Get schedules for a campaign |
| POST | `/commercial/campaign/schedule` | Create or update a campaign schedule |
 
---
 
### Reference Data
 
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/reference/list-item/ddl` | Dropdown for reference data (statuses, days of week, etc.) |
 
DDL response shape:
```json
[
  {
    "id": 1,
    "item": "Active",
    "code": "ACTIVE"
  }
]
```
 
---
 
### Playlist (Screen Device API)
 
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/screens/{screen_id}/playlist?at=2025-01-01T10:30:00Z` | Fetch scheduled playlist for a screen at a given time |
 
> ⚠️ This endpoint is not yet fully implemented.
 
---
 
## Key Assumptions
 
- Each tenant is identified via `tenantCode` at login; all data is scoped per tenant.
- Database schema is managed DB first; EF Core scaffolding regenerates entities from SQL Server.
- Media files are stored on the server filesystem, not in cloud storage.
- Screens poll the playlist API to retrieve what to play at a given time.
---
 
## Known Limitations
 
- UI is **not responsive** designed for desktop only.
- **GET** Playlist API is not implemented.
- Minor bug: the media attachment dialog (`p-dialog`) occasionally does not close after editing in the campaign module.
 