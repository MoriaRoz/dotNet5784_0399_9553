
namespace BO;
/// <summary>
/// Represents a task entity.
/// </summary>
public class Task
{ 
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Gets or sets the alias of the task.
    /// </summary>
    public string? Alias { get; set; }
    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the task was created.
    /// </summary>
    public DateTime CreatedAtDate { get; init; }
    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    public Statuses Status { get; set; }
    /// <summary>
    /// Gets or sets the list of dependencies for the task.
    /// </summary>
    public List<BO.TaskInList>? Dependencies { get; set; }
    /// <summary>
    /// Gets or sets the required effort time for the task.
    /// </summary>
    public TimeSpan? RequiredEffortTime { get; init; }
    /// <summary>
    /// Gets or sets the start date of the task.
    /// </summary>
    public DateTime? StartDate { get; set; }
    ///// <summary>
    ///// Gets or sets the deadline date of the task.
    ///// </summary>
    ////public DateTime? DeadlineDate { get; set; }
    /// <summary>
    /// Gets or sets the scheduled date of the task.
    /// </summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// Gets or sets the forecasted date of the task.
    /// </summary>
    public DateTime? ForecastDate { get; set; }
    /// <summary>
    /// Gets or sets the completion date of the task.
    /// </summary>
    public DateTime? CompleteDate { get; set; }
    /// <summary>
    /// Gets or sets the deliverables of the task.
    /// </summary>
    public string? Deliverables { get; set; }
    /// <summary>
    /// Gets or sets the remarks about the task.
    /// </summary>
    public string? Remarks { get; set; }
    /// <summary>
    /// Gets or sets the engineer assigned to the task.
    /// </summary>
    public EngineerInTask? Engineer { get; set; }
    /// <summary>
    /// Gets or sets the complexity level of the task.
    /// </summary>
    public BO.LevelEngineer Complexity { get; set; } = LevelEngineer.None;
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Tools.ToStringProperties(this);
}