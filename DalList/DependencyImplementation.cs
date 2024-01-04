
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int idNew = DataSource.Config.NextDependencyId;
        Dependency dependencyNew = new Dependency(idNew, item.PreviousTask,item.DependsOnTask);
        DataSource.Dependencys.Add(dependencyNew);
        return idNew;
    }

    public void Delete(int id)
    {
        throw new Exception("Dependencies cannot be deleted");
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencys.Find(Dependency=>Dependency.Id == id);
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }

    public void Update(Dependency item)
    {
        Dependency? oldDependency = DataSource.Dependencys.Find(Dependency => Dependency.Id == item.Id);
        if (oldDependency == null)
            throw new Exception($"Dependency with ID={item.Id} dose not exist");
        DataSource.Dependencys.Remove(oldDependency);
        DataSource.Dependencys.Add(item);
    }
}
