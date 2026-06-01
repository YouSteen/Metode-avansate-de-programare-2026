using Spectre.Console;
using TaskManager.Core.Models;
using TaskManager.Core.Services;

namespace TaskManager.UI;

public class ConsoleMenu
{
    private readonly TaskService _service;

    public ConsoleMenu(TaskService service) => _service = service;

    public void Run()
    {
        AnsiConsole.Write(new FigletText("Task Manager").Color(Color.Green));
        var running = true;
        while (running)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Meniu principal[/]")
                    .AddChoices(
                        "Lista sarcini",
                        "Filtrare sarcini",
                        "Adauga sarcina",
                        "Editeaza sarcina",
                        "Sterge sarcina",
                        "Finalizeaza sarcina",
                        "Iesire"));

            try
            {
                running = choice switch
                {
                    "Lista sarcini" => ListTasks(null, null),
                    "Filtrare sarcini" => FilterTasks(),
                    "Adauga sarcina" => AddTask(),
                    "Editeaza sarcina" => EditTask(),
                    "Sterge sarcina" => DeleteTask(),
                    "Finalizeaza sarcina" => CompleteTask(),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Eroare:[/] {Markup.Escape(ex.Message)}");
            }

            if (running)
                AnsiConsole.WriteLine();
        }
    }

    private bool ListTasks(WorkStatus? status, int? priority)
    {
        var tasks = status.HasValue || priority.HasValue
            ? _service.Filter(status, priority)
            : _service.GetAll();

        if (tasks.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]Nu exista sarcini.[/]");
            return true;
        }

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("Id");
        table.AddColumn("Titlu");
        table.AddColumn("Tip");
        table.AddColumn("Status");
        table.AddColumn("Prioritate");
        table.AddColumn("Notificare");
        table.AddColumn("DueDate");

        foreach (var t in tasks)
        {
            table.AddRow(
                t.Id.ToString(),
                Markup.Escape(t.Title),
                t.Kind.ToString(),
                t.Status.ToString(),
                t.Priority.ToString(),
                t.NotificationType.ToString(),
                t.DueDate?.ToString("yyyy-MM-dd") ?? "-");
        }

        AnsiConsole.Write(table);
        return true;
    }

    private bool FilterTasks()
    {
        var statusChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Status")
                .AddChoices("Toate", "Todo", "InProgress", "Done"));

        WorkStatus? status = statusChoice switch
        {
            "Todo" => WorkStatus.Todo,
            "InProgress" => WorkStatus.InProgress,
            "Done" => WorkStatus.Done,
            _ => null
        };

        var priorityChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Prioritate")
                .AddChoices("Toate", "1", "2", "3"));

        int? priority = priorityChoice == "Toate" ? null : int.Parse(priorityChoice);
        return ListTasks(status, priority);
    }

    private bool AddTask()
    {
        var kind = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Tip sarcina")
                .AddChoices("Standard", "Recurring", "Deadline"));

        var title = AnsiConsole.Ask<string>("Titlu:");
        var description = AnsiConsole.Ask<string>("Descriere (gol pentru skip):", string.Empty);
        var priority = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("Prioritate")
                .AddChoices(1, 2, 3));
        var status = AnsiConsole.Prompt(
            new SelectionPrompt<WorkStatus>()
                .Title("Status initial")
                .AddChoices(WorkStatus.Todo, WorkStatus.InProgress));
        var notification = AnsiConsole.Prompt(
            new SelectionPrompt<NotificationChannel>()
                .Title("Tip notificare la finalizare")
                .AddChoices(
                    NotificationChannel.Email,
                    NotificationChannel.Console,
                    NotificationChannel.FileLog,
                    NotificationChannel.Slack));

        TaskItem task = kind switch
        {
            "Recurring" => new RecurringTask(),
            "Deadline" => new DeadlineTask(),
            _ => new TaskItem()
        };

        task.Title = title;
        task.Description = string.IsNullOrWhiteSpace(description) ? null : description;
        task.Priority = priority;
        task.Status = status;
        task.NotificationType = notification;

        if (task is RecurringTask)
        {
            task.RecurrenceInterval = AnsiConsole.Ask<int>("Interval recurenta (zile):", 7);
            var due = AnsiConsole.Ask<string>("DueDate initial (yyyy-MM-dd, gol=azi):", string.Empty);
            task.DueDate = string.IsNullOrWhiteSpace(due) ? DateTime.UtcNow.Date : DateTime.Parse(due).ToUniversalTime();
        }

        if (task is DeadlineTask)
        {
            var due = AnsiConsole.Ask<string>("DueDate (yyyy-MM-dd):");
            task.DueDate = DateTime.Parse(due).ToUniversalTime().AddHours(12);
        }

        _service.Add(task);
        AnsiConsole.MarkupLine($"[green]Sarcina adaugata cu Id {task.Id}.[/]");
        return true;
    }

    private bool EditTask()
    {
        var id = AnsiConsole.Ask<int>("Id sarcina:");
        var task = _service.GetById(id)
            ?? throw new InvalidOperationException("Sarcina nu exista");

        task.Title = AnsiConsole.Ask("Titlu nou:", task.Title);
        task.Priority = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("Prioritate")
                .AddChoices(1, 2, 3));
        task.Status = AnsiConsole.Prompt(
            new SelectionPrompt<WorkStatus>()
                .Title("Status")
                .AddChoices(WorkStatus.Todo, WorkStatus.InProgress, WorkStatus.Done));

        _service.Update(task);
        AnsiConsole.MarkupLine("[green]Sarcina actualizata.[/]");
        return true;
    }

    private bool DeleteTask()
    {
        var id = AnsiConsole.Ask<int>("Id sarcina de sters:");
        _service.Delete(id);
        AnsiConsole.MarkupLine("[green]Sarcina stearsa.[/]");
        return true;
    }

    private bool CompleteTask()
    {
        var id = AnsiConsole.Ask<int>("Id sarcina de finalizat:");
        _service.Complete(id);
        AnsiConsole.MarkupLine("[green]Sarcina finalizata. Notificare trimisa.[/]");
        return true;
    }
}
