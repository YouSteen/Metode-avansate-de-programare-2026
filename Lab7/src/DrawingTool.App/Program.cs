using DrawingTool.Core.Canvas;
using DrawingTool.Core.Composite;
using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Proxy;
using DrawingTool.Core.Shapes;

Console.WriteLine("=== DrawingTool (Lab 7) ===");
Console.WriteLine();

var logo = new Picture("logo");
logo.Add(new Circle(50, 50, 30));
logo.Add(new Rectangle(20, 80, 60, 20));

var icon = new Picture("icon");
icon.Add(new Ellipse(15, 15, 10, 6));
icon.Add(new Line(5, 25, 25, 25));

var scene = new Picture("scena");
scene.Add(logo);
scene.Add(icon);
scene.Add(new Line(0, 0, 200, 0));

Console.WriteLine("Desen consola (Composite + Bridge — ConsoleCanvas):");
Console.WriteLine($"Picture '{scene.Name}' ({scene.Count} elemente directe):");
scene.Draw(new ConsoleCanvas());
Console.WriteLine();

Console.WriteLine("Bounding box scena:");
var box = scene.GetBoundingBox();
Console.WriteLine($"  ({box.MinX:F1},{box.MinY:F1}) -> ({box.MaxX:F1},{box.MaxY:F1})");
Console.WriteLine();

Console.WriteLine("Export SVG (Bridge — SvgCanvas):");
var svgCanvas = new SvgCanvas();
scene.Draw(svgCanvas);
var svg = svgCanvas.GetSvg();

var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
Directory.CreateDirectory(outputDir);
var svgPath = Path.Combine(outputDir, "scena.svg");
File.WriteAllText(svgPath, svg);
Console.WriteLine($"  Salvat: {svgPath}");
Console.WriteLine();

Console.WriteLine("Proxy read-only (Circle blocat):");
IShape lockedCircle = new ReadOnlyShapeProxy(new Circle(100, 100, 20));
Console.Write("  Draw: ");
lockedCircle.Draw(new ConsoleCanvas());
Console.Write("  Move: ");
try
{
    lockedCircle.Move(10, 10);
    Console.WriteLine("permis (neasteptat)");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}

Console.WriteLine();
Console.WriteLine("Operatii pe scena (Scale 2x, Move +10,+5):");
scene.Scale(2.0);
scene.Move(10, 5);
Console.WriteLine($"Picture '{scene.Name}' dupa transformari:");
scene.Draw(new ConsoleCanvas());
