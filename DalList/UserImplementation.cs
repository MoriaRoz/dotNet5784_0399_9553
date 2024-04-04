
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// The method implementation class of a user entity.
/// </summary>
internal class UserImplementation : IUser
{
    public int Create(User item)//Adding a new engineer to the list if there is no engineer with the same ID.
    {
        // Adding a new user to the list if there is no user with the same ID.        if (DataSource.Users.Find(User => User.EngineerId == item.EngineerId) != null)
        throw new DalAlreadyExistsException($"User with ID={item} already exist");
        if (DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id) == null)
            throw new DalDeletionImpossible($"Engineer with ID={item.Id} dose not exist");
        DataSource.Users.Add(item);//Adding the new user to the list.
        return item.Id;//Returning the user ID.
    }

    public void Delete(int id)//Deletion of a user with EngineerId=id if it exists and can be deleted.
    {
        User? toDel = DataSource.Users.Find(User => User.Id == id);//Reference to the user with Id=id or null to if it doesn't exist.
        if (toDel == null)//user does not exist, error.
            throw new DalDeletionImpossible($"User with ID={id} dose not exist");
        if (DataSource.Tasks.Find(Task => Task.Id == id) != null)//The user is assigned to the task, error.
            throw new DalDeletionImpossible($"User with ID={id} already working on tasks");
        DataSource.Users.Remove(toDel);//Deleting the user.
    }

    public User? Read(int id)//Returning a reference to the user with EngineerId=id if it is in the list and null if not.
    {
        return DataSource.Users.FirstOrDefault(User => User.Id == id);//Searching for a user with EngineerId=id and returning the reference to it.
    }

    public User? Read(Func<User, bool> filter) // stage 2
    {
        // Use the FirstOrDefault method to retrieve the first user satisfying the filter condition.
        return DataSource.Users.FirstOrDefault(filter);
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null) //stage 2
    {
        if (filter != null)
        { // Apply the filter condition to the uses in the data source.
            return from item in DataSource.Users
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Users
               select item; // If no filter is provided, return all users from the data source
    }

    public void Update(User item)//Update of an existing user.
    {
        //Check if there is a engineer with the same ID as 'item', if there is it returns a reference to it and otherwise null:
        User? oldUser = DataSource.Users.Find(User => User.Id == item.Id);
        //There is no user that needs to be updated, so there is an exception:
        if (oldUser == null)
            throw new DalDoesNotExistException($"User with ID={item.Id} dose not exist");
        DataSource.Users.Remove(oldUser);//Deleting the old user.
        DataSource.Users.Add(item);//Adding the new user.
    }
}
