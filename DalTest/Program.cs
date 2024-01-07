using Dal;
using DalApi;
using DO;
using System;

namespace DalTest
{
    public enum Entitys { Exit = 0, Engineer, Task, Dependence };
    public enum Actions { Exit = 0,Create, Read, ReadAll, Update, Delete };
    public class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
                Entitys entity;

                do
                {
                    Console.WriteLine("Select entity you want to check-\n" +
                        "0- Exit\n" +
                        "1- Engineer\n" +
                        "2- Task\n" +
                        "3- Dependence");
                    entity = (Entitys)int.Parse(Console.ReadLine());

                    if (entity != 0)
                    {
                        Console.WriteLine("Select the method you want to perform-\n" +
                            "0- Exit main menu\n"+
                        "1- Adding a new object of the entity type to the list (Create)\n" +
                        "2- Object display by ID (Read)\n" +
                        "3- Display the list of all objects of the entity type (ReadAll)\n" +
                        "4- Update existing object data (Update)\n" +
                        "5- Deleting an existing object from a list. (delete)\n" +
                        "   Please note: this option is only available for some entities");
                        Actions action = (Actions)int.Parse(Console.ReadLine());

                        switch (entity)
                        {
                            case Entitys.Exit:
                                break;
                            case Entitys.Engineer:
                                MEngineer(action);
                                break;
                            case Entitys.Task:
                                MTask(action);
                                break;
                            case Entitys.Dependence:
                                MDependency(action);
                                break;
                            default:
                                //In case a number is chosen that is not among the available options
                                throw new Exception("Incorrect input - the choice must be in numbers between 0-4");
                        }
                    }
                }
                while(entity!=Entitys.Exit);
            }
            catch (Exception ex) { }
        }

        static void MEngineer(Actions action)
        {
            DO.Engineer newE;
            switch (action) {
                case Actions.Exit:
                    break;
                case Actions.Create:
                    newE = GetEng();
                    s_dalEngineer.Create(newE);
                    break;
                case Actions.Read:
                    Console.WriteLine("enter id of Engineer:");
                    int id=int.Parse(Console.ReadLine());
                    newE = s_dalEngineer.Read(id);
                    Console.WriteLine(newE.ToString());
                    break;
                case Actions.ReadAll: 
                    break;
                case Actions.Update: 
                    break;
                case Actions.Delete: 
                    break;
                default:
                    throw new Exception("Incorrect input - the choice must be in numbers between 0-5");
                    }
        }
        static void MTask(Actions action)
        {

        }
        static void MDependency(Actions action)
        {

        }

        static DO.Engineer GetEng()
        {
            DO.Engineer engineer = null;
            return engineer;
        }
        static DO.Task GetTask()
        {
            DO.Task task = null;
            return task;
        }
        static DO.Dependency GetDep()
        {
            DO.Dependency dep = null;
            return dep;
        }
    }
}

























//            using Dal;
//using DalApi;
//using DO;
//using System;

//namespace DalTest
//{
//    public enum Entitys { Exit = 0, Engineer, Task, Dependence };
//    public enum Actions { Create = 0, Read, ReadAll, Update, Delete };
//    public class Program
//    {
//        private static IEngineer? s_dalEngineer = new EngineerImplementation();
//        private static ITask? s_dalTask = new TaskImplementation();
//        private static IDependency? s_dalDependency = new DependencyImplementation();
//        static void Main(string[] args)
//        { 
//            try
//            {
//                Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
//                Entitys? entity;

//                do
//                {
//                    Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
//                    Console.WriteLine("Select entity you want to check-\r\n" +
//                        "0- Exit\r\n" +
//                        "1- Engineer\r\n" +
//                        "2- Task\r\n" +
//                        "3- Dependence");
//                    entity = (Entitys)int.Parse(Console.ReadLine());

//                    if (entity != 0)
//                    {
//                        Console.WriteLine("Select the method you want to perform-\r\n" +
//                        "0- Adding a new object of the entity type to the list (Create)\r\n" +
//                        "1- Object display by ID (Read)\r\n" +
//                        "2- Display the list of all objects of the entity type (ReadAll)\r\n" +
//                        "3- Update existing object data (Update)\r\n" +
//                        "4- Deleting an existing object from a list. (delete)\r\n" +
//                        "   Please note: this option is only available for some entities");
//                        Actions? action = (Actions)int.Parse(Console.ReadLine());

//                        switch (entity)
//                        {
//                            case Entitys.Exit: //exit from main menu
//                                break;
//                            case Entitys.Engineer:
                                
//                                break;
//                            case Entitys.Task:
//                                switch (action)
//                                {
//                                    case Actions.Create:
//                                        break;
//                                    case Actions.Read:
//                                        break;
//                                    case Actions.ReadAll:
//                                        break;
//                                    case Actions.Update:
//                                        break;
//                                    case Actions.Delete:
//                                        break;
//                                    default:
//                                        throw new Exception("Incorrect input - the choice must be in numbers between 0-4");
//                                        break;
//                                }
//                                break;
//                            case Entitys.Dependence:
//                                switch (action)
//                                {
//                                    case Actions.Create:
//                                        break;
//                                    case Actions.Read:
//                                        break;
//                                    case Actions.ReadAll:
//                                        break;
//                                    case Actions.Update:
//                                        break;
//                                    case Actions.Delete:
//                                        break;
//                                    default:
//                                        throw new Exception("Incorrect input - the choice must be in numbers between 0-4");
//                                        break;
//                                }
//                                break;
//                            default:
//                                {//In case a number is chosen that is not among the available options
//                                    throw new Exception("Incorrect input - the choice must be in numbers between 0-4");
//                                    break;
//                                }
//                        }

//                    }
//                    else
//                        throw new Exception("Incorrect input - the choice must be in numbers between 0-3\"");
//                }
//                while (entity != Entitys.Exit);

//                void Inge(Actions action)
//                {
//                    switch (action)
//                    {
//                        case Actions.Create:
//                            {
//                                Engineer engineer = creat
//                                break;
//                            }
//                        case Actions.Read:
//                            break;
//                        case Actions.ReadAll:
//                            break;
//                        case Actions.Update:
//                            break;
//                        case Actions.Delete:
//                            break;
//                        default:
//                            throw new Exception("Incorrect input - the choice must be in numbers between 0-4");
//                            break;
//                    }
//                }
//            }
//            catch (Exception ex) { }
//        }
//    }
//}
