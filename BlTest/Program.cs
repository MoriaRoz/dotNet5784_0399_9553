using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BlTest
{
    public enum Entitys { Exit = 0, Engineer, Task, Schedule };
    public enum Actions { Exit = 0, Create, Read, ReadAll, Update, Delete };
    internal class Program
    {
        static readonly IBl s_bl = Factory.Get();
        static BO.ProjectStatus ProjectStatus = BO.ProjectStatus.Inlanning;
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Would you like to create Initial data? (Y/N)");
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                if (ans == "Y")
                    DalTest.Initialization.Do();
                
                Entitys entity;
                do
                {
                    //printing the entities menu:
                    Console.WriteLine("Select entity you want to check-\n" +
                                      "0- Exit\n" +
                                      "1- Engineer\n" +
                                      "2- Task\n"+
                                      "3- Create Schedule\n");
                    entity = (Entitys)int.Parse(Console.ReadLine());//user choice.

                    switch (entity)
                    {
                        case Entitys.Exit:// 0 was inserted, so we will exit the menu.
                            break;
                        case Entitys.Engineer://1 was inserted, so the engineer function was called.
                            try { MEngineer(); }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            break;
                        case Entitys.Task://2 was inserted, so the function of the task is called.
                            try { MTask(); }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            break;
                        case Entitys.Schedule:
                            try
                            {
                                Console.WriteLine("Enter the project start date:");
                                DateTime start = DateTime.Parse(Console.ReadLine());
                                s_bl.CreateSchedule(start);
                                ProjectStatus = BO.ProjectStatus.InExecution;
                            }
                            //חריגה אם הוכנס משהו שהוא לא תאריך
                            break;
                        default://A value was entered that is not between 0 and 3 - exception,Throw exception.
                            throw new BO.BlNumberOutOfRangeException("Incorrect input - the choice must be in numbers between 0-4");
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
                                  "0- Exit menu\n" +
                                  "1- Adding a new object of the engineer type to the list (Create)\n" +
                                  "2- Engineer display by ID (Read)\n" +
                                  "3- Display the list of all objects of the engineer type (ReadAll)\n" +
                                  "4- Update existing engineer data (Update)\n" +
                                  "5- Deleting an existing engineer from a list. (delete)\n");

                action = (Actions)int.Parse(Console.ReadLine());//user choice.
                BO.Engineer newE;//An object of type engineer that will be used by us in operations.

                switch (action)
                {
                    case Actions.Exit:// 0 was inserted, therefore exiting the function back to the main menu.
                        break;
                    case Actions.Create://1 was inserted, so we will create a new engineer.
                        try
                        {
                            newE = GetEng();//Call to the function that receives the values from the user and returns a new engineer.
                            s_bl.Engineer.Create(newE);//Adding the engineer to the list.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Actions.Read:// 2 was inserted, so we will look for the engineer with the id from the user and print it if it exists.
                        try
                        {
                            Console.WriteLine("Enter the id of the engineer you want to print:");//print message insert id.
                            int idR = int.Parse(Console.ReadLine());//getting the id.
                            newE = s_bl.Engineer.Read(idR);//copy of the engineer if exists and null if not.
                            if (newE == null)//The engineer does not exist in the list - throwing an exception.
                                throw new BO.BlDoesNotExistException($"The engineer with the ID-{idR} does not exist");
                            Console.WriteLine(newE);//print the engineer.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Actions.ReadAll:// 3 was entered, so we will print all the engineers that exist in the list.
                        {
                            var listEng = s_bl.Engineer.ReadAll();//Creating a copy to the list of engineers.
                            foreach (BO.Engineer engineer in listEng)//Go through all the engineers in the list.
                                Console.WriteLine(engineer);//print the engineer.
                            break;
                        }
                    case Actions.Update:// 4 was inserted, so we will update the engineer with the id we received from the user.
                        try
                        { 
                            Console.WriteLine("Enter the ID of the engineer you would like to update: ");//print message insert id.
                            int idU = int.Parse(Console.ReadLine());//getting the id.
                            if (s_bl.Engineer.Read(idU) == null)//The engineer does not exist in the list - throwing an exception.
                                throw new BO.BlDoesNotExistException("There is no engineer with ID-" + idU);
                            BO.Engineer upEng = GetEng(idU);//The engineer exists, a call to a function that will receive the rest of its values except for the id.
                            s_bl.Engineer.Update(upEng);//Update the engineer with the new values.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Actions.Delete:
                        try
                        {
                            Console.WriteLine("Enter the ID of the engineer you would like to delete: ");//print message insert id.
                            int idD = int.Parse(Console.ReadLine());//getting the id.
                            if (s_bl.Engineer.Read(idD) == null)//The engineer does not exist in the list - throwing an exception.
                                throw new BO.BlDalDeletionImpossible("There is no engineer with ID-" + idD);
                            s_bl.Engineer.Delete(idD);//deleting the engineer.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    default://A value was entered that is not between 0 and 5 - exception,Throw exception.
                        throw new BO.BlNumberOutOfRangeException("Incorrect input - the choice must be in numbers between 0-5");
                }
            }
            while (action != 0);//The loop will run as long as we didn't get 0.
        }


        static void MTask()//A function that performs the actions on the task.
        {
            Actions action;
            //printing the methods menu for the task:
            Console.WriteLine("Select the method you want to perform-\n" +
                                "0- Exit main menu\n" +
                                "1- Adding a new object of the task type to the list (Create)\n" +
                                "2- Task display by ID (Read)\n" +
                                "3- Display the list of all objects of the task type (ReadAll)\n" +
                                "4- Update existing task data (Update)\n" +
                                "5- Deleting an existing task from a list. (delete)\n");
            action = (Actions)int.Parse(Console.ReadLine());//user choice.

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
                BO.Task newT;//An object of type task that will be used by us in operations.
                switch (action)
                {
                    case Actions.Exit:// 0 was inserted, therefore exiting the function back to the main menu.
                        break;
                    case Actions.Create://1 was inserted, so we will create a new task.
                        try
                        {
                            if(ProjectStatus==BO.ProjectStatus.InExecution) 
                                throw new BO.BlTheScheduleIsSet("The schedule is set, it is not possible to add a new task");
                            newT = GetTask();//Call to the function that receives the values from the user and returns a new task.
                            s_bl.Task.Create(newT);//Adding the task to the list.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Actions.Read:// 2 was inserted, so we will look for the task with the id from the user and print it if it exists.
                        try
                        {
                            Console.WriteLine("Enter the id of the task you want to print:");//print message insert id.
                            int idR = int.Parse(Console.ReadLine());//getting the id.
                            newT = s_bl.Task.Read(idR);//copy of the task if exists and null if not.
                            if (newT == null)//The task does not exist in the list - throwing an exception.
                                throw new BO.BlDoesNotExistException($"The task with the ID-{idR} does not exist");
                            Console.WriteLine(newT);//print the task.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Actions.ReadAll:// 3 was entered, so we will print all the task that exist in the list.
                        try
                        {
                            var listTask = s_bl.Task.ReadAll();//Creating a copy to the list of task.
                            foreach (BO.TaskInList task in listTask)//Go through all the task in the list.
                                Console.WriteLine(task);//print the task.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Actions.Update:// 4 was inserted, so we will update the task with the id we received from the user.
                        try
                        {
                            Console.WriteLine("Enter the ID of the task you would like to update: ");//print message insert id.
                            int idU = int.Parse(Console.ReadLine());//getting the id.
                            if (s_bl.Task.Read(idU) == null)//The task does not exist in the list - throwing an exception.
                                throw new BO.BlDoesNotExistException("There is no task with ID-" + idU);
                            BO.Task upTask = GetTask(idU);//The task exists, a call to a function that will receive the rest of its values except for the id.
                            s_bl.Task.Update(upTask);//Update the task with the new values.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case Actions.Delete:
                        try
                        {
                            if (ProjectStatus == BO.ProjectStatus.InExecution)
                                throw new BO.BlTheScheduleIsSet("The schedule is set, it is not possible to delete a task");
                            Console.WriteLine("Enter the ID of the task you would like to delete: ");//print message insert id.
                            int idD = int.Parse(Console.ReadLine());//getting the id.
                            if (s_bl.Task.Read(idD) == null)//The task does not exist in the list - throwing an exception.
                                throw new BO.BlDalDeletionImpossible("There is no task with ID-" + idD);
                            s_bl.Task.Delete(idD);//deleting the task.
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    default://A value was entered that is not between 0 and 5 - exception,Throw exception.
                        throw new BO.BlNumberOutOfRangeException("Incorrect input - the choice must be in numbers between 0-5");
                }
            } while (action != 0); //The loop will run as long as we didn't get 0.

        }
        static BO.Engineer GetEng(int id = 0)//A function that receives variables from the user and creates a new engineer.
        {
            if (id == 0)
            {
                Console.WriteLine("Enter engineer id:");
                id = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("Enter his name:");
            String? name = Console.ReadLine();

            Console.WriteLine("Enter his email:");
            String? email = Console.ReadLine();

            Console.WriteLine("Enter his level:{ Beginner=0, AdvancedBeginner=1, Intermediate=2, Advanced=3, Expert=4}");
            int intLevel = int.Parse(Console.ReadLine());
            BO.LevelEngineer level = (BO.LevelEngineer)intLevel;

            Console.WriteLine("Enter his cost:");
            double? cost = double.Parse(Console.ReadLine());

            BO.Engineer engineer = new BO.Engineer()
            {
                Id = id,
                Name = name,
                Email = email,
                Level = level,
                Cost = cost,
            };
            return engineer;
        }

        static BO.Task GetTask(int id = 0)//A function that receives variables from the user and creates a new task.
        {
            if (id == 0)
            {
                Console.WriteLine("Enter task id:");
                id = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("enter description:");
            string? description = Console.ReadLine();

            Console.WriteLine("enter alias:");
            string? alias = Console.ReadLine();

            //CreatedAtDate
            //status

            Console.WriteLine("enter number between 0 to 4 for level of complexity (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
            int levelInt;
            int.TryParse(Console.ReadLine(), out levelInt);
            BO.LevelEngineer level = (BO.LevelEngineer)levelInt;

            Console.WriteLine("Enter crea Effort Time:");
            DateTime CreatedAtDate;




            Console.WriteLine("Enter Required Effort Time:");
            TimeSpan? requiredEffortTime;
            TimeSpan time;
            requiredEffortTime = TimeSpan.TryParse(Console.ReadLine(), out time) ? time : null;

            Console.WriteLine("enter start date:");
            DateTime? startDate;
            DateTime date;
            startDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : null;

            Console.WriteLine("enter scheduled date:");
            DateTime? scheduledDate;
            scheduledDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : null;

            Console.WriteLine("enter deadline date:");
            DateTime? deadlineDate;
            deadlineDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : null;

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

            BO.Task? task = new BO.Task()
            {
                Description = description,
                Alias = alias,
            }; //to change
            return task;
        }
    }
}
