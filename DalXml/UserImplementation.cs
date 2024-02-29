
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security;
using System.Xml.Linq;

internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";

    // Add a new engineer to the xml file
    public int Create(User entity)
    {
        XElement userRoot = XMLTools.LoadListFromXMLElement(s_users_xml);
        XElement user = new XElement("User",
            new XElement("EngineerId", entity.EngineerId),
            new XElement("Password", entity.Password),
            new XElement("Rool", entity.Rool));
        userRoot.Add(user);
        //engineerRoot.Save(s_engineers_xml);
        XMLTools.SaveListToXMLElement(userRoot, s_users_xml);
        return entity.EngineerId;
    }

    //Deleted an engineer with id=id from an xml file
    public void Delete(int id)
    {
        XElement userRoot = XMLTools.LoadListFromXMLElement(s_users_xml);
        XElement? delUser = userRoot.Elements("User").FirstOrDefault(e => (int)e.Element("EngineerId") == id);

        if (delUser != null)
        {
            delUser.Remove();
            XMLTools.SaveListToXMLElement(userRoot, s_users_xml);
        }
    }

    //Returning an engineer with id=id from an xml file
    public User? Read(int id)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return users.FirstOrDefault(t => t.EngineerId == id);
    }

    //Returning an engineer that meets the filter from an xml file
    public User? Read(Func<User, bool> filter)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return users.FirstOrDefault(filter);
    }

    //Returning all the engineers that meet the filter from the xml file
    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null)
    {
        List<User> user = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml); //deserialize
        if (filter == null)
            return user.Select(user => user);
        return user.Select(user => user).Where(filter);
    }

    // updates existing Engineer
    public void Update(User entity)
    {
        if (Read(entity.EngineerId) == null)
            throw new DalDoesNotExistException($"User with ID={entity.EngineerId} does Not exist");
        else
        {
            Delete(entity.EngineerId); //Remove old entity
            Create(entity); //add updated entity
        }
    }

    //A helper method that accepts an object of type element and returns an object of type engineer
    static User getUser(XElement e)
    {
        string passwordValue = e.Element("Password")?.Value ?? ""; // Retrieve the value of the "Password" element
        SecureString passwordSecure = new SecureString();

        foreach (char c in passwordValue)
        {
            passwordSecure.AppendChar(c); // Populate the SecureString character by character
        }

        return new User()
        {
            EngineerId = e.ToIntNullable("EngineerId") ?? throw new FormatException("Can not convert id"),
            Password = passwordSecure,
            Rool = e.ToEnumNullable<UserRole>("Rool") ?? UserRole.Engineer,
        };
    }
}

