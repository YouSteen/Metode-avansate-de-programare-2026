# Lab 7 — Pattern-uri structurale II (Composite, Bridge, Proxy)

Laborator **Curs 7** MAP: instrument grafic de desenare forme — **DrawingTool**.

Material curs: [curs7_patterns_structural2.html](../curs7_patterns_structural2.html)

## Structura

```
Lab7/
  DrawingTool.sln
  src/
    DrawingTool.Core/
    DrawingTool.App/
  tests/
    DrawingTool.Tests/
```

## Rulare

```bash
cd Lab7
dotnet restore DrawingTool.sln
dotnet build DrawingTool.sln
dotnet test DrawingTool.sln
dotnet run --project src/DrawingTool.App
```

SVG-ul generat se salveaza in `output/scena.svg`.

## Pattern-uri

### Composite

**Problema:** scene grafice din primitive si grupuri (`Picture`) imbricate; clientul trateaza uniform frunze si noduri compuse.

**Unde:** `IShape`, primitive in `Shapes/` (`Line`, `Circle`, `Rectangle`, `Ellipse`), `Picture` in `Composite/`.

Exemplu scena imbricata:

```csharp
var logo = new Picture("logo");
logo.Add(new Circle(50, 50, 30));
logo.Add(new Rectangle(20, 80, 60, 20));

var scene = new Picture("scena");
scene.Add(logo);                        // Picture in Picture
scene.Add(new Line(0, 0, 200, 0));

scene.Scale(2.0);   // se propaga recursiv
scene.Move(10, 5);
```

### Bridge

**Problema:** modelul formelor nu trebuie sa stie daca randarea e in consola sau SVG.

**Unde:** `ICanvas` (Implementor), `ConsoleCanvas`, `SvgCanvas` (`Canvas/`). `IShape.Draw(ICanvas)` delega randarea la canvas-ul primit.

### Proxy

**Problema:** forme blocate (read-only) — desen permis, mutare/scalare interzise.

**Unde:** `ReadOnlyShapeProxy` (`Proxy/`).

```csharp
IShape locked = new ReadOnlyShapeProxy(new Circle(100, 100, 20));
locked.Draw(canvas);   // OK
locked.Move(10, 10);   // InvalidOperationException: Shape is locked
```

## Cerinte laborator

| Cerinta | Implementare |
|---------|----------------|
| Ierarhie forme (Composite) | da |
| Bridge ConsoleCanvas + SvgCanvas | da |
| Proxy read-only | da |
| 5 teste NUnit cerute | da (9 teste) |

## Teste

- `CompositeTests` — propagare `Scale`, bounding box unificat, picturi imbricate
- `BridgeTests` — tag `<circle>` in SVG, document SVG complet
- `ProxyTests` — `Move`/`Scale` arunca exceptie, `Draw` si `GetBoundingBox` delega

## Exemplu SVG generat

Dupa `dotnet run`, `output/scena.svg` contine elemente `<circle>`, `<rect>`, `<line>`, `<ellipse>` pentru intreaga scena imbricata.
