# String Trim and Normalization

## 1. What is it?

**Trim** removes leading and trailing whitespace from a string:

```csharp
"  kg  ".Trim(); // "kg"
```

**Normalization** (in this learning context) means converting messy input into a canonical stored form — for InventoryEngine `Item`, that is: trim required strings, and convert blank optional SKU to `null`.

## 2. Why does it exist?

Without normalization:

- `"Espresso"` and `"Espresso "` look different to databases and uniqueness checks.
- Accidental spaces cause failed lookups and duplicate-looking catalog rows.

Trimming at the domain boundary keeps storage consistent.

## 3. How does it work?

- `Trim()` returns a **new** string; it does not modify the original.
- It removes characters like spaces, tabs, and newlines from both ends (default whitespace set).
- It does not remove spaces in the middle (`"A  B"` stays `"A  B"`).

InventoryEngine applies trim only after validating that required values are not whitespace-only.

## 4. Practical examples

| Raw input | After Item construction |
|-----------|-------------------------|
| `"  Beans  "` (name) | `Name = "Beans"` |
| `" kg"` (unit) | `Unit = "kg"` |
| `"  SKU1  "` (sku) | `Sku = "SKU1"` |
| `"   "` (sku) | `Sku = null` |

## 5. Code examples with explanations

From `Item` constructor:

```csharp
this.Name = name.Trim();
this.Unit = unit.Trim();
this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
```

Order matters for required fields:

1. Reject null/whitespace-only with `IsNullOrWhiteSpace`.
2. Then `Trim()` for storage.

If you trimmed first only:

```csharp
var trimmed = name.Trim(); // "   " → ""
```

you would still need to reject empty — InventoryEngine rejects with `IsNullOrWhiteSpace` before assign.

## 6. Common misconceptions

- **“Trim changes the variable in place.”** Strings are immutable; you must assign the result.
- **“Trim fixes all bad input.”** It does not collapse internal double spaces or enforce case rules.
- **“Trimming optional SKU to `""` is fine.”** InventoryEngine instead stores `null` for blank SKU.

## 7. Engineering considerations and trade-offs

| Approach | Trade-off |
|----------|-----------|
| Trim in domain constructor | Consistent entity state; every creation path must use the constructor |
| Trim only in API layer | Easy to forget alternate entry points |
| Trim in database constraints | Late; error messages less clear |

Current InventoryEngine approach: normalize in the `Item` constructor.

## 8. Key takeaways

- Trim removes edge whitespace; assign the returned value.
- Normalization defines the canonical form you store.
- `Item` trims name/unit/sku and maps blank sku → null.

## 9. Related concepts

- [Null-vs-Empty-vs-Whitespace.md](Null-vs-Empty-vs-Whitespace.md)
- [Constructors.md](Constructors.md)
- [Ternary-Conditional-Operator.md](Ternary-Conditional-Operator.md)
- [Domain-Invariants.md](../02-Domain-Modeling/Domain-Invariants.md)
