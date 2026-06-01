using TaskManager.Core.Models;
using TaskManager.Core.Services;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class TaskValidatorTests
{
    private TaskValidator _validator = null!;

    [SetUp]
    public void SetUp() => _validator = new TaskValidator();

    [Test]
    public void Validate_EmptyTitle_Throws()
    {
        var task = new TaskItem { Title = "   " };
        Assert.Throws<ArgumentException>(() => _validator.Validate(task));
    }

    [Test]
    public void Validate_TitleOver200Chars_Throws()
    {
        var task = new TaskItem { Title = new string('x', 201) };
        Assert.Throws<ArgumentException>(() => _validator.Validate(task));
    }

    [Test]
    public void Validate_DeadlineWithPastDueDate_Throws()
    {
        var task = new DeadlineTask
        {
            Title = "Deadline",
            DueDate = DateTime.UtcNow.AddMinutes(-5)
        };
        Assert.Throws<ArgumentException>(() => _validator.Validate(task));
    }

    [Test]
    public void Validate_ValidTask_DoesNotThrow()
    {
        var task = new TaskItem { Title = "Valid" };
        Assert.DoesNotThrow(() => _validator.Validate(task));
    }
}
