# Optional Parameters

## 1. What is it?

An **optional parameter** has a default value. Callers may omit it:

```csharp
public Item(string name, string unit, string? sku = null)
```

Here `sku` defaults to `null` when omitted.

## 2. Why does it exist?

Optional parameters reduce overload noise when a value is commonly absent.

Instead of:

```csharp
Create(name, unit);
Create(name, unit, sku);
```

one method serves both call sites.

## 3. How does it work?

Rules that matter in practice:

- Optional parameters must come **after** required parameters.
- The default must be a compile-time constant expression (including `null`).
- Omitting the argument passes the default.
- You can still pass the argument explicitly (`sku: "A1"` or positional).

## 4. Practical examples

```csharp
new Item("House Blend", "kg");
new Item("House Blend", "kg", null);
new Item("House Blend", "kg", "HB-01");
```

All use the same constructor. The first two treat SKU as absent going into normalization logic.

## 5. Code examples with explanations

Signature:

```csharp
public Item(string name, string unit, string? sku = null)
```

| Call | `sku` inside constructor before normalize |
|------|-------------------------------------------|
| `new Item("A", "kg")` | `null` (default) |
| `new Item("A", "kg", "X")` | `"X"` |

Then blank handling still runs:

```csharp
this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
```

So optional parameter default (`null`) and whitespace normalization work together.

## 6. Common misconceptions

- **“Optional means the parameter can be skipped in the middle.”** You cannot omit `unit` and still pass `sku` positionally; use named arguments if you need to skip (and only trailing params are optional here anyway).
- **“Default value runs as server-side logic.”** Defaults are filled at the **call site** by the compiler.
- **“Optional parameter replaces nullable types.”** They solve different problems; `string? sku = null` combines both.

## 7. Engineering considerations and trade-offs

| Approach | Trade-off |
|----------|-----------|
| Optional parameter | Convenient; changing default can be binary-fragile across assemblies |
| Overloads | Explicit; more methods to maintain |
| Required nullable argument | Forces callers to pass `null` deliberately |

For a learning domain constructor, `sku = null` matches “SKU often omitted.”

## 8. Key takeaways

- Optional parameters provide defaults when arguments are omitted.
- They must be last in the parameter list.
- `Item` uses `sku = null` because SKU is optional.

## 9. Related concepts

- [Constructors.md](Constructors.md)
- [Nullable-Reference-Types.md](Nullable-Reference-Types.md)
- [Null-vs-Empty-vs-Whitespace.md](Null-vs-Empty-vs-Whitespace.md)
- [Item-Entity-Design.md](../02-Domain-Modeling/Item-Entity-Design.md)
