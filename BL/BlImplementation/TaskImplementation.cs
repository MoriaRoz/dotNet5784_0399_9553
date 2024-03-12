
namespace BlImplementation;

using BlApi;
using BO;
using DO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
/// <summary>
/// Implementation of the task management functionality.
/// </summary>
internal class TaskImplementation : ITask
{


    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="boTask">The task object to create.</param>
    /// <returns>The ID of the newly created task.</returns>
    /// <exception cref="BO.Exceptions.BlNullPropertyException">Thrown when the task object is null.</exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException">Thrown when an invalid value is encountered in the task object.</exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException">Thrown when trying to create a task that already exists.</exception>    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException"></exception>
    public int Create(BO.Task boTask)
    {
        if (boTask == null)
            throw new BO.BlNullPropertyException("Task is null");
        if (!TaskCheck(boTask))
            throw new BO.BlInvalidValueException("An task with an invalid value was entered");

        int? engId = null;
        if (boTask.Engineer != null)
            engId = boTask.Engineer.Id;
        DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description,
            boTask.CreatedAtDate, boTask.RequiredEffortTime,
            (DO.LevelEngineer)boTask.Complexity, boTask.StartDate,
            boTask.ScheduledDate, boTask.CompleteDate,
            boTask.Deliverables, boTask.Remarks, engId);

        try
        {
            int idTask = _dal.Task.Create(doTask);
            if (boTask.Dependencies != null)
                AddDependencys(boTask.Dependencies, idTask);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDalAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }

    /// <summary>
    /// Deletes a task with the given ID.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    /// <exception cref="BO.Exceptions.BlDalDeletionImpossible">Thrown when the task cannot be deleted due to dependencies.</exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when trying to delete a non-existing task.</exception>
    public void Delete(int id)
    {
        if (_dal.Task.GetProjectStatus() == 0)
        {
            BO.Task? boTask = Read(id);
            if (boTask != null)
            {
                int? engId = null;
                if (boTask.Engineer != null)
                    engId = boTask.Engineer.Id;
                DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.RequiredEffortTime,
                (DO.LevelEngineer)boTask.Complexity, boTask.StartDate, boTask.ScheduledDate, boTask.CompleteDate,
                boTask.Deliverables, boTask.Remarks, engId);

                var dependenceis = _dal.Dependency.ReadAll();
                var depend = dependenceis.Where(d => d.DependsOnTask == id).Select(p => p).FirstOrDefault();

                if (depend != null)
                    throw new BO.BlDalDeletionImpossible($"There is a task that depends on the task with ID={doTask.Id} so it cannot be deleted.");
                else
                {
                    try
                    {
                        _dal.Task.Delete(id);
                    }
                    catch (DO.DalDoesNotExistException ex)
                    {
                        throw new BO.BlDoesNotExistException($"Task with ID={id} does not exist", ex);
                    }
                }
            }
        }
        else
            throw new BO.BlTheScheduleIsSet("The schedule is set, so it is not possible to delete a task");
    }
    /// <summary>
    /// Reads a task with the given ID.
    /// </summary>
    /// <param name="id">The ID of the task to read.</param>
    /// <returns>The task object if found; otherwise, null.</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the task with the given ID does not exist.</exception>
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = StatusCalculation(doTask),
            Dependencies = DependenciesCalculation(doTask.Id),
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask?.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            ForecastDate = ForecastDateCalculation(doTask),
            //DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = EngineerCalculation(doTask.EngineerId),
            Complexity = (BO.LevelEngineer)doTask.Complexity
        };
    }
    /// <summary>
    /// Reads all tasks.
    /// </summary>
    /// <param name="filter">Optional filter function to apply.</param>
    /// <returns>An enumerable collection of task objects.</returns>
    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter != null)
            return (from DO.Task doTask in _dal.Task.ReadAll()
                    let boTask = Read(doTask.Id)
                    where filter(boTask)
                    select new BO.TaskInList
                    {
                        Id = boTask.Id,
                        Description = boTask.Description,
                        Alias = boTask.Alias,
                        Status = boTask.Status,
                    });

        return (
            from DO.Task doTask in _dal.Task.ReadAll()
            let boTask = Read(doTask.Id)
            select new BO.TaskInList
            {
                Id = boTask.Id,
                Description = boTask.Description,
                Alias = boTask.Alias,
                Status = boTask.Status,
            });
    }
    /// <summary>
    /// Updates the start date of a task with the given ID.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="start">The new start date for the task.</param>
    /// <exception cref="BO.Exceptions.BlUnUpdatedTaskStartDate">Thrown when the start date update violates task dependencies or deadlines.</exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException">Thrown when trying to update a task that already exists.</exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when trying to update a non-existing task.</exception>
    public void StartDateUpdate(int id, DateTime start)
    {
        BO.Task? boTask = Read(id);
        if (boTask != null)
        {
            var startDates = boTask.Dependencies.Where(t => t.Status == BO.Statuses.Scheduled).ToList();
            if (startDates.Any())
                throw new BO.BlUnUpdatedTaskStartDate($"There is a task that the task with ID={id} depends on that has no prep start date");

            var depTasks = (from BO.TaskInList depTask in boTask.Dependencies
                            let task = _dal.Task.Read(depTask.Id)
                            select task);
            var endDates = depTasks.Where(t => start < (t.StartDate+t.RequiredEffortTime)).ToList();
            if (endDates.Any())
                throw new BO.BlUnUpdatedTaskStartDate($"The given date is earlier than the deadline date of the task that the task with ID={id} depends on");
            DO.Task? doTask = new DO.Task
            {
                Id = boTask.Id,
                Description = boTask.Description,
                Alias = boTask.Alias,
                CreatedAtDate = boTask.CreatedAtDate,
                RequiredEffortTime = boTask.RequiredEffortTime,
                Complexity = (DO.LevelEngineer)boTask.Complexity,
                StartDate = start,
                ScheduledDate = boTask.ScheduledDate,
                //DeadlineDate = boTask.DeadlineDate,
                CompleteDate = boTask.CompleteDate,
                Deliverables = boTask.Deliverables,
                Remarks = boTask.Remarks,
                EngineerId = id
            };
            try
            {
                _dal.Task.Update(doTask);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlDalAlreadyExistsException($"Task with ID={boTask!.Id} already exists", ex);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={boTask!.Id} does Not exist", ex);
            }
        }
    }
    /// <summary>
    /// Updates a task.
    /// </summary>
    /// <param name="boTask">The task object to update.</param>
    /// <exception cref="BO.Exceptions.BlNullPropertyException">Thrown when the task object to update is null.</exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException">Thrown when an invalid value is encountered in the task object.</exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the task to update does not exist.</exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException">Thrown when trying to update a task that already exists.</exception>
    public void Update(BO.Task boTask)
    {
        if (boTask == null)
            throw new BO.BlNullPropertyException("Task to update is null");
        if (!TaskCheck(boTask))
            throw new BO.BlInvalidValueException("An task with an invalid value was entered");

        DO.Task? doTask = _dal.Task.Read(boTask.Id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={boTask!.Id} does Not exist");
        int? engId = null;
        if (boTask.Engineer != null)
            engId = boTask.Engineer.Id;
        try
        {
            _dal.Task.Update(new DO.Task()
            {
                Id = boTask.Id,
                Alias = boTask.Alias,
                Description = boTask.Description,
                CreatedAtDate = boTask.CreatedAtDate,
                RequiredEffortTime = boTask.RequiredEffortTime,
                Complexity = (DO.LevelEngineer)boTask.Complexity,
                StartDate = boTask.StartDate,
                ScheduledDate = boTask.ScheduledDate,
                CompleteDate = boTask.CompleteDate,
                Deliverables = boTask.Deliverables,
                Remarks = boTask.Remarks,
                EngineerId = engId,
            });
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDalAlreadyExistsException($"Task with ID={boTask!.Id} already exists", ex);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask!.Id} does Not exist", ex);
        }
    }
    // Private methods...

    /// <summary>
    /// Checks if a task object is valid.
    /// </summary>
    /// <param name="boTask">The task object to check.</param>
    /// <returns>True if the task is valid; otherwise, false.</returns>
    bool TaskCheck(BO.Task boTask)
    {
        if (boTask.Alias == "")
            return false;
        return true;
    }
    /// <summary>
    /// Adds dependencies to a task.
    /// </summary>
    /// <param name="dependencyList">The list of dependencies to add.</param>
    /// <param name="taskId">The ID of the task to which dependencies will be added.</param>
    void AddDependencys(List<BO.TaskInList>? dependencyList, int taskId)
    {
        var dependencys = (from taskInList in dependencyList
                           let dep = new DO.Dependency()
                           {
                               Id = 0,
                               PreviousTask = taskInList.Id,
                               DependsOnTask = taskId
                           }
                           select new
                           {
                               a = _dal.Dependency.Create(dep)
                           });
    }
    /// <summary>
    /// Calculates the status of a task based on its properties.
    /// </summary>
    /// <param name="doTask">The task object to calculate the status for.</param>
    /// <returns>The status of the task.</returns>
    Statuses StatusCalculation(DO.Task doTask)
    {
        if (doTask.ScheduledDate != null)
        {
            if (doTask.StartDate == null)
                return BO.Statuses.Scheduled;
            if (doTask.CompleteDate == null)
                return BO.Statuses.Started;
            else
                return BO.Statuses.Done;
        }
        return BO.Statuses.Unscheduled;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idTask"></param>
    /// <returns></returns>
    List<BO.TaskInList>? DependenciesCalculation(int idTask)
    {
        return (from DO.Dependency dep in _dal.Dependency.ReadAll()
                where dep.DependsOnTask == idTask
                let prevTask = _dal.Task.Read((int)dep.PreviousTask)
                select new BO.TaskInList()
                {
                    Id = prevTask.Id,
                    Description = prevTask.Description,
                    Alias = prevTask.Alias,
                    Status = StatusCalculation(prevTask)
                }).ToList();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dTask"></param>
    /// <returns></returns>
    DateTime? ForecastDateCalculation(DO.Task dTask)
    {
        DateTime? forecst = null;
        if (dTask.ScheduledDate != null && dTask.StartDate == null)
            forecst = dTask.ScheduledDate + dTask.RequiredEffortTime;
        if (dTask.ScheduledDate != null && dTask.StartDate != null)
            forecst = (dTask.ScheduledDate < dTask.StartDate ? dTask.StartDate : dTask.ScheduledDate) + dTask.RequiredEffortTime;
        return forecst;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="engId"></param>
    /// <returns></returns>
    BO.EngineerInTask? EngineerCalculation(int? engId)
    {
        if (engId == null)
            return null;

        return (from DO.Engineer doEng in _dal.Engineer.ReadAll()
                where doEng.Id == engId
                select new BO.EngineerInTask
                {
                    Id = doEng.Id,
                    Name = doEng.Name
                }).FirstOrDefault();
    }
}
