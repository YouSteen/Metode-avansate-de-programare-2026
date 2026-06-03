using MusicPlayer.Core.Audio;

namespace MusicPlayer.Core.Commands;

public class PauseCommand : IPlayerCommand
{
    private readonly IAudioPlayer _player;

    public PauseCommand(IAudioPlayer player) => _player = player;

    public bool CanUndo => false;

    public string Description => "Pause";

    public void Execute() => _player.Pause();

    public void Undo() { }
}
