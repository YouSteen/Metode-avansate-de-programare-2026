using DrawingTool.Core.Canvas;
using DrawingTool.Core.Shapes;
using NUnit.Framework;

namespace DrawingTool.Tests;

[TestFixture]
public class BridgeTests
{
    [Test]
    public void SvgCanvas_AfterCircleDraw_ContainsCircleTag()
    {
        var canvas = new SvgCanvas();
        var circle = new Circle(50, 50, 25);

        circle.Draw(canvas);
        var svg = canvas.GetSvg();

        Assert.That(svg, Does.Contain("<circle"));
    }

    [Test]
    public void SvgCanvas_GetSvg_ReturnsCompleteDocument()
    {
        var canvas = new SvgCanvas();
        new Line(0, 0, 10, 10).Draw(canvas);
        new Rectangle(1, 2, 3, 4).Draw(canvas);

        var svg = canvas.GetSvg();

        Assert.That(svg, Does.Contain("<?xml"));
        Assert.That(svg, Does.Contain("<svg"));
        Assert.That(svg, Does.Contain("</svg>"));
        Assert.That(svg, Does.Contain("<line"));
        Assert.That(svg, Does.Contain("<rect"));
    }
}
