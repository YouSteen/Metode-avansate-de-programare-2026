using System.ComponentModel;
using MusicPlayer.Core.Audio;
using MusicPlayer.Core.Models;

namespace MusicPlayer.Tests;

internal class FakeAudioPlayer : IAudioPlayer
{
    public Track? CurrentTrack { get; private set; }
    public PlayerState State { get; private set; } = PlayerState.Stopped;
    public TimeSpan Position { get; private set; }
    public TimeSpan Duration { get; private set; }
    public double Volume { get; set; } = 0.8;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? TrackEnded;

    public void Load(Track track)
    {
        CurrentTrack = track;
        Duration = track.Duration;
        Position = TimeSpan.Zero;
        State = PlayerState.Stopped;
    }

    public void Play() => State = PlayerState.Playing;

    public void Pause() => State = PlayerState.Paused;

    public void Stop() => State = PlayerState.Stopped;

    public void Seek(TimeSpan position) => Position = position;

    public void RaiseTrackEnded() => TrackEnded?.Invoke(this, EventArgs.Empty);

    public void Dispose()
    {
    }
}
