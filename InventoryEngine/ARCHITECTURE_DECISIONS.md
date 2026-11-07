# InventoryEngine — Architecture Decisions

Record important decisions with context and trade-offs. Prefer short ADRs over long essays.

Format:

- **Status:** Proposed | Accepted | Superseded
- **Context:** why a decision was needed
- **Decision:** what we chose
- **Consequences:** what becomes easier / harder

---

## ADR-001: Project location inside Explaining_POS_backend

- **Status:** Accepted (2026-11-07)
- **Context:** InventoryEngine must live in this personal learning workspace, remain independent of `POS.Api`, and preserve existing files.
- **Decision:** Place InventoryEngine at workspace root:
  `Explaining_POS_backend/InventoryEngine/`
  as a sibling of `POS.Api`, not inside `POS.Api` or empty `src/Services/Inventory`.
- **Consequences:**
  - Clear boundary between POS learning API and InventoryEngine
  - Shared Git history/workspace without shared runtime coupling
  - Empty `src/` tree remains untouched until we explicitly decide otherwise

---

## ADR-002: Dedicated solution file

- **Status:** Accepted (2026-11-07)
- **Context:** There was no root `.sln`. Coupling InventoryEngine into `POS.Api` would blur project boundaries.
- **Decision:** Use `InventoryEngine/InventoryEngine.sln` containing only InventoryEngine projects. Classic `.sln` format (not `.slnx`) for broad tooling compatibility.
- **Consequences:**
  - InventoryEngine builds/runs on its own
  - Optional future root multi-project solution is still possible without merging code

---

## ADR-003: Single Web API project for now (no extra layers)

- **Status:** Accepted (2026-11-07)
- **Context:** Extra projects (Domain, Application, Infrastructure) add ceremony before we have behavior to separate.
- **Decision:** Start with one project: `InventoryEngine.Api`. Add projects/layers only when a concrete pain appears (e.g., reusable domain logic, test isolation).
- **Consequences:**
  - Faster learning feedback loop
  - Risk of putting too much in one project later — mitigate by revisiting when the first real feature lands

---

## ADR-004: Intended data stack = EF Core + PostgreSQL

- **Status:** Accepted as *intent* (2026-11-07); packages not installed yet
- **Context:** Approved learning stack. Installing unused packages before a DbContext/entity exists would add noise.
- **Decision:** Document EF Core + PostgreSQL as the data-access stack. Install packages and create DbContext only when the first persistence feature is approved.
- **Consequences:**
  - Clear technical direction
  - No premature connection strings, migrations, or company-DB risk

---

## ADR-005: Target framework `net10.0`

- **Status:** Accepted (2026-11-07); superseded earlier scaffold choice of `net8.0`
- **Context:** InventoryEngine was scaffolded on `net8.0` (LTS; closer to the company inventory reference stack). The owner approved aligning with the workspace sibling `POS.Api`, which already targets `net10.0`. Both SDKs are installed locally.
- **Decision:** Target `net10.0`. Align OpenAPI package majors with ASP.NET Core 10:
  - `Microsoft.AspNetCore.OpenApi` → `10.0.12`
  - `Swashbuckle.AspNetCore` → `10.3.0`
- **Consequences:**
  - Same TFM as `POS.Api` (still no shared code or database)
  - When EF Core / Npgsql are added later, use package versions compatible with .NET 10
  - Differs from the company inventory service’s .NET 8 — acceptable for this independent learning project

---

## ADR-006: No sample domain endpoint in the template

- **Status:** Accepted (2026-11-07)
- **Context:** `dotnet new webapi` ships a WeatherForecast demo. That is unrelated inventory noise.
- **Decision:** Keep OpenAPI/Swagger pipeline; remove WeatherForecast endpoint and type.
- **Consequences:** API starts empty; Swagger UI opens with no operations until we approve the first endpoint
