using MusicPlayer.Core.Commands;
using MusicPlayer.Core.Models;
using NUnit.Framework;

namespace MusicPlayer.Tests;

[TestFixture]
public class CommandHistoryTests
{
    [Test]
    public void Execute_UndoableCommand_PushesToUndoStack()
    {
        var playlist = new Playlist();
        var track = new Track(Guid.NewGuid(), "A", "B", "C", TimeSpan.FromMinutes(1), "a.mp3");
        var history = new CommandHistory();

        history.Execute(new AddTrackCommand(playlist, track));

        Assert.That(playlist.Count, Is.EqualTo(1));
        Assert.That(history.CanUndo, Is.True);
        Assert.That(history.CanRedo, Is.False);
    }

    [Test]
    public void Undo_RestoresPreviousState()
    {
        var playlist = new Playlist();
        var track = new Track(Guid.NewGuid(), "A", "B", "C", TimeSpan.FromMinutes(1), "a.mp3");
        var history = new CommandHistory();
        history.Execute(new AddTrackCommand(playlist, track));

        history.Undo();

        Assert.That(playlist.Count, Is.EqualTo(0));
        Assert.That(history.CanRedo, Is.True);
    }

    [Test]
    public void NewExecute_ClearsRedoStack()
    {
        var playlist = new Playlist();
        var t1 = new Track(Guid.NewGuid(), "1", "A", "", TimeSpan.FromMinutes(1), "1.mp3");
        var t2 = new Track(Guid.NewGuid(), "2", "A", "", TimeSpan.FromMinutes(1), "2.mp3");
        var history = new CommandHistory();

        history.Execute(new AddTrackCommand(playlist, t1));
        history.Undo();
        history.Execute(new AddTrackCommand(playlist, t2));

        Assert.That(history.CanRedo, Is.False);
        Assert.That(playlist.Count, Is.EqualTo(1));
        Assert.That(playlist.GetAt(0), Is.EqualTo(t2));
    }
}
