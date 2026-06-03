using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Strategies;

public class SequentialStrategy : IPlaybackStrategy
{
    public string Name => "Sequential";

    public bool RepeatAll { get; set; }

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        if (playlist.Count == 0)
            return null;

        if (currentTrack is null)
            return playlist.GetAt(0);

        var index = playlist.IndexOf(currentTrack);
        if (index < 0)
            return playlist.GetAt(0);

        if (index < playlist.Count - 1)
            return playlist.GetAt(index + 1);

        return RepeatAll ? playlist.GetAt(0) : null;
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        if (playlist.Count == 0 || currentTrack is null)
            return null;

        var index = playlist.IndexOf(currentTrack);
        if (index <= 0)
            return RepeatAll ? playlist.GetAt(playlist.Count - 1) : null;

        return playlist.GetAt(index - 1);
    }

    public void Reset(Playlist playlist)
    {
    }
}
