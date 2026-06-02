using System.Xml;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Adapter;

public class XmlParserAdapter : IDocumentParser
{
    private readonly LegacyXmlParser _legacy = new();

    public Document Parse(string content)
    {
        var xml = new XmlDocument();
        xml.LoadXml(content);
        var legacy = _legacy.ParseXml(xml);
        return new Document
        {
            Title = legacy.HeaderText,
            Content = legacy.BodyText,
            SourceFormat = "Xml"
        };
    }
}
