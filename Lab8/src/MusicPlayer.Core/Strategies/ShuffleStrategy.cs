using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Strategies;

public class ShuffleStrategy : IPlaybackStrategy
{
    private readonly Random _random = new();
    private readonly List<Track> _order = new();
    private int _index = -1;

    public string Name => "Shuffle";

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        EnsureOrder(playlist);
        if (_order.Count == 0)
            return null;

        if (currentTrack is null)
        {
            _index = 0;
            return _order[0];
        }

        var currentIndex = _order.FindIndex(t => t.Id == currentTrack.Id);
        if (currentIndex < 0)
        {
            _index = 0;
            return _order[0];
        }

        if (currentIndex < _order.Count - 1)
        {
            _index = currentIndex + 1;
            return _order[_index];
        }

        Reset(playlist);
        _index = 0;
        return _order[0];
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        EnsureOrder(playlist);
        if (_order.Count == 0 || currentTrack is null)
            return null;

        var currentIndex = _order.FindIndex(t => t.Id == currentTrack.Id);
        if (currentIndex <= 0)
            return null;

        _index = currentIndex - 1;
        return _order[_index];
    }

    public void Reset(Playlist playlist)
    {
        _order.Clear();
        _order.AddRange(playlist.Snapshot());
        Shuffle(_order);
        _index = -1;
    }

    private void EnsureOrder(Playlist playlist)
    {
        if (_order.Count != playlist.Count)
            Reset(playlist);
    }

    private void Shuffle(List<Track> items)
    {
        for (var i = items.Count - 1; i > 0; i--)
        {
            var j = _random.Next(i + 1);
            (items[i], items[j]) = (items[j], items[i]);
        }
    }
}
