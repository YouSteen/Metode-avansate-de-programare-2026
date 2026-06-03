using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Shapes;

public class Circle : IShape
{
    public double Cx { get; private set; }
    public double Cy { get; private set; }
    public double Radius { get; private set; }

    public Circle(double cx, double cy, double radius)
    {
        Cx = cx;
        Cy = cy;
        Radius = radius;
    }

    public void Draw(ICanvas canvas) => canvas.DrawCircle(Cx, Cy, Radius);

    public void Move(double dx, double dy)
    {
        Cx += dx;
        Cy += dy;
    }

    public void Scale(double factor) => Radius *= factor;

    public BoundingBox GetBoundingBox() => BoundingBox.FromCenterRadius(Cx, Cy, Radius, Radius);
}
