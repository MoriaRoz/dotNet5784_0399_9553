namespace BlImplementation;
using BlApi;

/// <summary>
/// Implementation of the business logic interface.
/// </summary>
internal class Bl : IBl
{
    /// <summary>
    /// Gets the instance of the business logic for managing engineers.
    /// </summary>
    public IEngineer Student => new EngineerImplementation();

    /// <summary>
    /// Gets the instance of the business logic for managing tasks.
    /// </summary>
    public ITask Course => new TaskImplementation();
}
