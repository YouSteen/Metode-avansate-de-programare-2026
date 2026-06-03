using MusicPlayer.Core.Audio;
using MusicPlayer.Core.Models;
using MusicPlayer.Core.Strategies;

namespace MusicPlayer.Core.Controllers;

public class PlaybackController
{
    private IPlaybackStrategy _strategy;

    public PlaybackController(IAudioPlayer player, Playlist playlist, IPlaybackStrategy strategy)
    {
        Player = player;
        Playlist = playlist;
        _strategy = strategy;
    }

    public IAudioPlayer Player { get; }

    public Playlist Playlist { get; }

    public IPlaybackStrategy Strategy => _strategy;

    public void SetStrategy(IPlaybackStrategy strategy) => _strategy = strategy;

    public void Next()
    {
        var next = _strategy.GetNextTrack(Playlist, Player.CurrentTrack);
        if (next is null)
        {
            Player.Stop();
            return;
        }

        Player.Load(next);
        Player.Play();
    }

    public void Previous()
    {
        var previous = _strategy.GetPreviousTrack(Playlist, Player.CurrentTrack);
        if (previous is null)
            return;

        Player.Load(previous);
        Player.Play();
    }
}
