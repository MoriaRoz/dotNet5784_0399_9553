using System.Collections;

namespace PL;
internal class LevelEngineerCollection : IEnumerable
{
    static readonly IEnumerable<BO.LevelEngineer> s_enum =
        (Enum.GetValues(typeof(BO.LevelEngineer)) as IEnumerable<BO.LevelEngineer>)!;

    public IEnumerator GetEnumerator() => s_enum.GetEnumerator();
}
internal class StatusesCollection : IEnumerable
{
    static readonly IEnumerable<BO.Statuses> s_enumStats =
        (Enum.GetValues(typeof(BO.Statuses)) as IEnumerable<BO.Statuses>)!;

    public IEnumerator GetEnumerator() => s_enumStats.GetEnumerator();
}
internal class StatusProjectCollection : IEnumerable
{
    static readonly IEnumerable<BO.ProjectStatus> s_enumStats =
        (Enum.GetValues(typeof(BO.ProjectStatus)) as IEnumerable<BO.ProjectStatus>)!;

    public IEnumerator GetEnumerator() => s_enumStats.GetEnumerator();
}

internal class RoleCollection : IEnumerable
{
    static readonly IEnumerable<BO.UserRole> s_enumRole=
        (Enum.GetValues(typeof(BO.UserRole)) as IEnumerable<BO.UserRole>)!;

    public IEnumerator GetEnumerator()=>s_enumRole.GetEnumerator();
}