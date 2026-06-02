using DocumentProcessor.Core.Adapter;
using DocumentProcessor.Core.Decorators;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;
using DocumentProcessor.Core.Parsers;
using DocumentProcessor.Core.Services;

namespace DocumentProcessor.Core.Facade;

public class DocumentProcessingFacade
{
    private readonly IDocumentTransformer _transformer;
    private readonly IDocumentStorage _storage;
    private readonly bool _enableLogging;

    public DocumentProcessingFacade(
        IDocumentTransformer? transformer = null,
        IDocumentStorage? storage = null,
        bool enableLogging = true)
    {
        _transformer = transformer ?? new InternalDocumentTransformer();
        _storage = storage ?? new FileDocumentStorage();
        _enableLogging = enableLogging;
    }

    public ProcessingResult Process(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return ProcessingResult.Failure("Fisierul nu exista");

            var content = File.ReadAllText(filePath);
            var parser = CreateParser(filePath);
            var document = parser.Parse(content);
            var transformed = _transformer.Transform(document);
            var savedPath = _storage.Save(transformed);
            return ProcessingResult.Success(savedPath);
        }
        catch (Exception ex)
        {
            return ProcessingResult.Failure(ex.Message);
        }
    }

    private IDocumentParser CreateParser(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        IDocumentParser core = extension switch
        {
            ".json" => new JsonDocumentParser(),
            _ => new XmlParserAdapter()
        };

        IDocumentParser pipeline = new ValidationDocumentParser(
            new CachingDocumentParser(core));

        if (_enableLogging)
            pipeline = new LoggingDocumentParser(pipeline);

        return pipeline;
    }
}
