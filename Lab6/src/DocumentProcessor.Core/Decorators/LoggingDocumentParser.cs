using System.Diagnostics;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Decorators;

public class LoggingDocumentParser : DocumentParserDecorator
{
    public LoggingDocumentParser(IDocumentParser inner) : base(inner)
    {
    }

    public override Document Parse(string content)
    {
        Console.WriteLine($"[Log] Start parsing, input length={content.Length}");
        var sw = Stopwatch.StartNew();
        var document = Inner.Parse(content);
        sw.Stop();
        Console.WriteLine($"[Log] End parsing, elapsed={sw.ElapsedMilliseconds}ms, content size={document.Content.Length}");
        return document;
    }
}
