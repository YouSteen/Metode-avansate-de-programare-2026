namespace MusicPlayer.Core.Models;

public record Track(
    Guid Id,
    string Title,
    string Artist,
    string Album,
    TimeSpan Duration,
    string FilePath);
