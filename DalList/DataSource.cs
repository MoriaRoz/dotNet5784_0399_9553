
namespace Dal;

/// <summary>
/// Database of the data layer.
/// a list for each entity that will contain a collection of objects of this type.
///Config class definition.
/// </summary>
internal static class DataSource
{
    internal static class Config //class that creates automatic running numbers.
    {
        public static DateTime? ProjectStartDate { get; set; } = null;
        internal const int startDependencyId = 0;//initial value of the running number of dependency.
        private static int nextDependencyId = startDependencyId;//The ID value of the last added dependency.
        internal static int NextDependencyId { get => nextDependencyId++; set => nextDependencyId = value; }//Advancing the running number by 1 for the new dependency.
        internal const int startTaskId = 0;//Initial value of the running number of a task.
        private static int nextTaskId = startTaskId;//The ID value of the last task added.
        internal static int NextTaskId { get => nextTaskId++; set => nextTaskId = value; }//Advance the running number by 1 for the new task.
    }
    internal static List<DO.Engineer> Engineers { get; } = new();//list of engineers.
    internal static List<DO.Dependency> Dependencys { get; } = new();//List of dependencies.
    internal static List<DO.Task> Tasks { get; } = new();//List of tasks.
    internal static List<DO.User> Users { get; } = new();//list of engineers.

}
