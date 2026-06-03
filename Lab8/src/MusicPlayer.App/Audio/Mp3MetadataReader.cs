using MusicPlayer.Core.Models;
using NAudio.Wave;

namespace MusicPlayer.App.Audio;

public static class Mp3MetadataReader
{
    public static Track ReadFromFile(string path)
    {
        var fileName = Path.GetFileNameWithoutExtension(path);

        if (path.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
        {
            using var reader = new Mp3FileReader(path);
            var duration = reader.TotalTime;
            var title = string.IsNullOrWhiteSpace(reader.Tag.Title) ? fileName : reader.Tag.Title;
            var artist = string.IsNullOrWhiteSpace(reader.Tag.FirstPerformer)
                ? "Unknown"
                : reader.Tag.FirstPerformer;
            var album = reader.Tag.Album ?? string.Empty;

            return new Track(Guid.NewGuid(), title, artist, album, duration, path);
        }

        using var wavReader = new AudioFileReader(path);
        return new Track(
            Guid.NewGuid(),
            fileName,
            "Unknown",
            string.Empty,
            wavReader.TotalTime,
            path);
    }
}
