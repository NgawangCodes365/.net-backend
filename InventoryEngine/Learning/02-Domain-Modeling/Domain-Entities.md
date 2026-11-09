# Domain Entities

## 1. What is it?

A **domain entity** is an object defined by **identity** and **business meaning**, not only by its field values.

Two items with the same name are still different entities if they have different ids.

In InventoryEngine, `Item` is the first domain entity: a catalog product/ingredient the system tracks.

## 2. Why does it exist?

Inventory software is not only rows in a database. Domain entities:

- Capture business language (`Item`, later `Store`, stock balances, etc.).
- Carry rules about what makes a valid instance.
- Give the rest of the system a stable identity to reference.

## 3. How does it work?

Typical entity characteristics:

1. **Identity** — `Item.Id` (`Guid` in InventoryEngine).
2. **Attributes** — `Name`, `Unit`, optional `Sku`.
3. **Lifecycle** — created through a constructor that enforces rules.
4. **Referenceability** — other concepts will eventually point at `Item` by id.

An entity is different from:

- A **DTO** (data transfer shape for APIs)
- A **value object** (defined by values, often no independent identity)

## 4. Practical examples

```csharp
var a = new Item("Milk", "L", "MILK-1");
var b = new Item("Milk", "L", "MILK-1");
// a.Id != b.Id → two entities, even if attributes match
```

Identity comes from `Guid.NewGuid()` inside the constructor.

## 5. Code examples with explanations

```csharp
namespace InventoryEngine.Api.Domain
{
    public class Item
    {
        public Guid Id { get; private set; }

        public Item(string name, string unit, string? sku = null)
        {
            // validation + normalization...
            this.Id = Guid.NewGuid();
        }

        public string Name { get; private set; } = string.Empty;
        public string? Sku { get; private set; }
        public string Unit { get; private set; } = string.Empty;
    }
}
```

This is an entity skeleton:

- Lives under `Domain`
- Has identity
- Encapsulates attribute assignment
- Does not yet include persistence, API, or inventory quantities

## 6. Common misconceptions

- **“Entity = database table.”** Tables persist entities; the entity is the business concept.
- **“Matching names means same entity.”** Identity decides sameness.
- **“All public setters are fine on entities.”** That often turns entities into bags of data with no protection.

## 7. Engineering considerations and trade-offs

| Decision | InventoryEngine today |
|----------|------------------------|
| Id type | `Guid` (generated in constructor) |
| Location | `InventoryEngine.Api/Domain/Item.cs` inside a single API project |
| Persistence | Not wired yet |

Company inventory `Item` uses `int` identity and public setters; InventoryEngine is intentionally a separate learning design, not a copy.

## 8. Key takeaways

- Entities have identity + business meaning.
- `Item` is InventoryEngine’s first domain entity.
- Construction is where entity validity begins.

## 9. Related concepts

- [Domain-Invariants.md](Domain-Invariants.md)
- [Encapsulation.md](Encapsulation.md)
- [Item-Entity-Design.md](Item-Entity-Design.md)
- [Constructors.md](../01-CSharp-Fundamentals/Constructors.md)
