# Nullable Reference Types

## 1. What is it?

With **nullable reference types** enabled, the compiler distinguishes:

- `string` — intended to be non-null
- `string?` — may be null

This is a compile-time nullability annotation system (warnings), not a separate runtime type for references.

InventoryEngine.Api has `<Nullable>enable</Nullable>` in its project file.

## 2. Why does it exist?

Null reference bugs are common. Annotating intent lets the compiler warn when you:

- Pass null where non-null is expected.
- Forget to check before using a possibly null value.
- Forget that a property is allowed to be absent.

## 3. How does it work?

- `string Name` means “callers should treat this as non-null.”
- `string? Sku` means “null is a valid state.”
- The compiler emits warnings (not always hard errors) when flow analysis thinks nullability rules are broken.
- At runtime, references can still be null if rules are ignored or data comes from outside (reflection, disabled context, legacy code).

## 4. Practical examples

For an inventory item:

- **Name** and **Unit** are required → `string`
- **SKU** is optional → `string?`

That matches “every item has a name/unit; SKU may be missing.”

## 5. Code examples with explanations

From `Item`:

```csharp
public string Name { get; private set; } = string.Empty;
public string? Sku { get; private set; }
public string Unit { get; private set; } = string.Empty;
```

Constructor parameter:

```csharp
public Item(string name, string unit, string? sku = null)
```

Normalization stores absence as null:

```csharp
this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
```

So after construction, `Sku` is either a trimmed non-empty string or `null` — not `""`.

## 6. Common misconceptions

- **“`string?` is a different runtime type.”** It is still `System.String`; `?` is nullability metadata for the compiler.
- **“Nullable enable makes null impossible.”** It reduces mistakes; it does not eliminate all nulls.
- **“Optional business data should be empty string.”** Empty and null mean different things; InventoryEngine normalizes blank SKU to null.

## 7. Engineering considerations and trade-offs

| Choice | Meaning |
|--------|---------|
| `string` + default `""` | Always a string instance; “missing” must be encoded another way |
| `string?` + null | Explicit absence; callers must handle null |
| Mixing `""` and null | Ambiguous; avoid for the same field |

## 8. Key takeaways

- `T` vs `T?` documents null intent for reference types.
- InventoryEngine marks optional SKU as `string?`.
- Required name/unit stay non-nullable `string` after successful construction.

## 9. Related concepts

- [Null-vs-Empty-vs-Whitespace.md](Null-vs-Empty-vs-Whitespace.md)
- [Null-Coalescing-Operator.md](Null-Coalescing-Operator.md)
- [Optional-Parameters.md](Optional-Parameters.md)
- [Item-Entity-Design.md](../02-Domain-Modeling/Item-Entity-Design.md)
