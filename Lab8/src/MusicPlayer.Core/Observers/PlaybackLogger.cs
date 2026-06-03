using MusicPlayer.Core.Audio;
using MusicPlayer.Core.Controllers;
using MusicPlayer.Core.Models;
using MusicPlayer.Core.Strategies;

namespace MusicPlayer.Core.Observers;

public class PlaybackLogger : IDisposable
{
    private readonly IAudioPlayer _player;
    private readonly PlaybackController _controller;
    private readonly string _logPath;
    private readonly StreamWriter _writer;

    public PlaybackLogger(IAudioPlayer player, PlaybackController controller, string logPath)
    {
        _player = player;
        _controller = controller;
        _logPath = logPath;
        _writer = new StreamWriter(logPath, append: true) { AutoFlush = true };

        _player.PropertyChanged += OnPlayerPropertyChanged;
        _player.TrackEnded += OnTrackEnded;
    }

    public void LogStrategyChanged(IPlaybackStrategy strategy) =>
        Write($"StrategyChanged: {strategy.Name}");

    private void OnPlayerPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IAudioPlayer.CurrentTrack) && _player.CurrentTrack is Track track)
            Write($"TrackStarted: {track.Title} — {track.Artist}");
    }

    private void OnTrackEnded(object? sender, EventArgs e) =>
        Write("TrackEnded: natural end");

    private void Write(string message) =>
        _writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}");

    public void Dispose()
    {
        _player.PropertyChanged -= OnPlayerPropertyChanged;
        _player.TrackEnded -= OnTrackEnded;
        _writer.Dispose();
    }
}
