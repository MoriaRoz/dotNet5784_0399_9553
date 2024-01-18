
namespace DO;

/// <summary>
/// A file of exception types
/// </summary>

[Serializable] //Exception No such object exists.
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

[Serializable] //Exception already exists such an object.
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable] //An exception cannot be deleted.
public class DalDeletionImpossible : Exception
{
    public DalDeletionImpossible(string? message) : base(message) { }
}

[Serializable] //Exceeding the number outside the requested range.
public class NumberOutOfRangeException : Exception
{
    public NumberOutOfRangeException(string? message) : base(message) { }
}
