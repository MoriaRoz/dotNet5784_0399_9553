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
    /// <summary>
    /// Gets the instance of the business logic for managing users.
    /// </summary>
    public IUser User => new UserImplementation();

    #region Start date and status of the project
    public DateTime? GetProjectStartDate()
    {
        return DalApi.Factory.Get.Task.GetProjectStartDate();
    }
    public ProjectStatus GetProjectStatus()
    {
        int enumVal = DalApi.Factory.Get.Task.GetProjectStatus();
        if (enumVal == 0)
            return BO.ProjectStatus.Inlanning;
        else
            return BO.ProjectStatus.InExecution;
    }
    public void SetProjectStartDate(DateTime startDate)
    {
        DalApi.Factory.Get.Task.SetProjectStartDate(startDate);
    }
    #endregion

    public void CreateSchedule(DateTime projectStartDate)
    {
        if (GetProjectStatus() == ProjectStatus.InExecution)
            throw new BO.BlCreationImpossibleException("Can not create project schedule while project is in Execution stage");

        //reading tasks and saving in sorted list
        var tasks = Task.ReadAll();
        List<BO.Task> tasksList = new List<BO.Task>();
        foreach (var task in tasks)
        {
            Task? t = Task.Read(task.Id);
            if (t != null)
                tasksList.Add(t);
        }
        tasksList.OrderBy(item => item.Id).ToList();

        //making sure all tasks have required effort time assigned
        if (tasksList.Any(task => task.RequiredEffortTime == null))
            throw new BO.BlInvalidValueException("Can not plan project schedule if not all tasks have required effort time assigned");

        //for each task, finding the earliest possible date and assigning to task
        tasksList.ForEach(task => DataForTask(task.Id, projectStartDate));

        //updating project start date in config
        SetProjectStartDate(projectStartDate);

        ////finding maximal planned finish date of tasks
        //tasks = Task.ReadAllFullTasks().ToList();
        //return (DateTime)tasks.Max(task => task.ForecastDate)!;
    }

    void DataForTask(int id, DateTime projectStartDate)
    {
        BO.Task task = Task.Read(id) ?? new BO.Task();
        if (task.ScheduledDate != null) return;

        List<BO.TaskInList>? prevTasks = task.Dependencies ?? new List<BO.TaskInList>();

        prevTasks.ForEach(task => DataForTask(task.Id, projectStartDate));

        DateTime? startDate = GetEarliestDate(Task.Read(task.Id), projectStartDate);

        if (startDate == null) throw new BO.BlInvalidValueException("Forecast date is null");
        AssignScheduledDateToTask(task.Id, (DateTime)startDate);
    }
    private DateTime? GetEarliestDate(BO.Task task, DateTime projectStartDate)
    {
        DateTime? earliestDate = projectStartDate;
        if (task.Dependencies != null)
        {
            foreach (BO.TaskInList Dependencies in task.Dependencies)
            {
                BO.Task? DependenciesTask = Task.Read(Dependencies.Id);
                if (DependenciesTask != null)
                {
                    DateTime? endOfDependencies = DependenciesTask.ScheduledDate?.Add(DependenciesTask.RequiredEffortTime ?? TimeSpan.Zero);
                    if (endOfDependencies > earliestDate)
                        earliestDate = endOfDependencies;
                }
            }
        }
        return earliestDate;
    }

    private void AssignScheduledDateToTask(int taskId, DateTime scheduledDate)
    {
        BO.Task task = Task.Read(taskId);
        task.ScheduledDate = scheduledDate;
        task.Status = Statuses.Scheduled;
        Task.Update(task);
    }

    private void RecursiveProjectSchedule(int taskId, DateTime projectStartDate)
    {
        BO.Task task = Task.Read(taskId);
        if (task.ScheduledDate == null)
            DataForTask(taskId, projectStartDate);
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
                task.ScheduledDate = null;
                task.CompleteDate = null;
                task.Status = BO.Statuses.Unscheduled;
                if (task.Dependencies != null)
                    foreach (BO.TaskInList Dependencies in task.Dependencies)
                        Dependencies.Status = Statuses.Unscheduled;
                s_bl.Task.Update(task);
            }
        }
    }
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB()
    {
        UnscheduleProject();
        DalApi.Factory.Get.Reset();
    }

    #region gantt
    public List<DateTime?> getProjectDates()
    {
        List<DateTime?> dates = new List<DateTime?>();
        foreach(BO.TaskInList t in s_bl.Task.ReadAll())
        {
            BO.Task task = s_bl.Task.Read(t.Id);
            dates.Add((task.StartDate)??task.ScheduledDate);
            dates.Add((task.CompleteDate)?? task.ForecastDate);
        }
        DateTime? lastDate = dates.MaxBy(date => date);
        DateTime? firstDate = dates.MinBy(date => date);
        List<DateTime?> datesInRange = new List<DateTime?>();
        for (DateTime date = firstDate.Value; date <= lastDate.Value; date = date.AddDays(7))
        {
            datesInRange.Add(date);
        }
        return datesInRange;
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
                Duration = (endT - startT).Days*100/7,
            };
            tasksG.Add(tGantt);
        }
        foreach (BO.TaskGantt t in tasksG)
        {
            t.TimeFromStart = (t.TaskStart - start).Days * 100 / 7;
            t.TimeToEnd = (end - t.TaskEnd).Days * 100 / 7;
        }

        return tasksG;
    }
    #endregion

    #region Clock
    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }
    public void addHourToClock()
    {
        Clock= s_Clock.AddHours(1);
    }
    public void addDayToClock()
    {
        Clock=s_Clock.AddDays(1);
    }
    public void addHalfMinToClock()
    {
        Clock = s_Clock.AddSeconds(31);
    }
    public void restartClock()
    {
        Clock=s_Clock = DateTime.Now;
    }
    #endregion
}