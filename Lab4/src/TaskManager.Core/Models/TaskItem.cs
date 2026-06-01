namespace TaskManager.Core.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkStatus Status { get; set; } = WorkStatus.Todo;
    public int Priority { get; set; } = 2;
    public virtual TaskKind Kind => TaskKind.Standard;
    public NotificationChannel NotificationType { get; set; } = NotificationChannel.Console;
    public DateTime? DueDate { get; set; }
    public int? RecurrenceInterval { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual bool IsOverdue =>
        Status != WorkStatus.Done && DueDate.HasValue && DueDate.Value < DateTime.UtcNow;

    public void Complete()
    {
        if (Status == WorkStatus.Done)
            throw new InvalidOperationException("Sarcina este deja finalizata");

        if (Status == WorkStatus.Done && IsOverdue)
            throw new InvalidOperationException("Invarianta incalcata: Done si Overdue");

        CompleteCore();

        if (Status != WorkStatus.Done)
            throw new InvalidOperationException("Postconditie incalcata: Status trebuie sa fie Done");

        if (Status == WorkStatus.Done && IsOverdue)
            throw new InvalidOperationException("Invarianta incalcata: Done si Overdue");
    }

    protected virtual void CompleteCore()
    {
        Status = WorkStatus.Done;
    }

    public virtual TaskItem Clone()
    {
        return new TaskItem
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Status = Status,
            Priority = Priority,
            NotificationType = NotificationType,
            DueDate = DueDate,
            RecurrenceInterval = RecurrenceInterval,
            CreatedAt = CreatedAt
        };
    }
}
