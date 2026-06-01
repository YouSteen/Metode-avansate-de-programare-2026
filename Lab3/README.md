# Lab 3 — SOLID I (Task Manager)

Laborator **Curs 3** MAP: aplicatie .NET de gestiune sarcini cu **SRP**, **OCP**, **LSP** (DbC), repository SQLite si teste NUnit (texte fara diacritice in cod).

Material curs: [curs3_solid1.html](../curs3_solid1.html)

## Structura

```
Lab3/
  TaskManager.sln
  src/
    TaskManager.Core/       modele, interfete, servicii, notificari
    TaskManager.Data/       SqliteTaskRepository, InMemoryTaskRepository
    TaskManager.UI/         consola Spectre.Console + SlackNotifier (OCP demo)
  tests/
    TaskManager.Tests/      NUnit (doar InMemory, fara SQLite)
```

## Rulare

Din folderul `Lab3/`:

```bash
dotnet restore TaskManager.sln
dotnet build TaskManager.sln
dotnet test TaskManager.sln
dotnet run --project src/TaskManager.UI
```

Baza de date: `tasks.db` in directorul de rulare al UI. Log notificari fisier: `tasks.log`.

## Pachete NuGet

| Proiect | Pachet |
|---------|--------|
| TaskManager.Data | Microsoft.Data.Sqlite |
| TaskManager.UI | Spectre.Console |
| TaskManager.Tests | NUnit, NUnit3TestAdapter, Microsoft.NET.Test.Sdk |

## Arhitectura

| Strat | Rol |
|-------|-----|
| UI | Meniu; apeleaza doar `TaskService` |
| Core / Services | `TaskService` orchestreaza; `TaskValidator` valideaza |
| Core / Models | `TaskItem`, `RecurringTask`, `DeadlineTask` + `Complete()` |
| Core / Notifications | `ITaskNotifier` + implementari |
| Data | `ITaskRepository` — SQLite (productie) sau InMemory (teste) |

## Justificare SOLID (actori)

| Clasa | Actor | Motiv schimbare |
|-------|-------|-----------------|
| `ConsoleMenu` | Utilizator UI | Layout meniu, texte consola |
| `TaskService` | Product owner flux | Reguli adaugare/finalizare |
| `TaskValidator` | Reguli de validare | Politici titlu, due date |
| `ITaskRepository` / implementari | DBA / infrastructura | SQLite vs test in-memory |
| `ITaskNotifier` / implementari | Canal notificare | Email, Slack, log |
| `TaskItem` (+ subtipuri) | Domeniu sarcini | Contract `Complete()` |

O schimbare la SMTP/Slack nu modifica `TaskService` (OCP prin dictionar). O schimbare la SQL nu modifica validatorul (SRP).

## Cerinte implementate

### SRP

- UI fara logica de business
- `TaskService` fara acces direct SQL
- `TaskValidator` separat

### OCP

- `ITaskNotifier`, implementari in Core
- `TaskService` foloseste `IReadOnlyDictionary<NotificationChannel, ITaskNotifier>` (fara if/switch pe tip)
- `SlackNotifier` in UI, inregistrat doar in `Program.cs` — clase existente nemodificate

### LSP + DbC

- `Complete()`: preconditie (nu e Done), postconditie (Status Done), invarianta (nu Done + Overdue)
- Subclase suprascriu `CompleteCore()`; `RecurringTask` avanseaza `DueDate`

### Repository

- `SqliteTaskRepository` — mapare manuala `SqliteDataReader`
- `InMemoryTaskRepository` — teste

### Teste

- 13 teste NUnit, toate cu `InMemoryTaskRepository`
- Test parametrizat LSP (3 subtipuri), preconditie Done, OCP `MockNotifier`, validator titlu

## Legatura cu Lab 1-2

[Lab1-2](../Lab1-2/) — UML + Order Management. Acest lab este proiect **independent** (Task Manager), pregatit pentru extindere la Laboratorul 4 (ISP, DIP).
