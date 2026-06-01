namespace DocumentGenerator.Core.Components;

public interface IDocumentSectionComponent
{
    DocumentSectionPart Build(string sectionTitle, string content);
}

public class DocumentSectionPart
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
