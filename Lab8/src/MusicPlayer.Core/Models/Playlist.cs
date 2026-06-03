using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MusicPlayer.Core.Models;

public class Playlist
{
    private readonly ObservableCollection<Track> _tracks = new();

    public Playlist()
    {
        Tracks = new ReadOnlyObservableCollection<Track>(_tracks);
    }

    public ReadOnlyObservableCollection<Track> Tracks { get; }

    public int Count => _tracks.Count;

    public void Add(Track track) => _tracks.Add(track);

    public void Insert(int index, Track track) => _tracks.Insert(index, track);

    public void Remove(Track track) => _tracks.Remove(track);

    public void RemoveAt(int index) => _tracks.RemoveAt(index);

    public void Move(int oldIndex, int newIndex) => _tracks.Move(oldIndex, newIndex);

    public void Clear() => _tracks.Clear();

    public int IndexOf(Track track) => _tracks.IndexOf(track);

    public Track GetAt(int index) => _tracks[index];

    public IReadOnlyList<Track> Snapshot() => _tracks.ToList();

    public void ReplaceAll(IEnumerable<Track> tracks)
    {
        _tracks.Clear();
        foreach (var track in tracks)
            _tracks.Add(track);
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged
    {
        add => _tracks.CollectionChanged += value;
        remove => _tracks.CollectionChanged -= value;
    }
}
