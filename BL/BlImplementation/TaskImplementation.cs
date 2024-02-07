
//namespace BlImplementation;

//using BlApi;
//using BO;
//using System.Security.Cryptography;

//internal class TaskImplementation : ITask
//{
//    private DalApi.IDal _dal = DalApi.Factory.Get;
//    public int Create(BO.Task boTask)
//    {
//        if (boTask == null)
//            throw new BO.Exceptions.BlNullPropertyException("Task is null");
//        if (!TaskCheck(boTask))
//            throw new BO.Exceptions.BlInvalidValueException("An task with an invalid value was entered");

//        DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.RequiredEffortTime,
//            (DO.LevelEngineer)boTask.Complexity, boTask.StartDate, boTask.DeadlineDate, boTask.CompleteDate, 
//            boTask.Deliverables, boTask.Remarks, boTask.Engineer.Id);
        
//        try
//        {
//            int idTask = _dal.Task.Create(doTask);
//            AddDependencys(boTask.Dependencies, idTask);
//            return idTask;
//        }
//        catch(DO.DalAlreadyExistsException ex)
//        {
//            throw new BO.Exceptions.BlDalAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
//        }
//    }

//    public void Delete(int id)
//    {
//        BO.Task? boTask = Read(id);
//        if(boTask != null)
//        {
//            DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.RequiredEffortTime,
//            (DO.LevelEngineer)boTask.Complexity, boTask.StartDate, boTask.DeadlineDate, boTask.CompleteDate,
//            boTask.Deliverables, boTask.Remarks, boTask.Engineer.Id);
//            var dependenceis = _dal.Dependency.ReadAll();
//            var depend=(from d in dependenceis
//                        where d.DependsOnTask==id
//                        select d).FirstOrDefault();
//            if (depend != null)
//                throw new BO.Exceptions.BlDalDeletionImpossible($"There is a task that depends on the task with ID={doTask.Id} so it cannot be deleted.");
//            else
//            {
//                try
//                {
//                    _dal.Task.Delete(id);

//                }
//                catch (DO.DalDoesNotExistException ex)
//                {
//                    throw new BO.Exceptions.BlDoesNotExistException($"Task with ID={id} does not exist", ex);
//                }
//            }
//        }
//    }

//    public BO.Task? Read(int id)
//    {
//        throw new NotImplementedException();
//    }

//    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
//    {
//        throw new NotImplementedException();
//    }

//    public void StartDateUpdate(int id, DateTime start)
//    {
//        throw new NotImplementedException();
//    }

//    public void Update(BO.Task task)
//    {
//        throw new NotImplementedException();
//    }

//    bool TaskCheck(BO.Task boTask)
//    {
//        if (boTask.Id < 0)
//            return false;
//        if (boTask.Alias != "")
//            return false;
//        return true;
//    }
//    void AddDependencys(List<BO.TaskInList>? dependencyList, int taskId)
//    {
//        var dependencys = (from taskInList in dependencyList
//                           let dep = new DO.Dependency()
//                           {
//                               Id = 0,
//                               PreviousTask = taskInList.Id,
//                               DependsOnTask = taskId
//                           }
//                           select dep);

        
//    }
//}
