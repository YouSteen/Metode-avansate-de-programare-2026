using DocumentGenerator.Core.Assembly;
using DocumentGenerator.Core.Builders;
using DocumentGenerator.Core.Configuration;
using DocumentGenerator.Core.Export;
using DocumentGenerator.Core.Models;
using DocumentGenerator.Core.Templates;

namespace DocumentGenerator.Core.Services;

public class DocumentGenerationService
{
    private readonly DocumentExporter _exporter;
    private readonly TemplateRegistry _templates;

    public DocumentGenerationService(DocumentExporter exporter, TemplateRegistry templates)
    {
        _exporter = exporter;
        _templates = templates;
    }

    public string GenerateAndSave(DocumentData data, string? fileName = null)
    {
        var content = _exporter.Export(data);
        var config = AppConfiguration.Instance;
        var dir = Path.Combine(AppContext.BaseDirectory, config.OutputDirectory);
        Directory.CreateDirectory(dir);
        var extension = _exporter is HtmlDocumentExporter ? "html" : "txt";
        var name = fileName ?? $"{Sanitize(data.Title)}_{DateTime.UtcNow:yyyyMMddHHmmss}";
        var path = Path.Combine(dir, $"{name}.{extension}");
        File.WriteAllText(path, content);
        return path;
    }

    public DocumentData BuildFromTemplate(string templateKey, string author, Action<DocumentDataBuilder>? customize = null)
    {
        var template = _templates.Clone(templateKey);
        var builder = new DocumentDataBuilder()
            .ByAuthor(author)
            .WithDate(DateTime.UtcNow);
        template.ApplyTo(builder);
        customize?.Invoke(builder);
        return builder.Build();
    }

    private static string Sanitize(string title)
    {
        var invalid = Path.GetInvalidFileNameChars();
        return string.Concat(title.Select(c => invalid.Contains(c) ? '_' : c));
    }
}
