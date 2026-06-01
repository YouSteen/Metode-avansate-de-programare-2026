namespace DocumentGenerator.Core.Models;

public class DocumentData
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public List<DocumentSection> Sections { get; set; } = new();
    public PageFormat Format { get; set; } = PageFormat.A4;
    public PageOrientation Orientation { get; set; } = PageOrientation.Portrait;
    public string? Footnote { get; set; }
    public string HeaderText { get; set; } = string.Empty;
    public string FooterText { get; set; } = string.Empty;
}
