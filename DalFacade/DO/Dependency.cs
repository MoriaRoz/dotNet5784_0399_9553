
namespace DO;

/// <summary>
/// A dependency entity represents a dependency with all its accessories.
/// </summary>
/// <param name="Id">Personal identifier of a dependecy</param>
/// <param name="PreviousTask">Previous assignment ID number</param>
/// <param name="DependsTask">ID number of pending task</param>
public record Dependency
(
    int Id,
    int? PreviousTask=null,
    int? DependsTask = null
)
{
    //Empty constructor
    public Dependency() : this (0, null, null) { }
}