namespace BlImplementation;
using BlApi;
using BO;
using System;

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

    public DateTime? GetProjectStartDate()
    {
        return DalApi.Factory.Get.Task.GetProjectStartDate();
    }

    public ProjectStatus GetProjectStatus()
    {
        return (BO.ProjectStatus)DalApi.Factory.Get.Task.GetProjectStatus();
    }

    public void SetProjectStartDate(DateTime startDate)
    {
        DalApi.Factory.Get.Task.SetProjectStartDate(startDate);
    }

    public void CreateSchedule(DateTime startDate)
    {
        //לממש לוז
    }
}
