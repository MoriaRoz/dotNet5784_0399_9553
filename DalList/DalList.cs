namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    public IUser User => new UserImplementation();

    public IDependency Dependency => new DependencyImplementation();
    public void Reset() 
    { 
        DataSource.Engineers.Clear();
        DataSource.Dependencys.Clear();
        DataSource.Tasks.Clear();

        DataSource.Config.ProjectStartDate = null;
        DataSource.Config.NextDependencyId = DataSource.Config.startDependencyId;
        DataSource.Config.NextTaskId = DataSource.Config.startTaskId;
    }
}

