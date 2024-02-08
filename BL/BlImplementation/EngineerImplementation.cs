
namespace BlImplementation;

using BlApi;
using BO;
using DalApi;
using DO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
/// <summary>
/// 
/// </summary>
internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private BlImplementation.TaskImplementation taskImplementation = new BlImplementation.TaskImplementation();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="boEng"></param>
    /// <returns></returns>
    /// <exception cref="BO.Exceptions.BlNullPropertyException"></exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException"></exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException"></exception>
    public int Create(BO.Engineer boEng)
    {
        if (boEng == null)
            throw new BO.Exceptions.BlNullPropertyException("Enigneer is null");
        if (!EngineerCheck(boEng))
            throw new BO.Exceptions.BlInvalidValueException("An engineer with an invalid value was entered");
        DO.Engineer doEng = new DO.Engineer
    (boEng.Id, boEng.Name, boEng.Email, (DO.LevelEngineer)boEng.Level, boEng.Cost);
        try
        {
            int idEng = _dal.Engineer.Create(doEng);
            return idEng;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions.BlDalAlreadyExistsException($"Enigneer with ID={boEng.Id} already exists", ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.Exceptions.BlDalDeletionImpossible"></exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
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
                        throw new BO.Exceptions.BlDalDeletionImpossible("Can not delete engineer that is currently working on a task");
            }
            try
            {
                _dal.Engineer.Delete(id);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.Exceptions.BlDoesNotExistException($"Engineer with ID={id} does not exist", ex);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEng = _dal.Engineer.Read(id);
        if (doEng == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

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
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <param name="boEng"></param>
    /// <exception cref="BO.Exceptions.BlNullPropertyException"></exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException"></exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException"></exception>
    public void Update(BO.Engineer boEng)
    {
        if (boEng == null)
            throw new BO.Exceptions.BlNullPropertyException("Enigneer to update is null");
        if (!EngineerCheck(boEng))
            throw new BO.Exceptions.BlInvalidValueException("An engineer with an invalid value was entered");

        DO.Engineer? doEng = _dal.Engineer.Read(boEng.Id);
        if (doEng == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with ID={boEng!.Id} does Not exist");
        if (doEng.Level > (DO.LevelEngineer)boEng.Level)
            throw new BO.Exceptions.BlInvalidValueException("You cannot lower the level of the engineer");
        try
        {
            _dal.Engineer.Update(doEng);
            AssignmentTaskToEngineer(boEng.Task, boEng.Id, boEng.Name);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions.BlDalAlreadyExistsException($"Engineer with ID={boEng!.Id} already exists", ex);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with ID={boEng!.Id} does Not exist", ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eng"></param>
    /// <returns></returns>
    bool EngineerCheck(BO.Engineer eng)
    {
        if (eng.Id < 0)
            return false;
        if (eng.Name == "")
            return false;
        if (!eng.Email.Contains('@') || eng.Email.Contains(' '))
            return false;
        if (eng.Cost < 0)
            return false;
        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="engId"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <param name="taskE"></param>
    /// <param name="engId"></param>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException"></exception>
    void AssignmentTaskToEngineer(TaskInEngineer taskE, int engId, string engNa)
    {
        DO.Task? doTask = _dal.Task.Read(taskE.Id);
        if (doTask == null)
            throw new BO.Exceptions.BlDoesNotExistException("");
        if (doTask.EngineerId != engId || doTask.EngineerId != null)
            throw new BO.Exceptions.BlInvalidValueException("");
        BO.Task task = taskImplementation.Read(doTask.Id);
        if (task.Dependencies != null)
        {
            var tIL = task.Dependencies.Where(taskDep => taskDep.Status != BO.Statuses.Done).FirstOrDefault();

            if (tIL != null)
                throw new BO.Exceptions.BlInvalidValueException($"The task with ID={taskE.Id} depends on the unfinished tasks");
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
            DeadlineDate = task.StartDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Engineer = engineerInTask
        });
    }
}