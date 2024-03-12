namespace Dal;
using DalApi;
using DO;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Linq;

sealed public class DalXml : IDal
{ 
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public IUser User => new UserImplementation();
    public void Reset()
    {
        try
        {
            IEnumerable<DO.Task?> tasks = Task.ReadAll();
            if (tasks.Count() != 0)
            {
                foreach (var task in tasks)
                {
                    if(task.ScheduledDate==null)
                        Task.Delete(task.Id);
                }
            }
            IEnumerable<Dependency?> dependencies = Dependency.ReadAll();
            if (dependencies.Count() != 0)
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
        catch (Exception ex) { Console.WriteLine(ex.Message); }

        XElement config = XMLTools.LoadListFromXMLElement("data-config");
        config.Element("NextTaskId")!.Value = "1";
        config.Element("NextDependencyId")!.Value = "1";
        config.Element("ProjectStartDate")!.Value = null;
        XMLTools.SaveListToXMLElement(config, "data-config");
    }
}
