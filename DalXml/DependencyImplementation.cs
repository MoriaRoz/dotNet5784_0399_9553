
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependency_xml = "dependencys";

    public int Create(Dependency entity)
    {
       List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);
        int nextId = Config.NextDependencyId;//ID for the new dependency.
        //Creating a new dependency with the new ID and values from 'item':
        Dependency dependencyNew = entity with { Id = nextId };
        dependencies.Add(dependencyNew);//Adding the new dependency to the list.
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
        return nextId;
    }

    public void Delete(int id)
    {
        throw new DalDeletionImpossible("Dependencies cannot be deleted");
    }

    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
        return dependencies.FirstOrDefault(Dependency => Dependency.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
        return dependencies.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);
        if (filter != null)
        {  // Apply the filter condition to the dependencies in the data source.
            return from item in dependencies
                   where filter(item)
                   select item;
        }
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
        return from item in dependencies
               select item;
    }

    public void Update(Dependency entity)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);
        if (dependencies.RemoveAll(it => it.Id == entity.Id) == 0)
            throw new DalDoesNotExistException($"Dependency with ID={entity.Id} dose not exist");
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
    }
}