
namespace DalApi;

/// <summary>
/// An interface that represents the data layer
/// </summary>
public interface IDal
{
    IEngineer Engineer { get; }
    ITask Task { get; }
    IDependency Dependency { get; }
    IUser User { get; }
    public void Reset() { }
    public void ResetIds() { }
}