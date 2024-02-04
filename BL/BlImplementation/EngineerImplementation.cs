
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer boEngineer)
    {
        DO.Engineer doEngineer = new DO.Engineer
    (boEngineer.Id, boEngineer.Name, boEngineer.Email, (DO.LevelEngineer)boEngineer.Level,boEngineer.Cost);
        try
        {
            int idEng = _dal.Engineer.Create(doEngineer);
            return idEng;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={boEngineer.Id} already exists", ex);
        }
    }

    public void Create(BO.Engineer? engineer)
    {
        try
        {
            if (engineer == null)
                throw new BO.BlNullPropertyException("Enigneer is null");
            if (engineer.Id <= 0 || engineer.Name == "" || engineer.Cost <= 0 || !CheckEmail(engineer.Email!))
                throw new BO.BlInvalidValueException("Engineer with invalid values");

            //adding engineer using dal Create method
            _dal.Engineer.Create(new DO.Engineer(engineer.Id, (DO.EngineerExperience)engineer.Level, engineer.Email, engineer.Cost, engineer.Name));

        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={engineer!.Id} already exists", ex);
        }
    }


    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");


        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.LevelEngineer)doEngineer.Level,
            Cost = doEngineer.Cost
        };

    }

    public IEnumerable<Engineer?> ReadAll()
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                    Email = doEngineer.Email,
                    Level = (BO.LevelEngineer)doEngineer.Level,
                    Cost = doEngineer.Cost,
                    Task= new TaskInEngineer()
                };

    }

    public void Update(Engineer engineer)
    {
        throw new NotImplementedException();
    }
    public TaskInEngineer GetDetailedTaskForEngineer(int engId, int taskId)
    {
        throw new NotImplementedException();
    }
}
