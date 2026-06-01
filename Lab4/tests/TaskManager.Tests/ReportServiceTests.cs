using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Services;
using TaskManager.Data;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class ReportServiceTests
{
    [Test]
    public void Constructor_AcceptsInMemoryRepositoryAsTaskReader()
    {
        ITaskReader reader = new InMemoryTaskRepository();
        var report = new ReportService(reader);
        Assert.That(report.GenerateSummary(), Does.Contain("Total sarcini: 0"));
    }

    [Test]
    public void GenerateSummary_ReturnsCorrectCounts()
    {
        var repo = new InMemoryTaskRepository();
        ITaskReader reader = repo;
        repo.Add(new TaskItem { Title = "A", Status = WorkStatus.Todo, NotificationType = NotificationChannel.Console });
        repo.Add(new TaskItem { Title = "B", Status = WorkStatus.Done, NotificationType = NotificationChannel.Console });
        repo.Add(new TaskItem { Title = "C", Status = WorkStatus.Done, NotificationType = NotificationChannel.Console });

        var report = new ReportService(reader);
        var summary = report.GenerateSummary();

        Assert.That(summary, Is.EqualTo("Total sarcini: 3, finalizate (Done): 2"));
    }
}
