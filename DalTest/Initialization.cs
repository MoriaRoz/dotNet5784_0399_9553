namespace DalTest;
using DalApi;
using DO;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Xml.Linq;

//לתעד
public static class Initialization
{
    private static IEngineer? s_dalEngineer; //stage 1
    private static IDependency? s_dalDependency; //stage 1
    private static ITask? s_dalTask; //stage 1

    private static readonly Random s_rand = new();
    private static void createEngineer()
    {
        string[] engineerNames =
        {
            "Dani Levi", "Eli Amar", "Yair Cohen",
            "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

        foreach (var _name in engineerNames)
        {
            Random rnd = new Random();

            int _id;
            do
                _id = rnd.Next(200000000, 400000000);
            while (s_dalEngineer?.Read(_id) != null);

            string? _email = _name + "@gamil.com";
            _email.Replace(' ', '_');

            double _cost = 100.0;

            LevelEngineer _level = LevelEngineer.Beginner;
            int _levelNum = rnd.Next(0, 5);
            switch (_levelNum)
            {
                case 0:
                    _level = LevelEngineer.Beginner;
                    _cost = 117.133;
                    break;
                case 1:
                    _level = LevelEngineer.AdvancedBeginner;
                    _cost = 200.355;
                    break;
                case 2:
                    _level = LevelEngineer.Intermediate;
                    _cost = 283.566;
                    break;
                case 3:
                    _level = LevelEngineer.Advanced;
                    _cost = 366.783;
                    break;
                case 4:
                    _level = LevelEngineer.Expert;
                    _cost = 450.00;
                    break;
            }

            Engineer newEng = new(_id, _name, _email, _level, _cost);
            s_dalEngineer!.Create(newEng);
        }
    }
    private static void createTask()
    {
        int i = 0;
        Random rnd = new Random();

        string[] nameTasks =
        {
            //לתת שמות קצרים לכל משימה
        };
        string[] descripTasks =
        {
            "Define the purpose and scope of the project.",
            "Create a schedule for the project.",
            "Deciding on a budget for the project.",
            "Allocate resources to the project.",
            "Perform risk analysis and develop risk management strategies.",
            "Determine internal and public communication and protocols.",
            "Divide the project into small tasks.",
            "Attach an engineer to every task.",
            "Follow the progress of the project against the schedule.",
            "Receiving data from the client.",
            "Writing code in the data layer.",
            "Trying to run and test the code for the data layer.",
            "Writing code in the logical layer.",
            "The attempt to run and test the code for the logical layer.",
            "Designed the interface.",
            "Trying to run and test the code for the interface.",
            "Attempt to run the code.",
            "Sending the interface to the customer and focus groups.",
            "Fix recent errors.",
            "Completion of the project."
        };

        foreach (string _name in nameTasks)
        {
            string? _description = descripTasks[i];
            
            DateTime _createdAtDate = DateTime.Now;
            DateTime? _deadlineDate = null;

            TimeSpan? _requiredEffortTime = null;
            
            bool _isMilestone = false;

            LevelEngineer _complexity = LevelEngineer.Beginner;
            int _compNum = rnd.Next(0, 5);
            switch (_compNum)
            {
                case 0:
                    _complexity = LevelEngineer.Beginner;
                    break;
                case 1:
                    _complexity = LevelEngineer.AdvancedBeginner;
                    break;
                case 2:
                    _complexity = LevelEngineer.Intermediate;
                    break;
                case 3:
                    _complexity = LevelEngineer.Advanced;
                    break;
                case 4:
                    _complexity = LevelEngineer.Expert;
                    break;
            }

            DateTime? _startDate = null;
            
            DateTime? _completeDate = null;
            
            string? _deliverables = null;
            
            string? _remarks = null;

            int? _engineerId = null;
           
            Task newTa = new Task(0,_name,_description,_createdAtDate,
                _requiredEffortTime,_isMilestone,_complexity,_startDate,
                _deadlineDate,_completeDate,_deliverables,_remarks,_engineerId);
            s_dalTask!.Create(newTa);
            i++;
        }
    }
    private static void createDependency()
    {
        int?_previousTask;
        int? _dependsOnTask;

        for (int i = 1; i <= 19; i++)
        {
            _previousTask=20;
            _dependsOnTask=i;
            Dependency newDep = new(0,_dependsOnTask,_previousTask);
            s_dalDependency!.Create(newDep);
        }
        
        //לסיים עוד 11 תלותיות
        

        //Dependency newDep = new();
        //s_dalDependency!.Create();
    }

    public static void Do(IEngineer? engineer, ITask? task, IDependency dependency)
    {
        s_dalEngineer = engineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = task ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dependency ?? throw new NullReferenceException("DAL can not be null!");
        createEngineer();
        createTask();
        createDependency();
    }
}

