
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
/// <summary>
/// 
/// </summary>
public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)//Creating an ID for 'item' and adding it to the dependency list.
    {
        int idNew = DataSource.Config.NextDependencyId;//ID for the new dependency.
        //Creating a new dependency with the new ID and values from 'item':
        Dependency dependencyNew = new Dependency(idNew, item.PreviousTask,item.DependsOnTask);
        DataSource.Dependencys.Add(dependencyNew);//Adding the new dependency to the list.
        return idNew;//Returning the dependency ID.
    }

    public void Delete(int id)//Deletion of a dependency with Id=id if it exists and can be deleted.
    {
        throw new Exception("Dependencies cannot be deleted");// It is not possible to delete a dependency, error.
    }

    public Dependency? Read(int id)//Returning a reference to the dependency with Id=id if it is in the list and null if not.
    {
        return DataSource.Dependencys.Find(Dependency=>Dependency.Id == id);//Searching for a dependency with id=id and returning the reference to it.
    }

    public List<Dependency> ReadAll()//Return a copy of the dependency list.
    {
        return new List<Dependency>(DataSource.Dependencys);//Creating a copy and returning it.
    }

    public void Update(Dependency item)//Update of an existing dependency.
    {
        //Check if there is a dependency with the same ID as 'item', if there is it returns a reference to it and otherwise null:
        Dependency? oldDependency = DataSource.Dependencys.Find(Dependency => Dependency.Id == item.Id);
        //There is no dependency that needs to be updated, so there is an exception:
        if (oldDependency == null)
            throw new Exception($"Dependency with ID={item.Id} dose not exist");
        DataSource.Dependencys.Remove(oldDependency);//Deleting the old dependency.
        DataSource.Dependencys.Add(item);//Adding the new dependency.
    }
}
