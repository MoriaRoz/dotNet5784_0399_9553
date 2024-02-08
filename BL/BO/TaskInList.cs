
using System;

namespace BO;

/// <summary>
/// Represents a task item within a list.
/// </summary>
public class TaskInList
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Gets or sets the alias of the task.
    /// </summary>
    public string? Alias { get; set; }
    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    public Statuses Status { get; set; }
    //public override string? ToString() => this.ToStringProperty();
}