using DocumentGenerator.Core.Components;
using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Assembly;

public class DocumentAssembler
{
    private readonly IDocumentComponentFactory _factory;

    public DocumentAssembler(IDocumentComponentFactory factory) =>
        _factory = factory;

    public DocumentData Assemble(
        string title,
        string author,
        DateTime date,
        IReadOnlyList<SectionInput> sections,
        decimal? invoiceTotal = null)
    {
        var header = _factory.CreateHeader();
        var sectionBuilder = _factory.CreateSection();
        var footer = _factory.CreateFooter();

        var data = new DocumentData
        {
            Title = title,
            Author = author,
            Date = date,
            HeaderText = header.Build()
        };

        foreach (var input in sections)
        {
            var part = sectionBuilder.Build(input.Title, input.Content);
            data.Sections.Add(new DocumentSection { Title = part.Title, Body = part.Body });
        }

        data.FooterText = footer.Build(invoiceTotal);
        return data;
    }
}
