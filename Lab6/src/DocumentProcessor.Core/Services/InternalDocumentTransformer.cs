using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Services;

public class InternalDocumentTransformer : IDocumentTransformer
{
    public Document Transform(Document document) =>
        new()
        {
            Title = document.Title.Trim(),
            Content = document.Content.Trim(),
            SourceFormat = $"Internal-{document.SourceFormat}"
        };
}
