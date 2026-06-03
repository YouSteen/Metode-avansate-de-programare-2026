using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Proxy;

public class ReadOnlyShapeProxy : IShape
{
    private readonly IShape _inner;

    public ReadOnlyShapeProxy(IShape inner) => _inner = inner;

    public void Draw(ICanvas canvas) => _inner.Draw(canvas);

    public void Move(double dx, double dy) =>
        throw new InvalidOperationException("Shape is locked");

    public void Scale(double factor) =>
        throw new InvalidOperationException("Shape is locked");

    public BoundingBox GetBoundingBox() => _inner.GetBoundingBox();
}
