
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;

internal class TaskImplementation : ITask
{
    readonly string s_task_xml = "tasks";

    public int Create(Task entity)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);
        int nextId = Config.nextTaskId;//ID for the new task.
        //Creating a new task with the new ID and values from 'item':
        Task taskNew = entity with { Id=nextId };
        tasks.Add(taskNew);//Adding the new task to the list.
        XMLTools.SaveListToXMLSerializer(tasks, "s_task_xml");
        return nextId;
    }

    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);
        Task? toDel = tasks.Find(Task => Task.Id == id);//Reference to the task with Id=id or null to if it doesn't exist.
        if (toDel == null)//Task does not exist, error.
            throw new DalDeletionImpossible($"Task with ID={id} dose not exist");
        if (toDel.CompleteDate == null)//Task not completed, error.
            throw new DalDeletionImpossible($"Task with ID={id} already scheduled");
        tasks.Remove(toDel);//Deleting the task.
        XMLTools.SaveListToXMLSerializer(tasks, "s_task_xml");
    }

    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);
        XMLTools.SaveListToXMLSerializer(tasks, "s_task_xml");
        return tasks.FirstOrDefault(Task => Task.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);
        XMLTools.SaveListToXMLSerializer(tasks, "s_task_xml");
        return tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);
        if (filter != null)
        { // Apply the filter condition to the tasks in the data source.
            return from item in tasks
                   where filter(item)
                   select item;
        }
        XMLTools.SaveListToXMLSerializer(tasks, "s_task_xml");
        return from item in tasks
               select item; // If no filter is provided, return all tasks from the data source.
    }

    public void Update(Task entity)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);
        if (tasks.RemoveAll(it => it.Id == entity.Id) == 0)
            throw new DalDoesNotExistException($"Task with ID={entity.Id} dose not exist");
        XMLTools.SaveListToXMLSerializer(tasks, "s_task_xml");
    }
}
