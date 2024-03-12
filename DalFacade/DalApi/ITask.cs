
namespace DalApi;
using DO;

/// <summary>
/// An interface of a task, contains all the methods that a task entity has.
/// </summary>
public interface ITask : ICrud<Task> 
{ 
    void SetProjectStartDate(DateTime startDate);
    DateTime? GetProjectStartDate();
    int GetProjectStatus();
}