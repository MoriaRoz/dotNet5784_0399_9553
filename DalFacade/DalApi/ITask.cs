
namespace DalApi;
using DO;

/// <summary>
/// An interface of a task, contains all the methods that a task entity has.
/// </summary>
public interface ITask
{
    int Create(Task item); //Creates new entity object in DAL
    Task? Read(int id); //Reads entity object by its ID 
    List<Task> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Task item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
