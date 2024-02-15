using System.Collections;

namespace PL;
internal class LevelEngineerCollection : IEnumerable
{
    static readonly IEnumerable<BO.LevelEngineer> s_enums =
        (Enum.GetValues(typeof(BO.LevelEngineer)) as IEnumerable<BO.LevelEngineer>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

