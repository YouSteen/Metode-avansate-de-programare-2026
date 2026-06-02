using DocumentProcessor.Core.Facade;

var facade = new DocumentProcessingFacade();
var samplesDir = Path.Combine(AppContext.BaseDirectory, "samples");

Console.WriteLine("=== Procesor Documente (Lab 6) ===");
Console.WriteLine();

if (!Directory.Exists(samplesDir))
{
    Console.WriteLine("Folder samples lipseste.");
    return;
}

foreach (var file in Directory.GetFiles(samplesDir))
{
    Console.WriteLine($"Procesez: {Path.GetFileName(file)}");
    var result = facade.Process(file);
    if (result.IsSuccess)
        Console.WriteLine($"  OK -> {result.SavedPath}");
    else
        Console.WriteLine($"  ESEC -> {result.Message}");
    Console.WriteLine();
}

Console.WriteLine("Introdu calea unui fisier XML/JSON (gol pentru iesire):");
var path = Console.ReadLine();
while (!string.IsNullOrWhiteSpace(path))
{
    var result = facade.Process(path.Trim());
    Console.WriteLine(result.IsSuccess
        ? $"OK: {result.SavedPath}"
        : $"Esec: {result.Message}");
    Console.WriteLine();
    Console.WriteLine("Cale urmatoare (gol pentru iesire):");
    path = Console.ReadLine();
}
