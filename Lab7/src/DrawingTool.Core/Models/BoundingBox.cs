namespace DrawingTool.Core.Models;

public readonly record struct BoundingBox(double MinX, double MinY, double MaxX, double MaxY)
{
    public static BoundingBox FromPoints(double x1, double y1, double x2, double y2) =>
        new(Math.Min(x1, x2), Math.Min(y1, y2), Math.Max(x1, x2), Math.Max(y1, y2));

    public static BoundingBox FromCenterRadius(double cx, double cy, double rx, double ry) =>
        new(cx - rx, cy - ry, cx + rx, cy + ry);

    public static BoundingBox Empty =>
        new(double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);

    public bool IsEmpty => double.IsPositiveInfinity(MinX);

    public BoundingBox Union(BoundingBox other)
    {
        if (IsEmpty)
            return other;
        if (other.IsEmpty)
            return this;

        return new BoundingBox(
            Math.Min(MinX, other.MinX),
            Math.Min(MinY, other.MinY),
            Math.Max(MaxX, other.MaxX),
            Math.Max(MaxY, other.MaxY));
    }
}
