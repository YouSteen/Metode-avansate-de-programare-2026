using System.Text;
using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Export;

public class HtmlDocumentRenderer : IDocumentRenderer
{
    public string Render(DocumentData data)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html><html><head><meta charset=\"utf-8\"/>");
        sb.AppendLine($"<title>{Escape(data.Title)}</title></head><body>");
        sb.AppendLine($"<header>{Escape(data.HeaderText)}</header>");
        sb.AppendLine($"<h1>{Escape(data.Title)}</h1>");
        sb.AppendLine($"<p><em>{Escape(data.Author)}</em> — {data.Date:yyyy-MM-dd}</p>");
        foreach (var section in data.Sections)
        {
            sb.AppendLine($"<section><h2>{Escape(section.Title)}</h2>");
            sb.AppendLine($"<div>{Escape(section.Body)}</div></section>");
        }
        if (!string.IsNullOrEmpty(data.Footnote))
            sb.AppendLine($"<aside>{Escape(data.Footnote)}</aside>");
        sb.AppendLine($"<footer>{Escape(data.FooterText)}</footer>");
        sb.AppendLine("</body></html>");
        return sb.ToString();
    }

    private static string Escape(string text) =>
        text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
}
