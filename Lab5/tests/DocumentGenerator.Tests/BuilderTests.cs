using DocumentGenerator.Core.Builders;
using NUnit.Framework;

namespace DocumentGenerator.Tests;

[TestFixture]
public class BuilderTests
{
    [Test]
    public void Build_Throws_WhenTitleMissing()
    {
        var builder = new DocumentDataBuilder()
            .ByAuthor("Autor")
            .WithSection("S", "C");

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Test]
    public void Build_Succeeds_WhenValid()
    {
        var data = new DocumentDataBuilder()
            .WithTitle("Valid")
            .ByAuthor("Autor")
            .WithSection("S", "C")
            .Build();

        Assert.That(data.Title, Is.EqualTo("Valid"));
        Assert.That(data.Sections.Count, Is.EqualTo(1));
    }
}
