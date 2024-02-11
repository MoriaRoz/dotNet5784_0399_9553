namespace BlImplementation;
using BlApi;
using BO;
using System;

/// <summary>
/// Implementation of the business logic interface.
/// </summary>
internal class Bl : IBl
{
    static readonly IBl s_bl = Factory.Get();
    /// <summary>
    /// Gets the instance of the business logic for managing engineers.
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Gets the instance of the business logic for managing tasks.
    /// </summary>
    public ITask Task => new TaskImplementation();

    public DateTime? GetProjectStartDate()
    {
        return DalApi.Factory.Get.Task.GetProjectStartDate();
    }

    public ProjectStatus GetProjectStatus()
    {
        return (BO.ProjectStatus)DalApi.Factory.Get.Task.GetProjectStatus();
    }

    public void SetProjectStartDate(DateTime startDate)
    {
        DalApi.Factory.Get.Task.SetProjectStartDate(startDate);
    }

    public void CreateSchedule(DateTime startDate)
    {
        IEnumerable<BO.Task> allTasks = s_bl.Task.ReadAll();
        List<BO.Task> didntEntered = new List<BO.Task>();
        foreach (BO.Task task in allTasks)
        {
            if (task.Dependencies == null)
                ProcessTask(task);
            else
                didntEntered.Add(task);
        }
        bool tasksProcessed = true;
        while (tasksProcessed)
        {
            tasksProcessed = false;
            foreach (BO.Task task in didntEntered)
            {
                bool allDependenciesEntered = true;
                foreach (BO.TaskInList dependency in task.Dependencies)
                    if (dependency.Status == Statuses.Unscheduled)
                    {
                        allDependenciesEntered = false;
                        break;
                    }
                if (allDependenciesEntered)
                {
                    ProcessTask(task);
                    didntEntered.Remove(task);
                    tasksProcessed = true;
                }
            }
        }
        foreach (BO.Task task in allTasks)
        {
            if (task.Status != Statuses.Scheduled)
                throw new BlTasksLoop("The dependencies between the tasks create a loop - there is a task that " +
                    "depends on one or more tasks that are directly or indirectly dependent on it.");
        }
        }
    private void ProcessTask(BO.Task task)
    {
        if (task.RequiredEffortTime == null)
            throw new BlDoesNotExistException($"It is not possible to assign a start date to the task with the ID {task.Id} " +
                $"because it is not given a specified time that it will take to perform it");
        DateTime theLastDate = s_bl.GetProjectStartDate();
        foreach (BO.TaskInList dependency in task.Dependencies)
        {
            foreach (BO.Task dep in allTasks)
            {
                if (dep.Id == dependency.Id)
                {
                    DateTime endOfTask = dep.StartDate.Value.Add(dep.RequiredEffortTime.Value);
                    if (endOfTask > theLastDate)
                        theLastDate = endOfTask;
                }
            }
        }
        Console.WriteLine($"The earliest date for the task {task.Alias} (with id {task.Id})" +
            $" execution is on {theLastDate} Would you like to schedule it on this date or a later date? (Y/N)\n");
        if (ans != "Y" && ans != "N")
            throw new FormatException("Wrong input");
        else if (ans == "N")
        {
            Console.WriteLine($"What is the date on which you would like the task to start, note that this date cannot be earlier than the date {theLastDate}.\n");
            Console.WriteLine("what is the year of the date?\n");
            int year = Console.ReadLine();
            Console.WriteLine("what is the month of the date?\n");
            int month = Console.ReadLine();
            Console.WriteLine("what is the day of the date?\n");
            int day = Console.ReadLine();
            DateTime tasksDate = new DateTime(year, month, day);
            if (tasksDate < theLastDate)
                throw new BlNumberOutOfRangeException("The date is no later than the start date of the project or one of the tasks on which the task depends")
           theLastDate = tasksDate;
        }
        task.StartDate = theLastDate;
        task.Status = Statuses.Scheduled;
        IEnumerable<BO.Task> allTasks = s_bl.Task.ReadAll();
        foreach (BO.Task tasks in allTasks)
            foreach (BO.TaskInList dependency in tasks.Dependencies)
                if (dependency.Id == task.Id)
                    dependency.Status = Statuses.Scheduled;
    }
}
public void UnscheduleProject()
{
    IEnumerable<BO.Task> allTasks = s_bl.Task.ReadAll();
    foreach (BO.Task task in allTasks)
    {
        task.StartDate.Value = null;
        task.Status = unchecked;
        foreach (BO.TaskInList dependency in task.Dependencies)
            dependency.Status = Statuses.Unscheduled;
    }
}