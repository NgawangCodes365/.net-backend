# Properties and Access Modifiers

## 1. What is it?

A **property** is a member that exposes data on a type using `get` and optionally `set` accessors.

An **access modifier** (`public`, `private`, `protected`, `internal`, etc.) controls who can read or write that member.

Common property shapes:

```csharp
public string Name { get; set; }           // public get + public set
public Guid Id { get; private set; }       // public get + private set
public string Unit { get; }                // get-only (init elsewhere)
```

## 2. Why does it exist?

Properties let you:

- Expose state without making raw fields public.
- Control mutation (`private set` vs public `set`).
- Add validation or computed logic later without changing callers that use property syntax.

## 3. How does it work?

- `get` runs when code reads `item.Name`.
- `set` runs when code assigns `item.Name = value` (if the setter is accessible).
- Auto-properties (`{ get; set; }`) use a compiler-generated backing field.
- `private set` means only code inside the declaring type can assign the property.

## 4. Practical examples

In InventoryEngine, catalog fields are readable everywhere, but only the `Item` type can assign them after construction:

- Outside code can read `item.Name`.
- Outside code cannot do `item.Name = "x"` because the setter is private.

## 5. Code examples with explanations

From `InventoryEngine.Api/Domain/Item.cs`:

```csharp
public Guid Id { get; private set; }
public string Name { get; private set; } = string.Empty;
public string? Sku { get; private set; }
public string Unit { get; private set; } = string.Empty;
```

| Member | Get | Set | Meaning |
|--------|-----|-----|---------|
| `Id` | public | private | Callers can read identity; only `Item` assigns it |
| `Name` | public | private | Same for required name |
| `Sku` | public | private | Optional SKU; nullable type |
| `Unit` | public | private | Required unit |

Assignment happens inside the constructor:

```csharp
this.Name = name.Trim();
this.Unit = unit.Trim();
this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
this.Id = Guid.NewGuid();
```

Those assignments compile because they are inside `Item`.

## 6. Common misconceptions

- **“Properties are just fields.”** Fields and properties differ: properties are accessors; tools, serializers, and interfaces treat them differently.
- **“`private set` means the value never changes.”** It can still change inside the class (constructor or methods).
- **“Public getter + public setter is always fine for domain types.”** It allows any caller to bypass construction rules unless something else validates.

## 7. Engineering considerations and trade-offs

| Choice | Upside | Downside |
|--------|--------|----------|
| Public setters | Simple DTOs / EF-friendly anemic models | Weak encapsulation |
| Private setters | Protects invariants after create | Mutation needs methods on the type |
| Init-only (`init`) | Set during object initializer | Different from constructor-only patterns |

InventoryEngine currently uses **public get + private set** on `Item`.

## 8. Key takeaways

- Access modifiers on `get`/`set` separately control read vs write.
- `private set` is a primary tool for encapsulating entity state.
- InventoryEngine `Item` exposes data for reading but centralizes writes inside the type.

## 9. Related concepts

- [Constructors.md](Constructors.md)
- [Encapsulation.md](../02-Domain-Modeling/Encapsulation.md)
- [Nullable-Reference-Types.md](Nullable-Reference-Types.md)
- [Item-Entity-Design.md](../02-Domain-Modeling/Item-Entity-Design.md)
