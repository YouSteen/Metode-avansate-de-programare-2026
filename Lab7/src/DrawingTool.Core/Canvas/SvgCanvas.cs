using System.Globalization;
using System.Text;
using DrawingTool.Core.Interfaces;

namespace DrawingTool.Core.Canvas;

public class SvgCanvas : ICanvas
{
    private readonly List<string> _elements = new();

    public void DrawLine(double x1, double y1, double x2, double y2) =>
        _elements.Add(
            $"<line x1=\"{Fmt(x1)}\" y1=\"{Fmt(y1)}\" x2=\"{Fmt(x2)}\" y2=\"{Fmt(y2)}\" stroke=\"black\"/>");

    public void DrawCircle(double cx, double cy, double r) =>
        _elements.Add(
            $"<circle cx=\"{Fmt(cx)}\" cy=\"{Fmt(cy)}\" r=\"{Fmt(r)}\" fill=\"none\" stroke=\"black\"/>");

    public void DrawRect(double x, double y, double w, double h) =>
        _elements.Add(
            $"<rect x=\"{Fmt(x)}\" y=\"{Fmt(y)}\" width=\"{Fmt(w)}\" height=\"{Fmt(h)}\" fill=\"none\" stroke=\"black\"/>");

    public void DrawEllipse(double cx, double cy, double rx, double ry) =>
        _elements.Add(
            $"<ellipse cx=\"{Fmt(cx)}\" cy=\"{Fmt(cy)}\" rx=\"{Fmt(rx)}\" ry=\"{Fmt(ry)}\" fill=\"none\" stroke=\"black\"/>");

    public string GetSvg()
    {
        var body = string.Join("\n  ", _elements);
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 400 400\">");
        if (!string.IsNullOrEmpty(body))
            sb.Append("  ").AppendLine(body);
        sb.AppendLine("</svg>");
        return sb.ToString();
    }

    private static string Fmt(double value) =>
        value.ToString("0.##", CultureInfo.InvariantCulture);
}
