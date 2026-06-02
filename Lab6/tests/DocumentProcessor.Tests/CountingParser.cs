using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Tests;

public class CountingParser : IDocumentParser
{
    public int ParseCount { get; private set; }

    public Document Parse(string content)
    {
        ParseCount++;
        return new Document
        {
            Title = "Titlu test",
            Content = "1234567890 continut",
            SourceFormat = "Mock"
        };
    }
}
