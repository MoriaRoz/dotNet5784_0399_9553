
namespace BO;

public class Task
{
    public int Id { get; init; }
    public string? Description {  get; set; }
    public string? Alias {  get; set; }
    public DateTime? CreatedAtDate { get; init; }
    public Statuses Status {  get; set; }
    public List<BO.TaskInList>? Dependencies { get; set; }
    public TimeSpan? RequiredEffortTime { get; init; }
    public DateTime? StartDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public Tuple<int, string>? Engineer { get; set; }
    public BO.LevelEngineer Complexity { get; set; } = LevelEngineer.None;
    //public override string? ToString() => this.ToStringProperty();
}
