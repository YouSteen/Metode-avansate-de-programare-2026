using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Services;
using TaskManager.Data;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class DipAndIspTests
{
    [Test]
    public void TaskService_Constructor_RequiresTaskRepository()
    {
        var validator = new TaskValidator();
        var notifiers = new Dictionary<NotificationChannel, ITaskNotifier>
        {
            [NotificationChannel.Console] = new MockNotifier()
        };

        Assert.Throws<ArgumentNullException>(() =>
            new TaskService(null!, validator, notifiers));
    }

    [TestCase(NotificationChannel.Email)]
    [TestCase(NotificationChannel.Console)]
    [TestCase(NotificationChannel.FileLog)]
    public void Complete_InvokesNotifier_ForEachRegisteredChannel(NotificationChannel channel)
    {
        var repository = new InMemoryTaskRepository();
        var validator = new TaskValidator();
        var mock = new MockNotifier();
        var notifiers = new Dictionary<NotificationChannel, ITaskNotifier> { [channel] = mock };
        var service = new TaskService(repository, validator, notifiers);

        var task = new TaskItem
        {
            Title = "Notificare",
            Status = WorkStatus.Todo,
            NotificationType = channel
        };
        service.Add(task);
        service.Complete(task.Id);

        Assert.That(mock.NotifyCount, Is.EqualTo(1));
    }
}
