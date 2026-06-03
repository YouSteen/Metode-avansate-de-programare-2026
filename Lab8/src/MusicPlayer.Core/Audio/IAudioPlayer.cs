using System.ComponentModel;
using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Audio;

public interface IAudioPlayer : INotifyPropertyChanged, IDisposable
{
    Track? CurrentTrack { get; }
    PlayerState State { get; }
    TimeSpan Position { get; }
    TimeSpan Duration { get; }
    double Volume { get; set; }

    event EventHandler? TrackEnded;

    void Load(Track track);
    void Play();
    void Pause();
    void Stop();
    void Seek(TimeSpan position);
}
