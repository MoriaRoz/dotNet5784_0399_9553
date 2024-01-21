
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// The method implementation class of a task entity.
/// </summary>
internal class TaskImplementation : ITask
{
    public int Create(Task item)//Creating an ID for 'item' and adding it to the task list.
    {
        int idNew = DataSource.Config.NextTaskId;//ID for the new task.
        //Creating a new task with the new ID and values from 'item':
        Task task = new Task(idNew,item.Alias,item.Description,item.CreatedAtDate,
            item.RequiredEffortTime,item.IsMilestone,item.Complexity,item.StartDate,
            item.DeadlineDate,item.CompleteDate,item.Deliverables,item.Remarks,item.EngineerId);
        DataSource.Tasks.Add(task);//Adding the new task to the list.
        return idNew;//Returning the task ID.
    }

    public void Delete(int id)//Deletion of a task with Id=id if it exists and can be deleted.
    {
        Task? toDel = DataSource.Tasks.Find(Task => Task.Id == id);//Reference to the task with Id=id or null to if it doesn't exist.
        if (toDel == null)//Task does not exist, error.
            throw new DalDeletionImpossible($"Task with ID={id} dose not exist");
        if (toDel.CompleteDate == null)//Task not completed, error.
            throw new DalDeletionImpossible($"Task with ID={id} already scheduled");
        DataSource.Tasks.Remove(toDel);//Deleting the task.
    }

    public Task? Read(int id)//Returning a reference to the task with Id=id if it is in the list and null if not.
    {
        return DataSource.Tasks.FirstOrDefault(Task=>Task.Id == id);//Searching for a task with id=id and returning the reference to it.
    }

    public Task? Read(Func<Task, bool> filter) // stage 2
    { // Use the FirstOrDefault method to retrieve the first task satisfying the filter condition.
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter != null)
        { // Apply the filter condition to the tasks in the data source.
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item; // If no filter is provided, return all tasks from the data source.
    }

    public void Update(Task item)//Update of an existing task.
    {
        //Check if there is a task with the same ID as 'item', if there is it returns a reference to it and otherwise null:
        Task? oldTask=DataSource.Tasks.Find(Task=> Task.Id == item.Id);
        //There is no task that needs to be updated, so there is an exception:
        if (oldTask == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} dose not exist");
        DataSource.Tasks.Remove(oldTask);//Deleting the old task.
        DataSource.Tasks.Add(item);//Adding the new task.
    }
}
