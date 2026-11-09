# Null vs Empty vs Whitespace

## 1. What is it?

Three different string states:

| State | Example | Meaning |
|-------|---------|---------|
| **Null** | `null` | No string instance; absence |
| **Empty** | `""` | A string exists, length 0 |
| **Whitespace** | `"   "`, `"\t"` | A string exists, only whitespace characters |

`string.IsNullOrEmpty(s)` is true for null or `""`.  
`string.IsNullOrWhiteSpace(s)` is true for null, `""`, or whitespace-only.

## 2. Why does it exist?

User input and APIs often send `"   "` when they mean “nothing.” Treating that as a real name/unit/SKU creates bad data.

Backend code must decide, per field:

- Is blank input **invalid** (required field)?
- Or blank input **means null** (optional field)?

## 3. How does it work?

Check helpers:

```csharp
string.IsNullOrEmpty(value);       // null or ""
string.IsNullOrWhiteSpace(value);  // null, "", or whitespace-only
```

`IsNullOrWhiteSpace` is usually the right gate for human-entered text fields.

## 4. Practical examples

Inventory item rules as implemented today:

- **Name** — null/empty/whitespace → invalid → throw
- **Unit** — null/empty/whitespace → invalid → throw
- **SKU** — null/empty/whitespace → store as `null` (optional)

## 5. Code examples with explanations

Required field (name):

```csharp
if (string.IsNullOrWhiteSpace(name))
{
    throw new ArgumentException(
        "Item cannot be empty or whitespace.",
        nameof(name));
}
this.Name = name.Trim();
```

Optional field (sku):

```csharp
this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
```

| Input `sku` | Stored `Sku` |
|-------------|--------------|
| `null` | `null` |
| `""` | `null` |
| `"   "` | `null` |
| `" BEAN-1 "` | `"BEAN-1"` (after trim) |

## 6. Common misconceptions

- **“Empty and null are the same.”** Not in C#. Databases and APIs often treat them differently too.
- **“`IsNullOrEmpty` catches spaces.”** It does not; `"   "` passes `IsNullOrEmpty` as false.
- **“Whitespace-only names are fine if trimmed later.”** Trim of `"   "` yields `""`, which is still not a useful required name — reject before or after trim consistently.

## 7. Engineering considerations and trade-offs

| Policy | Use when |
|--------|----------|
| Reject whitespace | Required business fields (`Name`, `Unit`) |
| Normalize to null | Optional fields (`Sku`) |
| Keep empty string | Rare; only if empty is a deliberate distinct value |

Pick one representation for “missing optional string” and stick to it. InventoryEngine uses **null**.

## 8. Key takeaways

- Null, empty, and whitespace are different.
- Prefer `IsNullOrWhiteSpace` for text validation.
- Required vs optional fields need different handling of blank input.

## 9. Related concepts

- [String-Trim-and-Normalization.md](String-Trim-and-Normalization.md)
- [Nullable-Reference-Types.md](Nullable-Reference-Types.md)
- [Ternary-Conditional-Operator.md](Ternary-Conditional-Operator.md)
- [Domain-Invariants.md](../02-Domain-Modeling/Domain-Invariants.md)
