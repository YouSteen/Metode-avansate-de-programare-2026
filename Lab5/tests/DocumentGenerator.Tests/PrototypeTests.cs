using DocumentGenerator.Core.Models;
using DocumentGenerator.Core.Templates;
using NUnit.Framework;

namespace DocumentGenerator.Tests;

[TestFixture]
public class PrototypeTests
{
    [Test]
    public void Clone_ReturnsIndependentObjects_ModifyingCloneDoesNotAffectPrototype()
    {
        var registry = new TemplateRegistry();
        var prototype = new DocumentTemplate
        {
            DefaultTitle = "Sablon original",
            PredefinedSections =
            {
                new DocumentSection { Title = "A", Body = "1" }
            },
            Format = new FormatSettings { DefaultFootnote = "Nota A" }
        };
        registry.Register("test", prototype);

        var clone1 = registry.Clone("test");
        var clone2 = registry.Clone("test");

        clone1.DefaultTitle = "Modificat 1";
        clone1.PredefinedSections[0].Body = "999";
        clone1.Format.DefaultFootnote = "Nota B";

        Assert.That(prototype.DefaultTitle, Is.EqualTo("Sablon original"));
        Assert.That(prototype.PredefinedSections[0].Body, Is.EqualTo("1"));
        Assert.That(prototype.Format.DefaultFootnote, Is.EqualTo("Nota A"));

        Assert.That(clone2.DefaultTitle, Is.EqualTo("Sablon original"));
        Assert.That(clone2.PredefinedSections[0].Body, Is.EqualTo("1"));
    }
}
