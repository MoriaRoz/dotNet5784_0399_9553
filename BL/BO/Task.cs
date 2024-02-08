
namespace BO;
/// <summary>
/// 
/// </summary>
public class Task
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Alias { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime CreatedAtDate { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public Statuses Status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<BO.TaskInList>? Dependencies { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public TimeSpan? RequiredEffortTime { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? ForecastDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? DeadlineDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? CompleteDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Deliverables { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Remarks { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public EngineerInTask? Engineer { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public BO.LevelEngineer Complexity { get; set; } = LevelEngineer.None;
    //public override string? ToString() => this.ToStringProperty();
}