# Null-Coalescing Operator

## 1. What is it?

The **null-coalescing operator** `??` returns the left operand if it is not null; otherwise it returns the right operand:

```csharp
var result = left ?? right;
```

Related assignment form:

```csharp
value ??= fallback; // assign fallback only if value is null
```

## 2. Why does it exist?

It expresses “use this value, or a fallback when null” without an `if` or a full ternary:

```csharp
var name = input ?? "unknown";
// equivalent to: input is not null ? input : "unknown"
```

## 3. How does it work?

1. Evaluate the left expression.
2. If it is **not null**, that value is the result (right side is not evaluated).
3. If it **is null**, evaluate and use the right expression.

Important: `??` only cares about **null**.  
Empty string `""` and whitespace `"   "` are **not** null, so `??` will keep them.

## 4. Practical examples

Useful patterns:

```csharp
string? maybe = GetOptional();
string display = maybe ?? "(none)";

int? page = null;
int pageNumber = page ?? 1;
```

For InventoryEngine SKU, blank whitespace must become null. That is **not** a pure null-coalesce problem, because `"   "` is non-null.

## 5. Code examples with explanations

What `Item` actually uses today (ternary + whitespace check):

```csharp
this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
```

Why not only `??`?

```csharp
this.Sku = sku ?? null;          // useless; same as sku
this.Sku = sku.Trim() ?? null;   // throws if sku is null; keeps "" if sku is "   ".Trim()
```

Correct mental model:

| Need | Operator |
|------|----------|
| Fallback when **null** | `??` |
| Branch on **any condition** (including whitespace) | `?:` or `if` |

Example where `??` fits after normalization:

```csharp
string? sku = string.IsNullOrWhiteSpace(input) ? null : input.Trim();
string label = sku ?? "no-sku"; // display fallback
```

## 6. Common misconceptions

- **“`??` treats empty string as missing.”** It does not.
- **“`??` and `?:` are interchangeable.”** Only when the condition is exactly null-check.
- **“`a ?? b ?? c` is invalid.”** Chaining is valid; first non-null wins.

## 7. Engineering considerations and trade-offs

| Prefer `??` | Prefer `IsNullOrWhiteSpace` + ternary/`if` |
|-------------|--------------------------------------------|
| True null fallback | User text that may be spaces |
| Numeric nullable defaults | Domain normalization of optional strings |

InventoryEngine’s SKU rule needs whitespace awareness, so construction uses ternary, not `??` alone.

## 8. Key takeaways

- `??` means “if null, use the other value.”
- It does not handle empty/whitespace.
- Distinguish `??` from the ternary used in `Item` SKU normalization.

## 9. Related concepts

- [Ternary-Conditional-Operator.md](Ternary-Conditional-Operator.md)
- [Null-vs-Empty-vs-Whitespace.md](Null-vs-Empty-vs-Whitespace.md)
- [Nullable-Reference-Types.md](Nullable-Reference-Types.md)
