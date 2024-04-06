
using System.Security;

namespace DO;

/// <summary>
/// Represents a user entity with all its attributes.
/// </summary>
/// <param name="EngineerId">The personal unique ID of the user</param>
/// <param name="Name">The user's first and last name</param>
/// <param name="Email">The email address of the user</param>
/// <param name="Level">The user's level</param>
/// <param name="Cost">The cost per hour of the user's work</param>
public record User
(
    int Id,
    String? Name = null,
    String? Password = null,
    UserRole Role = UserRole.Engineer
)
{
    //Empty constructor
    public User() : this(0, null,null, UserRole.Engineer) { }
}