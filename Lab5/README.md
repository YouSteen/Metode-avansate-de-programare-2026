# Lab 5 — Pattern-uri creationale (Generator Documente)

Laborator **Curs 5** MAP: sistem de generare rapoarte si facturi in HTML si text simplu.

Material curs: [curs5_patterns_creational.html](../curs5_patterns_creational.html)

## Structura

```
Lab5/
  DocumentGenerator.sln
  src/
    DocumentGenerator.Core/
    DocumentGenerator.App/
  tests/
    DocumentGenerator.Tests/
```

## Rulare

```bash
cd Lab5
dotnet restore DocumentGenerator.sln
dotnet build DocumentGenerator.sln
dotnet test DocumentGenerator.sln
dotnet run --project src/DocumentGenerator.App
```

Fisiere generate in `output/` (configurabil in `appsettings.json`).

## Pattern-uri si unde se regasesc

### Factory Method

**Problema:** clientul nu trebuie sa stie ce renderer concret se foloseste.

**Unde:** `DocumentExporter` + `CreateRenderer()` in `HtmlDocumentExporter` / `PlainTextDocumentExporter` (`Export/`).

### Abstract Factory

**Problema:** familii de componente (raport vs factura) trebuie create consistent.

**Unde:** `IDocumentComponentFactory`, `ReportComponentFactory`, `InvoiceComponentFactory`, `DocumentAssembler` (`Components/`, `Assembly/`).

### Builder

**Problema:** `DocumentData` are multi parametri; validare centralizata la `Build()`.

**Unde:** `DocumentDataBuilder` (`Builders/`).

### Prototype

**Problema:** sabloane predefinite trebuie clonate fara a modifica originalul.

**Unde:** `DocumentTemplate.DeepClone()`, `TemplateRegistry.Clone()` (`Templates/`).

### Singleton

**Problema:** configuratie globala incarcata o singura data.

**Unde:** `AppConfiguration` cu `Lazy<AppConfiguration>` (`Configuration/`).

**De ce Singleton aici:** setarile sunt read-only dupa incarcare, exista o singura sursa de adevar per proces. Pentru aplicatii mari, `AddSingleton<IOptions<AppSettings>>()` in IoC inlocuieste Singleton manual si permite testare mai usoara.

## Cerinte laborator

| Cerinta | Status |
|---------|--------|
| Factory Method exportatori | da |
| Abstract Factory componente | da |
| Builder metadate | da |
| Prototype registru sabloane | da |
| Singleton configuratie + appsettings.json | da |
| 4+ teste NUnit cerute | da (6 teste) |

## Teste

- `FactoryMethodTests` — HTML vs PlainText diferit
- `AbstractFactoryTests` — antet raport vs factura
- `BuilderTests` — validare titlu
- `PrototypeTests` — independenta clonari
