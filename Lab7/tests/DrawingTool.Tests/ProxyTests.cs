using DrawingTool.Core.Canvas;
using DrawingTool.Core.Proxy;
using DrawingTool.Core.Shapes;
using NUnit.Framework;

namespace DrawingTool.Tests;

[TestFixture]
public class ProxyTests
{
    [Test]
    public void ReadOnlyShapeProxy_Move_ThrowsInvalidOperationException()
    {
        IShape locked = new ReadOnlyShapeProxy(new Circle(0, 0, 10));

        Assert.Throws<InvalidOperationException>(() => locked.Move(1, 1));
    }

    [Test]
    public void ReadOnlyShapeProxy_Scale_ThrowsInvalidOperationException()
    {
        IShape locked = new ReadOnlyShapeProxy(new Circle(0, 0, 10));

        Assert.Throws<InvalidOperationException>(() => locked.Scale(2.0));
    }

    [Test]
    public void ReadOnlyShapeProxy_Draw_DelegatesWithoutThrowing()
    {
        IShape locked = new ReadOnlyShapeProxy(new Circle(10, 10, 5));
        var canvas = new SvgCanvas();

        Assert.DoesNotThrow(() => locked.Draw(canvas));
        Assert.That(canvas.GetSvg(), Does.Contain("<circle"));
    }

    [Test]
    public void ReadOnlyShapeProxy_GetBoundingBox_DelegatesToInnerShape()
    {
        var circle = new Circle(5, 5, 10);
        IShape locked = new ReadOnlyShapeProxy(circle);

        var box = locked.GetBoundingBox();

        Assert.That(box.MinX, Is.EqualTo(-5));
        Assert.That(box.MaxX, Is.EqualTo(15));
    }
}
