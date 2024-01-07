using Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
                    //printing the entities menu:
                    Console.WriteLine("Select entity you want to check-\n" +
                        "0- Exit\n" +
                        "1- Engineer\n" +
                        "2- Task\n" +
                        "3- Dependence");
                    entity = (Entitys)int.Parse(Console.ReadLine());//user choice.

                    switch (entity)
                    {
                        case Entitys.Exit:// 0 was inserted, so we will exit the menu.
                            break;
                        case Entitys.Engineer://1 was inserted, so the engineer function was called.
                            MEngineer();
                            break;
                        case Entitys.Task://2 was inserted, so the function of the task is called.
                            MTask();
                            break;
                        case Entitys.Dependence://3 was inserted, so the function of the dependency is called.
                            MDependency();
                            break;
                        default://A value was entered that is not between 0 and 3 - exception.
                            throw new Exception("Incorrect input - the choice must be in numbers between 0-4");
                    }
                }
                while (entity != Entitys.Exit);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); } 
        }

        static void MEngineer()//A function that performs the actions on the engineer.
        {
            Actions action;
            do
            {
                //printing the methods menu for the engineer:
                Console.WriteLine("Select the method you want to perform-\n" +
                                    "0- Exit main menu\n" +
                                    "1- Adding a new object of the entity type to the list (Create)\n" +
                                    "2- Object display by ID (Read)\n" +
                                    "3- Display the list of all objects of the entity type (ReadAll)\n" +
                                    "4- Update existing object data (Update)\n" +
                                    "5- Deleting an existing object from a list. (delete)\n");
                action = (Actions)int.Parse(Console.ReadLine());//user choice.

                DO.Engineer newE;//An object of type engineer that will be used by us in operations.
                switch (action)
                {
                    case Actions.Exit:// 0 was inserted, therefore exiting the function back to the main menu.
                        break;
                    case Actions.Create://1 was inserted, so we will create a new engineer.
                        {
                            newE = GetEng();//Call to the function that receives the values from the user and returns a new engineer.
                            s_dalEngineer.Create(newE);//Adding the engineer to the list.
                            break;
                        }
                    case Actions.Read:// 2 was inserted, so we will look for the engineer with the id from the user and print it if it exists.
                        {
                            Console.WriteLine("Enter the id of the engineer you want to print:");//print message insert id.
                            int idR = int.Parse(Console.ReadLine());//getting the id.
                            newE = s_dalEngineer.Read(idR);//copy of the engineer if exists and null if not.
                            if (newE == null)//The engineer does not exist in the list - throwing an exception.
                                throw new Exception($"The engineer with the ID-{idR} does not exist" );
                            Console.WriteLine(newE);//print the engineer.
                            break;
                        }
                    case Actions.ReadAll:// 3 was entered, so we will print all the engineers that exist in the list.
                        {
                            List<DO.Engineer> listEng = s_dalEngineer.ReadAll();//Creating a copy to the list of engineers.
                            foreach (DO.Engineer engineer in listEng)//Go through all the engineers in the list.
                                Console.WriteLine(engineer);//print the engineer.
                            break;
                        }
                    case Actions.Update:// 4 was inserted, so we will update the engineer with the id we received from the user.
                        {
                            Console.WriteLine("Enter the ID of the engineer you would like to update: ");//print message insert id.
                            int idU = int.Parse(Console.ReadLine());//getting the id.
                            if (s_dalEngineer.Read(idU) == null)//The engineer does not exist in the list - throwing an exception.
                                throw new Exception("There is no engineer with ID-" + idU);
                            DO.Engineer upEng = GetEng(idU);//The engineer exists, a call to a function that will receive the rest of its values except for the id.
                            s_dalEngineer.Update(upEng);//Update the engineer with the new values.
                            break;
                        }
                    case Actions.Delete:
                        {
                            Console.WriteLine("Enter the ID of the engineer you would like to delete: ");//print message insert id.
                            int idD = int.Parse(Console.ReadLine());//getting the id.
                            if (s_dalEngineer.Read(idD) == null)//The engineer does not exist in the list - throwing an exception.
                                throw new Exception("There is no engineer with ID-" + idD);
                            s_dalEngineer.Delete(idD);//deleting the engineer.
                            break;
                        }
                    default://A value was entered that is not between 0 and 5 - exception.
                        throw new Exception("Incorrect input - the choice must be in numbers between 0-5");
                }
            }
            while (action != 0);//The loop will run as long as we didn't get 0.
        }


        static void MTask()//A function that performs the actions on the task.
        {
            Actions action;
            do
            {
                //printing the methods menu for the task:
                Console.WriteLine("Select the method you want to perform-\n" +
                                    "0- Exit main menu\n" +
                                    "1- Adding a new object of the entity type to the list (Create)\n" +
                                    "2- Object display by ID (Read)\n" +
                                    "3- Display the list of all objects of the entity type (ReadAll)\n" +
                                    "4- Update existing object data (Update)\n" +
                                    "5- Deleting an existing object from a list. (delete)\n");
                action = (Actions)int.Parse(Console.ReadLine());//user choice.

                DO.Task newT;//An object of type task that will be used by us in operations.
                switch (action)
                {
                    case Actions.Exit:// 0 was inserted, therefore exiting the function back to the main menu.
                        break;
                    case Actions.Create://1 was inserted, so we will create a new task.
                        {
                            newT = GetTask();//Call to the function that receives the values from the user and returns a new task.
                            s_dalTask.Create(newT);//Adding the task to the list.
                            break;
                        }
                    case Actions.Read:// 2 was inserted, so we will look for the task with the id from the user and print it if it exists.
                        {
                            Console.WriteLine("Enter the id of the task you want to print:");//print message insert id.
                            int idR = int.Parse(Console.ReadLine());//getting the id.
                            newT = s_dalTask.Read(idR);//copy of the task if exists and null if not.
                            if (newT == null)//The task does not exist in the list - throwing an exception.
                                throw new Exception($"The task with the ID-{idR} does not exist");
                            Console.WriteLine(newT);//print the task.
                            break;
                        }
                    case Actions.ReadAll:// 3 was entered, so we will print all the task that exist in the list.
                        {
                            List<DO.Task> listTask = s_dalTask.ReadAll();//Creating a copy to the list of task.
                            foreach (DO.Task task in listTask)//Go through all the task in the list.
                                Console.WriteLine(task);//print the task.
                            break;
                        }
                    case Actions.Update:// 4 was inserted, so we will update the task with the id we received from the user.
                        {
                            Console.WriteLine("Enter the ID of the task you would like to update: ");//print message insert id.
                            int idU = int.Parse(Console.ReadLine());//getting the id.
                            if (s_dalTask.Read(idU) == null)//The task does not exist in the list - throwing an exception.
                                throw new Exception("There is no task with ID-" + idU);
                            DO.Task upTask = GetTask(idU);//The task exists, a call to a function that will receive the rest of its values except for the id.
                            s_dalTask.Update(upTask);//Update the task with the new values.
                            break;
                        }
                    case Actions.Delete:
                        {
                            Console.WriteLine("Enter the ID of the task you would like to delete: ");//print message insert id.
                            int idD = int.Parse(Console.ReadLine());//getting the id.
                            if (s_dalTask.Read(idD) == null)//The task does not exist in the list - throwing an exception.
                                throw new Exception("There is no task with ID-" + idD);
                            s_dalTask.Delete(idD);//deleting the task.
                            break;
                        }
                    default://A value was entered that is not between 0 and 5 - exception.
                        throw new Exception("Incorrect input - the choice must be in numbers between 0-5");
                }
            }
            while (action != 0);//The loop will run as long as we didn't get 0.
        }


        static void MDependency()//A function that performs the actions on the dependency.
        {
            Actions action;
            do
            {
                //printing the methods menu for the dependency:
                Console.WriteLine("Select the method you want to perform-\n" +
                                    "0- Exit main menu\n" +
                                    "1- Adding a new object of the entity type to the list (Create)\n" +
                                    "2- Object display by ID (Read)\n" +
                                    "3- Display the list of all objects of the entity type (ReadAll)\n" +
                                    "4- Update existing object data (Update)\n");
                action = (Actions)int.Parse(Console.ReadLine());//user choice.

                DO.Dependency newD;//An object of type dependency that will be used by us in operations.
                switch (action)
                {
                    case Actions.Exit:// 0 was inserted, therefore exiting the function back to the main menu.
                        break;
                    case Actions.Create://1 was inserted, so we will create a new dependency.
                        {
                            newD = GetDep();//Call to the function that receives the values from the user and returns a new dependency.
                            s_dalDependency.Create(newD);//Adding the dependency to the list.
                            break;
                        }
                    case Actions.Read:// 2 was inserted, so we will look for the dependency with the id from the user and print it if it exists.
                        {
                            Console.WriteLine("Enter the id of the dependency you want to print:");//print message insert id.
                            int idR = int.Parse(Console.ReadLine());//getting the id.
                            newD = s_dalDependency.Read(idR);//copy of the dependency if exists and null if not.
                            if (newD == null)//The dependency does not exist in the list - throwing an exception.
                                throw new Exception($"The dependency with the ID-{idR} does not exist");
                            Console.WriteLine(newD);//print the dependency.
                            break;
                        }
                    case Actions.ReadAll:// 3 was entered, so we will print all the dependency that exist in the list.
                        {
                            List<DO.Dependency> listDependency = s_dalDependency.ReadAll();//Creating a copy to the list of dependency.
                            foreach (DO.Dependency dependency in listDependency)//Go through all the dependency in the list.
                                Console.WriteLine(dependency);//print the dependency.
                            break;
                        }
                    case Actions.Update:// 4 was inserted, so we will update the dependency with the id we received from the user.
                        {
                            Console.WriteLine("Enter the ID of the dependency you would like to update: ");//print message insert id.
                            int idU = int.Parse(Console.ReadLine());//getting the id.
                            if (s_dalDependency.Read(idU) == null)//The dependency does not exist in the list - throwing an exception.
                                throw new Exception("There is no dependency with ID-" + idU);
                            DO.Dependency upDep = GetDep(idU);//The dependency exists, a call to a function that will receive the rest of its values except for the id.
                            s_dalDependency.Update(upDep);//Update the dependency with the new values.
                            break;
                        }
                    default://A value was entered that is not between 0 and 4 - exception.
                        throw new Exception("Incorrect input - the choice must be in numbers between 0-4");
                }
            }
            while (action != 0);//The loop will run as long as we didn't get 0.
        }


        static DO.Engineer GetEng(int id = 0)
        {
            if (id == 0)
            {
                Console.WriteLine("Enter id:");
                id = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("Enter his email:");
            String email = Console.ReadLine();
            Console.WriteLine("Enter his cost:");
            double cost = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter his name:");
            String name = Console.ReadLine();
            Console.WriteLine("Enter his level:");
            int intLevel = int.Parse(Console.ReadLine());
            LevelEngineer level = (LevelEngineer)intLevel;
            DO.Engineer engineer = new(id, name,email, level, cost);
            return engineer;
        }

        static DO.Task GetTask(int id = 0)
        {
            Console.WriteLine("enter number between 0 to 4 for level of complexity (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
            int levelInt;
            int.TryParse(Console.ReadLine(), out levelInt);
            LevelEngineer level = (LevelEngineer)levelInt;

            Console.WriteLine("enter alias:");
            string? alias = Console.ReadLine();

            Console.WriteLine("enter description:");
            string? description = Console.ReadLine();

            Console.WriteLine("Enter Required Effort Time:");
            TimeSpan? requiredEffortTime;
            TimeSpan time;
            requiredEffortTime = TimeSpan.TryParse(Console.ReadLine(), out time) ? time : null;

            Console.WriteLine("enter true if task is milestone, otherwise false");
            bool isMilestone = bool.Parse(Console.ReadLine());

            Console.WriteLine("enter start date:");
            DateTime? startDate;
            DateTime date;
            startDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : null;

            Console.WriteLine("enter scheduled date:");
            DateTime? scheduledDate;
            scheduledDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : null;

            Console.WriteLine("enter complete date");
            DateTime? completeDate;
            completeDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : null;

            Console.WriteLine("enter deliverables:");
            string? deliverables = Console.ReadLine();

            Console.WriteLine("enter remarks: (optional)");
            string? remarks = Console.ReadLine();

            Console.WriteLine("enter id of engineer working on task:");
            int? engineerId;
            int engId;
            engineerId = int.TryParse(Console.ReadLine(), out engId) ? engId : null;

            DO.Task? task = new(0,alias,description,DateTime.Now,requiredEffortTime,isMilestone,level,startDate
                ,scheduledDate,completeDate,deliverables,remarks,engineerId);
            return task;

        }

        static DO.Dependency GetDep(int id = 0)
        {
            DO.Dependency dep = null;
            Console.WriteLine("Enter the ID of the task you want to create a dependency for:");
            int dependent = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the ID of the task that the task depends on:");
            int dependsOn = int.Parse(Console.ReadLine());
            dep = new(0, dependent, dependsOn);
            return dep;
        }

    }
}
