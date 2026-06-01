using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Services;
using TaskManager.Data;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class TaskServiceTests
{
    private InMemoryTaskRepository _repository = null!;
    private TaskValidator _validator = null!;
    private MockNotifier _mockNotifier = null!;
    private TaskService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new InMemoryTaskRepository();
        _validator = new TaskValidator();
        _mockNotifier = new MockNotifier();
        var notifiers = new Dictionary<NotificationChannel, ITaskNotifier>
        {
            [NotificationChannel.Console] = _mockNotifier
        };
        _service = new TaskService(_repository, _validator, notifiers);
    }

    [Test]
    public void Add_And_GetById_ReturnsTask()
    {
        var task = new TaskItem { Title = "Nou", NotificationType = NotificationChannel.Console };
        _service.Add(task);

        var loaded = _service.GetById(task.Id);
        Assert.That(loaded, Is.Not.Null);
        Assert.That(loaded!.Title, Is.EqualTo("Nou"));
    }

    [Test]
    public void Delete_RemovesTask()
    {
        var task = new TaskItem { Title = "Sterge", NotificationType = NotificationChannel.Console };
        _service.Add(task);
        _service.Delete(task.Id);
        Assert.That(_service.GetById(task.Id), Is.Null);
    }

    [Test]
    public void Complete_InvokesInjectedNotifier()
    {
        var task = new TaskItem
        {
            Title = "Finalizare",
            Status = WorkStatus.Todo,
            NotificationType = NotificationChannel.Console
        };
        _service.Add(task);
        _service.Complete(task.Id);

        Assert.That(_mockNotifier.NotifyCount, Is.EqualTo(1));
        Assert.That(_mockNotifier.LastTask?.Id, Is.EqualTo(task.Id));
    }

    [Test]
    public void Filter_ByStatus_ReturnsMatching()
    {
        _service.Add(new TaskItem { Title = "A", Status = WorkStatus.Todo, NotificationType = NotificationChannel.Console });
        _service.Add(new TaskItem { Title = "B", Status = WorkStatus.Done, NotificationType = NotificationChannel.Console });

        var todo = _service.Filter(WorkStatus.Todo, null);
        Assert.That(todo.Count, Is.EqualTo(1));
        Assert.That(todo[0].Title, Is.EqualTo("A"));
    }

    [Test]
    public void Update_ChangesTitle()
    {
        var task = new TaskItem { Title = "Vechi", NotificationType = NotificationChannel.Console };
        _service.Add(task);
        task.Title = "Nou";
        _service.Update(task);

        Assert.That(_service.GetById(task.Id)!.Title, Is.EqualTo("Nou"));
    }
}
