
namespace DO;

/// <summary>
/// An engineer entity represents an engineer with all its accessories.
/// </summary>
/// <param name="Id">Personal unique ID of the engineer</param>
/// <param name="Name">Engineer's first and last name</param>
/// <param name="Email">Email address of the engineer</param>
/// <param name="Level">Engineer level</param>
/// <param name="Cost">Cost per hour of engineer work</param>
public record Engineer
(
    int Id,
    string? Name=null,
    string? Email=null,
    LevelEngineer Level=LevelEngineer.Beginner,
    double? Cost = null
)
{
    //Empty constructor
    public Engineer() : this (0,null,null, LevelEngineer.Beginner, null) { }
}
