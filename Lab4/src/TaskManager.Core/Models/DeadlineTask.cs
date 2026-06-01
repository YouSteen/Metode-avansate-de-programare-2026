namespace TaskManager.Core.Models;

public class DeadlineTask : TaskItem
{
    public override TaskKind Kind => TaskKind.Deadline;

    public override bool IsOverdue =>
        Status != WorkStatus.Done && DueDate.HasValue && DueDate.Value < DateTime.UtcNow;

    protected override void CompleteCore()
    {
        Status = WorkStatus.Done;
    }

    public override TaskItem Clone()
    {
        return new DeadlineTask
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
