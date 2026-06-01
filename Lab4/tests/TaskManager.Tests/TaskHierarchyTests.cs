using TaskManager.Core.Models;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class TaskHierarchyTests
{
    [TestCaseSource(nameof(AllTaskTypes))]
    public void Complete_SetsStatusDone(TaskItem task)
    {
        task.Complete();
        Assert.That(task.Status, Is.EqualTo(WorkStatus.Done));
    }

    [Test]
    public void Complete_WhenAlreadyDone_Throws()
    {
        var task = new TaskItem { Title = "Test", Status = WorkStatus.Done };
        Assert.Throws<InvalidOperationException>(() => task.Complete());
    }

    [Test]
    public void RecurringTask_Complete_AdvancesDueDate()
    {
        var start = DateTime.UtcNow.Date.AddDays(5);
        var task = new RecurringTask
        {
            Title = "Recurenta",
            DueDate = start,
            RecurrenceInterval = 3,
            Status = WorkStatus.Todo
        };

        task.Complete();

        Assert.That(task.Status, Is.EqualTo(WorkStatus.Done));
        Assert.That(task.DueDate, Is.EqualTo(start.AddDays(3)));
    }

    [Test]
    public void DeadlineTask_IsOverdue_WhenPastDueAndNotDone()
    {
        var task = new DeadlineTask
        {
            Title = "Deadline",
            DueDate = DateTime.UtcNow.AddDays(-1),
            Status = WorkStatus.Todo
        };

        Assert.That(task.IsOverdue, Is.True);
    }

    private static IEnumerable<TaskItem> AllTaskTypes()
    {
        yield return new TaskItem { Title = "Standard", Status = WorkStatus.Todo };
        yield return new RecurringTask
        {
            Title = "Recurenta",
            RecurrenceInterval = 7,
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = WorkStatus.Todo
        };
        yield return new DeadlineTask
        {
            Title = "Deadline",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = WorkStatus.Todo
        };
    }
}
