
namespace BO;
/// A file of exception types
/// </summary>

internal class Exceptions
{


    [Serializable] //Exception No such object exists.
    public class BlDoesNotExistException : Exception
    {
        public BlDoesNotExistException(string? message) : base(message) { }
        public BlDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
    }

    [Serializable] //Exception already exists such an object.
    public class BlDalAlreadyExistsException : Exception
    {
        public BlDalAlreadyExistsException(string? message) : base(message) { }
        public BlDalAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

    }

    [Serializable] //An exception cannot be deleted.
    public class BlDalDeletionImpossible : Exception
    {
        public BlDalDeletionImpossible(string? message) : base(message) { }
        public BlDalDeletionImpossible(string message, Exception innerException) : base(message, innerException) { }
    }
    [Serializable] //Exceeding the number outside the requested range.
    public class BlNumberOutOfRangeException : Exception
    {
        public BlNumberOutOfRangeException(string? message) : base(message) { }
        public BlNumberOutOfRangeException(string message, Exception innerException) : base(message, innerException) { }
    }
    [Serializable] //Exceeding the number outside the requested range.
    public class BlDalXMLFileLoadCreateException : Exception
    {
        public BlDalXMLFileLoadCreateException(string? message) : base(message) { }
        public BlDalXMLFileLoadCreateException(string message, Exception innerException) : base(message, innerException) { }

    }
    [Serializable]
    public class BlNullPropertyException : Exception
    {
        public BlNullPropertyException(string? message) : base(message) { }
    }
    [Serializable]
    public class BlInvalidValueException : Exception
    {
        public BlInvalidValueException(string? message) : base(message) { }
    }
    [Serializable]
    public class BlUnUpdatedTaskStartDate : Exception
    {
        public BlUnUpdatedTaskStartDate(string? message) : base(message) { }
    }
}