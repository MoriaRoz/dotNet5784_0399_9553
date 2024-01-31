namespace Dal;
using DalApi;
using System.Diagnostics;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public IEngineer Engineer => throw new NotImplementedException();
    public ITask Task => throw new NotImplementedException();
    public IDependency Dependency => throw new NotImplementedException();
}
