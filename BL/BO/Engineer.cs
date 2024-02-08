
namespace BO;
/// <summary>
/// Represents an engineer entity with associated attributes.
/// </summary>
public class Engineer
{
    /// <summary>
    /// Gets or sets the unique identifier of the engineer.
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Gets or sets the name of the engineer.
    /// </summary>
    public string? Name { get; init; }
    /// <summary>
    /// Gets or sets the email address of the engineer.
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    /// Gets or sets the level of the engineer.
    /// </summary>
    public LevelEngineer Level { get; set; } = LevelEngineer.None;
    /// <summary>
    /// Gets or sets the cost of engineer work.
    /// </summary>
    public double? Cost { get; set; }
    /// <summary>
    /// Gets or sets the task associated with the engineer.
    /// </summary>
    public TaskInEngineer? Task { get; set; }
    //public override string? ToString() => this.ToStringProperty();
}