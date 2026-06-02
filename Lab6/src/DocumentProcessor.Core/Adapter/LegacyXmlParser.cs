using System.Xml;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Adapter;

public class LegacyXmlParser
{
    public LegacyDocument ParseXml(XmlDocument xml)
    {
        var root = xml.DocumentElement
            ?? throw new InvalidOperationException("XML fara element radacina");

        var titleNode = root.SelectSingleNode("title")
            ?? root.SelectSingleNode("Title");
        var bodyNode = root.SelectSingleNode("body")
            ?? root.SelectSingleNode("content")
            ?? root.SelectSingleNode("Content");

        return new LegacyDocument
        {
            HeaderText = titleNode?.InnerText ?? string.Empty,
            BodyText = bodyNode?.InnerText ?? string.Empty
        };
    }
}
