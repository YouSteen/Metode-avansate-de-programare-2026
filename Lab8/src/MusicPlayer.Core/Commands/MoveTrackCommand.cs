using MusicPlayer.Core.Models;

namespace MusicPlayer.Core.Commands;

public class MoveTrackCommand : IPlayerCommand
{
    private readonly Playlist _playlist;
    private readonly int _fromIndex;
    private readonly int _toIndex;

    public MoveTrackCommand(Playlist playlist, int fromIndex, int toIndex)
    {
        _playlist = playlist;
        _fromIndex = fromIndex;
        _toIndex = toIndex;
    }

    public bool CanUndo => true;

    public string Description => $"Move track {_fromIndex + 1} → position {_toIndex + 1}";

    public void Execute() => _playlist.Move(_fromIndex, _toIndex);

    public void Undo() => _playlist.Move(_toIndex, _fromIndex);
}
