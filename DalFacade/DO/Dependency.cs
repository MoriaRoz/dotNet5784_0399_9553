using System.Threading.Tasks;

namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="PreviousTask"></param>
/// <param name="DependsOnTask"></param>
public record Dependency
(
    int Id,
    int? PreviousTask=null,
    int? DependsOnTask = null
)
{
    public Dependency() : this (0, null, null) { }
    public Dependency(int id,int previousTask,int dependsOnTask)
    {
        Id = id;
        PreviousTask = previousTask;
        DependsOnTask = dependsOnTask;
    }
}
