using System.Text.Json;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Parsers;

public class JsonDocumentParser : IDocumentParser
{
    public Document Parse(string content)
    {
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;
        return new Document
        {
            Title = root.GetProperty("title").GetString() ?? string.Empty,
            Content = root.GetProperty("content").GetString() ?? string.Empty,
            SourceFormat = "Json"
        };
    }
}
