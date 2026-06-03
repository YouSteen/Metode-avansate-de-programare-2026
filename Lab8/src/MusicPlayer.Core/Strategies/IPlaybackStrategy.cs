using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Strategies;

public interface IPlaybackStrategy
{
    string Name { get; }

    Track? GetNextTrack(Playlist playlist, Track? currentTrack);

    Track? GetPreviousTrack(Playlist playlist, Track? currentTrack);

    void Reset(Playlist playlist);
}
