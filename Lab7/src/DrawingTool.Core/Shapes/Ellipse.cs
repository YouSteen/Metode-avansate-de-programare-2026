using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Shapes;

public class Ellipse : IShape
{
    public double Cx { get; private set; }
    public double Cy { get; private set; }
    public double Rx { get; private set; }
    public double Ry { get; private set; }

    public Ellipse(double cx, double cy, double rx, double ry)
    {
        Cx = cx;
        Cy = cy;
        Rx = rx;
        Ry = ry;
    }

    public void Draw(ICanvas canvas) => canvas.DrawEllipse(Cx, Cy, Rx, Ry);

    public void Move(double dx, double dy)
    {
        Cx += dx;
        Cy += dy;
    }

    public void Scale(double factor)
    {
        Rx *= factor;
        Ry *= factor;
    }

    public BoundingBox GetBoundingBox() => BoundingBox.FromCenterRadius(Cx, Cy, Rx, Ry);
}
