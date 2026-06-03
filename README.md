# Metode avansate de programare — 2026

Repository pentru cursurile MAP: materiale HTML + laboratoare grupate pe foldere.

## Laboratoare

| Folder | Curs | Continut |
|--------|------|----------|
| [**Lab1-2/**](Lab1-2/) | Curs 2 — UML | Diagrame + `OrderManagement.Before` / `.After` |
| [**Lab3/**](Lab3/) | Curs 3 — SOLID I | Task Manager: SRP, OCP, LSP |
| [**Lab4/**](Lab4/) | Curs 4 — SOLID II | Task Manager: ISP, DIP, IoC (extinde Lab 3) |
| [**Lab5/**](Lab5/) | Curs 5 — Pattern-uri creationale | Generator documente (Factory, Builder, Prototype, Singleton) |
| [**Lab6/**](Lab6/) | Curs 6 — Pattern-uri structurale I | Procesor documente (Adapter, Decorator, Facade) |
| [**Lab7/**](Lab7/) | Curs 7 — Pattern-uri structurale II | DrawingTool (Composite, Bridge, Proxy) |
| [**Lab8/**](Lab8/) | Curs 8 — Pattern-uri comportamentale I | MusicPlayer (Observer, Strategy, Command) |

## Materiale curs

| Fisier | Subiect |
|--------|---------|
| [curs1_oop.html](curs1_oop.html) | Recapitulare OOP |
| [curs2_uml.html](curs2_uml.html) | UML → **Lab1-2** |
| [curs3_solid1.html](curs3_solid1.html) | SOLID I → **Lab3** |
| [curs4_solid2.html](curs4_solid2.html) | SOLID II → **Lab4** |
| [curs5_patterns_creational.html](curs5_patterns_creational.html) | Pattern-uri creationale → **Lab5** |
| [curs6_patterns_structural1.html](curs6_patterns_structural1.html) | Pattern-uri structurale I → **Lab6** |
| [curs7_patterns_structural2.html](curs7_patterns_structural2.html) | Pattern-uri structurale II → **Lab7** |
| [curs8_patterns_behavioral1.html](curs8_patterns_behavioral1.html) | Pattern-uri comportamentale I → **Lab8** |

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

**Lab 5 (Generator documente):**
```bash
cd Lab5
dotnet test DocumentGenerator.sln
dotnet run --project src/DocumentGenerator.App
```

**Lab 6 (Procesor documente):**
```bash
cd Lab6
dotnet test DocumentProcessor.sln
dotnet run --project src/DocumentProcessor.App
```

**Lab 7 (DrawingTool):**
```bash
cd Lab7
dotnet test DrawingTool.sln
dotnet run --project src/DrawingTool.App
```

**Lab 8 (MusicPlayer):**
```bash
cd Lab8
dotnet test MusicPlayer.sln
dotnet run --project src/MusicPlayer.App
```

Documentatie: [Lab1-2/README.md](Lab1-2/README.md) · [Lab3/README.md](Lab3/README.md) · [Lab4/README.md](Lab4/README.md) · [Lab5/README.md](Lab5/README.md) · [Lab6/README.md](Lab6/README.md) · [Lab7/README.md](Lab7/README.md) · [Lab8/README.md](Lab8/README.md)
