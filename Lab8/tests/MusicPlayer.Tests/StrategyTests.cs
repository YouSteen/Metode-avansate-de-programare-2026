using MusicPlayer.Core.Models;
using MusicPlayer.Core.Strategies;
using NUnit.Framework;

namespace MusicPlayer.Tests;

[TestFixture]
public class StrategyTests
{
    private static Track Track(int index) =>
        new(Guid.NewGuid(), $"Title {index}", $"Artist {index}", "Album", TimeSpan.FromMinutes(3), $"t{index}.mp3");

    [Test]
    public void SequentialStrategy_ReturnsNextInOrder()
    {
        var playlist = new Playlist();
        var t1 = Track(1);
        var t2 = Track(2);
        playlist.Add(t1);
        playlist.Add(t2);

        var strategy = new SequentialStrategy();
        Assert.That(strategy.GetNextTrack(playlist, t1), Is.EqualTo(t2));
        Assert.That(strategy.GetNextTrack(playlist, t2), Is.Null);
    }

    [Test]
    public void SequentialStrategy_RepeatAll_WrapsToFirst()
    {
        var playlist = new Playlist();
        var t1 = Track(1);
        var t2 = Track(2);
        playlist.Add(t1);
        playlist.Add(t2);

        var strategy = new SequentialStrategy { RepeatAll = true };
        Assert.That(strategy.GetNextTrack(playlist, t2), Is.EqualTo(t1));
    }

    [Test]
    public void ShuffleStrategy_UsesAllTracksBeforeReset()
    {
        var playlist = new Playlist();
        for (var i = 0; i < 4; i++)
            playlist.Add(Track(i));

        var strategy = new ShuffleStrategy();
        strategy.Reset(playlist);
        var first = strategy.GetNextTrack(playlist, null);
        var seen = new HashSet<Guid> { first!.Id };

        var current = first;
        for (var i = 0; i < 3; i++)
        {
            current = strategy.GetNextTrack(playlist, current)!;
            seen.Add(current.Id);
        }

        Assert.That(seen.Count, Is.EqualTo(4));
    }

    [Test]
    public void RepeatOneStrategy_AlwaysReturnsCurrent()
    {
        var playlist = new Playlist();
        var track = Track(1);
        playlist.Add(track);

        var strategy = new RepeatOneStrategy();
        Assert.That(strategy.GetNextTrack(playlist, track), Is.EqualTo(track));
        Assert.That(strategy.GetPreviousTrack(playlist, track), Is.EqualTo(track));
    }
}
