# Lab 6 — Pattern-uri structurale I (Adapter, Decorator, Facade)

Laborator **Curs 6** MAP: procesare documente din XML legacy si JSON.

Material curs: [curs6_patterns_structural1.html](../curs6_patterns_structural1.html)

## Structura

```
Lab6/
  DocumentProcessor.sln
  src/
    DocumentProcessor.Core/
    DocumentProcessor.App/
  tests/
    DocumentProcessor.Tests/
```

## Rulare

```bash
cd Lab6
dotnet restore DocumentProcessor.sln
dotnet build DocumentProcessor.sln
dotnet test DocumentProcessor.sln
dotnet run --project src/DocumentProcessor.App
```

Fisiere procesate salvate in `processed/`.

## Pattern-uri

### Adapter

**Problema:** `LegacyXmlParser` foloseste `XmlDocument` si `LegacyDocument` — incompatibil cu `IDocumentParser`.

**Unde:** `LegacyXmlParser` (nemodificat), `XmlParserAdapter` (`Adapter/`).

### Decorator

**Problema:** logging, validare si cache trebuie combinate la runtime fara subclassing.

**Unde:** `DocumentParserDecorator`, `LoggingDocumentParser`, `ValidationDocumentParser`, `CachingDocumentParser` (`Decorators/`).

Exemplu lant:

```csharp
new LoggingDocumentParser(
  new ValidationDocumentParser(
    new XmlParserAdapter()));
```

### Facade

**Problema:** clientul nu trebuie sa cunoasca parseri, decoratori, transformare si stocare.

**Unde:** `DocumentProcessingFacade.Process(filePath)` (`Facade/`).

## Cerinte laborator

| Cerinta | Implementare |
|---------|----------------|
| Adapter XML legacy + JSON nativ | da |
| Decoratori Logging, Validation, Caching | da |
| Facade cu ProcessingResult | da |
| 4 teste NUnit | da (+ test succes JSON) |

## Teste

- `AdapterTests` — XML si JSON echivalent
- `DecoratorTests` — validare, cache cu `CountingParser`, lant combinat
- `FacadeTests` — esec fara exceptie, succes JSON
