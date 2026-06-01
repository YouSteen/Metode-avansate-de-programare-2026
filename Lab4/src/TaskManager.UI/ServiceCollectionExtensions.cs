using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Notifications;
using TaskManager.Core.Services;
using TaskManager.Data;

namespace TaskManager.UI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTaskManagerServices(this IServiceCollection services, string databasePath)
    {
        services.AddSingleton<ITaskRepository>(_ => new SqliteTaskRepository(databasePath));
        services.AddSingleton<ITaskReader>(sp => sp.GetRequiredService<ITaskRepository>());
        services.AddSingleton<ITaskWriter>(sp => sp.GetRequiredService<ITaskRepository>());
        services.AddTransient<TaskValidator>();
        services.AddTransient<TaskService>();
        services.AddTransient<ReportService>();
        services.AddTaskManagerNotifiers();
        return services;
    }

    public static IServiceCollection AddTaskManagerNotifiers(this IServiceCollection services)
    {
        services.AddSingleton<IReadOnlyDictionary<NotificationChannel, ITaskNotifier>>(_ =>
            new Dictionary<NotificationChannel, ITaskNotifier>
            {
                [NotificationChannel.Email] = new EmailNotifier(),
                [NotificationChannel.Console] = new ConsoleNotifier(),
                [NotificationChannel.FileLog] = new FileLogNotifier(),
                [NotificationChannel.Slack] = new SlackNotifier()
            });
        return services;
    }
}
