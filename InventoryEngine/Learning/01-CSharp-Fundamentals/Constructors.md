# Constructors

## 1. What is it?

A **constructor** is a special method that runs when an instance is created with `new`.

It has the same name as the type and no return type:

```csharp
public Item(string name, string unit, string? sku = null) { ... }
```

## 2. Why does it exist?

Constructors establish a valid initial state. For domain types, they are the natural place to:

- Require mandatory data (`name`, `unit`).
- Reject invalid input early.
- Assign identity (`Id`).
- Normalize values (trim strings).

## 3. How does it work?

1. Caller executes `new Item(...)`.
2. Memory for the object is allocated.
3. Constructor body runs.
4. If the constructor throws, the instance is not successfully created for the caller.
5. If it completes, the caller receives a fully constructed object.

If you declare no constructor, C# supplies a public parameterless constructor (for non-static classes). `Item` declares an explicit constructor, so callers must use that signature.

## 4. Practical examples

Creating an inventory catalog item:

```csharp
var item = new Item("Espresso Beans", "kg", "BEAN-001");
var itemNoSku = new Item("House Blend", "kg"); // sku omitted → null
```

Invalid input fails at construction time instead of producing a half-valid object:

```csharp
new Item("   ", "kg"); // throws ArgumentException
```

## 5. Code examples with explanations

From `InventoryEngine.Api/Domain/Item.cs`:

```csharp
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
            nameof(name)); // note: argument name used here is "name"
    }
    this.Unit = unit.Trim();
    this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
    this.Id = Guid.NewGuid();
}
```

What this constructor does:

1. Validates `name` and `unit`.
2. Stores trimmed values.
3. Normalizes blank SKU to `null`.
4. Generates a new `Guid` for `Id`.

## 6. Common misconceptions

- **“Constructors only assign fields.”** They can validate, normalize, and enforce invariants.
- **“If construction is complex, skip validation and fix later.”** Invalid objects spread bugs through the system.
- **“A class always has a public parameterless constructor.”** Not once you define another constructor (unless you also add one).

## 7. Engineering considerations and trade-offs

| Approach | Trade-off |
|----------|-----------|
| Fat constructor with validation | Strong guarantees at creation; constructor can grow |
| Parameterless + public setters | Easy for serializers/ORMs; weaker domain guarantees |
| Factory methods | Named creation paths; more API surface |

InventoryEngine `Item` currently uses a validating constructor and private setters.

## 8. Key takeaways

- Constructors are the creation gate for objects.
- Throwing on invalid arguments prevents invalid entities from existing.
- `Item` creates identity and normalized name/unit/sku in one place.

## 9. Related concepts

- [ArgumentException-and-nameof.md](ArgumentException-and-nameof.md)
- [Optional-Parameters.md](Optional-Parameters.md)
- [Domain-Invariants.md](../02-Domain-Modeling/Domain-Invariants.md)
- [Encapsulation.md](../02-Domain-Modeling/Encapsulation.md)
