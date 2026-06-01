namespace DocumentGenerator.Core.Models;

public class FormatSettings
{
    public PageFormat PageFormat { get; set; } = PageFormat.A4;
    public PageOrientation Orientation { get; set; } = PageOrientation.Portrait;
    public string? DefaultFootnote { get; set; }

    public FormatSettings Clone() => new()
    {
        PageFormat = PageFormat,
        Orientation = Orientation,
        DefaultFootnote = DefaultFootnote
    };
}
