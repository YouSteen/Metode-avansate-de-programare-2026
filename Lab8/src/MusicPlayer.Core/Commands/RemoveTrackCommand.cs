using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Commands;

public class RemoveTrackCommand : IPlayerCommand
{
    private readonly Playlist _playlist;
    private readonly Track _track;
    private readonly int _originalIndex;

    public RemoveTrackCommand(Playlist playlist, Track track)
    {
        _playlist = playlist;
        _track = track;
        _originalIndex = playlist.IndexOf(track);
    }

    public bool CanUndo => true;

    public string Description => $"Remove \"{_track.Title}\"";

    public void Execute() => _playlist.Remove(_track);

    public void Undo()
    {
        var index = Math.Clamp(_originalIndex, 0, _playlist.Count);
        _playlist.Insert(index, _track);
    }
}
