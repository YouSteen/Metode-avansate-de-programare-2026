using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.UI;

public class SlackNotifier : ITaskNotifier
{
    public void Notify(TaskItem task) =>
        Console.WriteLine($"[Slack] Canal #echipa: sarcina '{task.Title}' (Id {task.Id}) marcata Done.");
}
