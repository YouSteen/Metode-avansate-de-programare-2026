using System.Security.Cryptography;
using System.Text;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Decorators;

public class CachingDocumentParser : DocumentParserDecorator
{
    private readonly Dictionary<string, Document> _cache = new();

    public CachingDocumentParser(IDocumentParser inner) : base(inner)
    {
    }

    public override Document Parse(string content)
    {
        var key = ComputeHash(content);
        if (_cache.TryGetValue(key, out var cached))
            return cached;

        var document = Inner.Parse(content);
        _cache[key] = document;
        return document;
    }

    private static string ComputeHash(string content)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(content));
        return Convert.ToHexString(bytes);
    }
}
