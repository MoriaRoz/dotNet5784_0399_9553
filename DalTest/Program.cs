using Dal;
using DalApi;

namespace DalTest
{
    public enum Entitys { Exit=0, Engineer, Task, Dependence};
    public enum Actions { Create = 0, Read, ReadAll), Update, Delete };
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        static void Main(string[] args)
        {
            try
            {
                do
                {
                    Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
                    Console.WriteLine("Select entity you want to check-\r\n" +
                        "0- Exit\r\n" +
                        "1- Engineer\r\n" +
                        "2- Task\r\n" +
                        "3- Dependence");
                    int entity = int.Parse(Console.ReadLine());
                    if (entity != 0)
                    {
                        Console.WriteLine("Select the method you want to perform-\r\n" +
                   "0- Adding a new object of the entity type to the list (Create)\r\n" +
                   "1- Object display by ID (Read)\\r\\n\" +
                   "2- Display the list of all objects of the entity type (ReadAll)\\r\\n\" +
                   "3- Update existing object data (Update)\\r\\n\" +
                   "4- Deleting an existing object from a list. (delete)\\r\\n\" +
                       "Please note: this option is only available for some entities");
                        int action = int.Parse(Console.ReadLine());
                    }
                    switch (entity)
                    {
                        case Entitys.Exit: //exit from main menu
                            break;
                        case Entitys.Engineer:
                            switch (action)
                            {
                                case Actions.Create:
                                    break;
                                 case Actions.Read:
                                    break;
                                case Actions.ReadAll:
                                    break;
                                case Actions.Update:
                                    break;
                                case Actions.Delete:
                                    break;
                                default:
                                    throw new Exception("Incorrect input - the choice must be in numbers between 0-4")
                                    break;
                            }
                            break;
                        case Entitys.Task:
                            switch (action)
                            {
                                case Actions.Create:
                                    break;
                                case Actions.Read:
                                    break;
                                case Actions.ReadAll:
                                    break;
                                case Actions.Update:
                                    break;
                                case Actions.Delete:
                                    break;
                                default:
                                    throw new Exception("Incorrect input - the choice must be in numbers between 0-4")
                                    break;
                            }
                            break;
                        case Entitys.Dependence:
                            switch (action)
                            {
                                case Actions.Create:
                                    break;
                                case Actions.Read:
                                    break;
                                case Actions.ReadAll:
                                    break;
                                case Actions.Update:
                                    break;
                                case Actions.Delete:
                                    break;
                                default:
                                    throw new Exception("Incorrect input - the choice must be in numbers between 0-4")
                                    break;
                            }
                            break;
                        default: //In case a number is chosen that is not among the available options
                            throw new Exception("Incorrect input - the choice must be in numbers between 0-4")
                    }
                }
                while (entity!=0)


               
            }
            }
           catch (Exception ex) { }
        }
    }

}
