namespace DocumentGenerator.Core.Models;

public class DocumentSection
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public DocumentSection Clone() => new() { Title = Title, Body = Body };
}
