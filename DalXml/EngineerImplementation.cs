
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    // Add a new engineer to the xml file
    public int Create(Engineer entity)
    {
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement engineer = new XElement("Engineer",
            new XElement("id", entity.Id),
            new XElement("name", entity.Name),
            new XElement("email", entity.Email),
            new XElement("level", entity.Level),
            new XElement("cost", entity.Cost));

        engineerRoot.Add(engineer);
        engineerRoot.Save("Engineer");
        return entity.Id;
    }

    //Deleted an engineer with id=id from an xml file
    public void Delete(int id)
    {
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement delEngineer = engineerRoot.Elements("Engineer").FirstOrDefault(e => (int)e.Element("ID") == id);

        if (delEngineer != null)
        {
            delEngineer.Remove();
            engineerRoot.Save("Engineer.xml");
        }
    }

    //Returning an engineer with id=id from an xml file
    public Engineer? Read(int id)
    {
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement? readEngineer = engineerRoot.Elements("Engineer").FirstOrDefault(e => (int)e.Element("Id") == id);
        if(readEngineer == null)
            return null;

        LevelEngineer level = readEngineer.ToEnumNullable<LevelEngineer>("Level") ?? LevelEngineer.Beginner;
        string email = readEngineer.Element("Email").Value ?? "";
        double? cost = readEngineer.ToDoubleNullable("Cost") ?? null;
        string name = readEngineer.Element("Name").Value ?? "";
        return new Engineer(id,name,email,level,cost);

    }

    //Returning an engineer that meets the filter from an xml file
    public Engineer? Read(Func<Engineer, bool> filter)
    {
       return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e)).FirstOrDefault(filter);
    }

    //Returning all the engineers that meet the filter from the xml file
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e));
        else
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e)).Where(filter);
    }

    // updates existing Engineer
    public void Update(Engineer entity)
    {
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);

        XElement upEngineer = engineerRoot.Elements("Engineer").FirstOrDefault(e => (int)e.Element("Id") == entity.Id);
        if (upEngineer != null)
        {
            upEngineer.Remove();
            engineerRoot.Add(entity);
            engineerRoot.Save("Engineer");
        }
        else
            throw new DalDoesNotExistException($"Engineer with ID={entity.Id} does Not exist");
    }

    //A helper method that accepts an object of type elemnt and returns an object of type engineer
    static Engineer getEngineer(XElement e)
    {
        return new Engineer()
        {
            Id = e.ToIntNullable("Id") ?? throw new FormatException("Can not convert id"),
            Level = e.ToEnumNullable<LevelEngineer>("Level") ?? LevelEngineer.Beginner,
            Email = e.Element("Email").Value ?? "",
            Cost = e.ToDoubleNullable("Cost") ?? null,
            Name = e.Element("Name").Value ?? "",
        };
    }

}