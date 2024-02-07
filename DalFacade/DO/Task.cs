
namespace DO;

/// <summary>
/// A task entity represents a task with all its attributes.
/// </summary>
/// <param name="Id">Personal unique ID of the task (automatically assigned upon creation)</param>
/// <param name="Alias">Alias for the task</param>
/// <param name="Description">Description of the task</param>
/// <param name="CreatedAtDate">Date and time when the task was created</param>
/// <param name="RequiredEffortTime">Estimated effort time required for the task</param>
/// <param name="IsMilestone">Indicates whether the task is a milestone</param>
/// <param name="Complexity">The level of complexity of the task - that is, what is the minimum level of an engineer who can work on it</param>
/// <param name="StartDate">Start date of the task</param>
/// <param name="DeadlineDate">Deadline for completing the task</param>
/// <param name="CompleteDate">Date when the task was completed</param>
/// <param name="Deliverables">Deliverables associated with the task</param>
/// <param name="Remarks">Additional remarks or notes about the task</param>
/// <param name="EngineerId">ID of the engineer assigned to the task</param>
public record Task
(
    int Id,
    string? Alias = null,
    string? Description = null,
    DateTime CreatedAtDate = default,
    TimeSpan? RequiredEffortTime = null,
    LevelEngineer Complexity = LevelEngineer.Beginner,
    DateTime? StartDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null
)
{
    //Empty constructor
    public Task() : this (0, null, null, default, null, LevelEngineer.Beginner, null, null, null, null, null, null) { }
}