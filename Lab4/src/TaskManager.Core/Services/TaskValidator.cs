using TaskManager.Core.Models;

namespace TaskManager.Core.Services;

public class TaskValidator
{
    public void Validate(TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("Titlul este obligatoriu");

        if (task.Title.Length > 200)
            throw new ArgumentException("Titlul depaseste 200 de caractere");

        if (task is DeadlineTask && task.DueDate.HasValue && task.DueDate.Value <= DateTime.UtcNow)
            throw new ArgumentException("DueDate trebuie sa fie in viitor pentru DeadlineTask");

        if (task is RecurringTask recurring && (!recurring.RecurrenceInterval.HasValue || recurring.RecurrenceInterval < 1))
            throw new ArgumentException("RecurrenceInterval trebuie sa fie pozitiv pentru RecurringTask");
    }
}
