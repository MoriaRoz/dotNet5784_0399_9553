namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Runtime.Intrinsics.Arm;

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
    public IUser User => new UserImplementation();
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
        IEnumerable<BO.TaskInList> allTasksINList = s_bl.Task.ReadAll();
        var allTasks =(from t in allTasksINList
                       let task= s_bl.Task.Read(t.Id)
                       select task);
        List<BO.Task> undatedTasks = null;
        foreach (BO.Task task in allTasks)
        {
            if (!task.Dependencies.Any())
                DataForTask(task);
            else
                undatedTasks.Add(task);

        }
       while (undatedTasks != null)
        {
            foreach(BO.Task task in undatedTasks)
            {
                bool canUpdata = true;
                foreach(BO.TaskInList depTask in task.Dependencies)
                {
                    if(depTask.Status==BO.Statuses.Unscheduled)
                        canUpdata = false;break;
                }

                if(canUpdata)
                {
                    DataForTask(task);
                    undatedTasks.Remove(task);
                }
            }
        }
        foreach (BO.Task task in allTasks)
        {
            if (task.Status != Statuses.Scheduled)
                throw new BlTasksLoop("The dependencies between the tasks create a loop - \n" +
                    "There is a task that depends on one or more tasks that are directly or indirectly dependent on it.");
        }
    }
    private void DataForTask(BO.Task task)
    {
        if (task.RequiredEffortTime == null)
            throw new BlDoesNotExistException($"It is not possible to assign a start date to the task with the ID {task.Id} " +
                "because it is not given a specified time that it will take to perform it");

        DateTime? theLastDate = s_bl.GetProjectStartDate();
        foreach (BO.TaskInList dep in task.Dependencies)
        {
            BO.Task taskDep = s_bl.Task.Read(dep.Id);
            if (taskDep != null)
            {
                DateTime endOfTask = taskDep.StartDate.Value.Add(taskDep.RequiredEffortTime.Value);
                if (endOfTask > theLastDate)
                        theLastDate = endOfTask;
            }
        }

        Console.WriteLine($"The earliest date for the task {task.Alias} (with id {task.Id})" +
            $" execution is on {theLastDate} Would you like to schedule it on this date or a later date? (Y/N)\n");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans != "Y" && ans != "N")
            throw new FormatException("Wrong input");
        else if (ans == "N")
        {
            Console.WriteLine($"What is the date on which you would like the task to start, note that this date cannot be earlier than the date {theLastDate}.\n");
            Console.WriteLine("what is the year of the date?\n");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("what is the month of the date?\n");
            int month = int.Parse(Console.ReadLine());
            Console.WriteLine("what is the day of the date?\n");
            int day = int.Parse(Console.ReadLine());
            DateTime tasksDate = new DateTime(year, month, day);
            if (tasksDate < theLastDate)
                throw new BlNumberOutOfRangeException("The date is no later than the start date of the project or one of the tasks on which the task depends");
            theLastDate = tasksDate;
        }
        task.StartDate = theLastDate;
        task.Status = Statuses.Scheduled;

        IEnumerable<BO.TaskInList> allTasksINList = s_bl.Task.ReadAll();
        foreach (BO.TaskInList taskInList in allTasksINList)
        {
            BO.Task taskDep = s_bl.Task.Read(taskInList.Id);
            foreach (BO.TaskInList dependency in taskDep.Dependencies)
                if (dependency.Id == task.Id)
                    dependency.Status = Statuses.Scheduled;
        }
    }
    public void UnscheduleProject()
    {
        IEnumerable<BO.TaskInList> allTasks = s_bl.Task.ReadAll();
        foreach (BO.TaskInList taskInList in allTasks)
        {
            BO.Task? task = s_bl.Task.Read(taskInList.Id);
            if (task != null)
            {
                task.StartDate = null;
                task.Status = BO.Statuses.Unscheduled;
                if(task.Dependencies!=null)
                    foreach (BO.TaskInList dependency in task.Dependencies)
                        dependency.Status = Statuses.Unscheduled;
            }
        }
    }
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();
}