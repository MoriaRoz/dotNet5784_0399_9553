
namespace BO;
/// <summary>
/// Represents an engineer associated with a task.
/// </summary>
public class EngineerInTask
{
    /// <summary>
    /// Gets or sets the unique identifier of the engineer.
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Gets or sets the name of the engineer.
    /// </summary>
    public string? Name { get; init; }
    //public override string? ToString() => this.ToStringProperty();
    public override string ToString() => Tools.ToStringProperties(this);
}