using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using MusicPlayer.Core.Audio;
using MusicPlayer.Core.Models;
using NAudio.Wave;

namespace MusicPlayer.App.Audio;

public class AudioPlayer : IAudioPlayer
{
    private WaveOutEvent? _waveOut;
    private AudioFileReader? _reader;
    private Track? _currentTrack;
    private PlayerState _state = PlayerState.Stopped;
    private double _volume = 0.8;
    private bool _manualStop;
    private readonly DispatcherTimer _positionTimer;

    public AudioPlayer()
    {
        _positionTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
        _positionTimer.Tick += (_, _) => OnPropertyChanged(nameof(Position));
    }

    public Track? CurrentTrack
    {
        get => _currentTrack;
        private set
        {
            if (_currentTrack == value)
                return;

            _currentTrack = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Duration));
        }
    }

    public PlayerState State
    {
        get => _state;
        private set
        {
            if (_state == value)
                return;

            _state = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan Position => _reader?.CurrentTime ?? TimeSpan.Zero;

    public TimeSpan Duration => _reader?.TotalTime ?? TimeSpan.Zero;

    public double Volume
    {
        get => _volume;
        set
        {
            _volume = Math.Clamp(value, 0.0, 1.0);
            if (_reader is not null)
                _reader.Volume = (float)_volume;

            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler? TrackEnded;

    public void Load(Track track)
    {
        StopInternal(manual: true);
        DisposeReader();

        _reader = new AudioFileReader(track.FilePath) { Volume = (float)_volume };
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_reader);
        _waveOut.PlaybackStopped += OnPlaybackStopped;

        CurrentTrack = track;
        State = PlayerState.Stopped;
        OnPropertyChanged(nameof(Position));
        OnPropertyChanged(nameof(Duration));
    }

    public void Play()
    {
        if (_waveOut is null || _reader is null)
            return;

        _manualStop = false;
        _waveOut.Play();
        State = PlayerState.Playing;
        _positionTimer.Start();
    }

    public void Pause()
    {
        if (_waveOut is null)
            return;

        _waveOut.Pause();
        State = PlayerState.Paused;
        _positionTimer.Stop();
    }

    public void Stop()
    {
        StopInternal(manual: true);
        State = PlayerState.Stopped;
    }

    public void Seek(TimeSpan position)
    {
        if (_reader is null)
            return;

        _reader.CurrentTime = position;
        OnPropertyChanged(nameof(Position));
    }

    public void Dispose()
    {
        _positionTimer.Stop();
        StopInternal(manual: true);
        DisposeReader();
    }

    private void StopInternal(bool manual)
    {
        _manualStop = manual;
        _positionTimer.Stop();

        if (_waveOut is not null)
        {
            _waveOut.PlaybackStopped -= OnPlaybackStopped;
            _waveOut.Stop();
            _waveOut.Dispose();
            _waveOut = null;
        }
    }

    private void DisposeReader()
    {
        _reader?.Dispose();
        _reader = null;
    }

    private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
    {
        _positionTimer.Stop();

        if (!_manualStop && _reader is not null && Position >= Duration - TimeSpan.FromMilliseconds(300))
            TrackEnded?.Invoke(this, EventArgs.Empty);

        State = PlayerState.Stopped;
        OnPropertyChanged(nameof(Position));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
