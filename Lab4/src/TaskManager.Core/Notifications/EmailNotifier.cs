using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Notifications;

public class EmailNotifier : ITaskNotifier
{
    public void Notify(TaskItem task) =>
        Console.WriteLine($"Email sent to echipa@firma.local: sarcina '{task.Title}' (Id {task.Id}) finalizata.");
}
