using Dal;
using DalApi;

namespace DalTest
{
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
                        "2- task\r\n" +
                        "3- dependence");
                    int entity = int.Parse(Console.ReadLine());
                    switch (entity)
                    {
                        case 1:
                            break;

                        case 2:
                            break;

                    }
                }
                while ()


                Console.WriteLine("Select the method you want to perform-\r\n" +
                    "0- exit from main menu\r\n" +
                    "1- Adding a new object of the entity type to the list (Create)\r\n" +
                    "2- Object display by ID (Read)\r\n" +
                    "3- Display the list of all objects of the entity type (ReadAll)\r\n" +
                    "4- Update existing object data (Update)\r\n" +
                    "5- Deleting an existing object from a list. (delete)\r\n" +
                    "     Please note: this option is only available for some entities");
                int action = int.Parse(Console.ReadLine());
            }
            }
           catch (Exception ex) { }
        }
    }

}
