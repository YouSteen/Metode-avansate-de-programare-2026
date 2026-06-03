using MusicPlayer.Core.Commands;
using MusicPlayer.Core.Models;
using NUnit.Framework;

namespace MusicPlayer.Tests;

[TestFixture]
public class PlaylistCommandTests
{
    [Test]
    public void RemoveTrackCommand_Undo_ReinsertsAtOriginalIndex()
    {
        var playlist = new Playlist();
        var t1 = new Track(Guid.NewGuid(), "1", "A", "", TimeSpan.FromMinutes(1), "1.mp3");
        var t2 = new Track(Guid.NewGuid(), "2", "B", "", TimeSpan.FromMinutes(1), "2.mp3");
        var t3 = new Track(Guid.NewGuid(), "3", "C", "", TimeSpan.FromMinutes(1), "3.mp3");
        playlist.Add(t1);
        playlist.Add(t2);
        playlist.Add(t3);

        var command = new RemoveTrackCommand(playlist, t2);
        command.Execute();
        Assert.That(playlist.Count, Is.EqualTo(2));

        command.Undo();
        Assert.That(playlist.Count, Is.EqualTo(3));
        Assert.That(playlist.GetAt(1), Is.EqualTo(t2));
    }

    [Test]
    public void ClearPlaylistCommand_Undo_RestoresAllTracks()
    {
        var playlist = new Playlist();
        playlist.Add(new Track(Guid.NewGuid(), "1", "A", "", TimeSpan.FromMinutes(1), "1.mp3"));
        playlist.Add(new Track(Guid.NewGuid(), "2", "B", "", TimeSpan.FromMinutes(1), "2.mp3"));

        var command = new ClearPlaylistCommand(playlist);
        command.Execute();
        Assert.That(playlist.Count, Is.EqualTo(0));

        command.Undo();
        Assert.That(playlist.Count, Is.EqualTo(2));
    }
}
