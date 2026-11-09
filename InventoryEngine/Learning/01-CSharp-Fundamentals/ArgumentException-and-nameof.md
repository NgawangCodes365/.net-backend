# ArgumentException and nameof

## 1. What is it?

**`ArgumentException`** is the standard exception for “this argument value is invalid.”

**`nameof(expr)`** is a compile-time operator that produces the name of a variable, type, or member as a `string`.

Typical validation throw:

```csharp
throw new ArgumentException("Unit cannot be empty or be whitespace", nameof(unit));
```

## 2. Why does it exist?

- Callers and logs need to know **which argument** failed.
- `nameof` avoids magic strings that break when you rename a parameter.
- Failing fast in a constructor prevents invalid domain objects.

## 3. How does it work?

`ArgumentException` common constructors:

```csharp
new ArgumentException(message);
new ArgumentException(message, paramName);
```

`paramName` is surfaced as `ArgumentException.ParamName`.

`nameof(name)` becomes `"name"` at compile time — no runtime reflection cost for the name itself.

## 4. Practical examples

In `Item`, bad required inputs fail construction:

```csharp
new Item("", "kg");     // ArgumentException for name
new Item("Tea", "  ");  // ArgumentException intended for unit
```

## 5. Code examples with explanations

From `InventoryEngine.Api/Domain/Item.cs`:

```csharp
if (string.IsNullOrWhiteSpace(name))
{
    throw new ArgumentException(
        "Item cannot be empty or whitespace.",
        nameof(name));
}

if (string.IsNullOrWhiteSpace(unit))
{
    throw new ArgumentException(
        "Unit cannot be empty or be whitespace",
        nameof(name)); // current code passes nameof(name) here
}
```

What to notice from the **actual** code:

- Name validation correctly uses `nameof(name)`.
- Unit validation currently passes `nameof(name)` as the parameter name string, even though the checked variable is `unit`.

That is useful as a learning reminder: `nameof` only emits the identifier you give it; it does not infer which variable you validated.

## 6. Common misconceptions

- **“Any exception is fine for bad input.”** Prefer `ArgumentException` / `ArgumentNullException` / `ArgumentOutOfRangeException` for caller mistakes.
- **“`nameof` validates the value.”** It only produces the name string.
- **“Message and paramName are the same.”** Message is human-readable; `paramName` identifies the argument.

## 7. Engineering considerations and trade-offs

| Practice | Why |
|----------|-----|
| Throw in constructors for invariant breaks | Invalid entity never escapes |
| Always match `nameof` to the validated parameter | Clearer API errors |
| Keep messages precise | Faster debugging |

Related specialized types:

- `ArgumentNullException` — value is null
- `ArgumentOutOfRangeException` — value outside allowed range

InventoryEngine currently uses `ArgumentException` with `IsNullOrWhiteSpace` checks.

## 8. Key takeaways

- `ArgumentException` signals invalid arguments.
- `nameof` keeps parameter names refactor-safe.
- Use the parameter name that matches the value you rejected.

## 9. Related concepts

- [Constructors.md](Constructors.md)
- [Null-vs-Empty-vs-Whitespace.md](Null-vs-Empty-vs-Whitespace.md)
- [Domain-Invariants.md](../02-Domain-Modeling/Domain-Invariants.md)
- [Item-Entity-Design.md](../02-Domain-Modeling/Item-Entity-Design.md)
