
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
        Engineer toDel = DataSource.Tasks.Find(Task => Task.Id == id;
        if (toDel == null)
            throw new Exception($"Task with ID={id} dose not exist");
        if (toDel.Complexity > ComplexityTask.Unscheduled)
            throw new Exception($"Task with ID={id} already scheduled");
        if (DataSource.Dependency.Find(Dependency => Dependency.DependsOnTask == id) == null)
            throw new Exception($"Task with ID={id} cannot be deleted because there are other tasks that depend on it");
        Engineers.Remove(toDel);
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
