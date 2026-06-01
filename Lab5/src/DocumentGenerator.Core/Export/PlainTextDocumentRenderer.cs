using System.Text;
using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Export;

public class PlainTextDocumentRenderer : IDocumentRenderer
{
    public string Render(DocumentData data)
    {
        var sb = new StringBuilder();
        sb.AppendLine(data.HeaderText);
        sb.AppendLine(new string('=', 60));
        sb.AppendLine(data.Title.ToUpperInvariant());
        sb.AppendLine($"Autor: {data.Author} | Data: {data.Date:yyyy-MM-dd}");
        sb.AppendLine(new string('-', 60));
        foreach (var section in data.Sections)
        {
            sb.AppendLine($"[{section.Title}]");
            sb.AppendLine(section.Body);
            sb.AppendLine();
        }
        if (!string.IsNullOrEmpty(data.Footnote))
            sb.AppendLine($"Nota: {data.Footnote}");
        sb.AppendLine(new string('-', 60));
        sb.AppendLine(data.FooterText);
        return sb.ToString();
    }
}
