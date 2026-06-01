namespace DocumentGenerator.Core.Components;

public class InvoiceComponentFactory : IDocumentComponentFactory
{
    public IDocumentHeader CreateHeader() => new InvoiceHeader();

    public IDocumentSectionComponent CreateSection() => new InvoiceSectionComponent();

    public IDocumentFooter CreateFooter() => new InvoiceFooter();
}

internal class InvoiceHeader : IDocumentHeader
{
    public string Build() =>
        "[FACTURA] SC Exemplu SRL | CUI: RO12345678 | Adresa: Str. Principala 1";
}

internal class InvoiceSectionComponent : IDocumentSectionComponent
{
    public DocumentSectionPart Build(string sectionTitle, string content) =>
        new()
        {
            Title = sectionTitle,
            Body = $"Linii factura:\n{content}"
        };
}

internal class InvoiceFooter : IDocumentFooter
{
    public string Build(decimal? totalAmount = null)
    {
        var total = totalAmount ?? 0m;
        return $"Subsol factura: TOTAL DE PLATA: {total:C}";
    }
}
