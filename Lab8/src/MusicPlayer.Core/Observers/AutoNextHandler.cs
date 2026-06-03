using MusicPlayer.Core.Audio;
using MusicPlayer.Core.Controllers;
using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Observers;

public class AutoNextHandler : IDisposable
{
    private readonly PlaybackController _controller;
    private readonly IAudioPlayer _player;

    public AutoNextHandler(PlaybackController controller)
    {
        _controller = controller;
        _player = controller.Player;
        _player.TrackEnded += OnTrackEnded;
    }

    private void OnTrackEnded(object? sender, EventArgs e) => _controller.Next();

    public void Dispose() => _player.TrackEnded -= OnTrackEnded;
}
