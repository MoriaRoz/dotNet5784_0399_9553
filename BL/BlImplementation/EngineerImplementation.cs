
namespace BlImplementation;

using BlApi;
using BO;
using DalApi;
using DO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
/// <summary>
/// Implementation of engineers.
/// </summary>
internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private BlImplementation.TaskImplementation taskImplementation = new BlImplementation.TaskImplementation();
    /// <summary>
    /// Creates a new engineer in the system.
    /// </summary>
    /// <param name="boEng">The engineer object to create.</param>
    /// <returns>The ID of the newly created engineer.</returns>
    /// <exception cref="BO.Exceptions.BlNullPropertyException">Thrown when the engineer object is null.</exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException">Thrown when the engineer object contains invalid values.</exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException">Thrown when trying to create an engineer that already exists in the system.</exception>
    public int Create(BO.Engineer boEng)
    {
        if (boEng == null)
            throw new BO.BlNullPropertyException("Enigneer is null");
        if (!EngineerCheck(boEng))
            throw new BO.BlInvalidValueException("An engineer with an invalid value was entered");
        DO.Engineer doEng = new DO.Engineer
    (boEng.Id, boEng.Name, boEng.Email, (DO.LevelEngineer)boEng.Level, boEng.Cost);
        try
        {
            int idEng = _dal.Engineer.Create(doEng);
            return idEng;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDalAlreadyExistsException($"Enigneer with ID={boEng.Id} already exists", ex);
        }
    }
    /// <summary>
    /// Deletes an engineer from the system.
    /// </summary>
    /// <param name="id">The ID of the engineer to delete.</param>
    /// <exception cref="BO.Exceptions.BlDalDeletionImpossible">Thrown when deletion of the engineer is not possible due to ongoing tasks.</exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the engineer with the specified ID does not exist.</exception>
    public void Delete(int id)
    {
        BO.Engineer? boEng = Read(id);
        if (boEng != null)
        {
            if (boEng.Task != null)
            {
                BO.Task? engTask = taskImplementation.Read(boEng.Task.Id);
                if (engTask != null)
                    if (engTask.Status == BO.Statuses.Started || engTask.Status == BO.Statuses.Done)
                        throw new BO.BlDalDeletionImpossible("Can not delete engineer that is currently working on a task");
            }
            try
            {
                _dal.Engineer.Delete(id);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exist", ex);
            }
        }
    }
    /// <summary>
    /// Reads an engineer from the system based on the specified ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to read.</param>
    /// <returns>The engineer object if found, otherwise null.</returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the engineer with the specified ID does not exist.</exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEng = _dal.Engineer.Read(id);
        if (doEng == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Name = doEng.Name,
            Email = doEng.Email,
            Level = (BO.LevelEngineer)doEng.Level,
            Cost = doEng.Cost,
            Task = FindTaskForEngineer(id)
        };
    }
    /// <summary>
    /// Reads all engineers from the system based on an optional filter.
    /// </summary>
    /// <param name="filter">An optional filter predicate to apply on the engineers.</param>
    /// <returns>An enumerable collection of engineers.</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter)
    {
        if (filter != null)
            return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                    let boEng = Read(doEngineer.Id)
                    where filter(boEng)
                    select boEng);

        return (
            from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
            let boEng = Read(doEngineer.Id)
            select boEng);
    }
    /// <summary>
    /// Updates an existing engineer in the system.
    /// </summary>
    /// <param name="boEng">The engineer object containing updated information.</param>
    /// <exception cref="BO.Exceptions.BlNullPropertyException">Thrown when the engineer object is null.</exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException">Thrown when the engineer object contains invalid values.</exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the engineer to update does not exist.</exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException">Thrown when trying to update an engineer that already exists in the system.</exception>
    public void Update(BO.Engineer boEng)
    {
        if (boEng == null)
            throw new BO.BlNullPropertyException("Enigneer to update is null");
        if (!EngineerCheck(boEng))
            throw new BO.BlInvalidValueException("An engineer with an invalid value was entered");

        DO.Engineer? doEng = _dal.Engineer.Read(boEng.Id);
        if (doEng == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={boEng!.Id} does Not exist");
        if (doEng.Level > (DO.LevelEngineer)boEng.Level)
            throw new BO.BlInvalidValueException("You cannot lower the level of the engineer");
        try
        {
            _dal.Engineer.Update(doEng);
            AssignmentTaskToEngineer(boEng.Task,boEng.Id,boEng.Name);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDalAlreadyExistsException($"Engineer with ID={boEng!.Id} already exists", ex);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={boEng!.Id} does Not exist", ex);
        }
    }
    /// <summary>
    /// Checks if the provided engineer object contains valid data.
    /// </summary>
    /// <param name="eng">The engineer object to validate.</param>
    /// <returns>True if the engineer object is valid, otherwise false.</returns>
    bool EngineerCheck(BO.Engineer eng)
    {
        if (eng.Id < 0)
            return false;
        if(eng.Name!=null)
            if (eng.Name == "")
                return false;
        if(eng.Email!=null)
            if (!eng.Email.Contains('@') || eng.Email.Contains(' '))
                return false;
        if (eng.Cost != null)
            if (eng.Cost < 0)
                return false;
        return true;
    }
    /// <summary>
    /// Finds the task associated with the specified engineer.
    /// </summary>
    /// <param name="engId">The ID of the engineer.</param>
    /// <returns>The task associated with the engineer if found, otherwise null.</returns>
    BO.TaskInEngineer? FindTaskForEngineer(int engId)
    {
        IEnumerable<DO.Task>? dTasks = _dal.Task.ReadAll(item => item.EngineerId == engId);

        DO.Task? currentTask = (from DO.Task dTask in dTasks
                                where new TaskImplementation().Read(dTask.Id).Status == BO.Statuses.Started
                                select dTask).FirstOrDefault();

        if (currentTask != null)
            return new BO.TaskInEngineer()
            {
                Id = currentTask.Id,
                Alias = currentTask.Alias
            };
        return null;
    }
    /// <summary>
    /// Assigns a task to an engineer.
    /// </summary>
    /// <param name="taskE">The task to assign.</param>
    /// <param name="engId">The ID of the engineer to assign the task to.</param>
    /// <param name="engNa">The name of the engineer.</param>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the task or engineer does not exist.</exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException">Thrown when the task cannot be assigned to the engineer due to dependencies or other constraints.</exception>
    void AssignmentTaskToEngineer(TaskInEngineer taskE, int engId, string engNa)
    {
        DO.Task? doTask = _dal.Task.Read(taskE.Id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException("");
        if (doTask.EngineerId != engId || doTask.EngineerId != null)
            throw new BO.BlInvalidValueException("");
        BO.Task task = taskImplementation.Read(doTask.Id);
        if (task.Dependencies != null)
        {
            var tIL = task.Dependencies.Where(taskDep => taskDep.Status != BO.Statuses.Done).FirstOrDefault();

            if (tIL != null)
                throw new BO.BlInvalidValueException($"The task with ID={taskE.Id} depends on the unfinished tasks");
        }

        _dal.Task.Update(new DO.Task()
        {
            Id = doTask.Id,
            Complexity = doTask.Complexity,
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            EngineerId = engId
        });
        EngineerInTask engineerInTask = (new EngineerInTask()
        {
            Name = engNa,
            Id = engId
        });

        taskImplementation.Update(new BO.Task()
        {
            Id = task.Id,
            Complexity = task.Complexity,
            Alias = task.Alias,
            Description = task.Description,
            CreatedAtDate = task.CreatedAtDate,
            Status = task.Status,
            Dependencies = task.Dependencies,
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            ScheduledDate = task.StartDate,
            ForecastDate = task.StartDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Engineer = engineerInTask
        });
    }
}