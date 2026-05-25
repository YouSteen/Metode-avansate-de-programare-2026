# Cerinta 2 — Diagrama de clasa

## Enunt (din laborator)

1. Modelati structura codului **asa cum este** (cu problemele de design).
2. Evidentiati: God Class, cuplare la DB, responsabilitati mixte.
3. Adaugati o a **doua versiune** — structura **propusa** dupa refactorizare.

## Cod sursa modelat

| Versiune | Proiect C# |
|----------|------------|
| Stare actuala (before) | `src/OrderManagement.Before/` — clasa `OrderManager` |
| Structura propusa (after) | `src/OrderManagement.After/` — `OrderService` + interfete |

---

## Livrabil 2a — Diagrama BEFORE (stare actuala)

![Diagrama clasa - before](class-diagram-before.svg)

**Fisier:** [class-diagram-before.svg](class-diagram-before.svg)

**Probleme marcate in diagrama:**

- `OrderManager` = **God Class** (validare, stoc, plata, SQL, SMTP, raport)
- `OrderController` → dependenta directa de `OrderManager`
- `Order` / `OrderItem` anemice (fara comportament de domeniu)
- Cuplare mare, coeziune mica

---

## Livrabil 2b — Diagrama AFTER (structura propusa)

![Diagrama clasa - after](class-diagram-after.svg)

**Fisier:** [class-diagram-after.svg](class-diagram-after.svg)

**Imbunatatiri:**

- `OrderService` orchestreaza fluxul (coeziune mare)
- Dependente prin `IOrderRepository`, `IStockService`, `IPaymentService`, `IEmailService` (cuplare mica)
- `Order` cu `Confirm()`, `Cancel()`, `CanBeCancelled()`
- Implementari concrete in infrastructura

---

## Surse (editare / export PNG)

| Diagrama | PlantUML | Mermaid |
|----------|----------|---------|
| Before | [surse/class-diagram-before.puml](surse/class-diagram-before.puml) | [surse/class-diagram-before.mmd](surse/class-diagram-before.mmd) |
| After | [surse/class-diagram-after.puml](surse/class-diagram-after.puml) | [surse/class-diagram-after.mmd](surse/class-diagram-after.mmd) |
