namespace DocumentGenerator.Core.Components;

public class ReportComponentFactory : IDocumentComponentFactory
{
    private int _reportSequence = 1;

    public IDocumentHeader CreateHeader() => new ReportHeader(_reportSequence++);

    public IDocumentSectionComponent CreateSection() => new ReportSectionComponent();

    public IDocumentFooter CreateFooter() => new ReportFooter();
}

internal class ReportHeader : IDocumentHeader
{
    private readonly int _number;

    public ReportHeader(int number) => _number = number;

    public string Build() =>
        $"[RAPORT] Logo firma | Nr. raport: RPT-{_number:D4}";
}

internal class ReportSectionComponent : IDocumentSectionComponent
{
    public DocumentSectionPart Build(string sectionTitle, string content) =>
        new()
        {
            Title = sectionTitle,
            Body = $"Tabel date:\n| Coloana | Valoare |\n|---------|--------|\n{content}"
        };
}

internal class ReportFooter : IDocumentFooter
{
    public string Build(decimal? totalAmount = null) =>
        "Subsol raport: Semnatura responsabil _______________";
}
