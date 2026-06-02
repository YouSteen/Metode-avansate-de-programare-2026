namespace DocumentProcessor.Core.Models;

public class Document
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string SourceFormat { get; set; } = string.Empty;
}
