using MusicPlayer.Core.Controllers;

namespace MusicPlayer.Core.Commands;

public class NextCommand : IPlayerCommand
{
    private readonly PlaybackController _controller;

    public NextCommand(PlaybackController controller) => _controller = controller;

    public bool CanUndo => false;

    public string Description => "Next";

    public void Execute() => _controller.Next();

    public void Undo() { }
}
