using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Strategies;

public class SmartShuffleStrategy : IPlaybackStrategy
{
    private const int DefaultWindow = 5;
    private readonly Random _random = new();
    private readonly Queue<Guid> _recentIds = new();
    private readonly List<Track> _playOrder = new();
    private int _playIndex = -1;

    public string Name => "Smart Shuffle";

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        var tracks = playlist.Snapshot();
        if (tracks.Count == 0)
            return null;

        if (currentTrack is null)
        {
            var first = PickRandom(tracks);
            Remember(first);
            _playOrder.Clear();
            _playOrder.Add(first);
            _playIndex = 0;
            return first;
        }

        if (_playIndex >= 0 && _playIndex < _playOrder.Count - 1)
        {
            _playIndex++;
            Remember(_playOrder[_playIndex]);
            return _playOrder[_playIndex];
        }

        var next = PickRandom(tracks, currentTrack);
        Remember(next);
        _playOrder.Add(next);
        _playIndex = _playOrder.Count - 1;
        return next;
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        if (_playIndex <= 0 || currentTrack is null)
            return null;

        _playIndex--;
        return _playOrder[_playIndex];
    }

    public void Reset(Playlist playlist)
    {
        _recentIds.Clear();
        _playOrder.Clear();
        _playIndex = -1;
    }

    private Track PickRandom(IReadOnlyList<Track> tracks, Track? exclude = null)
    {
        var window = Math.Min(DefaultWindow, Math.Max(1, tracks.Count - 1));
        var recent = _recentIds.Take(window).ToHashSet();
        var candidates = tracks.Where(t => !recent.Contains(t.Id) && (exclude is null || t.Id != exclude.Id)).ToList();
        if (candidates.Count == 0)
            candidates = tracks.Where(t => exclude is null || t.Id != exclude.Id).ToList();
        if (candidates.Count == 0)
            candidates = tracks.ToList();

        return candidates[_random.Next(candidates.Count)];
    }

    private void Remember(Track track)
    {
        _recentIds.Enqueue(track.Id);
        var window = Math.Min(DefaultWindow, Math.Max(1, _recentIds.Count));
        while (_recentIds.Count > window)
            _recentIds.Dequeue();
    }
}
