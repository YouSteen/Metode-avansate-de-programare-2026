using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Templates;

public class DocumentTemplate : ICloneable
{
    public string DefaultTitle { get; set; } = string.Empty;
    public List<DocumentSection> PredefinedSections { get; set; } = new();
    public FormatSettings Format { get; set; } = new();

    public object Clone()
    {
        return new DocumentTemplate
        {
            DefaultTitle = DefaultTitle,
            PredefinedSections = PredefinedSections.Select(s => s.Clone()).ToList(),
            Format = Format.Clone()
        };
    }

    public DocumentTemplate DeepClone() => (DocumentTemplate)Clone();

    public void ApplyTo(DocumentDataBuilder builder)
    {
        builder.WithTitle(DefaultTitle);
        foreach (var section in PredefinedSections)
            builder.WithSection(section.Title, section.Body);
        builder.WithPageFormat(Format.PageFormat);
        if (Format.Orientation == PageOrientation.Landscape)
            builder.InLandscape();
        if (!string.IsNullOrEmpty(Format.DefaultFootnote))
            builder.WithFootnote(Format.DefaultFootnote!);
    }
}
