using DocumentGenerator.Core.Assembly;
using DocumentGenerator.Core.Builders;
using DocumentGenerator.Core.Components;
using DocumentGenerator.Core.Configuration;
using DocumentGenerator.Core.Export;
using DocumentGenerator.Core.Models;
using DocumentGenerator.Core.Services;
using DocumentGenerator.Core.Templates;

var config = AppConfiguration.Instance;
var registry = BootstrapTemplates();
var running = true;

Console.WriteLine("=== Generator Documente (Lab 5) ===");
Console.WriteLine($"Director iesire: {config.OutputDirectory}");
Console.WriteLine($"Format implicit: {config.DefaultFormat}");
Console.WriteLine($"Autor implicit: {config.DefaultAuthor}");
Console.WriteLine();

while (running)
{
    Console.WriteLine("1. Raport HTML (Factory Method + Abstract Factory)");
    Console.WriteLine("2. Factura text (Factory Method + Abstract Factory)");
    Console.WriteLine("3. Document din sablon (Prototype)");
    Console.WriteLine("4. Document custom (Builder)");
    Console.WriteLine("0. Iesire");
    Console.Write("Optiune: ");
    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                GenerateReportHtml(registry);
                break;
            case "2":
                GenerateInvoiceText(registry);
                break;
            case "3":
                GenerateFromTemplate(registry, config.DefaultAuthor);
                break;
            case "4":
                GenerateWithBuilder(config.DefaultAuthor);
                break;
            case "0":
                running = false;
                break;
            default:
                Console.WriteLine("Optiune invalida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Eroare: {ex.Message}");
    }

    Console.WriteLine();
}

static TemplateRegistry BootstrapTemplates()
{
    var registry = new TemplateRegistry();
    registry.Register("raport-lunar", new DocumentTemplate
    {
        DefaultTitle = "Raport lunar vanzari",
        PredefinedSections =
        {
            new DocumentSection { Title = "Rezumat", Body = "Venit | 10000\nCheltuieli | 7000" },
            new DocumentSection { Title = "Detalii", Body = "Regiune Nord | 4000\nRegiune Sud | 6000" }
        },
        Format = new FormatSettings
        {
            PageFormat = PageFormat.A4,
            Orientation = PageOrientation.Portrait,
            DefaultFootnote = "Date provizorii, sub rezerva auditului."
        }
    });
    registry.Register("factura-servicii", new DocumentTemplate
    {
        DefaultTitle = "Factura servicii IT",
        PredefinedSections =
        {
            new DocumentSection { Title = "Servicii", Body = "Mentenanta | 1 x 500 RON\nConsultanta | 10 ore x 150 RON" }
        },
        Format = new FormatSettings { PageFormat = PageFormat.A4 }
    });
    return registry;
}

static void GenerateReportHtml(TemplateRegistry registry)
{
    DocumentExporter exporter = new HtmlDocumentExporter();
    var assembler = new DocumentAssembler(new ReportComponentFactory());
    var data = assembler.Assemble(
        "Raport performanta Q1",
        AppConfiguration.Instance.DefaultAuthor,
        DateTime.UtcNow,
        new[]
        {
            new SectionInput { Title = "KPI", Content = "Conversie | 12%\nRetentie | 85%" }
        });
    MergeMetadata(data, registry, "raport-lunar");
    var service = new DocumentGenerationService(exporter, registry);
    var path = service.GenerateAndSave(data);
    Console.WriteLine($"Salvat: {path}");
}

static void GenerateInvoiceText(TemplateRegistry registry)
{
    DocumentExporter exporter = new PlainTextDocumentExporter();
    var assembler = new DocumentAssembler(new InvoiceComponentFactory());
    var data = assembler.Assemble(
        "Factura 2026-042",
        AppConfiguration.Instance.DefaultAuthor,
        DateTime.UtcNow,
        new[]
        {
            new SectionInput { Title = "Produse", Content = "Licenta software | 1200 RON\nSuport | 300 RON" }
        },
        invoiceTotal: 1500m);
    var service = new DocumentGenerationService(exporter, registry);
    var path = service.GenerateAndSave(data);
    Console.WriteLine($"Salvat: {path}");
}

static void GenerateFromTemplate(TemplateRegistry registry, string author)
{
    DocumentExporter exporter = AppConfiguration.Instance.DefaultFormat.Equals("Html", StringComparison.OrdinalIgnoreCase)
        ? new HtmlDocumentExporter()
        : new PlainTextDocumentExporter();
    var service = new DocumentGenerationService(exporter, registry);
    var data = service.BuildFromTemplate("raport-lunar", author, b =>
        b.WithTitle("Raport lunar personalizat"));
    var path = service.GenerateAndSave(data);
    Console.WriteLine($"Salvat din sablon: {path}");
}

static void GenerateWithBuilder(string author)
{
    var data = new DocumentDataBuilder()
        .WithTitle("Nota interna")
        .ByAuthor(author)
        .WithDate(DateTime.UtcNow)
        .WithSection("Introducere", "Scopul documentului este informarea echipei.")
        .WithSection("Concluzii", "Proiectul este in grafic.")
        .InLandscape()
        .WithFootnote("Distribuire interna.")
        .Build();

    data.HeaderText = "[NOTA] Departament IT";
    data.FooterText = "Generat automat";

    DocumentExporter exporter = new HtmlDocumentExporter();
    var service = new DocumentGenerationService(exporter, new TemplateRegistry());
    var path = service.GenerateAndSave(data);
    Console.WriteLine($"Salvat: {path}");
}

static void MergeMetadata(DocumentData target, TemplateRegistry registry, string templateKey)
{
    if (!registry.Contains(templateKey))
        return;
    var template = registry.Clone(templateKey);
    if (string.IsNullOrWhiteSpace(target.Footnote) && !string.IsNullOrEmpty(template.Format.DefaultFootnote))
        target.Footnote = template.Format.DefaultFootnote;
    target.Format = template.Format.PageFormat;
    target.Orientation = template.Format.Orientation;
}
