# StockDemo API — Warehouse Management Backend

[![CI](https://github.com/nminhcuongdev/stockdemo.API/actions/workflows/ci.yml/badge.svg)](https://github.com/nminhcuongdev/stockdemo.API/actions/workflows/ci.yml)

A REST API built with **.NET 8.0**, **Entity Framework Core**, and **SQL Server**. It powers the
StockDemo Android app: products, stock, warehouse operations, reporting, and RFID tag mapping.

## Features

### Authentication & Users
* JWT-based login (`POST /api/users/login`).
* User CRUD, password hashing (PBKDF2), and change-password.

### Catalog & Master Data
* **Products** — CRUD, with min/max reorder levels.
* **Locations** — CRUD and lookup by code.
* **Stocks** — on-hand quantity per product/location, lookup by QR code.

### Warehouse Operations
* **Stock In / Out** — record receipts and dispatches, with history.
* **Stock Transfer** — move stock between locations (transfer history preserved).
* **Stocktake** — create counts and reconcile system vs. counted quantities.
* **Delivery Orders** — lookup by QR code for guided stock-in.

### Reporting & Alerts
* **Stock-movement report** over a date range.
* **Low-stock alerts** for products below their minimum level.

### RFID EPC Mapping
* Pair a physical tag's **EPC** with a **stock** so scanned tags resolve to product info,
  shared across all devices.
* `GET / POST / DELETE /api/epcmappings`.

## Tech Stack

* **Framework:** .NET 8.0 (LTS), ASP.NET Core Web API
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First migrations, seeded demo data)
* **Auth:** JWT Bearer
* **Mapping:** AutoMapper (Domain ↔ DTO)
* **Docs:** Swagger / OpenAPI
* **Pattern:** Repository pattern over EF Core
* **Deployment:** Docker Compose (API + SQL Server)

## API Overview

| Area | Base route |
| --- | --- |
| Auth / Users | `/api/users` |
| Products | `/api/products` |
| Locations | `/api/locations` |
| Stocks | `/api/stocks` |
| Stock In / Out | `/api/stockins`, `/api/stockouts` |
| Transfers | `/api/stocktransfers` |
| Stocktakes | `/api/stocktakes` |
| Delivery Orders | `/api/deliveryorders` |
| Reports | `/api/reports` |
| Low-stock alerts | `/api/stockalerts` |
| EPC mappings | `/api/epcmappings` |

All endpoints except login require a `Bearer` token. Explore them interactively at `/swagger`.

## Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Docker Desktop, **or** a local SQL Server instance

### Run with Docker (recommended)
Brings up the API and SQL Server together; EF migrations and demo seed data are applied on startup.

```bash
docker-compose up --build -d
```

* API: <http://localhost:5000>
* Swagger: <http://localhost:5000/swagger>

### Run with the .NET SDK
Point the connection string at your SQL Server (see `appsettings.json`), then:

```bash
dotnet run
```

Migrations run automatically on startup.

## Project Structure

```text
StockDemo.API/
|-- Controllers/     # API endpoints
|-- Services/        # JWT and other services
|-- Repositories/    # Data access (repository pattern over EF Core)
|-- Models/
|   |-- Domain/      # EF entities
|   `-- DTO/         # Request/response DTOs
|-- Mappings/        # AutoMapper profiles
|-- Data/            # DbContext + seed data
`-- Migrations/      # EF Core migrations
```

> **Note:** `appsettings.json` in this repo contains demo credentials for local development only.
> For any real deployment, move secrets (DB password, JWT key) into environment variables or
> user-secrets and run with `ASPNETCORE_ENVIRONMENT=Production`.

## License

Developed for internal warehouse management purposes.
