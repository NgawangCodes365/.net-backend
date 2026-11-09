# InventoryEngine Learning Knowledge Base

Permanent technical reference for concepts studied while building **InventoryEngine**.

**Rules**

- One concept per Markdown file.
- Update an existing file instead of duplicating it.
- Only mark **Completed** after the concept has been genuinely studied.
- Do not invent InventoryEngine implementation details that are not in the code.

**Source of examples (when relevant)**

- `../InventoryEngine.Api/Domain/Item.cs`

---

## Learning status legend

| Status | Meaning |
|--------|---------|
| Completed | Studied with notes in this folder |
| In Progress | Partially studied; notes incomplete |
| Not Started | Folder reserved; no concept notes yet |

---

## Table of contents

### 01 — C# Fundamentals

| Concept | File | Status |
|---------|------|--------|
| Properties and access modifiers | [Properties-and-Access-Modifiers.md](01-CSharp-Fundamentals/Properties-and-Access-Modifiers.md) | Completed |
| Constructors | [Constructors.md](01-CSharp-Fundamentals/Constructors.md) | Completed |
| Nullable reference types | [Nullable-Reference-Types.md](01-CSharp-Fundamentals/Nullable-Reference-Types.md) | Completed |
| Null vs empty vs whitespace | [Null-vs-Empty-vs-Whitespace.md](01-CSharp-Fundamentals/Null-vs-Empty-vs-Whitespace.md) | Completed |
| String trim and normalization | [String-Trim-and-Normalization.md](01-CSharp-Fundamentals/String-Trim-and-Normalization.md) | Completed |
| Optional parameters | [Optional-Parameters.md](01-CSharp-Fundamentals/Optional-Parameters.md) | Completed |
| Ternary conditional operator | [Ternary-Conditional-Operator.md](01-CSharp-Fundamentals/Ternary-Conditional-Operator.md) | Completed |
| Null-coalescing operator | [Null-Coalescing-Operator.md](01-CSharp-Fundamentals/Null-Coalescing-Operator.md) | Completed |
| `ArgumentException` and `nameof` | [ArgumentException-and-nameof.md](01-CSharp-Fundamentals/ArgumentException-and-nameof.md) | Completed |

### 02 — Domain Modeling

| Concept | File | Status |
|---------|------|--------|
| Domain entities | [Domain-Entities.md](02-Domain-Modeling/Domain-Entities.md) | Completed |
| Domain invariants | [Domain-Invariants.md](02-Domain-Modeling/Domain-Invariants.md) | Completed |
| Encapsulation | [Encapsulation.md](02-Domain-Modeling/Encapsulation.md) | Completed |
| Item entity design | [Item-Entity-Design.md](02-Domain-Modeling/Item-Entity-Design.md) | Completed |

### 03 — ASP.NET Core

| Concept | File | Status |
|---------|------|--------|
| *(none yet)* | `03-ASP.NET-Core/` | Not Started |

### 04 — Entity Framework Core

| Concept | File | Status |
|---------|------|--------|
| *(none yet)* | `04-Entity-Framework-Core/` | Not Started |

### 05 — Database Engineering

| Concept | File | Status |
|---------|------|--------|
| *(none yet)* | `05-Database-Engineering/` | Not Started |

### 06 — Inventory Management

| Concept | File | Status |
|---------|------|--------|
| *(none yet)* | `06-Inventory-Management/` | Not Started |

### 07 — Transactions and Concurrency

| Concept | File | Status |
|---------|------|--------|
| *(none yet)* | `07-Transactions-and-Concurrency/` | Not Started |

Company concurrency findings for later study live outside this tree in [`../REFERENCE_PARITY.md`](../REFERENCE_PARITY.md). Move distilled learning notes here when that topic is formally studied as InventoryEngine design.

### 08 — API Architecture

| Concept | File | Status |
|---------|------|--------|
| *(none yet)* | `08-API-Architecture/` | Not Started |

### 09 — Testing and Reliability

| Concept | File | Status |
|---------|------|--------|
| *(none yet)* | `09-Testing-and-Reliability/` | Not Started |

---

## How to add a new concept

1. Choose the correct numbered folder (or create a new numbered topic folder if needed).
2. Add `Concept-Name.md` using the standard nine-section template.
3. Link it from this README and set status to **Completed** or **In Progress**.
4. Prefer relative links to related notes.
