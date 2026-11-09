# Encapsulation

## 1. What is it?

**Encapsulation** means keeping an object’s internal state hidden and exposing only a controlled interface.

In C# terms for `Item`:

- State is held in properties.
- Outside code can **read** state.
- Outside code cannot **freely write** state (`private set`).
- Creation and validation happen through the constructor.

## 2. Why does it exist?

Without encapsulation, any layer can do:

```csharp
item.Name = "   "; // bypasses rules
```

That breaks [domain invariants](Domain-Invariants.md). Encapsulation makes the type responsible for its own consistency.

## 3. How does it work?

Mechanisms used in InventoryEngine `Item`:

1. `private set` on properties — block external assignment.
2. Public constructor — single controlled creation path.
3. Validation/normalization inside that path — state is cleaned before stored.

Reading remains public so other code can use the entity’s data without mutating it.

## 4. Practical examples

Allowed:

```csharp
var item = new Item("Sugar", "kg");
Console.WriteLine(item.Name); // read OK
```

Not allowed (compile error):

```csharp
item.Name = "Salt";
item.Id = Guid.NewGuid();
```

## 5. Code examples with explanations

```csharp
public class Item
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Sku { get; private set; }
    public string Unit { get; private set; } = string.Empty;

    public Item(string name, string unit, string? sku = null)
    {
        // only place that currently assigns these properties
        ...
        this.Name = name.Trim();
        this.Unit = unit.Trim();
        this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
        this.Id = Guid.NewGuid();
    }
}
```

Encapsulation here is **creation-time control**. There are not yet methods like `Rename(...)`; when those appear, they must keep enforcing the same rules.

## 6. Common misconceptions

- **“Encapsulation means no public properties.”** Public getters are fine; uncontrolled setters are the usual problem.
- **“Private fields without validation equal encapsulation.”** Hiding data is not enough; protecting rules matters.
- **“DTOs and entities should encapsulate the same way.”** DTOs often use public setters for transport; entities usually should not.

## 7. Engineering considerations and trade-offs

| Style | Pros | Cons |
|-------|------|------|
| Private setters + constructor | Strong invariants | ORM/materialization may need configuration later |
| Public setters | Easy mapping | Weak protection |
| Methods for every change | Explicit use-cases | More code |

InventoryEngine currently chose private setters for `Item`. Persistence is not implemented yet, so ORM mapping trade-offs are future work.

## 8. Key takeaways

- Encapsulation restricts how state changes.
- `Item` allows reads, restricts writes, validates on construct.
- Future mutators must preserve the same guarantees.

## 9. Related concepts

- [Properties-and-Access-Modifiers.md](../01-CSharp-Fundamentals/Properties-and-Access-Modifiers.md)
- [Constructors.md](../01-CSharp-Fundamentals/Constructors.md)
- [Domain-Invariants.md](Domain-Invariants.md)
- [Item-Entity-Design.md](Item-Entity-Design.md)
