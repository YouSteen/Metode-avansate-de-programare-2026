using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Notifications;
using TaskManager.Core.Services;
using TaskManager.Data;

var dbPath = Path.Combine(AppContext.BaseDirectory, "tasks.db");
using var repository = new SqliteTaskRepository(dbPath);
var validator = new TaskValidator();

IReadOnlyDictionary<NotificationChannel, ITaskNotifier> notifiers =
    new Dictionary<NotificationChannel, ITaskNotifier>
    {
        [NotificationChannel.Email] = new EmailNotifier(),
        [NotificationChannel.Console] = new ConsoleNotifier(),
        [NotificationChannel.FileLog] = new FileLogNotifier(),
        [NotificationChannel.Slack] = new SlackNotifier()
    };

var service = new TaskService(repository, validator, notifiers);
var menu = new ConsoleMenu(service);
menu.Run();
