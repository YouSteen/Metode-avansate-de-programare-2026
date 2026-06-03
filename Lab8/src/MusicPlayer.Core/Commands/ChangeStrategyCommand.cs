using MusicPlayer.Core.Controllers;
using MusicPlayer.Core.Strategies;

namespace MusicPlayer.Core.Commands;

public class ChangeStrategyCommand : IPlayerCommand
{
    private readonly PlaybackController _controller;
    private readonly IPlaybackStrategy _newStrategy;
    private IPlaybackStrategy? _previousStrategy;

    public ChangeStrategyCommand(PlaybackController controller, IPlaybackStrategy newStrategy)
    {
        _controller = controller;
        _newStrategy = newStrategy;
    }

    public bool CanUndo => true;

    public string Description =>
        _previousStrategy is null
            ? $"Strategy → {_newStrategy.Name}"
            : $"{_previousStrategy.Name} → {_newStrategy.Name}";

    public void Execute()
    {
        _previousStrategy = _controller.Strategy;
        _controller.SetStrategy(_newStrategy);
        _newStrategy.Reset(_controller.Playlist);
    }

    public void Undo()
    {
        if (_previousStrategy is null)
            return;

        _controller.SetStrategy(_previousStrategy);
    }
}
