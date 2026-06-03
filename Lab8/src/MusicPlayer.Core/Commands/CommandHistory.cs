namespace MusicPlayer.Core.Commands;

public class CommandHistory
{
    private const int MaxHistory = 50;
    private readonly Stack<IPlayerCommand> _undoStack = new();
    private readonly Stack<IPlayerCommand> _redoStack = new();

    public event Action? HistoryChanged;

    public bool CanUndo => _undoStack.Count > 0;

    public bool CanRedo => _redoStack.Count > 0;

    public IReadOnlyList<string> GetRecentDescriptions(int max = 10) =>
        _undoStack.Take(max).Select(c => c.Description).ToList();

    public void Execute(IPlayerCommand command)
    {
        command.Execute();
        _redoStack.Clear();

        if (!command.CanUndo)
        {
            HistoryChanged?.Invoke();
            return;
        }

        _undoStack.Push(command);
        TrimStack(_undoStack);
        HistoryChanged?.Invoke();
    }

    public void Undo()
    {
        if (!CanUndo)
            return;

        var command = _undoStack.Pop();
        command.Undo();
        _redoStack.Push(command);
        HistoryChanged?.Invoke();
    }

    public void Redo()
    {
        if (!CanRedo)
            return;

        var command = _redoStack.Pop();
        command.Execute();
        _undoStack.Push(command);
        TrimStack(_undoStack);
        HistoryChanged?.Invoke();
    }

    private static void TrimStack(Stack<IPlayerCommand> stack)
    {
        if (stack.Count <= MaxHistory)
            return;

        var items = stack.ToArray();
        stack.Clear();
        var count = Math.Min(MaxHistory, items.Length);
        for (var i = count - 1; i >= 0; i--)
            stack.Push(items[i]);
    }
}
