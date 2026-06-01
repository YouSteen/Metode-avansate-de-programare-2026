using DocumentGenerator.Core.Builders;
using DocumentGenerator.Core.Export;
using NUnit.Framework;

namespace DocumentGenerator.Tests;

[TestFixture]
public class FactoryMethodTests
{
    private DocumentGenerator.Core.Models.DocumentData _sample = null!;

    [SetUp]
    public void SetUp()
    {
        _sample = new DocumentDataBuilder()
            .WithTitle("Test")
            .ByAuthor("Autor")
            .WithSection("S1", "Continut")
            .Build();
        _sample.HeaderText = "Antet";
        _sample.FooterText = "Subsol";
    }

    [Test]
    public void HtmlAndPlainText_ProduceDifferentOutput_ForSameData()
    {
        DocumentExporter html = new HtmlDocumentExporter();
        DocumentExporter plain = new PlainTextDocumentExporter();

        var htmlOut = html.Export(_sample);
        var plainOut = plain.Export(_sample);

        Assert.That(htmlOut, Does.Contain("<html>"));
        Assert.That(plainOut, Does.Not.Contain("<html>"));
        Assert.That(htmlOut, Is.Not.EqualTo(plainOut));
    }
}
