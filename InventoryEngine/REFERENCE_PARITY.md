# InventoryEngine — Reference Parity

Compare **business capabilities** with the company inventory system without copying its code, schema, or architecture.

## Purpose

- Capture *what* an inventory system needs to do
- Decide *whether* InventoryEngine should support each capability
- Track parity status without treating the company design as the only valid design

## Rules

1. Describe capabilities in business language (stock on hand, receive goods, adjust stock, etc.).
2. Do **not** paste company source, connection strings, or proprietary schema dumps here.
3. “Parity” means capability coverage, not identical tables/APIs/folder layouts.
4. Mark items as goals only after explicit approval to implement them.

## Capability tracker

| Capability area | Company reference (notes only) | InventoryEngine goal? | Status |
|-----------------|--------------------------------|------------------------|--------|
| Product / item master | TBD from your Obsidian notes / investigation | TBD | Not started |
| Stock on hand / quantities | TBD | TBD | Not started |
| Stock movements / history | TBD | TBD | Not started |
| Adjustments | TBD | TBD | Not started |
| Purchases / receiving | TBD | TBD | Not started |
| Sales / consumption impact | TBD | TBD | Not started |
| Suppliers | TBD | TBD | Not started |
| Locations / warehouses | TBD | TBD | Not started |
| Reporting | TBD | TBD | Not started |
| Authn / authz | TBD | TBD | Not started |

## How to use this file

1. After each company-backend investigation session, add capability rows (not code).
2. For each row, decide: **out of scope**, **later**, or **implement next**.
3. Link decisions to ADRs in `ARCHITECTURE_DECISIONS.md` when design choices follow.

## Explicit non-goals (current)

- Connecting to the company database
- Reusing company projects, DTOs, or EF models
- Matching company microservice folder layout (`src/Services/...`)

## Concurrency and Transaction Handling

Findings from the company inventory investigation (read-only reference).  
**InventoryEngine status:** documented for learning only — no concurrency strategy chosen or implemented yet.

Investigation references (paths under company backend `Desktop/backend`, plus Obsidian notes):

- `src/services/inventory/Inventory.Models/StoreItemStock.cs`
- `src/services/inventory/Inventory.Persistance/Data/InventoryDbContext.cs`
- `src/services/inventory/Inventory.Persistance/Data/Migrations/` (incl. model snapshot)
- `src/services/inventory/Inventory.Persistance/Services/InventoryStockService.cs` (`AdjustStockAsync`, `TransferStockAsync`, `DeductStockForSaleAsync`, balance create path)
- `src/services/inventory/Inventory.Persistance/Repositories/PurchaseEntryRepository.cs` (`PostAsync`)
- `src/services/inventory/InventoryService/Controllers/InventoryStockController.cs`
- `src/services/inventory/InventoryService/Middleware/GlobalExceptionMiddleware.cs` (generic error path)
- Obsidian: `Desktop/InventorySystem/Inventory System.md` (Concurrency / known gaps)
- Chat investigation: company concurrency trace (Adjust / Transfer / Purchase post / Sale deduct)

### 1. Optimistic concurrency on `StoreItemStocks` via PostgreSQL `xmin`

| Field | Notes |
|-------|--------|
| **Company behavior** | Stock balance rows (`StoreItemStock`) expose a concurrency token (`RowVersion`) mapped through EF Core as a row version. On PostgreSQL this uses system column `xmin`, so updates include an optimistic check against the version loaded into the change tracker. |
| **Limitation / risk** | Protection applies to **updates of an already-tracked stock row**. It does not by itself protect every business invariant (e.g. “remaining PO qty”, first insert races). |
| **Confidence** | **Confirmed** |
| **References** | `StoreItemStock.cs`; `InventoryDbContext` concurrency mapping; migrations / `InventoryDbContextModelSnapshot`; Obsidian concurrency section |

### 2. Concurrent updates of the same previously loaded stock row → `DbUpdateConcurrencyException`

| Field | Notes |
|-------|--------|
| **Company behavior** | Typical mutators follow read → validate → mutate in memory → `SaveChanges` inside a transaction. If two requests load the same `StoreItemStock` and both save, the second save can fail with `DbUpdateConcurrencyException` when `xmin` no longer matches. |
| **Limitation / risk** | Prevents silent last-write-wins on that row, but turns the conflict into a hard failure unless handled upstream. |
| **Confidence** | **Confirmed** (behavior of EF optimistic concurrency + company token mapping; race outcome inferred from that mechanism, not from a live multi-client load test in this workspace) |
| **References** | `InventoryStockService` mutators; `PurchaseEntryRepository.PostAsync` when updating existing balances; EF concurrency token on `StoreItemStock` |

### 3. Concurrency exceptions are not handled → generic HTTP 500

| Field | Notes |
|-------|--------|
| **Company behavior** | No application-level catch/retry/reload path for `DbUpdateConcurrencyException` was found on stock mutation flows. Unhandled failures fall through to generic exception handling and surface as HTTP 500. |
| **Limitation / risk** | Callers (POS, operators, API clients) get a server error instead of a clear conflict / “retry” outcome; operators may retry blindly and re-hit the same race window. |
| **Confidence** | **Confirmed** (no handlers found in inventory codebase search; middleware treats unhandled exceptions generically) |
| **References** | Inventory-wide search for `DbUpdateConcurrencyException`; `GlobalExceptionMiddleware`; stock controllers/services |

### 4. Concurrent purchase-order receipts can potentially double-post stock

| Field | Notes |
|-------|--------|
| **Company behavior** | Receiving against a purchase order updates stock and PO line remaining quantities as part of posting logic. Stock rows that already exist benefit from `xmin` optimistic concurrency; **purchase-order lines were not found to use an equivalent concurrency token**. |
| **Limitation / risk** | Two concurrent receipts against the same PO line can both observe “remaining qty” as sufficient, both post stock, and over-receive / double-post relative to the PO — stock-row concurrency alone does not serialize the PO remaining check. |
| **Confidence** | **Confirmed mechanism gap**; **potential** double-post is a reasoned race (not proven with a recorded production incident in this investigation) |
| **References** | `PurchaseEntryRepository.PostAsync`; purchase-order / line models + DbContext configuration (no PO-line concurrency token found); contrast with `StoreItemStock` token |

### 5. Concurrent first-time stock-balance creation can hit unique constraints

| Field | Notes |
|-------|--------|
| **Company behavior** | First movement for a (store, item) often **creates** the `StoreItemStock` row (get-or-create style). Unique constraints enforce one balance row per store/item (tenant-scoped as configured). |
| **Limitation / risk** | Two concurrent first-time creates can both see “no row”, both insert, and one fails with a unique constraint / `DbUpdateException` instead of a clean merge. |
| **Confidence** | **Confirmed** race class given unique index + insert path; exact HTTP mapping depends on generic exception handling |
| **References** | `InventoryStockService` get-or-create / balance creation paths; unique indexes on `StoreItemStocks` in migrations / DbContext |

### 6. Stock mutations use explicit transactions with default isolation

| Field | Notes |
|-------|--------|
| **Company behavior** | Core stock mutators generally wrap work in an explicit EF/database transaction. No custom `IsolationLevel` was set; PostgreSQL therefore uses its default (**Read Committed**). |
| **Limitation / risk** | Read Committed allows concurrent transactions to proceed based on non-repeatable reads of business fields that are not concurrency-protected (e.g. PO remaining). Transactions give atomicity of the unit of work, not full serializability of all inventory invariants. |
| **Confidence** | **Confirmed** for explicit transactions + absence of custom isolation settings in traced mutators |
| **References** | `InventoryStockService` (`AdjustStockAsync`, `TransferStockAsync`, `DeductStockForSaleAsync`); `PurchaseEntryRepository.PostAsync` |

### 7. No pessimistic locking, atomic conditional updates, or retry mechanism found

| Field | Notes |
|-------|--------|
| **Company behavior** | Investigation did not find `FOR UPDATE` / explicit row locks, atomic SQL conditional updates (e.g. update-where-quantity-sufficient), or application retry/reload loops after concurrency failures. |
| **Limitation / risk** | Concurrency strategy is effectively “optimistic token on stock balance only,” with conflicts and some races left to fail loudly or slip through unprotected invariants. |
| **Confidence** | **Confirmed** (absence across traced inventory services/repos; not a claim about every file in the entire monorepo) |
| **References** | Grep/trace across inventory services for locking, `ExecuteUpdate`, isolation, retry; mutators listed above |

### InventoryEngine implication (parity only — no decision yet)

These findings define **risks to avoid or improve on** when InventoryEngine later designs stock mutations.  
Do **not** treat company behavior as the required design. Choosing optimistic vs pessimistic vs atomic SQL updates, conflict HTTP semantics, and retry policy remains **pending** and must be recorded in `ARCHITECTURE_DECISIONS.md` only after explicit approval.
