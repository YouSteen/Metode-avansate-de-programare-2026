using DocumentProcessor.Core.Exceptions;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Decorators;

public class ValidationDocumentParser : DocumentParserDecorator
{
    public ValidationDocumentParser(IDocumentParser inner) : base(inner)
    {
    }

    public override Document Parse(string content)
    {
        var document = Inner.Parse(content);
        if (string.IsNullOrWhiteSpace(document.Title))
            throw new ValidationException("Titlul documentului este gol");
        if (document.Content.Length < 10)
            throw new ValidationException("Continutul trebuie sa aiba minim 10 caractere");
        return document;
    }
}
