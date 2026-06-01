using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services;

public class ReportService
{
    private readonly ITaskReader _reader;

    public ReportService(ITaskReader reader) =>
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));

    public string GenerateSummary()
    {
        var all = _reader.GetAll();
        var done = all.Count(t => t.Status == WorkStatus.Done);
        return $"Total sarcini: {all.Count}, finalizate (Done): {done}";
    }
}
