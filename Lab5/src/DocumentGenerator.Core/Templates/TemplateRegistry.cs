namespace DocumentGenerator.Core.Templates;

public class TemplateRegistry
{
    private readonly Dictionary<string, DocumentTemplate> _prototypes = new();

    public void Register(string key, DocumentTemplate prototype) =>
        _prototypes[key] = prototype;

    public DocumentTemplate Clone(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new InvalidOperationException($"Sablon inexistent: {key}");
        return prototype.DeepClone();
    }

    public bool Contains(string key) => _prototypes.ContainsKey(key);
}
