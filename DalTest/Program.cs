using Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

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
                    {
                        newE = GetEng();
                        s_dalEngineer.Create(newE);
                        break;
                    }
                case Actions.Read:
                    {
                        Console.WriteLine("enter id of Engineer:");
                        int idR = int.Parse(Console.ReadLine());
                        newE = s_dalEngineer.Read(idR);
                        Console.WriteLine(newE.ToString());
                        break;
                    }
                case Actions.ReadAll:
                    {
                        List<Engineer> listEng = s_dalEngineer.ReadAll();
                        foreach (Engineer engineer in listEng)
                            Console.WriteLine(engineer.ToString());
                        break;
                    }
                case Actions.Update:
                    {
                        Console.WriteLine("Enter the ID of the engineer you would like to update: ");
                        int idU = int.Parse(Console.ReadLine());
                        if (s_dalEngineer.Read(idU) == null)
                            throw new Exception("There is no engineer with ID-" + idU);
                        DO.Engineer upEng = GetEng(idU);
                        s_dalEngineer.Update(upEng);
                        break;
                    }
                case Actions.Delete:
                    {
                        Console.WriteLine("Enter the ID of the engineer you would like to update: ");
                        int idD = int.Parse(Console.ReadLine());
                        if (s_dalEngineer.Read(idD) == null)
                            throw new Exception("There is no engineer with ID-" + idD);
                        s_dalEngineer.Delete(idD);
                        break;
                    }
            }
        }       
                default:
                    throw new Exception("Incorrect input - the choice must be in numbers between 0-5");
                    }
        }
        static void MTask(Actions action)
        {
            DO.Task newT;
            switch (action)
            {
                case Actions.Exit:
                    break;
                case Actions.Create:
                    {
                        newT = GetTask();
                        s_dalTask.Create(newT);
                        break;
                    }
                case Actions.Read:
                    {
                        Console.WriteLine("enter id of Task:");
                        int idR = int.Parse(Console.ReadLine());
                        newE = s_dalTask.Read(idR);
                        Console.WriteLine(newE.ToString());
                        break;
                    }
                case Actions.ReadAll:
                    {
                        List<Task> listTask = s_dalTask.ReadAll();
                        foreach (Task task in listTask)
                            Console.WriteLine(task.ToString());
                        break;
                    }
                case Actions.Update:
                    {
                        Console.WriteLine("Enter the ID of the task you would like to update: ");
                        int idU = int.Parse(Console.ReadLine());
                        if (s_dalTask.Read(idU) == null)
                            throw new Exception("There is no task with ID-" + idU);
                        DO.Task upTask = GetTask(idU);
                        s_dalTask.Update(upTask);
                        break;
                    }
                case Actions.Delete:
                    {
                        Console.WriteLine("Enter the ID of the task you would like to update: ");
                        int idD = int.Parse(Console.ReadLine());
                        if (s_dalTask.Read(idD) == null)
                            throw new Exception("There is no task with ID-" + idD);
                        s_dalTask.Delete(idD);
                        break;
                    }
            }
        }
static void MDependency(Actions action)
{
    DO.Dependency newDependency;
    switch (action)
    {
        case Actions.Exit:
            break;
        case Actions.Create:
            {
                newDependency = GetDependency();
                s_dalDependency.Create(newDependency);
                break;
            }
        case Actions.Read:
            {
                Console.WriteLine("Enter id of Dependency:");
                int idR = int.Parse(Console.ReadLine());
                newDependency = s_dalDependency.Read(idR);
                Console.WriteLine(newDependency.ToString());
                break;
            }
        case Actions.ReadAll:
            {
                List<Dependency> listDependency = s_dalDependency.ReadAll();
                foreach (Dependency dependency in listDependency)
                    Console.WriteLine(dependency.ToString());
                break;
            }
        case Actions.Update:
            {
                Console.WriteLine("Enter the ID of the dependency you would like to update: ");
                int idU = int.Parse(Console.ReadLine());
                if (s_dalDependency.Read(idU) == null)
                    throw new Exception("There is no dependency with ID-" + idU);
                DO.Dependency upDependency = GetDependency(idU);
                s_dalDependency.Update(upDependency);
                break;
            }
        case Actions.Delete:
            {
                Console.WriteLine("Enter the ID of the dependency you would like to delete: ");
                int idD = int.Parse(Console.ReadLine());
                if (s_dalDependency.Read(idD) == null)
                    throw new Exception("There is no dependency with ID-" + idD);
                s_dalDependency.Delete(idD);
                break;
            }
    }
}


static DO.Engineer GetEng(int id=0)
        {
            if(id==0)
            {
                Console.WriteLine("Enter id:");
            }
            DO.Engineer engineer = null;
            return engineer;
        }
        static DO.Task GetTask(int id=0)
        {
            DO.Task task = null;
            return task;
        }
        static DO.Dependency GetDep(int id = 0)
        {
            DO.Dependency dep = null;
            return dep;
        }
    }
}



















