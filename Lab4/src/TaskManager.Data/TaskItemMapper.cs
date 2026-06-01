using Microsoft.Data.Sqlite;
using TaskManager.Core.Models;

namespace TaskManager.Data;

internal static class TaskItemMapper
{
    public static TaskItem Read(SqliteDataReader reader)
    {
        var kind = Enum.Parse<TaskKind>(reader.GetString(reader.GetOrdinal("TaskType")));
        TaskItem task = kind switch
        {
            TaskKind.Recurring => new RecurringTask(),
            TaskKind.Deadline => new DeadlineTask(),
            _ => new TaskItem()
        };

        task.Id = reader.GetInt32(reader.GetOrdinal("Id"));
        task.Title = reader.GetString(reader.GetOrdinal("Title"));
        task.Description = reader.IsDBNull(reader.GetOrdinal("Description"))
            ? null
            : reader.GetString(reader.GetOrdinal("Description"));
        task.Status = Enum.Parse<WorkStatus>(reader.GetString(reader.GetOrdinal("Status")));
        task.Priority = reader.GetInt32(reader.GetOrdinal("Priority"));
        task.NotificationType = Enum.Parse<NotificationChannel>(reader.GetString(reader.GetOrdinal("NotificationType")));
        task.DueDate = ReadDateTime(reader, "DueDate");
        task.RecurrenceInterval = reader.IsDBNull(reader.GetOrdinal("RecurrenceInterval"))
            ? null
            : reader.GetInt32(reader.GetOrdinal("RecurrenceInterval"));
        task.CreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedAt")), null, System.Globalization.DateTimeStyles.RoundtripKind);

        return task;
    }

    private static DateTime? ReadDateTime(SqliteDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);
        if (reader.IsDBNull(ordinal))
            return null;
        return DateTime.Parse(reader.GetString(ordinal), null, System.Globalization.DateTimeStyles.RoundtripKind);
    }

    public static string KindToString(TaskItem task) => task.Kind.ToString();
}
