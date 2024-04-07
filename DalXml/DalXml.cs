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
    public IDependency Dependencies => new DependencyImplementation();
    public IUser User => new UserImplementation();
    public void Reset()
    {
        try
        {
            XElement config = XMLTools.LoadListFromXMLElement("data-config");
            config.Element("ProjectStartDate")!.Value = "";
            config.Element("NextTaskId")!.Value = "1";
            config.Element("NextDependencyId")!.Value = "1";
            XMLTools.SaveListToXMLElement(config, "data-config");

            IEnumerable<DO.Task?> tasks = Task.ReadAll();
            if (tasks.Count() != 0)
            {
                foreach (var task in tasks)
                    Task.Delete(task.Id);
            }
            IEnumerable<Dependency?> dependencies = Dependencies.ReadAll();
            if (dependencies.Count() != 0)
            {
                foreach (var Dependency in dependencies)
                    Dependencies.Delete(Dependency.Id);
            }
            IEnumerable<DO.Engineer?> engineers = Engineer.ReadAll();
            if (dependencies.Count() != 0)
            {
                foreach (var engineer in engineers)
                    Engineer.Delete(engineer.Id);
            }
            IEnumerable<DO.User?> users = User.ReadAll();
            if (users.Count() != 0)
            {
                foreach (var user in users)
                    User.Delete(user.Id);
            }
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
    }
    public void ResetIds()
    {
        XElement config = XMLTools.LoadListFromXMLElement("data-config");
        config.Element("NextTaskId")!.Value = "1";
        config.Element("NextDependencyId")!.Value = "1";
        XMLTools.SaveListToXMLElement(config, "data-config");
    }
}
