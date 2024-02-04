
namespace BlApi;
/// <summary>
/// 
/// </summary>
public interface IEngineer
{
    public int Create(BO.Engineer engineer);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.Engineer?> ReadAll();
    public void Update(BO.Engineer engineer);
    public void Delete(int id);
    public BO.TaskInEngineer GetDetailedTaskForEngineer(int engId,int taskId);
}
