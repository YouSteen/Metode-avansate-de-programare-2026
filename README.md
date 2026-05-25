# Laborator Curs 2 — Modelare UML (Sistem Gestiune Comenzi)

Proiect MAP — Curs 2 UML. Cod C# + diagrame grupate **pe cerinte**.

## Unde gasesti livrabilele

| Cerinta | Unde |
|---------|------|
| **1** Use case | [docs/cerinta-1-diagrama-use-case/](docs/cerinta-1-diagrama-use-case/) → `use-case.svg` |
| **2** Diagrama clasa | [docs/cerinta-2-diagrama-clasa/](docs/cerinta-2-diagrama-clasa/) → `class-diagram-before.svg`, `class-diagram-after.svg` |
| **3** Secventa (2 fluxuri) | [docs/cerinta-3-diagrame-secventa/](docs/cerinta-3-diagrame-secventa/) → `sequence-*.md` (Mermaid) |

Index complet: [docs/README.md](docs/README.md)

## Structura proiect

```
OrderManagement.sln
src/
  OrderManagement.Before/     # cod modelat — stare actuala (God Class)
  OrderManagement.After/      # cod modelat — structura propusa
docs/
  cerinta-1-diagrama-use-case/
  cerinta-2-diagrama-clasa/
  cerinta-3-diagrame-secventa/
  unelte/generate-png.ps1     # optional: export PNG din Mermaid
```

## Rulare cod C# (.NET 8)

```bash
dotnet run --project src/OrderManagement.Before
dotnet run --project src/OrderManagement.After
```

## Decizii de modelare (rezumat)

### Cerinta 1

- **Include:** Plaseaza comanda → Verifica stoc, Proceseaza plata, Trimite email
- **Extend:** Anuleaza comanda → Vizualizeaza comanda (optional, in prealabil)

### Cerinta 2

- **Before:** `OrderManager` centralizeaza tot (anti-pattern pentru Lab SOLID)
- **After:** separare repository / stoc / plata / email + `OrderService`

### Cerinta 3

- **Place order:** `alt` stoc si plata; `opt` email
- **Cancel order:** `alt` pentru `Shipped` (403) si comanda inexistenta (404)

Detalii si diagrame embed: README din fiecare folder `docs/cerinta-*`.
