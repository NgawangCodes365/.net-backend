# InventoryEngine — Agent Instructions

## What this project is

InventoryEngine is an independent inventory management learning system built with:

- C#
- ASP.NET Core Web API
- Entity Framework Core (intended; not wired yet)
- PostgreSQL (intended; not connected yet)

It lives in this workspace beside `POS.Api`, but must remain **fully independent** of it.

## Non-negotiable rules

1. **Do not modify** `POS.Api/`, `src/`, or company backend code.
2. **Do not connect** to the company database.
3. **Do not copy** company source code. Study capabilities; design and write original code.
4. **Do not implement** features without explicit user approval.
5. Prefer a **simple** architecture. Add layers only when a concrete need appears.
6. Explain **why** before **how**. Work incrementally.

## Current scaffold state

- Solution: `InventoryEngine.sln`
- API project: `InventoryEngine.Api` (ASP.NET Core Web API, `net8.0`)
- No inventory entities, DbContext, controllers, repositories, or business logic yet
- EF Core / Npgsql packages are **not** installed yet (documented intent only)

## Working style for assistants

1. Inspect existing InventoryEngine files before proposing changes.
2. Propose the smallest next step and wait for approval when the change is structural.
3. Keep architectural decisions in `ARCHITECTURE_DECISIONS.md`.
4. Keep learning notes in `LEARNING_PROGRESS.md`.
5. Keep company-capability comparison (not code copying) in `REFERENCE_PARITY.md`.
6. Never treat company implementation details as requirements to clone 1:1.

## Related but separate

| Path | Relationship |
|------|----------------|
| `../POS.Api` | Sibling learning project (Dapper / SQL Server). Do not share code or DB. |
| `../src` | Empty placeholders. Do not use for InventoryEngine. |
| Company backend (outside this repo) | Read-only reference for capabilities only. |
