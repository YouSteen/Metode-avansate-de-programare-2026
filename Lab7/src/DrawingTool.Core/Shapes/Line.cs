using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Shapes;

public class Line : IShape
{
    public double X1 { get; private set; }
    public double Y1 { get; private set; }
    public double X2 { get; private set; }
    public double Y2 { get; private set; }

    public Line(double x1, double y1, double x2, double y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public void Draw(ICanvas canvas) => canvas.DrawLine(X1, Y1, X2, Y2);

    public void Move(double dx, double dy)
    {
        X1 += dx;
        Y1 += dy;
        X2 += dx;
        Y2 += dy;
    }

    public void Scale(double factor)
    {
        X1 *= factor;
        Y1 *= factor;
        X2 *= factor;
        Y2 *= factor;
    }

    public BoundingBox GetBoundingBox() => BoundingBox.FromPoints(X1, Y1, X2, Y2);
}
