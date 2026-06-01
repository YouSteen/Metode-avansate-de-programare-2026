namespace TaskManager.Core.Models;

public class RecurringTask : TaskItem
{
    public override TaskKind Kind => TaskKind.Recurring;

    protected override void CompleteCore()
    {
        Status = WorkStatus.Done;
        var interval = RecurrenceInterval ?? 1;
        DueDate = (DueDate ?? DateTime.UtcNow).AddDays(interval);
    }

    public override TaskItem Clone()
    {
        return new RecurringTask
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
