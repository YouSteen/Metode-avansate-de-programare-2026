using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Decorators;

public abstract class DocumentParserDecorator : IDocumentParser
{
    protected readonly IDocumentParser Inner;

    protected DocumentParserDecorator(IDocumentParser inner) => Inner = inner;

    public virtual Document Parse(string content) => Inner.Parse(content);
}
