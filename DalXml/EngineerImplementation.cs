
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
        //engineerRoot.Save(s_engineers_xml);
        XMLTools.SaveListToXMLElement(engineerRoot, s_engineers_xml);
        return entity.Id;
    }

    //Deleted an engineer with id=id from an xml file
    public void Delete(int id)
    {
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement? delEngineer = engineerRoot.Elements("Engineer").FirstOrDefault(e => (int)e.Element("id") == id);

        if (delEngineer != null)
        {
            delEngineer.Remove();
            XMLTools.SaveListToXMLElement(engineerRoot, s_engineers_xml);
        }
    }

    //Returning an engineer with id=id from an xml file
    public Engineer? Read(int id)
    {
        //XElement? readEngineer = XMLTools.LoadListFromXMLElement(s_engineers_xml);.Elements().FirstOrDefault(eng => (int)eng.Element("Id") == id);
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement? readEngineer = null;
        if (engineerRoot.Elements().Any())
            readEngineer = engineerRoot.Elements().FirstOrDefault(e => e.ToIntNullable("id") == id);
        if(readEngineer == null)
            return null;

        LevelEngineer level = readEngineer.ToEnumNullable<LevelEngineer>("level") ?? LevelEngineer.Beginner;
        string email = readEngineer.Element("email").Value ?? "";
        double? cost = readEngineer.ToDoubleNullable("cost") ?? null;
        string name = readEngineer.Element("name").Value ?? "";
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
        if (Read(entity.Id) == null)
            throw new DalDoesNotExistException($"Engineer with ID={entity.Id} does Not exist");
        else
        {
            Delete(entity.Id); //Remove old entity
            Create(entity); //add updated entity
        }
    }

    //A helper method that accepts an object of type element and returns an object of type engineer
    static Engineer getEngineer(XElement e)
    {
        return new Engineer()
        {
            Id = e.ToIntNullable("id") ?? throw new FormatException("Can not convert id"),
            Level = e.ToEnumNullable<LevelEngineer>("level") ?? LevelEngineer.Beginner,
            Email = e.Element("email").Value ?? "",
            Cost = e.ToDoubleNullable("cost") ?? null,
            Name = e.Element("name").Value ?? "",
        };
    }

}