using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Services;
using TaskManager.UI;

var dbPath = Path.Combine(AppContext.BaseDirectory, "tasks.db");

var services = new ServiceCollection();
services.AddTaskManagerServices(dbPath);

using var provider = services.BuildServiceProvider(new ServiceProviderOptions
{
    ValidateScopes = true,
    ValidateOnBuild = true
});

var menu = new ConsoleMenu(
    provider.GetRequiredService<TaskService>(),
    provider.GetRequiredService<ReportService>());

menu.Run();
