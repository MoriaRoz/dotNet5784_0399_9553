
using BO;
using DalApi;

namespace BlImplementation;

using BlApi;
using BO;
using DO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
/// <summary>
/// 
/// </summary>
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    /// <exception cref="BO.Exceptions.BlNullPropertyException"></exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException"></exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException"></exception>
    public int Create(BO.Task boTask)
    {
        if (boTask == null)
            throw new BO.Exceptions.BlNullPropertyException("Task is null");
        if (!TaskCheck(boTask))
            throw new BO.Exceptions.BlInvalidValueException("An task with an invalid value was entered");

        DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.RequiredEffortTime,
            (DO.LevelEngineer)boTask.Complexity, boTask.StartDate, boTask.ScheduledDate, boTask.DeadlineDate, boTask.CompleteDate,
            boTask.Deliverables, boTask.Remarks, boTask.Engineer.Id);

        try
        {
            int idTask = _dal.Task.Create(doTask);
            if (boTask.Dependencies != null)
                AddDependencys(boTask.Dependencies, idTask);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions.BlDalAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
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
        BO.Task? boTask = Read(id);
        if (boTask != null)
        {
            DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.RequiredEffortTime,
            (DO.LevelEngineer)boTask.Complexity, boTask.StartDate,boTask.ScheduledDate, boTask.DeadlineDate, boTask.CompleteDate,
            boTask.Deliverables, boTask.Remarks, boTask.Engineer.Id);

            var dependenceis = _dal.Dependency.ReadAll();
            var depend = dependenceis.Where(d => d.DependsOnTask == id).Select(p => p).FirstOrDefault();

            if (depend != null)
                throw new BO.Exceptions.BlDalDeletionImpossible($"There is a task that depends on the task with ID={doTask.Id} so it cannot be deleted.");
            else
            {
                try
                {
                    _dal.Task.Delete(id);
                }
                catch (DO.DalDoesNotExistException ex)
                {
                    throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={id} does not exist", ex);
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
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
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = EngineerCalculation(doTask.EngineerId),
            Complexity = (BO.LevelEngineer)doTask.Complexity
        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="start"></param>
    /// <exception cref="BO.Exceptions.BlUnUpdatedTaskStartDate"></exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException"></exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
    public void StartDateUpdate(int id, DateTime start)
    {
       BO.Task? boTask=Read(id);
       if(boTask != null)
        {
            var startDates = boTask.Dependencies.Where(t => t.Status == BO.Statuses.Scheduled).ToList();
            if (startDates.Any())
                throw new BO.Exceptions.BlUnUpdatedTaskStartDate($"There is a task that the task with ID={id} depends on that has no prep start date");

            var depTasks = (from BO.TaskInList depTask in boTask.Dependencies
                            let task= _dal.Task.Read(depTask.Id)
                            select task);
            var endDates = depTasks.Where(t => start<t.DeadlineDate).ToList();
            if (endDates.Any())
                throw new BO.Exceptions.BlUnUpdatedTaskStartDate($"The given date is earlier than the deadline date of the task that the task with ID={id} depends on");
            DO.Task? doTask=new DO.Task
            {
                Id = boTask.Id,
                Description = boTask.Description,
                Alias = boTask.Alias,
                CreatedAtDate = boTask.CreatedAtDate,
                RequiredEffortTime = boTask.RequiredEffortTime,
                Complexity = (DO.LevelEngineer)boTask.Complexity,
                StartDate = start,
                ScheduledDate = boTask.ScheduledDate,
                DeadlineDate = boTask.DeadlineDate,
                CompleteDate = boTask.CompleteDate,
                Deliverables = boTask.Deliverables,
                Remarks = boTask.Remarks,
                EngineerId=id
            };
            try
            {
                _dal.Task.Update(doTask);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.Exceptions.BlDalAlreadyExistsException($"Task with ID={boTask!.Id} already exists", ex);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={boTask!.Id} does Not exist", ex);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="boTask"></param>
    /// <exception cref="BO.Exceptions.BlNullPropertyException"></exception>
    /// <exception cref="BO.Exceptions.BlInvalidValueException"></exception>
    /// <exception cref="BO.Exceptions.BlDoesNotExistException"></exception>
    /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException"></exception>
    public void Update(BO.Task boTask)
    {
        if (boTask == null)
            throw new BO.Exceptions.BlNullPropertyException("Task to update is null");
        if (!TaskCheck(boTask))
            throw new BO.Exceptions.BlInvalidValueException("An task with an invalid value was entered");

        DO.Task? doTask = _dal.Task.Read(boTask.Id);
        if (doTask == null)
            throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={boTask!.Id} does Not exist");
        
        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions.BlDalAlreadyExistsException($"Task with ID={boTask!.Id} already exists", ex);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={boTask!.Id} does Not exist", ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    bool TaskCheck(BO.Task boTask)
    {
        if (boTask.Id < 0)
            return false;
        if (boTask.Alias != "")
            return false;
        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dependencyList"></param>
    /// <param name="taskId"></param>
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
    Statuses StatusCalculation(DO.Task doTask)
    {
        if(doTask.ScheduledDate!=null)
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
                    Id=prevTask.Id,
                    Description=prevTask.Description,
                    Alias=prevTask.Alias,
                    Status=StatusCalculation(prevTask)
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
        if(dTask.ScheduledDate!=null&&dTask.StartDate==null)
            forecst = dTask.ScheduledDate+dTask.RequiredEffortTime;
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
                    Name=doEng.Name
                }).FirstOrDefault();
    }
}