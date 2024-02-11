
namespace BO;
/// <summary>
/// Represents a task assigned to an engineer.
/// </summary>
public class TaskInEngineer
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Gets or sets the alias of the task.
    /// </summary>
    public string? Alias { get; init; }
    //public override string? ToString() => this.ToStringProperty();
    public override string ToString() => Tools.ToStringProperties(this);
}