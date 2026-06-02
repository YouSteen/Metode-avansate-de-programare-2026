using DocumentProcessor.Core.Adapter;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Parsers;
using NUnit.Framework;

namespace DocumentProcessor.Tests;

[TestFixture]
public class AdapterTests
{
  private const string EquivalentTitle = "Raport lunar";
  private const string EquivalentContent = "1234567890 date procesate";

  [Test]
  public void XmlAdapter_And_JsonParser_ReturnEquivalentDocuments()
  {
    IDocumentParser xml = new XmlParserAdapter();
    IDocumentParser json = new JsonDocumentParser();

    var xmlContent = $"""
      <document>
        <title>{EquivalentTitle}</title>
        <content>{EquivalentContent}</content>
      </document>
      """;

    var jsonContent = $$"""
      {"title":"{{EquivalentTitle}}","content":"{{EquivalentContent}}"}
      """;

    var fromXml = xml.Parse(xmlContent);
    var fromJson = json.Parse(jsonContent);

    Assert.That(fromXml, Is.InstanceOf<DocumentProcessor.Core.Models.Document>());
    Assert.That(fromJson, Is.InstanceOf<DocumentProcessor.Core.Models.Document>());
    Assert.That(fromXml.Title, Is.EqualTo(fromJson.Title));
    Assert.That(fromXml.Content, Is.EqualTo(fromJson.Content));
  }
}
