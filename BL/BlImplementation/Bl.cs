namespace BlImplementation;
using BlApi;
using BO;

/// <summary>
/// Implementation of the business logic interface.
/// </summary>
internal class Bl : IBl
{
    /// <summary>
    /// Gets the instance of the business logic for managing engineers.
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Gets the instance of the business logic for managing tasks.
    /// </summary>
    public ITask Task => new TaskImplementation();
//    public ProjectStatus GetProjectStatus()
//    {
//        return Dal.Config
//    }
//}
