
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int idNew = DataSource.Config.NextTaskId;
        Task task = new Task(idNew,item.Alias,item.Description,item.CreatedAtDate,
            item.RequiredEffortTime,item.IsMilestone,item.Complexity,item.StartDate,
            item.DeadlineDate,item.CompleteDate,item.Deliverables,item.Remarks,item.EngineerId);
        DataSource.Tasks.Add(task);
        return idNew;
    }

    public void Delete(int id)
    {
        
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(Task=>Task.Id == id);
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        Task? oldTask=DataSource.Tasks.Find(Task=> Task.Id == item.Id);
        if(oldTask == null)
            throw new Exception($"Task with ID={item.Id} dose not exist");
        DataSource.Tasks.Remove(oldTask);
        DataSource.Tasks.Add(item);
    }
}
