namespace MusicPlayer.Core.Commands;

public interface IPlayerCommand
{
    bool CanUndo { get; }

    string Description { get; }

    void Execute();

    void Undo();
}
