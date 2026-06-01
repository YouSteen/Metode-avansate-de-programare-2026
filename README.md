# Metode avansate de programare — 2026

Repository pentru cursurile MAP: materiale HTML + laboratoare grupate pe foldere.

## Laboratoare

| Folder | Curs | Continut |
|--------|------|----------|
| [**Lab1-2/**](Lab1-2/) | Curs 2 — UML | Diagrame + `OrderManagement.Before` / `.After` |
| [**Lab3/**](Lab3/) | Curs 3 — SOLID I | Task Manager: SRP, OCP, LSP |
| [**Lab4/**](Lab4/) | Curs 4 — SOLID II | Task Manager: ISP, DIP, IoC (extinde Lab 3) |

## Materiale curs

| Fisier | Subiect |
|--------|---------|
| [curs1_oop.html](curs1_oop.html) | Recapitulare OOP |
| [curs2_uml.html](curs2_uml.html) | UML → **Lab1-2** |
| [curs3_solid1.html](curs3_solid1.html) | SOLID I → **Lab3** |
| [curs4_solid2.html](curs4_solid2.html) | SOLID II → **Lab4** |

## Quick start

**Lab 1–2 (comenzi):**
```bash
cd Lab1-2
dotnet run --project src/OrderManagement.Before
dotnet run --project src/OrderManagement.After
```

**Lab 3 / Lab 4 (Task Manager):**
```bash
cd Lab4
dotnet test TaskManager.sln
dotnet run --project src/TaskManager.UI
```

Documentatie: [Lab1-2/README.md](Lab1-2/README.md) · [Lab3/README.md](Lab3/README.md) · [Lab4/README.md](Lab4/README.md)
