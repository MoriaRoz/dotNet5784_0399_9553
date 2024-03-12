
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task entity)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize

        int newId = Config.NextTaskId;//ID for the new task.
        Task newTask = entity with { Id = newId };//new task
        tasks.Add(newTask);

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

        return newId;
    }

    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        
        Task? toDel = Read(id);//Reference to the task with Id=id or null to if it doesn't exist.
        if (toDel == null)//Task does not exist, error.
            throw new DalDeletionImpossible($"Task with ID={id} dose not exist");
        if (toDel.CompleteDate == null&&toDel.StartDate!=null)//Task not completed, error.
            throw new DalDeletionImpossible($"Task with ID={id} already scheduled");
        
        tasks.Remove(toDel);//Deleting the task.
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }

    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize
        if (filter == null)
            return tasks.Select(task => task);
        return tasks.Select(task => task).Where(filter);
        
    }

    public void Update(Task entity) 
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Task? oldTask = Read(entity.Id);
        if (oldTask == null)
            throw new DalDoesNotExistException($"Task with ID={entity.Id} does Not exist");
        else
        {
            tasks.Remove(oldTask);
            tasks.Add(entity);
        }

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }
    
    public DateTime? GetProjectStartDate()
    {
        var doc = XDocument.Load("data-config");

        DateTime? projectStartDate = DateTime.Parse(doc.Root.Element("ProjectStartDate").Value);
        return projectStartDate;
    }

    public int GetProjectStatus()
    {
        if (GetProjectStartDate() == null)
            return 0;
        else return 1;
    }

    public void SetProjectStartDate(DateTime startDate)
    {
        var doc = XDocument.Load("data-config.xml");
        Config.ProjectStartDate = startDate;
        doc.Save("data-config.xml");
    }
}
