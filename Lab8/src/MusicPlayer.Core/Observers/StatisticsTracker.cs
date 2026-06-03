using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Observers;

public record StatisticsSnapshot(
    string TotalPlayedFormatted,
    string TopArtist,
    int Skips,
    IReadOnlyList<string> TopTracks);

public class StatisticsTracker
{
    private readonly Dictionary<string, double> _minutesByArtist = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _playCounts = new(StringComparer.OrdinalIgnoreCase);
    private Track? _currentTrack;
    private DateTime? _trackStartedAt;
    private int _skips;

    public StatisticsSnapshot Snapshot
    {
        get
        {
            var totalMinutes = _minutesByArtist.Values.Sum();
            var topArtist = _minutesByArtist
                .OrderByDescending(p => p.Value)
                .Select(p => p.Key)
                .FirstOrDefault() ?? "-";

            var topTracks = _playCounts
                .OrderByDescending(p => p.Value)
                .Take(5)
                .Select(p => p.Key)
                .ToList();

            return new StatisticsSnapshot(
                StatisticsFormatting.FormatMinutes(totalMinutes),
                topArtist,
                _skips,
                topTracks);
        }
    }

    public void OnTrackStarted(Track track)
    {
        _currentTrack = track;
        _trackStartedAt = DateTime.UtcNow;
        _playCounts.TryGetValue(track.Title, out var count);
        _playCounts[track.Title] = count + 1;
    }

    public void OnTrackEnded(TimeSpan listenedDuration)
    {
        if (_currentTrack is null)
            return;

        var minutes = listenedDuration.TotalMinutes;
        _minutesByArtist.TryGetValue(_currentTrack.Artist, out var existing);
        _minutesByArtist[_currentTrack.Artist] = existing + minutes;
        _currentTrack = null;
        _trackStartedAt = null;
    }

    public void OnTrackSkipped(TimeSpan listenedDuration)
    {
        if (listenedDuration < TimeSpan.FromSeconds(30))
            _skips++;

        OnTrackEnded(listenedDuration);
    }
}
