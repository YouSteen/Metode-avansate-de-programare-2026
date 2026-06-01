using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Notifications;

public class ConsoleNotifier : ITaskNotifier
{
    public void Notify(TaskItem task) =>
        Console.WriteLine($"[Notificare] Sarcina finalizata: {task.Title} (Id {task.Id}, prioritate {task.Priority}).");
}
