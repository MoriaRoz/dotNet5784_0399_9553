
namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    public IEngineer Student => new EngineerImplementation();

    public ITask Course => new TaskImplementation();
}
