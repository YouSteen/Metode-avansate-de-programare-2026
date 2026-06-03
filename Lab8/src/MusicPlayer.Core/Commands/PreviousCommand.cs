using MusicPlayer.Core.Controllers;

namespace MusicPlayer.Core.Commands;

public class PreviousCommand : IPlayerCommand
{
    private readonly PlaybackController _controller;

    public PreviousCommand(PlaybackController controller) => _controller = controller;

    public bool CanUndo => false;

    public string Description => "Previous";

    public void Execute() => _controller.Previous();

    public void Undo() { }
}
