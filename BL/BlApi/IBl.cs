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
    public void InitializeDB();
    public void ResetDB();
    public DateTime Clock { get; }
    public void addHourToClock();
    public void addDayToClock();
    public void restartClock();
}