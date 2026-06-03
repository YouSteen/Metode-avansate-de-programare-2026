using MusicPlayer.Core.Audio;

namespace MusicPlayer.Core.Commands;

public class PlayCommand : IPlayerCommand
{
    private readonly IAudioPlayer _player;

    public PlayCommand(IAudioPlayer player) => _player = player;

    public bool CanUndo => false;

    public string Description => "Play";

    public void Execute() => _player.Play();

    public void Undo() { }
}
