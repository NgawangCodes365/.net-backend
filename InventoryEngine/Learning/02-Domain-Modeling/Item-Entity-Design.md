# Item Entity Design

## 1. What is it?

This note documents the **current** InventoryEngine `Item` design as implemented in:

`InventoryEngine.Api/Domain/Item.cs`

`Item` is a catalog entity: something the inventory system can name, measure, and optionally identify by SKU. It does **not** yet store stock quantities (those belong to a future store-balance concept).

## 2. Why does it exist?

A learning inventory system needs a product/ingredient master record before it can track stock, purchases, or movements.

`Item` is the first approved-style domain type in code (constructor + encapsulated properties). Broader model decisions for `Store` / `StoreItemStock` may still be evolving in docs; this file describes only what exists in `Item.cs`.

## 3. How does it work?

Responsibilities today:

1. Create a new identity (`Guid`).
2. Require and normalize `Name` and `Unit`.
3. Accept optional `Sku`, normalizing blanks to `null`.
4. Expose attributes as read-only to outsiders.

Non-responsibilities today (confirmed absent):

- Price
- Active/inactive flags
- Persistence / EF mapping
- Store relationships / quantities
- API endpoints

## 4. Practical examples

```csharp
var beans = new Item("Espresso Beans", "kg", "BEAN-001");
// beans.Id   → new Guid
// beans.Name → "Espresso Beans"
// beans.Unit → "kg"
// beans.Sku  → "BEAN-001"

var milk = new Item("Whole Milk", "L");
// milk.Sku → null
```

## 5. Code examples with explanations

Full current shape:

```csharp
namespace InventoryEngine.Api.Domain
{
    public class Item
    {
        public Guid Id { get; private set; }

        public Item(string name, string unit, string? sku = null)
        {
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
        }

        public string Name { get; private set; } = string.Empty;
        public string? Sku { get; private set; }
        public string Unit { get; private set; } = string.Empty;
    }
}
```

Design choices visible in code:

| Choice | Implementation |
|--------|----------------|
| Identity | `Guid`, assigned in constructor |
| Required text | `Name`, `Unit` with whitespace rejection |
| Optional text | `string? Sku` with blank → null |
| Encapsulation | `private set` |
| Optional arg | `sku = null` |

Observation from the current file: the unit-validation `ArgumentException` passes `nameof(name)` rather than `nameof(unit)`.

## 6. Common misconceptions

- **“Item must match the company `Item` model.”** InventoryEngine is independent; company uses `int` id and public setters, and includes fields such as `Price` / `PosReferenceId` that InventoryEngine `Item` does not have.
- **“Item holds stock quantity.”** Quantity is not on this class.
- **“This class is finished.”** It is an early domain sketch; persistence and APIs are not implemented.

## 7. Engineering considerations and trade-offs

| Topic | Current state | Trade-off |
|-------|---------------|-----------|
| `Guid` vs `int` id | `Guid` | Easy local generation; different from company `int` |
| No `Price` | Absent | Simpler catalog; pricing may be added later if needed |
| Constructor validation | Present | Strong create-time guarantees |
| No update methods | Present | Cannot rename yet without adding mutators |
| Single-project location | `InventoryEngine.Api/Domain` | Matches ADR preference for few layers early |

## 8. Key takeaways

- `Item` is identity + name + unit + optional sku.
- Validity is enforced at construction with encapsulated setters.
- Stock, stores, persistence, and HTTP APIs are outside this type today.

## 9. Related concepts

- [Domain-Entities.md](Domain-Entities.md)
- [Domain-Invariants.md](Domain-Invariants.md)
- [Encapsulation.md](Encapsulation.md)
- [Properties-and-Access-Modifiers.md](../01-CSharp-Fundamentals/Properties-and-Access-Modifiers.md)
- [Constructors.md](../01-CSharp-Fundamentals/Constructors.md)
- [Nullable-Reference-Types.md](../01-CSharp-Fundamentals/Nullable-Reference-Types.md)
- [ArgumentException-and-nameof.md](../01-CSharp-Fundamentals/ArgumentException-and-nameof.md)
