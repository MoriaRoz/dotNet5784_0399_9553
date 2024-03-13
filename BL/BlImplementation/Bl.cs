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
    public IEngineer Engineer => new EngineerImplementation(this);
    /// <summary>
    /// Gets the instance of the business logic for managing tasks.
    /// </summary>
    public ITask Task => new TaskImplementation(this);
    public IUser User => new UserImplementation();
    public DateTime? GetProjectStartDate()
    {
        return DalApi.Factory.Get.Task.GetProjectStartDate();
    }
    public ProjectStatus GetProjectStatus()
    {
        return (BO.ProjectStatus)DalApi.Factory.Get.Task.GetProjectStatus();
    }
    public void SetProjectStartDate(DateTime? startDate)
    {
        DalApi.Factory.Get.Task.SetProjectStartDate(startDate);
    }
    public void CreateSchedule(DateTime? startDate)
    {
        IEnumerable<BO.TaskInList> allTasksINList = s_bl.Task.ReadAll();
        var allTasks = (from t in allTasksINList
                        let task = s_bl.Task.Read(t.Id)
                        select task);
        List<BO.Task> undatedTasks = new List<BO.Task>();
        foreach (BO.Task task in allTasks)
        {
            if (!task.Dependencies.Any())
                DataForTask(task);
            else
                undatedTasks.Add(task);

        }
        while (undatedTasks.Count != 0)
        {
            foreach (BO.Task task in undatedTasks)
            {
                bool canUpdata = true;
                foreach (BO.TaskInList depTask in task.Dependencies)
                {
                    if (depTask.Status == BO.Statuses.Unscheduled)
                        canUpdata = false; break;
                }

                if (canUpdata)
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

        //Console.WriteLine($"The earliest date for the task {task.Alias} (with id {task.Id})" +
        //    $" execution is on {theLastDate} Would you like to schedule it on this date or a later date? (Y/N)\n");
        //string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        //if (ans != "Y" && ans != "N")
        //    throw new FormatException("Wrong input");
        //else if (ans == "N")
        //{
        //    Console.WriteLine($"What is the date on which you would like the task to start, note that this date cannot be earlier than the date {theLastDate}.\n");
        //    Console.WriteLine("what is the year of the date?\n");
        //    int year = int.Parse(Console.ReadLine());
        //    Console.WriteLine("what is the month of the date?\n");
        //    int month = int.Parse(Console.ReadLine());
        //    Console.WriteLine("what is the day of the date?\n");
        //    int day = int.Parse(Console.ReadLine());
        //    DateTime tasksDate = new DateTime(year, month, day);
        //    if (tasksDate < theLastDate)
        //        throw new BlNumberOutOfRangeException("The date is no later than the start date of the project or one of the tasks on which the task depends");
        //    theLastDate = tasksDate;
        //}
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
                if (task.Dependencies != null)
                    foreach (BO.TaskInList dependency in task.Dependencies)
                        dependency.Status = Statuses.Unscheduled;
            }
        }
    }
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalApi.Factory.Get.Reset();

    #region gantt
    public List<DateTime?> getProjectDates()
    {
        List<DateTime?> dates = new List<DateTime?>();
        foreach(BO.TaskInList t in s_bl.Task.ReadAll())
        {
            BO.Task task = s_bl.Task.Read(t.Id);
            dates.Add(task.StartDate);
            dates.Add(task.CompleteDate);
        }

        dates.Distinct().ToList();
        dates.OrderBy(date => date).ToList();
        return dates;
    }
    public List<BO.TaskGantt> tasksGantt()
    {
        List<BO.TaskGantt> tasksG = new List<BO.TaskGantt>();
        
        var tasks = s_bl.Task.ReadAll();
 
        DateTime start = s_bl.GetProjectStartDate() ?? s_bl.Clock;
        DateTime end = s_bl.Clock;

        foreach (BO.TaskInList t in tasks)
        {
            BO.Task task = s_bl.Task.Read(t.Id);

            DateTime startT = task.ScheduledDate ?? s_bl.Clock;
            DateTime endT = task.ForecastDate ?? s_bl.Clock;

            if (task.StartDate != null)
                startT = task.StartDate ?? s_bl.Clock;
            if (startT < start)
                start = startT;

            if (endT > end)
                end = endT;

            BO.TaskGantt tGantt = new BO.TaskGantt()
            {
                TaskId = task.Id,
                TaskAlias = task.Alias,
                TaskStart = startT,
                TaskEnd = endT,
                Duration = (endT - startT).Days,
            };
            tasksG.Add(tGantt);
        }
        foreach (BO.TaskGantt t in tasksG)
        {
            t.TimeFromStart = (t.TaskStart - start).Days;
            t.TimeToEnd = (end - t.TaskEnd).Days;
        }

        return tasksG;
    }
    #endregion

    #region Clock
    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }
    public void addHourToClock()
    {
        Clock= s_Clock.AddHours(1);
    }
    public void addDayToClock()
    {
        Clock=s_Clock.AddDays(1);
    }
    public void restartClock()
    {
        Clock=s_Clock = DateTime.Now;
    }
    #endregion
}