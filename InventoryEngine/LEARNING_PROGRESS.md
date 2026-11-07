# InventoryEngine — Learning Progress

Track what was learned, what was decided, and what comes next. Update this as work progresses.

## Session log

### 2026-11-07 — Initial scaffolding

**Goal:** Create an independent InventoryEngine home inside this workspace without implementing inventory features.

**What we did**

- Created `InventoryEngine/` at the workspace root (sibling of `POS.Api`)
- Created dedicated `InventoryEngine.sln`
- Created `InventoryEngine.Api` ASP.NET Core Web API project (`net8.0`)
- Added project instructions and architecture docs
- Removed the template WeatherForecast sample endpoint

**Concepts reinforced**

- **Solution (`.sln`)**: a container that lists one or more projects so the IDE/CLI can build them together. It is not the application itself.
- **Web API project (`.csproj`)**: the runnable application (dependencies, target framework, entry point).
- **Independence**: sharing a Git workspace does not mean sharing database, models, or project references.
- **Scaffold vs feature**: scaffolding creates the empty shell; features require deliberate design approval.

**Not done yet (intentionally)**

- Domain entities / PostgreSQL schema
- EF Core wiring
- Controllers / repositories / services
- Authentication / authorization

### 2026-11-07 — Company concurrency investigation (documented)

**Goal:** Capture verified company inventory concurrency/transaction behavior in InventoryEngine docs without implementing a strategy yet.

**What we did**

- Completed the initial concurrency investigation against the company inventory system (read-only)
- Documented findings in `REFERENCE_PARITY.md` → **Concurrency and Transaction Handling**
- Separated confirmed behavior from reasoned race risks (e.g. potential PO double-post)

**Concepts reinforced**

- **Optimistic concurrency:** detect conflicting updates using a version token (`xmin` via EF), rather than locking rows up front
- **Transactions vs isolation:** an explicit transaction gives atomic commit/rollback; default Read Committed does not by itself serialize all business checks
- **Token coverage matters:** protecting `StoreItemStock` updates does not automatically protect PO-line remaining qty or first-insert races
- **Unhandled conflicts:** a concurrency token without catch/retry often becomes HTTP 500 under load

**Not done yet (intentionally)**

- Designing InventoryEngine’s concurrency strategy (still pending)
- Implementing locking, retries, or conflict HTTP semantics in InventoryEngine
- Adding EF Core / PostgreSQL packages or stock entities

## Next learning targets (pending approval)

1. ~~Confirm target framework (`net8.0` vs `net10.0`)~~ — done: `net10.0` (ADR-005)
2. Agree on first vertical slice (likely a tiny health/readiness endpoint or a single read-only entity)
3. Decide when to add EF Core + Npgsql packages
4. Sketch the first domain model from business capabilities (not from company tables)
5. Design InventoryEngine concurrency/transaction strategy (after domain sketch) — **pending**
