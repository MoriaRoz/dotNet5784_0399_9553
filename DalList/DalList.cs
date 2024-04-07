namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    public IUser User => new UserImplementation();

    public IDependency Dependencies => new DependencyImplementation();
    public void Reset() 
    { 
        DataSource.Engineers.Clear();
        DataSource.dependencies.Clear();
        DataSource.Tasks.Clear();
        DataSource.Users.Clear();
        DataSource.Config.NextDependencyId = DataSource.Config.startDependenciesId;
        DataSource.Config.NextTaskId = DataSource.Config.startTaskId;
        DataSource.Config.ProjectStartDate = null;
    }
    public void ResetIds()
    {
        DataSource.Config.NextDependencyId = DataSource.Config.startDependenciesId;
        DataSource.Config.NextTaskId = DataSource.Config.startTaskId;
    }
}

