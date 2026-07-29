# MedStoreAPI - Solution Structure & Conventions

## Layers (dependency direction: API -> Service -> Entities <- Infrastructure)

| Project | Responsibility | Depends On |
|---|---|---|
| **MedStoreAPI.Domain** | Exact clones of DB tables (e.g. `Customer`, `Medicine`, `Batch`). Used only inside Infrastructure/Service. | None |
| **MedStoreAPI.Dtos** | `{Name}RequestDto` / `{Name}ResponseDto` - the shapes exposed to the Angular frontend. | None |
| **MedStoreAPI.Entities** | **Interfaces only.** `I{Name}Repository` (data contract, uses Domain) and `I{Name}Service` (business contract, uses Dtos). | Domain, Dtos, Common |
| **MedStoreAPI.Common** | Cross-cutting: `IDbConnectionFactory`, `ISqlDataAccess` (Dapper+SP wrapper), `ApiResponse<T>` wrapper, `StoredProcedureNames` constants. | None |
| **MedStoreAPI.Infrastructure** | Implements `I{Name}Repository` using Dapper + Stored Procedures. | Domain, Entities, Common |
| **MedStoreAPI.Service** | Implements `I{Name}Service`. All business logic + Domain<->Dto mapping lives here. | Domain, Dtos, Entities, Common |
| **MedStoreAPI.API** | Controllers only (thin). Registers everything in `Program.cs`. | All of the above |

## Naming Convention (strict)

- Table/Entity name **Customers** -> Domain class `Customer`, Repository `ICustomersRepository` / `CustomersRepository`, Service `ICustomersService` / `CustomersService`, Controller `CustomersController`.
- Request DTO: `{Name}RequestDto` (e.g. `CustomersRequestDto`)
- Response DTO: `{Name}ResponseDto` (e.g. `CustomersResponseDto`)
- Every service method returns `ApiResponse<T>`.

## How to Add a New Module (e.g. "Batches")

1. **Domain** - already exists as `Batch.cs` (exact table clone).
2. **Dtos/Batches/** - create `BatchesRequestDto.cs` and `BatchesResponseDto.cs`.
3. **Entities/Repositories/IBatchesRepository.cs** - method signatures using `Batch` (Domain).
4. **Entities/Services/IBatchesService.cs** - method signatures using DTOs, wrapped in `ApiResponse<T>`.
5. **Infrastructure/Repositories/BatchesRepository.cs** - implement `IBatchesRepository`, call stored procedures via `ISqlDataAccess` + `StoredProcedureNames.Batch.*`.
6. **Service/BatchesService.cs** - implement `IBatchesService`, map Domain <-> Dto, call repository.
7. **API/Controllers/BatchesController.cs** - thin controller calling `IBatchesService`.
8. **Program.cs** - register:
   ```csharp
   builder.Services.AddScoped<IBatchesRepository, BatchesRepository>();
   builder.Services.AddScoped<IBatchesService, BatchesService>();
   ```

Reference implementations for this exact pattern: **Customers**, **Medicines**, **Suppliers**, **Batches**, **Invoices**, **CustomerCredits**, **Dashboard**, and **Users/Auth** (fully built).

## Authentication & Multi-Store Security (Updated)

- `POST /api/auth/register` and `POST /api/auth/login` are public (`[AllowAnonymous]`).
- Login returns a JWT (`data.token`) containing `UserID`, `Username`, `StoreID`, and `Role` as claims - send it as `Authorization: Bearer {token}` on all other endpoints.
- **Every module controller (Customers, Medicines, Suppliers, Batches, Invoices, CustomerCredits, Dashboard, Users) now inherits `SecureControllerBase`**, which requires a valid JWT and exposes `CurrentStoreID` - read directly from the token, never from client-supplied query/body values.
- Controllers **overwrite** any client-sent `StoreID`/`storeID` with `CurrentStoreID` before calling the Service layer. This means a logged-in user of Store A cannot read or modify Store B's data by tampering with a `storeID` parameter - the token is the only source of truth for which store a request belongs to.
- `StoresController` is the one exception with mixed rules: `Add` (create a brand-new store) and `GetAll` (public directory) are `[AllowAnonymous]` since a new store has no user yet; `GetByID` / `Update` / `UploadLogo` require login **and** verify the requested `storeID` matches the caller's own `CurrentStoreID` (returns 403 otherwise).
- Categories, Units, GSTSlabs, PaymentModes are **global lookups** (no `StoreId` column) - they only require a valid JWT (any logged-in user from any store), not store-scoping.
- JWT signing key/issuer/audience/expiry live in `appsettings.json` under `"Jwt"` - **replace the placeholder `Key` with a real random secret (32+ characters) before deploying**.
- Passwords are hashed with BCrypt (`IPasswordHasher` in `MedStoreAPI.Common`) - never stored or returned in plain text.

### Record-Level Ownership Checks (Implemented)
Single-record endpoints now verify the record's own `StoreId` matches `CurrentStoreID` before returning/modifying/deleting it - a Store A user gets a "not found" response (not the data, and not a revealing "forbidden" for GET) if they try to access Store B's record by guessing an ID:
- **Medicines**: `GetByID`, `Update`, `Delete`
- **Suppliers**: `GetByID`, `Update`, `Delete`
- **Batches**: `Delete` (new `SP_BatchGetByID` added for this check)
- **Invoices**: `GetByID`, `Cancel`
- **CustomerCredits**: `GetByCustomer` (filters out other stores' credits), `AddPayment` (new `SP_CustomerCreditGetByID` added for this check)

### Known Remaining Gaps (Not Yet Implemented)
- No global exception-handling middleware, request validation attributes, pagination, or structured logging yet.
- No JWT refresh/revocation mechanism (token is valid until it naturally expires).
- Run the two new stored procedures (`SP_BatchGetByID`, `SP_CustomerCreditGetByID`) from `StoredProcedures/BATCHES` and `StoredProcedures/CUSTOMER_CREDITS` against your database before using Batches delete or CustomerCredits payment endpoints.

## Setup Steps

1. Update `MedStoreAPI.API/appsettings.json` -> `ConnectionStrings:DefaultConnection` with your SQL Server details.
2. Open `MedStoreAPI.sln` in Visual Studio 2022 (or `dotnet build` from CLI).
3. Set `MedStoreAPI.API` as startup project.
4. Run - Swagger UI opens automatically in Development mode at `/swagger`.
5. CORS is pre-configured for Angular dev server at `http://localhost:4200` (edit `appsettings.json` -> `Cors:AllowedOrigins` if needed).
