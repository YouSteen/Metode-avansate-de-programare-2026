using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Services;

public class FileDocumentStorage : IDocumentStorage
{
    private readonly string _outputDirectory;

    public FileDocumentStorage(string? outputDirectory = null) =>
        _outputDirectory = outputDirectory ?? Path.Combine(AppContext.BaseDirectory, "processed");

    public string Save(Document document)
    {
        Directory.CreateDirectory(_outputDirectory);
        var safeTitle = string.Concat(document.Title.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
        if (string.IsNullOrWhiteSpace(safeTitle))
            safeTitle = "document";
        var path = Path.Combine(_outputDirectory, $"{safeTitle}_{DateTime.UtcNow:yyyyMMddHHmmss}.txt");
        var payload = $"TITLE: {document.Title}{Environment.NewLine}FORMAT: {document.SourceFormat}{Environment.NewLine}{Environment.NewLine}{document.Content}";
        File.WriteAllText(path, payload);
        return path;
    }
}
