
namespace BlApi;
/// <summary>
/// 
/// </summary>
public interface ITask
{
    public int Create(BO.Task task);
    public BO.Task? Read(int id);
    public IEnumerable<BO.Task?> ReadAll();
    public void Update(BO.Task task);
    public void Delete(int id);
    public BO.EngineerInTask GetDetailedEngineerForTask(int taskId, int engId);
    public void StartDateUpdate(int id, DateTime start);
}
