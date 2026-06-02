using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Interfaces;

public interface IDocumentStorage
{
    string Save(Document document);
}
