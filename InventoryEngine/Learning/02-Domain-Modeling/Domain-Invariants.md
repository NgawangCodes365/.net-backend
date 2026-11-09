# Domain Invariants

## 1. What is it?

A **domain invariant** is a rule that must always be true for a valid domain object.

If the rule is broken, the object should not exist in that state (or the operation should be rejected).

## 2. Why does it exist?

Inventory data is dangerous when invalid:

- An item with no name is useless in UI and reports.
- An item with no unit makes quantities ambiguous.
- Mixing `"   "` and real SKUs creates ambiguous catalog data.

Invariants protect the business meaning of the model.

## 3. How does it work?

Invariants are enforced at change boundaries. For current `Item`, the boundary is the constructor:

1. Check incoming values.
2. Throw if a required rule fails.
3. Normalize and assign only after checks pass.

As long as setters stay private and no other methods mutate carelessly, construction rules hold for the object’s lifetime (until mutation methods are added later — those must preserve invariants too).

## 4. Practical examples

Current `Item` invariants (from code behavior):

| Rule | Enforcement |
|------|-------------|
| Name is required (not null/empty/whitespace) | `ArgumentException` |
| Unit is required (not null/empty/whitespace) | `ArgumentException` |
| Name/Unit stored trimmed | `.Trim()` after validation |
| Blank SKU stored as null | ternary normalization |
| Every item has an Id | `Guid.NewGuid()` |

## 5. Code examples with explanations

```csharp
if (string.IsNullOrWhiteSpace(name))
{
    throw new ArgumentException(
        "Item cannot be empty or whitespace.",
        nameof(name));
}
this.Name = name.Trim();

if (string.IsNullOrWhiteSpace(unit))
{
    throw new ArgumentException(
        "Unit cannot be empty or be whitespace",
        nameof(name));
}
this.Unit = unit.Trim();

this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
this.Id = Guid.NewGuid();
```

Successful construction ⇒ those invariants hold on the returned instance.

## 6. Common misconceptions

- **“Validation belongs only in the UI.”** UI checks improve UX; domain invariants protect the model regardless of entry point.
- **“Database constraints replace invariants.”** DB constraints help persistence integrity; domain rules should still be clear in code.
- **“Once constructed, invariants cannot be broken.”** Future public setters or mutating methods can break them if careless.

## 7. Engineering considerations and trade-offs

| Enforcement point | Strength |
|-------------------|----------|
| Constructor / domain methods | Immediate, in-language |
| API model validation attributes | Good for HTTP input; may not cover all paths |
| Database constraints | Last line of defense |

InventoryEngine currently enforces `Item` invariants in the constructor only.

## 8. Key takeaways

- Invariants are always-true business rules for an entity.
- `Item` currently guarantees required trimmed name/unit, normalized SKU, and an id.
- Throw early rather than allow invalid entities.

## 9. Related concepts

- [Domain-Entities.md](Domain-Entities.md)
- [Encapsulation.md](Encapsulation.md)
- [ArgumentException-and-nameof.md](../01-CSharp-Fundamentals/ArgumentException-and-nameof.md)
- [Null-vs-Empty-vs-Whitespace.md](../01-CSharp-Fundamentals/Null-vs-Empty-vs-Whitespace.md)
