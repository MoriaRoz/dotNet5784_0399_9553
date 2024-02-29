
namespace DalApi;

/// <summary>
/// An interface that represents the data layer
/// </summary>
public interface IDal
{
    IEngineer Engineer { get; }
    ITask Task { get; }
    IDependency Dependency { get; }
    public void Reset() { }
}