using DrawingTool.Core.Interfaces;

namespace DrawingTool.Core.Canvas;

public class ConsoleCanvas : ICanvas
{
    public void DrawLine(double x1, double y1, double x2, double y2) =>
        Console.WriteLine($"  LINE    ({x1:F1},{y1:F1}) -> ({x2:F1},{y2:F1})");

    public void DrawCircle(double cx, double cy, double r) =>
        Console.WriteLine($"  CIRCLE  center=({cx:F1},{cy:F1}) r={r:F1}");

    public void DrawRect(double x, double y, double w, double h) =>
        Console.WriteLine($"  RECT    ({x:F1},{y:F1}) {w:F1}x{h:F1}");

    public void DrawEllipse(double cx, double cy, double rx, double ry) =>
        Console.WriteLine($"  ELLIPSE center=({cx:F1},{cy:F1}) rx={rx:F1} ry={ry:F1}");
}
