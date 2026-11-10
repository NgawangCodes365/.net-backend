# Item Unit Immutability After Inventory History

## 1. What is it?

This is a **domain rule** for InventoryEngine’s `Item` entity:

1. **Before** an item has any inventory history, its **unit may be changed**.
2. **Once** an inventory-affecting record exists for that item, the unit **cannot be casually changed**.
3. **Inventory history** includes:
   - Opening stock entries
   - Purchase receipts
   - Stock adjustments
   - Stock transfers
   - Other stock-affecting transactions
4. If a unit correction is **genuinely necessary** after inventory history exists, it requires a **controlled correction or migration process** that preserves historical accuracy.

In short: unit is editable while the item is still “catalog only”; after stock meaning has been recorded, unit becomes locked for ordinary edits.

This note documents the **agreed business rule**. It does not claim that enforcement code already exists on `Item`.

## 2. Why does it exist?

An item’s **unit** is the measuring stick for every quantity tied to that item.

If an item is measured in `kg`, a quantity of `5` means five kilograms. If someone later changes the unit to `pcs`, the same stored number `5` suddenly reads as five pieces — even though historical receipts and balances were recorded as kilograms.

That is not a harmless label rename. It **rewrites the meaning** of past numbers without rewriting the numbers themselves. Reports, audits, and stock-on-hand interpretation become wrong while looking “complete.”

So InventoryEngine treats unit stability after inventory history as a rule that protects the meaning of stock data over time.

## 3. How does it work?

### Business invariant

A **business invariant** (also called a **domain invariant**) is a rule that must remain true for the business model to stay valid.

Related general idea: [Domain-Invariants.md](Domain-Invariants.md).

This rule is an invariant about **when** `Item.Unit` may change:

| Situation | Allowed? |
|-----------|----------|
| Item exists, but no inventory-affecting records yet | Yes — unit may change |
| Any inventory-affecting record exists for the item | No — casual unit change is forbidden |
| History exists, but the unit was configured incorrectly | Not a casual edit — only a controlled correction/migration |

“Casual change” means a normal edit path (for example a simple “update item” action) that just overwrites `Unit`.

### What counts as inventory history?

Anything that records or changes stock in a way that assumes a unit already:

- **Opening stock** — the first recorded on-hand quantity for the item (often at a store). That quantity was entered **in the item’s then-current unit**.
- **Purchase receipts** — received amounts (“we received 10”) are interpretable only with that unit.
- **Stock adjustments** — increases/decreases are unit-dependent.
- **Stock transfers** — moved quantities must mean the same thing at source and destination.
- **Other stock-affecting transactions** — any movement that creates, updates, or explains quantity.

Once such a record exists, historical quantities and current balances must remain **interpretable** under the same unit those records assumed.

### Changing a business property vs correcting a misconfiguration

| Kind of change | Meaning | Allowed after history? |
|----------------|---------|------------------------|
| **Ordinary business edit** | “We decided to sell this in pieces instead of kilograms going forward” while leaving old rows untouched | **No** as a casual overwrite |
| **Controlled correction / migration** | “We wrongly set `kg` when we meant `pcs`; we will fix the catalog **and** preserve or rewrite history so old quantities stay truthful” | **Only** through an explicit, careful process |

A controlled process might involve reviewing historical rows, converting quantities, or otherwise documenting the fix so audits still make sense. Exact migration steps are **not** decided in this note — only that casual overwrite is insufficient.

## 4. Practical examples

**Allowed — no history yet**

1. Create item “Coffee Beans” with unit `kg`.
2. No opening stock, purchases, adjustments, or transfers yet.
3. Realize the catalog should say `g` instead.
4. Change unit to `g` — still safe, because no quantities depend on the old unit.

**Forbidden as a casual edit — history exists**

1. Same item, unit `kg`.
2. Opening stock recorded as `10` (meaning 10 kg).
3. Later purchase receipt of `5` (5 kg).
4. Someone changes unit to `pcs` in a normal update form.
5. Balances and history still show `10` and `5`, but now appear as pieces — **false meaning**.

**Why opening stock counts as history**

Opening stock is not “just a setup field.” It is the first quantity claim about the real world for that item. From that moment, the unit is part of how that claim is read. Changing unit afterward without a controlled correction breaks that claim.

## 5. Code examples with explanations

No InventoryEngine application code implements this rule yet. The examples below are **conceptual** illustrations of the decision, not an approved API design.

```csharp
// Conceptual: casual update after history must be rejected.
// Exact method names / services are not decided here.

void ChangeUnitCasually(Item item, string newUnit, bool hasInventoryHistory)
{
    if (hasInventoryHistory)
    {
        // Reject ordinary overwrite — protects quantity meaning.
        throw new InvalidOperationException(
            "Unit cannot be changed after inventory history exists.");
    }

    // Before history: ordinary change is allowed (details TBD in code).
}
```

A future controlled correction would be a **different** operation (migration/admin path), not the same casual update.

## 6. Common misconceptions

- **“Unit is only a display label.”** No — it defines what every quantity number means.
- **“Opening stock is not real history.”** It is the first inventory-affecting quantity record; it locks unit for casual edits under this rule.
- **“We can always fix wrong units later with a simple edit.”** After history, a simple overwrite is unsafe; a controlled correction/migration is required.
- **“This rule is already enforced in `Item.cs`.”** This Learning note records the **agreed domain decision**. Enforcement in code is a separate implementation step.

## 7. Engineering considerations and trade-offs

| Approach | Upside | Downside |
|----------|--------|----------|
| Allow free unit edits forever | Simple UI/API | Historical quantities can become meaningless |
| Lock unit after any inventory history (this rule) | Preserves interpretability of stock data | Needs a clear “has history?” check and a rare correction path |
| Always immutable unit from creation | Very safe | Blocks harmless fixes before any stock exists |

InventoryEngine chose the middle rule: **mutable until history, then protected**.

Trade-off to remember later: detecting “has inventory history” depends on stock-related records existing in the system. That check belongs with inventory data, not with renaming alone.

## 8. Key takeaways

- Changing `kg` → `pcs` can silently change the meaning of every existing quantity for that item.
- Opening stock counts as inventory history because it records a quantity under the current unit.
- Purchases, movements, and balances must stay interpretable under the unit they were recorded with.
- Ordinary property edits ≠ controlled corrections after misconfiguration.
- This is a **business invariant** on `Item`: after inventory history, unit is not casually mutable.

## 9. Related concepts

- [Domain-Invariants.md](Domain-Invariants.md)
- [Domain-Entities.md](Domain-Entities.md)
- [Item-Entity-Design.md](Item-Entity-Design.md)
- [Encapsulation.md](Encapsulation.md)
