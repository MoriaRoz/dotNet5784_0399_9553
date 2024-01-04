namespace DO;

/// <summary>
/// An engineer entity represents an engineer with all its accessories.
/// </summary>
/// <param name="Id">Personal unique ID of the engineer</param>
/// <param name="Name">Engineer's first and last name</param>
/// <param name="Email">Email address of the engineer</param>
/// <param name="Level">Engineer level</param>
/// <param name="Cost">Cost per hour of engineer work</param>
public record Engineer
(
    string Id,
    string? Name=null,
    string? Email=null,
    LevelEngineer? Level=null,
    double? Cost = null
)
{
    public Engineer() : this ("","","",null,null) { }
    public Engineer(string id,string name,string email,LevelEngineer level,double cost)
    {
        Id = id;
        Name = name;
        Email = email;
        Level = level;  
        Cost = cost;
    }
}
