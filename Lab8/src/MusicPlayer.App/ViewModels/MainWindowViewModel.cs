using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MusicPlayer.App.Audio;
using MusicPlayer.Core.Audio;
using MusicPlayer.Core.Commands;
using MusicPlayer.Core.Controllers;
using MusicPlayer.Core.Models;
using MusicPlayer.Core.Observers;
using MusicPlayer.Core.Strategies;

namespace MusicPlayer.App.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly AudioPlayer _player;
    private readonly Playlist _playlist;
    private readonly PlaybackController _controller;
    private readonly CommandHistory _history;
    private readonly StatisticsTracker _statistics = new();
    private readonly PlaybackLogger _logger;
    private readonly AutoNextHandler _autoNext;
    private readonly Dictionary<string, IPlaybackStrategy> _strategies;

    private Track? _selectedTrack;
    private string _selectedStrategyName = "Smart Shuffle";
    private double _positionSeconds;
    private double _volumePercent = 80;
    private string _totalPlayed = "0m";
    private string _topArtist = "-";
    private int _skips;
    private bool _isUpdatingPosition;

    public MainWindowViewModel()
    {
        _player = new AudioPlayer();
        _playlist = new Playlist();
        _strategies = new Dictionary<string, IPlaybackStrategy>(StringComparer.OrdinalIgnoreCase)
        {
            ["Sequential"] = new SequentialStrategy(),
            ["Shuffle"] = new ShuffleStrategy(),
            ["Smart Shuffle"] = new SmartShuffleStrategy(),
            ["Repeat One"] = new RepeatOneStrategy()
        };

        _controller = new PlaybackController(_player, _playlist, _strategies[_selectedStrategyName]);
        _history = new CommandHistory();

        var logPath = Path.Combine(AppContext.BaseDirectory, "playback_log.txt");
        _logger = new PlaybackLogger(_player, _controller, logPath);
        _autoNext = new AutoNextHandler(_controller);

        Tracks = _playlist.Tracks;
        HistoryItems = new ObservableCollection<string>();
        StrategyOptions = new ObservableCollection<string>(_strategies.Keys);

        _player.PropertyChanged += OnPlayerPropertyChanged;
        _playlist.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(TrackCountLabel));
            RefreshCommands();
        };
        _history.HistoryChanged += RefreshHistory;

        PlayPauseCommand = new RelayCommand(_ => TogglePlayPause(), _ => _playlist.Count > 0 && _player.CurrentTrack is not null);
        NextCommand = new RelayCommand(_ => _history.Execute(new NextCommand(_controller)), _ => CanNavigate);
        PreviousCommand = new RelayCommand(_ => _history.Execute(new PreviousCommand(_controller)), _ => CanNavigate);
        UndoCommand = new RelayCommand(_ => _history.Undo(), _ => _history.CanUndo);
        RedoCommand = new RelayCommand(_ => _history.Redo(), _ => _history.CanRedo);
        AddFilesCommand = new RelayCommand(_ => AddFiles());
        UndoHistoryItemCommand = new RelayCommand(_ => _history.Undo(), _ => _history.CanUndo);

        LoadSampleTracksIfPresent();
        RefreshAll();
    }

    public ReadOnlyObservableCollection<Track> Tracks { get; }

    public ObservableCollection<string> HistoryItems { get; }

    public ObservableCollection<string> StrategyOptions { get; }

    public Track? SelectedTrack
    {
        get => _selectedTrack;
        set
        {
            if (_selectedTrack == value)
                return;

            _selectedTrack = value;
            OnPropertyChanged();
            if (value is not null)
            {
                _player.Load(value);
                _statistics.OnTrackStarted(value);
                RefreshCommands();
            }
        }
    }

    public string SelectedStrategyName
    {
        get => _selectedStrategyName;
        set
        {
            if (_selectedStrategyName == value || string.IsNullOrWhiteSpace(value))
                return;

            if (!_strategies.TryGetValue(value, out var strategy))
                return;

            var command = new ChangeStrategyCommand(_controller, strategy);
            _history.Execute(command);
            _selectedStrategyName = value;
            _logger.LogStrategyChanged(strategy);
            OnPropertyChanged();
            RefreshStatistics();
        }
    }

    public string TrackCountLabel => $"LIBRARY · {_playlist.Count} tracks";

    public string CurrentTitle => _player.CurrentTrack?.Title ?? "No track selected";

    public string CurrentArtist => _player.CurrentTrack?.Artist ?? string.Empty;

    public string CurrentAlbum => _player.CurrentTrack is null
        ? string.Empty
        : string.IsNullOrWhiteSpace(_player.CurrentTrack.Album)
            ? string.Empty
            : _player.CurrentTrack.Album;

    public string CurrentPrefix => _player.State switch
    {
        PlayerState.Playing => "▶",
        PlayerState.Paused => "⏸",
        _ => string.Empty
    };

    public string PositionLabel => FormatTime(TimeSpan.FromSeconds(PositionSeconds));

    public string DurationLabel => FormatTime(_player.Duration);

    public double DurationSeconds => Math.Max(_player.Duration.TotalSeconds, 0);

    public Guid? CurrentTrackId => _player.CurrentTrack?.Id;

    public double PositionSeconds
    {
        get => _positionSeconds;
        set
        {
            if (Math.Abs(_positionSeconds - value) < 0.01)
                return;

            _positionSeconds = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PositionLabel));

            if (!_isUpdatingPosition && _player.Duration > TimeSpan.Zero)
                _player.Seek(TimeSpan.FromSeconds(value));
        }
    }

    public double VolumePercent
    {
        get => _volumePercent;
        set
        {
            _volumePercent = value;
            _player.Volume = value / 100.0;
            OnPropertyChanged();
        }
    }

    public string TotalPlayed
    {
        get => _totalPlayed;
        private set { _totalPlayed = value; OnPropertyChanged(); }
    }

    public string TopArtist
    {
        get => _topArtist;
        private set { _topArtist = value; OnPropertyChanged(); }
    }

    public int Skips
    {
        get => _skips;
        private set { _skips = value; OnPropertyChanged(); }
    }

    public bool CanUndo => _history.CanUndo;

    public bool CanRedo => _history.CanRedo;

    public ICommand PlayPauseCommand { get; }

    public ICommand NextCommand { get; }

    public ICommand PreviousCommand { get; }

    public ICommand UndoCommand { get; }

    public ICommand RedoCommand { get; }

    public ICommand AddFilesCommand { get; }

    public ICommand UndoHistoryItemCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Dispose()
    {
        _player.PropertyChanged -= OnPlayerPropertyChanged;
        _autoNext.Dispose();
        _logger.Dispose();
        _player.Dispose();
    }

    private bool CanNavigate => _playlist.Count > 0;

    private void TogglePlayPause()
    {
        if (_player.State == PlayerState.Playing)
            _history.Execute(new PauseCommand(_player));
        else
            _history.Execute(new PlayCommand(_player));
    }

    private void AddFiles()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav",
            Multiselect = true
        };

        if (dialog.ShowDialog() != true)
            return;

        foreach (var path in dialog.FileNames)
        {
            var track = Mp3MetadataReader.ReadFromFile(path);
            _history.Execute(new AddTrackCommand(_playlist, track));
        }

        if (_player.CurrentTrack is null && _playlist.Count > 0)
            SelectedTrack = _playlist.GetAt(0);
    }

    private void LoadSampleTracksIfPresent()
    {
        var samplesDir = Path.Combine(AppContext.BaseDirectory, "samples");
        if (!Directory.Exists(samplesDir))
            return;

        foreach (var path in Directory.GetFiles(samplesDir, "*.*")
                     .Where(f => f.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase)
                              || f.EndsWith(".wav", StringComparison.OrdinalIgnoreCase)))
        {
            var track = Mp3MetadataReader.ReadFromFile(path);
            _history.Execute(new AddTrackCommand(_playlist, track));
        }

        if (_playlist.Count > 0)
            SelectedTrack = _playlist.GetAt(0);
    }

    private void OnPlayerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IAudioPlayer.Position):
                _isUpdatingPosition = true;
                PositionSeconds = _player.Position.TotalSeconds;
                _isUpdatingPosition = false;
                break;
            case nameof(IAudioPlayer.State):
            case nameof(IAudioPlayer.CurrentTrack):
                OnPropertyChanged(nameof(CurrentTitle));
                OnPropertyChanged(nameof(CurrentArtist));
                OnPropertyChanged(nameof(CurrentAlbum));
                OnPropertyChanged(nameof(CurrentPrefix));
                OnPropertyChanged(nameof(CurrentTrackId));
                OnPropertyChanged(nameof(DurationLabel));
                OnPropertyChanged(nameof(DurationSeconds));
                RefreshCommands();
                break;
        }
    }

    private void RefreshHistory()
    {
        HistoryItems.Clear();
        foreach (var item in _history.GetRecentDescriptions(10))
            HistoryItems.Add(item);

        _selectedStrategyName = _controller.Strategy.Name;
        OnPropertyChanged(nameof(SelectedStrategyName));
        OnPropertyChanged(nameof(CanUndo));
        OnPropertyChanged(nameof(CanRedo));
        RefreshCommands();
    }

    private void RefreshStatistics()
    {
        var snapshot = _statistics.Snapshot;
        TotalPlayed = snapshot.TotalPlayedFormatted;
        TopArtist = snapshot.TopArtist;
        Skips = snapshot.Skips;
    }

    private void RefreshCommands()
    {
        (PlayPauseCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (NextCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (PreviousCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (UndoHistoryItemCommand as RelayCommand)?.RaiseCanExecuteChanged();
        OnPropertyChanged(nameof(TrackCountLabel));
    }

    private void RefreshAll()
    {
        RefreshHistory();
        RefreshStatistics();
        RefreshCommands();
    }

    private static string FormatTime(TimeSpan time) =>
        $"{(int)time.TotalMinutes:D2}:{time.Seconds:D2}";

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
