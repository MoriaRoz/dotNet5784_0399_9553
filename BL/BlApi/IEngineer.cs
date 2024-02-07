
namespace BlApi;
/// <summary>
/// 
/// </summary>
public interface IEngineer
{
    public int Create(BO.Engineer engineer);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);
    public void Update(BO.Engineer engineer);
    public void Delete(int id);
}
