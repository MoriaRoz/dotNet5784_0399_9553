
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

/// <summary>
/// 
/// </summary>
internal class DependencyImplementation : IDependency
{ 
    public int Create(Dependency item)//Creating an ID for 'item' and adding it to the Dependency list.
    {
        int idNew = DataSource.Config.NextDependencyId;//ID for the new Dependency.
                                                         //Creating a new Dependency with the new ID and values from 'item':
        Dependency DependencyNew = new Dependency(idNew, item.PreviousTask,item.DependsTask);
        DataSource.dependencies.Add(DependencyNew);//Adding the new Dependency to the list.
        return idNew;//Returning the Dependency ID.
    }

    public void Delete(int id)//Deletion of a Dependency with Id=id if it exists and can be deleted.
    {
        throw new DalDeletionImpossible("Dependency cannot be deleted");// It is not possible to delete a Dependency, error.
    }

    public Dependency? Read(int id)//Returning a reference to the Dependency with Id=id if it is in the list and null if not.
    {
        return DataSource.dependencies.FirstOrDefault(Dependency=>Dependency.Id == id);//Searching for a Dependency with id=id and returning the reference to it.
    }

    public Dependency? Read(Func<Dependency, bool> filter) // stage 2
    {
        // Use the FirstOrDefault method to retrieve the first Dependency satisfying the filter condition.
        return DataSource.dependencies.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {  // Apply the filter condition to the Dependency in the data source.
            return from item in DataSource.dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.dependencies
               select item; // If no filter is provided, return all Dependency from the data source.
    }

    public void Update(Dependency item)//Update of an existing Dependency.
    {
        //Check if there is a Dependency with the same ID as 'item', if there is it returns a reference to it and otherwise null:
        Dependency? oldDependency = DataSource.dependencies.Find(Dependency => Dependency.Id == item.Id);
        //There is no Dependency that needs to be updated, so there is an exception:
        if (oldDependency == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} dose not exist");
        DataSource.dependencies.Remove(oldDependency);//Deleting the old Dependency.
        DataSource.dependencies.Add(item);//Adding the new Dependency.
    }
}
