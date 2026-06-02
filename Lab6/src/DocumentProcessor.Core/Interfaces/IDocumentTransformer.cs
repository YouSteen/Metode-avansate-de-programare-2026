using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Interfaces;

public interface IDocumentTransformer
{
    Document Transform(Document document);
}
