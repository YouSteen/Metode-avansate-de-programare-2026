using DrawingTool.Core.Composite;
using DrawingTool.Core.Shapes;
using NUnit.Framework;

namespace DrawingTool.Tests;

[TestFixture]
public class CompositeTests
{
    [Test]
    public void Picture_Scale_PropagatesToAllChildren()
    {
        var picture = new Picture("grup");
        var circle = new Circle(10, 10, 5);
        var rectangle = new Rectangle(0, 0, 10, 5);
        var line = new Line(0, 0, 10, 0);

        picture.Add(circle);
        picture.Add(rectangle);
        picture.Add(line);
        picture.Scale(2.0);

        Assert.That(circle.Radius, Is.EqualTo(10));
        Assert.That(rectangle.Width, Is.EqualTo(20));
        Assert.That(rectangle.Height, Is.EqualTo(10));
        Assert.That(line.X2, Is.EqualTo(20));
    }

    [Test]
    public void Picture_GetBoundingBox_UnifiesChildBounds()
    {
        var picture = new Picture("grup");
        picture.Add(new Circle(0, 0, 10));
        picture.Add(new Rectangle(5, 5, 10, 10));

        var box = picture.GetBoundingBox();

        Assert.That(box.MinX, Is.EqualTo(-10));
        Assert.That(box.MinY, Is.EqualTo(-10));
        Assert.That(box.MaxX, Is.EqualTo(15));
        Assert.That(box.MaxY, Is.EqualTo(15));
    }

    [Test]
    public void NestedPictures_Scale_PropagatesRecursively()
    {
        var inner = new Picture("inner");
        inner.Add(new Circle(0, 0, 5));

        var outer = new Picture("outer");
        outer.Add(inner);
        outer.Scale(2.0);

        var circle = (Circle)inner.Shapes[0];
        Assert.That(circle.Radius, Is.EqualTo(10));
    }
}
