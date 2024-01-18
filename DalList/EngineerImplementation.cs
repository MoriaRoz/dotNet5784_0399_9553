
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// The method implementation class of a task entity.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)//Adding a new engineer to the list if there is no engineer with the same ID.
    {
        //Searching if there is an engineer with the ID of 'item' and returning it or null if it doesn't exist:
        if (DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id) != null)
            //Error, an engineer with the id of 'item' exists.
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exist");
         
        DataSource.Engineers.Add(item);//Adding the new engineer to the list.
        return item.Id;//Returning the engineer ID.
    }

    public void Delete(int id)//Deletion of a engineer with Id=id if it exists and can be deleted.
    {
        Engineer? toDel = DataSource.Engineers.Find(Engineer => Engineer.Id == id);//Reference to the engineer with Id=id or null to if it doesn't exist.
        if (toDel == null)//engineer does not exist, error.
            throw new DalDeletionImpossible($"Engineer with ID={id} dose not exist");
        if (DataSource.Tasks.Find(Task => Task.EngineerId == id) != null)//The engineer is assigned to the task, error.
            throw new DalDeletionImpossible($"Engineer with ID={id} already working on tasks");
        DataSource.Engineers.Remove(toDel);//Deleting the engineer.
    }

    public Engineer? Read(int id)//Returning a reference to the engineer with Id=id if it is in the list and null if not.
    {
        return DataSource.Engineers.FirstOrDefault(Engineer => Engineer.Id == id);//Searching for a engineer with id=id and returning the reference to it.
    }

    public Engineer? Read(Func<Engineer, bool> filter) // stage 2
    {
        // Use the FirstOrDefault method to retrieve the first engineer satisfying the filter condition.
        return DataSource.Engineers.FirstOrDefault(filter);
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter != null)
        { // Apply the filter condition to the engineers in the data source.
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item; // If no filter is provided, return all engineers from the data source
    }

    public void Update(Engineer item)//Update of an existing engineer.
    {
        //Check if there is a engineer with the same ID as 'item', if there is it returns a reference to it and otherwise null:
        Engineer? oldEngineer = DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id);
        //There is no engineer that needs to be updated, so there is an exception:
        if ( oldEngineer == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} dose not exist");
        DataSource.Engineers.Remove(oldEngineer);//Deleting the old engineer.
        DataSource.Engineers.Add(item);//Adding the new engineer.
    }
}
