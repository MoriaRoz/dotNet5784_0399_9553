
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id) != null)
            throw new Exception($"Engineer with ID={item.Id} already exist");
         
        DataSource.Engineers.Add(item);
         return item.Id;
    }

    public void Delete(int id)
    {
        Engineer toDel = DataSource.Engineers.Find(Engineer => Engineer.Id == id;
        if (toDel == null)
            throw new Exception($"Engineer with ID={id} dose not exist");
        if (DataSource.Task.Find(Task => Task.EngineerId == id) != null)
            throw new Exception($"Engineer with ID={id} already working on tasks");
        Engineers.Remove(toDel);
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(Engineer => Engineer.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        Engineer? oldEngineer = DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id);
        if ( oldEngineer == null)
            throw new Exception($"Engineer with ID={item.Id} dose not exist");
        DataSource.Engineers.Remove(oldEngineer);
        DataSource.Engineers.Add(item);
    }
}
