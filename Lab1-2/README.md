# Lab 1–2 — Modelare UML (Sistem Gestiune Comenzi)

Livrabil **Curs 2 (UML)** din MAP: diagrame + cod C# de referință (stare actuală vs. structură propusă).

| Curs | Conținut |
|------|----------|
| **Curs 1** | Recapitulare OOP — material: [curs1_oop.html](../curs1_oop.html) |
| **Curs 2** | UML — **acest folder** |
| **Curs 3+** | SOLID / alte laburi — vor fi în `Lab3/`, etc. |

## Unde găsești livrabilele

| Cerință | Unde |
|---------|------|
| **1** Use case | [docs/cerinta-1-diagrama-use-case/](docs/cerinta-1-diagrama-use-case/) → `use-case.svg` |
| **2** Diagramă clasă | [docs/cerinta-2-diagrama-clasa/](docs/cerinta-2-diagrama-clasa/) → surse PlantUML/Mermaid (+ SVG when exported) |
| **3** Secvență (2 fluxuri) | [docs/cerinta-3-diagrame-secventa/](docs/cerinta-3-diagrame-secventa/) → `sequence-*.md` |

Index diagrame: [docs/README.md](docs/README.md)

## Structură

```
Lab1-2/
  OrderManagement.sln
  src/
    OrderManagement.Before/     # God Class — stare „actuală”
    OrderManagement.After/      # structură propusă (pregătire refactor SOLID)
  docs/
    cerinta-1-diagrama-use-case/
    cerinta-2-diagrama-clasa/
    cerinta-3-diagrame-secventa/
    unelte/generate-png.ps1     # opțional: export PNG din Mermaid
```

## Rulare cod (.NET 8)

Din acest folder (`Lab1-2/`):

```bash
dotnet run --project src/OrderManagement.Before
dotnet run --project src/OrderManagement.After
```

Sau:

```bash
dotnet build OrderManagement.sln
```

## Decizii de modelare (rezumat)

### Cerința 1

- **Include:** Plasează comanda → Verifică stoc, Procesează plată, Trimite email
- **Extend:** Anulează comanda → Vizualizează comanda (opțional, în prealabil)

### Cerința 2

- **Before:** `OrderManager` centralizează tot (anti-pattern)
- **After:** repository / stoc / plată / email + `OrderService`

### Cerința 3

- **Place order:** `alt` stoc și plată; `opt` email
- **Cancel order:** `alt` pentru `Shipped` (403) și comandă inexistentă (404)

Detalii: README din fiecare `docs/cerinta-*`.

## Legătură cu laboratoarele următoare

Codul `OrderManagement.After` ilustrează separarea responsabilităților descrisă în diagrame; refactorizarea SOLID completă pe acest domeniu poate continua după Cursul 4. **Laboratorul 3 (Task Manager)** din Curs 3 este un proiect separat — vezi [curs3_solid1.html](../curs3_solid1.html).
