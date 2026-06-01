using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Tests;

public class MockNotifier : ITaskNotifier
{
    public int NotifyCount { get; private set; }
    public TaskItem? LastTask { get; private set; }

    public void Notify(TaskItem task)
    {
        NotifyCount++;
        LastTask = task;
    }
}
