# Metode avansate de programare — 2026

Repository pentru cursurile MAP: materiale HTML + laboratoare grupate pe foldere.

## Laboratoare

| Folder | Curs | Status |
|--------|------|--------|
| [**Lab1-2/**](Lab1-2/) | Curs 2 — UML (gestiune comenzi) | Diagrame + `OrderManagement.Before` / `.After` |
| [**Lab3/**](Lab3/) | Curs 3 — SOLID I (Task Manager) | SQLite, SRP/OCP/LSP, NUnit, consola Spectre |

## Materiale curs

| Fișier | Subiect |
|--------|---------|
| [curs1_oop.html](curs1_oop.html) | Recapitulare OOP |
| [curs2_uml.html](curs2_uml.html) | UML — laborator → **Lab1-2** |
| [curs3_solid1.html](curs3_solid1.html) | SOLID I (SRP, OCP, LSP) — laborator → **Lab3** (viitor) |

## Quick start — Lab 1–2

```bash
cd Lab1-2
dotnet run --project src/OrderManagement.Before
dotnet run --project src/OrderManagement.After
```

Documentație: [Lab1-2/README.md](Lab1-2/README.md) · [Lab3/README.md](Lab3/README.md)

## Quick start — Lab 3

```bash
cd Lab3
dotnet test TaskManager.sln
dotnet run --project src/TaskManager.UI
```
