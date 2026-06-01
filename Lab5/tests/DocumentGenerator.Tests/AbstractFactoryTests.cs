using DocumentGenerator.Core.Assembly;
using DocumentGenerator.Core.Components;
using NUnit.Framework;

namespace DocumentGenerator.Tests;

[TestFixture]
public class AbstractFactoryTests
{
    [Test]
    public void Assembler_WithReportFactory_ProducesReportHeader()
    {
        var assembler = new DocumentAssembler(new ReportComponentFactory());
        var data = assembler.Assemble(
            "Titlu",
            "Autor",
            DateTime.UtcNow,
            new[] { new SectionInput { Title = "S", Content = "C" } });

        Assert.That(data.HeaderText, Does.Contain("RAPORT"));
        Assert.That(data.HeaderText, Does.Contain("Nr. raport"));
        Assert.That(data.FooterText, Does.Contain("Semnatura"));
    }

    [Test]
    public void Assembler_WithInvoiceFactory_ProducesInvoiceHeader()
    {
        var assembler = new DocumentAssembler(new InvoiceComponentFactory());
        var data = assembler.Assemble(
            "Titlu",
            "Autor",
            DateTime.UtcNow,
            new[] { new SectionInput { Title = "Linii", Content = "Item | 10" } },
            99m);

        Assert.That(data.HeaderText, Does.Contain("FACTURA"));
        Assert.That(data.HeaderText, Does.Contain("CUI"));
        Assert.That(data.FooterText, Does.Contain("TOTAL DE PLATA"));
    }
}
