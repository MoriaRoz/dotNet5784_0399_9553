using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BlTest
{
    public enum Entitys { Exit = 0, Engineer, Task, Schedule, Status };
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
                                      "3- Create Schedule\n"+
                                      "4- Project status");
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
                            catch { }
                            //חריגה אם הוכנס משהו שהוא לא תאריך
                            break;
                        case Entitys.Status:
                            Console.WriteLine("Status project is: "+s_bl.GetProjectStatus());
                            break;
                        default://A value was entered that is not between 0 and 3 - exception,Throw exception.
                            Console.WriteLine("Incorrect input - the choice must be in numbers between 0-4\n");
                            break;
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
                                  "5- Deleting an existing engineer from a list. (delete)");

                action = (Actions)int.Parse(Console.ReadLine());//user choice.
                BO.Engineer newE;//An object of type engineer that will be used by us in operations.

                switch (action)
                {
                    case Actions.Exit:// 0 was inserted, therefore exiting the function back to the main menu.
                        break;
                    case Actions.Create://1 was inserted, so we will create a new engineer.
                        try
                        {
                            newE = GetNewEng();//Call to the function that receives the values from the user and returns a new engineer.
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
                            BO.Engineer oldEng = s_bl.Engineer.Read(idU);
                            if (oldEng == null)//The engineer does not exist in the list - throwing an exception.
                                throw new BO.BlDoesNotExistException("There is no engineer with ID-" + idU);
                            BO.Engineer upEng = GetUpdateEng(oldEng);//The engineer exists, a call to a function that will receive the rest of its values except for the id.
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
                        Console.WriteLine("Incorrect input - the choice must be in numbers between 0-5");
                        break;
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
                                    "5- Deleting an existing object from a list. (delete)");
                action = (Actions)int.Parse(Console.ReadLine());//user choice.
                BO.Task newT;//An object of type task that will be used by us in operations.
                switch (action)
                {
                    case Actions.Exit:// 0 was inserted, therefore exiting the function back to the main menu.
                        break;
                    case Actions.Create://1 was inserted, so we will create a new task.
                        try
                        {
                            if (ProjectStatus == BO.ProjectStatus.InExecution)
                                throw new BO.BlTheScheduleIsSet("The schedule is set, it is not possible to add a new task");
                            newT = GetNewTask();//Call to the function that receives the values from the user and returns a new task.
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
                            BO.Task oldTask = s_bl.Task.Read(idU);
                            if (oldTask == null)//The task does not exist in the list - throwing an exception.
                                throw new BO.BlDoesNotExistException("There is no task with ID-" + idU);
                            BO.Task upTask = GetUpdateTask(oldTask);//The task exists, a call to a function that will receive the rest of its values except for the id.
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
                        Console.WriteLine("Incorrect input - the choice must be in numbers between 0-5");
                        break;
                }
            } while (action != 0); //The loop will run as long as we didn't get 0.

        }
        static BO.Engineer GetNewEng()//A function that receives variables from the user and creates a new engineer.
        {
            //assigning new id
            Console.WriteLine("Enter engineer id:");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
                throw new BO.BlInvalidValueException("Id must be a number only");

            //assigning new name
            Console.WriteLine("Enter a engineer name:");
            string? name = Console.ReadLine();

            //assigning new email
            Console.WriteLine("Enter an engineer email:");
            string? email = Console.ReadLine();

            //assigning new leval
            Console.WriteLine("Enter the engineer level: { 0- Beginner, 1- AdvancedBeginner, 2- Intermediate, 3- Advanced, 4- Expert}");
            int levelInt;
            int.TryParse(Console.ReadLine(), out levelInt);
            BO.LevelEngineer level = (BO.LevelEngineer)levelInt;

            //assigning new cost
            Console.WriteLine("Enter cost per hour:");
            double cost;
            if (!double.TryParse(Console.ReadLine(), out cost))
                throw new BO.BlInvalidValueException("Cost must be a number only");

            //assigning new tasks
            BO.TaskInEngineer? currentTask = null;
            Console.WriteLine("Do you want to assign a task to an engineer? (Y/N)");
            string? ans = Console.ReadLine();
            if (ans != null && ans == "Y")
            {
                Console.WriteLine("enter task id:");
                int taskId;
                if (!int.TryParse(Console.ReadLine(), out taskId))
                    throw new BO.BlInvalidValueException("Id must be a number only");
                currentTask = new BO.TaskInEngineer()
                {
                    Id = taskId,
                    Alias = s_bl.Task.Read(taskId).Alias
                };
            }

            return new BO.Engineer()
            {
                Id = id,
                Name = name,
                Email = email,
                Level = level,
                Cost = cost,
                Task = currentTask
            };
        }
        static BO.Engineer GetUpdateEng(BO.Engineer oldEng)
        {
            Console.WriteLine("If you want to update, insert a new value, if not press enter");
            
            //assigning name
            Console.WriteLine("Enter a engineer name:");
            string? _name = Console.ReadLine();
            if (_name == "") _name = oldEng.Name;
            
            //assigning email
            Console.WriteLine("Enter an engineer email:");
            string? _email = Console.ReadLine();
            if (_email == "") _email = oldEng.Email;
            
            //assigning level
            Console.WriteLine("Enter the engineer level: { 0- Beginner, 1- AdvancedBeginner, 2- Intermediate, 3- Advanced, 4- Expert}");
            int levelInt; ;
            bool success = int.TryParse(Console.ReadLine(), out levelInt);
            BO.LevelEngineer _level = success ? (BO.LevelEngineer)levelInt : oldEng.Level;

            //assigning cost
            Console.WriteLine("Enter cost per hour:");
            double? _cost;
            double cost;
            _cost = double.TryParse(Console.ReadLine(), out cost) ? cost : oldEng.Cost;

            //assigning current tasks in engineer
            Console.WriteLine("Do you want to update current task? (Y/N)");
            string? ans = Console.ReadLine();
            BO.TaskInEngineer? _newTask = oldEng.Task;
            if (ans != null && ans == "Y")
            {
                Console.WriteLine("Enter task id:");
                int taskId;
                if (!int.TryParse(Console.ReadLine(), out taskId))
                    throw new BO.BlInvalidValueException("Id must be a number only");
                _newTask = new BO.TaskInEngineer()
                {
                    Id = taskId,
                    Alias = s_bl.Task.Read(taskId).Alias
                };
            }
            


            return new BO.Engineer()
            {
                Id = oldEng.Id,
                Name = _name,
                Email = _email,
                Level = _level,
                Cost = _cost,
                Task = _newTask
            }; //create a new engineer
        }
        static BO.Task GetNewTask()//A function that receives variables from the user and creates a new task.
        {
            //assigning new alias
            Console.WriteLine("Enter alias:");
            string? alias = Console.ReadLine();

            //assigning new description
            Console.WriteLine("Enter description:");
            string? description = Console.ReadLine();
            
            //assigning new level
            Console.WriteLine("enter number between 0 to 4 for level of complexity { 0- Beginner, 1- AdvancedBeginner, 2- Intermediate, 3- Advanced, 4- Expert}");
            int levelInt;
            int.TryParse(Console.ReadLine(), out levelInt);
            BO.LevelEngineer level = (BO.LevelEngineer)levelInt;
            
            //assigning new Required Effort Time
            Console.WriteLine("Enter Required Effort Time:");
            TimeSpan requiredEffortTime;
            if (!TimeSpan.TryParse(Console.ReadLine(), out requiredEffortTime))
                throw new BO.BlInvalidValueException("Required effort time has to be in time span format");

            //assigning new deliverables
            Console.WriteLine("Enter deliverables:");
            string? deliverables = Console.ReadLine();

            //assigning new remarks
            Console.WriteLine("Enter remarks:");
            string? remarks = Console.ReadLine();

            //assigning new task dependencies
            Console.WriteLine("Does this task depend on previos tasks?");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            List<BO.TaskInList>? dependencies = null;
            while (ans == "Y")
            {
                Console.WriteLine("Enter id of previous task:");
                int taskId;
                if (!int.TryParse(Console.ReadLine(), out taskId))
                    throw new BO.BlInvalidValueException("Id has to contain numbers only");

                //reads requested task props
                BO.Task? depTask = s_bl.Task.Read(taskId);
                //creating new list of dependencies
                if (depTask != null && dependencies == null) 
                    dependencies = new List<BO.TaskInList>();
                dependencies.Add(new BO.TaskInList()
                {
                    Id = taskId,
                    Description = depTask.Description,
                    Alias = depTask.Alias,
                    Status = depTask.Status
                });
                Console.WriteLine("Does this task depend on more previos tasks?");
                ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            }

            return new BO.Task()
            {
                Id = 0,
                Description = description,
                Alias = alias,
                CreatedAtDate = DateTime.Now,
                Status = BO.Statuses.Unscheduled,
                Dependencies = dependencies,
                RequiredEffortTime = requiredEffortTime,
                StartDate = null,
                ScheduledDate = null,
                ForecastDate = null,
                CompleteDate = null,
                Deliverables = deliverables,
                Remarks = remarks,
                Engineer = null,
                Complexity = level
            };
        }
        static BO.Task GetUpdateTask(BO.Task oldTask)
        {

            Console.WriteLine("enter new values only for the fields you want to update, the rest will stay without change, \n if you enter info in worng format those fields will not be updated (those fields will stay the same as before)");
            //assigning level and check it with try parse method
            Console.WriteLine("enter number between 0 to 4 for level of complexity (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
            int levelInt;
            bool success = int.TryParse(Console.ReadLine(), out levelInt);
            BO.LevelEngineer level = success ? (BO.LevelEngineer)levelInt : oldTask.Complexity;

            //assigning alias
            Console.WriteLine("enter alias:");
            string? alias = Console.ReadLine();
            if (alias == "") alias = oldTask.Alias;

            //assigning description
            Console.WriteLine("enter description:");
            string? description = Console.ReadLine();
            if (description == "") description = oldTask.Description;

            //assigning Required Effort Time and check it with try parse method
            Console.WriteLine("Enter Required Effort Time:");
            TimeSpan? requiredEffortTime;
            TimeSpan time;
            requiredEffortTime = TimeSpan.TryParse(Console.ReadLine(), out time) ? time : oldTask.RequiredEffortTime;

            //assigning start date and check it with try parse method
            Console.WriteLine("enter start date:");
            DateTime? startDate;
            DateTime date;
            startDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : oldTask.StartDate;

            //assigning scheduled date and check it with try parse method
            Console.WriteLine("enter scheduled date:");
            DateTime? scheduledDate;
            scheduledDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : oldTask.ScheduledDate;

            //assigning complete date and check it with try parse method
            Console.WriteLine("enter complete date");
            DateTime? completeDate;
            completeDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : oldTask.CompleteDate;

            //assigning deliverables
            Console.WriteLine("enter deliverables: (optional)");
            string? deliverables = Console.ReadLine();
            if (deliverables == "") deliverables = oldTask.Deliverables;

            //assigning remarks
            Console.WriteLine("enter remarks: (optional)");
            string? remarks = Console.ReadLine();
            if (remarks == "") remarks = oldTask.Remarks;

            //assigning engineer id that working on task and check it with try parse method
            Console.WriteLine("enter id of engineer working on task:");
            int? engineerId;
            int engId;
            engineerId = int.TryParse(Console.ReadLine(), out engId) ? engId : (oldTask.Engineer != null ? oldTask.Engineer.Id : null);

            //assigning dependencies
            Console.WriteLine("Does this task depend on previos tasks?");
            string? ans = Console.ReadLine(); //?? throw new FormatException("Wrong input");
            List<BO.TaskInList>? dependencies = null;
            while (ans == "Y")
            {
                Console.WriteLine("Enter id of previous task:");
                int id;
                if (!int.TryParse(Console.ReadLine(), out id))
                    throw new BO.BlInvalidValueException("Id has to contain numbers only");
                BO.Task depTask = s_bl.Task.Read(id);
                if (dependencies == null)
                    dependencies = new List<BO.TaskInList>();
                dependencies.Add(new BO.TaskInList()
                {
                    Id = id,
                    Description = depTask.Description,
                    Alias = depTask.Alias,
                    Status = depTask.Status
                });
                Console.WriteLine("Does this task depend on more previos tasks?");
                ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            }


            return new BO.Task()
            {
                Id = oldTask.Id,
                Description = description,
                Alias = alias,
                CreatedAtDate = DateTime.Now,
                Status = oldTask.Status,
                Dependencies = dependencies,
                RequiredEffortTime = requiredEffortTime,
                StartDate = startDate,
                ScheduledDate = scheduledDate,
                ForecastDate = oldTask.ForecastDate,
                CompleteDate = completeDate,
                Deliverables = deliverables,
                Remarks = remarks,
                Engineer = engineerId == null ? null : new BO.EngineerInTask() { Id = (int)engineerId, Name = s_bl.Engineer.Read((int)engineerId).Name },
                Complexity = level
            };

        }
    }
}
