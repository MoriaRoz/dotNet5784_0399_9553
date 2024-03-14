namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public IUser User { get; }
    public void SetProjectStartDate(DateTime? startDate);
    public DateTime? GetProjectStartDate();
    public BO.ProjectStatus GetProjectStatus();
    public void CreateSchedule(DateTime? startDate);
    public void UnscheduleProject();
    public void InitializeDB();
    public void ResetDB();

    #region gantt
    public List<DateTime?> getProjectDates();
    public List<BO.TaskGantt> tasksGantt();
    #endregion

    #region Clock
    public DateTime Clock { get; }
    public void addHourToClock();
    public void addDayToClock();
    public void restartClock();
    public void addHalfMinToClock();
    #endregion
}
