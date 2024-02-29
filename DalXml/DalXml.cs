namespace Dal;
using DalApi;
using DO;
using System.Diagnostics;

sealed public class DalXml : IDal
{ 
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public void Reset()
    {
        IEnumerable<DO.Task?> tasks = Task.ReadAll();
        if (tasks.Count() != 0)
        {
            foreach (var task in tasks)
                Task.Delete(task.Id);
        }
        IEnumerable<Dependency?> dependencies = Dependency.ReadAll();
        if(dependencies.Count() != 0)
        {
            foreach (var dependency in dependencies)
                Dependency.Delete(dependency.Id);
        }
        IEnumerable<DO.Engineer?> engineers = Engineer.ReadAll();
        if (dependencies.Count() != 0)
        {
            foreach (var engineer in engineers)
                Engineer.Delete(engineer.Id);
        }
    }
}
