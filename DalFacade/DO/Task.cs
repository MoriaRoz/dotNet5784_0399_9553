namespace DO;
/// <summary>
/// An engineer entity represents an engineer with all its accessories.
/// </summary>
/// <param name="Id">Personal unique ID of the engineer</param>
/// <param name="Name">Engineer's first and last name</param>
/// <param name="Email">Email address of the engineer</param>
/// <param name="Level">Engineer level</param>
/// <param name="Cost">Cost per hour of engineer work</param>
public record Task
(
    int Id,
    string? Alias = null,
    string? Description = null,
    DateTime CreatedAtDate = default,
    TimeSpan? RequiredEffortTime = null,
    bool IsMilestone = false,   
    ComplexityTask Complexity = ComplexityTask.Unscheduled,
    DateTime? StartDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId
)
{

    public Task() : this(0) { }
    //public Task(string alias, string description, DateTime createdAtDate, TimeSpan requiredEffortTime, bool isMilestone,
    //    ComplexityTask complexity, DateTime startDate, DateTime deadlineDate, DateTime completeDate, string deliverables, string remarks, int engineerId)
    //{
    //    Id = 0; //need to be changed
    //    Alias = alias;
    //    Description = description;
    //    CreatedAtDate = createdAtDate;
    //    RequiredEffortTime = requiredEffortTime;
    //    IsMilestone = isMilestone;
    //    Complexity = complexity;
    //    StartDate = startDate;
    //    DeadlineDate = deadlineDate;
    //    CompleteDate = completeDate;
    //    Deliverables = deliverables;
    //    Remarks = remarks;
    //    EngineerId = engineerId;
    //}
}