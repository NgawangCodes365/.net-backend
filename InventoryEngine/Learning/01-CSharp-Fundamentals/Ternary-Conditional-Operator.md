# Ternary Conditional Operator

## 1. What is it?

The **ternary operator** chooses one of two expressions based on a boolean condition:

```csharp
condition ? valueIfTrue : valueIfFalse
```

It is an **expression** (produces a value), not a statement like `if`.

## 2. Why does it exist?

It fits short value selections without a multi-line `if`/`else` assignment:

```csharp
// instead of
string? sku;
if (string.IsNullOrWhiteSpace(input))
    sku = null;
else
    sku = input.Trim();
```

## 3. How does it work?

1. Evaluate `condition`.
2. If true, evaluate and use the middle expression.
3. If false, evaluate and use the last expression.
4. Only one branch’s value is used as the result (the unused branch is not needed as the result).

Both branches should be compatible types (with normal C# conversion rules).

## 4. Practical examples

Normalize optional SKU in one assignment:

- If blank → `null`
- Else → trimmed string

## 5. Code examples with explanations

From `Item`:

```csharp
this.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
```

Evaluation table:

| `sku` | Condition true? | Result |
|-------|-----------------|--------|
| `null` | yes | `null` |
| `"  "` | yes | `null` |
| `" A1 "` | no | `"A1"` |

Equivalent `if` form:

```csharp
if (string.IsNullOrWhiteSpace(sku))
    this.Sku = null;
else
    this.Sku = sku.Trim();
```

## 6. Common misconceptions

- **“Ternary replaces all if statements.”** Nesting ternaries quickly becomes unreadable; use `if` for multi-step logic.
- **“Both sides always run with side effects you rely on.”** Write branches as values; avoid hiding important side effects in ternary arms.
- **“Ternary and null-coalescing are the same.”** `??` only tests null; ternary tests any condition. See [Null-Coalescing-Operator.md](Null-Coalescing-Operator.md).

## 7. Engineering considerations and trade-offs

| Use ternary when | Prefer `if` when |
|------------------|------------------|
| Single short assignment | Multiple statements per branch |
| Condition is obvious | Logic needs comments / clarity |
| Mapping blank → null | Complex validation workflow |

InventoryEngine uses ternary for SKU normalization only.

## 8. Key takeaways

- `?:` picks between two values from a boolean condition.
- Ideal for compact normalization assignments.
- `Item` uses it to map blank SKU to `null`, otherwise trimmed text.

## 9. Related concepts

- [Null-Coalescing-Operator.md](Null-Coalescing-Operator.md)
- [Null-vs-Empty-vs-Whitespace.md](Null-vs-Empty-vs-Whitespace.md)
- [String-Trim-and-Normalization.md](String-Trim-and-Normalization.md)
