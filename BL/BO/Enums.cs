
namespace BO;
/// <summary>
/// Represents the level of an engineer.
/// </summary>
public enum LevelEngineer { None,Beginner, AdvancedBeginner, Intermediate, Advanced, Expert };
/// <summary>
/// Represents the status of a task.
/// </summary>
public enum Statuses { Unscheduled, Scheduled, Started, Done };//InJeopardy
/// <summary>
/// Represents the status of a project.
/// </summary>
public enum ProjectStatus { Inlanning, InExecution };
public enum UserRole { Engineer, Manager };