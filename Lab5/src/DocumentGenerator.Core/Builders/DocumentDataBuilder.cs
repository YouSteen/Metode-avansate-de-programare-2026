using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Builders;

public class DocumentDataBuilder
{
    private string? _title;
    private string? _author;
    private DateTime _date = DateTime.UtcNow;
    private readonly List<DocumentSection> _sections = new();
    private PageFormat _format = PageFormat.A4;
    private PageOrientation _orientation = PageOrientation.Portrait;
    private string? _footnote;

    public DocumentDataBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public DocumentDataBuilder ByAuthor(string author)
    {
        _author = author;
        return this;
    }

    public DocumentDataBuilder WithDate(DateTime date)
    {
        _date = date;
        return this;
    }

    public DocumentDataBuilder WithSection(string sectionTitle, string body)
    {
        _sections.Add(new DocumentSection { Title = sectionTitle, Body = body });
        return this;
    }

    public DocumentDataBuilder WithPageFormat(PageFormat format)
    {
        _format = format;
        return this;
    }

    public DocumentDataBuilder InLandscape()
    {
        _orientation = PageOrientation.Landscape;
        return this;
    }

    public DocumentDataBuilder WithFootnote(string footnote)
    {
        _footnote = footnote;
        return this;
    }

    public DocumentData Build()
    {
        if (string.IsNullOrWhiteSpace(_title))
            throw new InvalidOperationException("Titlul este obligatoriu");

        if (string.IsNullOrWhiteSpace(_author))
            throw new InvalidOperationException("Autorul este obligatoriu");

        if (_sections.Count == 0)
            throw new InvalidOperationException("Cel putin o sectiune este obligatorie");

        return new DocumentData
        {
            Title = _title,
            Author = _author,
            Date = _date,
            Sections = _sections.Select(s => s.Clone()).ToList(),
            Format = _format,
            Orientation = _orientation,
            Footnote = _footnote
        };
    }
}
