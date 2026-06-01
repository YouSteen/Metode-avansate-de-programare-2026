namespace DocumentGenerator.Core.Components;

public interface IDocumentFooter
{
    string Build(decimal? totalAmount = null);
}
