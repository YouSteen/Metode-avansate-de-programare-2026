using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Composite;

public class Picture : IShape
{
    private readonly string _name;
    private readonly List<IShape> _shapes = new();

    public Picture(string name) => _name = name;

    public string Name => _name;
    public IReadOnlyList<IShape> Shapes => _shapes;
    public int Count => _shapes.Count;

    public void Add(IShape shape) => _shapes.Add(shape);

    public void Remove(IShape shape) => _shapes.Remove(shape);

    public void Draw(ICanvas canvas)
    {
        foreach (var shape in _shapes)
            shape.Draw(canvas);
    }

    public void Move(double dx, double dy)
    {
        foreach (var shape in _shapes)
            shape.Move(dx, dy);
    }

    public void Scale(double factor)
    {
        foreach (var shape in _shapes)
            shape.Scale(factor);
    }

    public BoundingBox GetBoundingBox()
    {
        var box = BoundingBox.Empty;
        foreach (var shape in _shapes)
            box = box.Union(shape.GetBoundingBox());
        return box;
    }
}
