using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Data;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public IReadOnlyList<TaskItem> GetAll() =>
        _tasks.Select(t => t.Clone()).ToList();

    public TaskItem? GetById(int id)
    {
        var found = _tasks.FirstOrDefault(t => t.Id == id);
        return found?.Clone();
    }

    public void Add(TaskItem task)
    {
        task.Id = _nextId++;
        if (task.CreatedAt == default)
            task.CreatedAt = DateTime.UtcNow;
        _tasks.Add(task.Clone());
    }

    public void Update(TaskItem task)
    {
        var index = _tasks.FindIndex(t => t.Id == task.Id);
        if (index < 0)
            throw new InvalidOperationException("Sarcina nu exista");
        _tasks[index] = task.Clone();
    }

    public void Delete(int id)
    {
        var index = _tasks.FindIndex(t => t.Id == id);
        if (index < 0)
            throw new InvalidOperationException("Sarcina nu exista");
        _tasks.RemoveAt(index);
    }
}
