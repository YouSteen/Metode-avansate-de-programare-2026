using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly TaskValidator _validator;
    private readonly IReadOnlyDictionary<NotificationChannel, ITaskNotifier> _notifiers;

    public TaskService(
        ITaskRepository repository,
        TaskValidator validator,
        IReadOnlyDictionary<NotificationChannel, ITaskNotifier> notifiers)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _notifiers = notifiers ?? throw new ArgumentNullException(nameof(notifiers));
    }

    public IReadOnlyList<TaskItem> GetAll() => _repository.GetAll();

    public TaskItem? GetById(int id) => _repository.GetById(id);

    public IReadOnlyList<TaskItem> Filter(WorkStatus? status, int? priority) =>
        _repository.GetAll()
            .Where(t => !status.HasValue || t.Status == status.Value)
            .Where(t => !priority.HasValue || t.Priority == priority.Value)
            .ToList();

    public void Add(TaskItem task)
    {
        _validator.Validate(task);
        _repository.Add(task);
    }

    public void Update(TaskItem task)
    {
        _validator.Validate(task);
        if (_repository.GetById(task.Id) is null)
            throw new InvalidOperationException("Sarcina nu exista");
        _repository.Update(task);
    }

    public void Delete(int id)
    {
        if (_repository.GetById(id) is null)
            throw new InvalidOperationException("Sarcina nu exista");
        _repository.Delete(id);
    }

    public void Complete(int id)
    {
        var task = _repository.GetById(id)
            ?? throw new InvalidOperationException("Sarcina nu exista");

        task.Complete();
        _repository.Update(task);

        if (_notifiers.TryGetValue(task.NotificationType, out var notifier))
            notifier.Notify(task);
    }
}
