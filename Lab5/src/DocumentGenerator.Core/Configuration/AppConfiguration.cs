using System.Text.Json;

namespace DocumentGenerator.Core.Configuration;

public sealed class AppConfiguration
{
    private static readonly Lazy<AppConfiguration> InstanceHolder =
        new(() => new AppConfiguration());

    public static AppConfiguration Instance => InstanceHolder.Value;

    public string OutputDirectory { get; private set; } = "output";
    public string DefaultFormat { get; private set; } = "Html";
    public string DefaultAuthor { get; private set; } = "Echipa Documente";

    private AppConfiguration()
    {
        Load();
    }

    private void Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
            return;

        try
        {
            var json = File.ReadAllText(path);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.TryGetProperty("OutputDirectory", out var outDir))
                OutputDirectory = outDir.GetString() ?? OutputDirectory;
            if (root.TryGetProperty("DefaultFormat", out var fmt))
                DefaultFormat = fmt.GetString() ?? DefaultFormat;
            if (root.TryGetProperty("DefaultAuthor", out var author))
                DefaultAuthor = author.GetString() ?? DefaultAuthor;
        }
        catch
        {
        }
    }
}
