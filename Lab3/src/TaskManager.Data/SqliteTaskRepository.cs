using Microsoft.Data.Sqlite;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Data;

public class SqliteTaskRepository : ITaskRepository, IDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteTaskRepository(string? databasePath = null)
    {
        var path = databasePath ?? Path.Combine(AppContext.BaseDirectory, "tasks.db");
        _connection = new SqliteConnection($"Data Source={path}");
        _connection.Open();
        EnsureSchema();
    }

    private void EnsureSchema()
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Tasks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT,
                Status TEXT NOT NULL,
                Priority INTEGER NOT NULL,
                TaskType TEXT NOT NULL,
                NotificationType TEXT NOT NULL,
                DueDate TEXT,
                RecurrenceInterval INTEGER,
                CreatedAt TEXT NOT NULL
            );
            """;
        cmd.ExecuteNonQuery();
    }

    public IReadOnlyList<TaskItem> GetAll()
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Tasks ORDER BY Id";
        using var reader = cmd.ExecuteReader();
        var list = new List<TaskItem>();
        while (reader.Read())
            list.Add(TaskItemMapper.Read(reader));
        return list;
    }

    public TaskItem? GetById(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Tasks WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? TaskItemMapper.Read(reader) : null;
    }

    public void Add(TaskItem task)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = """
            INSERT INTO Tasks (Title, Description, Status, Priority, TaskType, NotificationType, DueDate, RecurrenceInterval, CreatedAt)
            VALUES ($title, $description, $status, $priority, $taskType, $notificationType, $dueDate, $recurrenceInterval, $createdAt);
            SELECT last_insert_rowid();
            """;
        BindTask(cmd, task, includeId: false);
        var newId = Convert.ToInt32(cmd.ExecuteScalar());
        task.Id = newId;
    }

    public void Update(TaskItem task)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = """
            UPDATE Tasks SET
                Title = $title,
                Description = $description,
                Status = $status,
                Priority = $priority,
                TaskType = $taskType,
                NotificationType = $notificationType,
                DueDate = $dueDate,
                RecurrenceInterval = $recurrenceInterval,
                CreatedAt = $createdAt
            WHERE Id = $id;
            """;
        BindTask(cmd, task, includeId: true);
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("Sarcina nu exista");
    }

    public void Delete(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Tasks WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("Sarcina nu exista");
    }

    private static void BindTask(SqliteCommand cmd, TaskItem task, bool includeId)
    {
        if (includeId)
            cmd.Parameters.AddWithValue("$id", task.Id);

        cmd.Parameters.AddWithValue("$title", task.Title);
        cmd.Parameters.AddWithValue("$description", (object?)task.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$status", task.Status.ToString());
        cmd.Parameters.AddWithValue("$priority", task.Priority);
        cmd.Parameters.AddWithValue("$taskType", TaskItemMapper.KindToString(task));
        cmd.Parameters.AddWithValue("$notificationType", task.NotificationType.ToString());
        cmd.Parameters.AddWithValue("$dueDate", task.DueDate.HasValue ? task.DueDate.Value.ToString("O") : DBNull.Value);
        cmd.Parameters.AddWithValue("$recurrenceInterval", task.RecurrenceInterval.HasValue ? task.RecurrenceInterval.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$createdAt", task.CreatedAt.ToString("O"));
    }

    public void Dispose() => _connection.Dispose();
}
