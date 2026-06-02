namespace DocumentProcessor.Core.Models;

public class ProcessingResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? SavedPath { get; init; }

    public static ProcessingResult Success(string savedPath, string message = "Procesare reusita") =>
        new() { IsSuccess = true, SavedPath = savedPath, Message = message };

    public static ProcessingResult Failure(string message) =>
        new() { IsSuccess = false, Message = message };
}
