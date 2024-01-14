namespace DalTest;
using DalApi;
using DO;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;

public static class Initialization
{
    private static IDal? s_dal;

    private static readonly Random s_rand = new();
    private static void createEngineer() //The action creates the engineers' data (randomly, according to the logic of each of the features)
    {
        string[] engineerNames =
        { //The database of names that engineers will have
            "Dani Levi", "Eli Amar", "Yair Cohen",
            "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

        foreach (var _name in engineerNames)
        {
            Random rnd = new Random();

            int _id;
            do //Matching a unique ID to each engineer
                _id = rnd.Next(200000000, 400000000);
            while (s_dal?.Engineer.Read(_id) != null);

            string? _email = _name + "@gamil.com"; //Adapting an email to each of the engineers in the following way - name_family name@gmail.com
            _email.Replace(' ', '_');

            double _cost = 100.0;

            LevelEngineer _level = LevelEngineer.Beginner;
            int _levelNum = rnd.Next(0, 5);
            switch (_levelNum) //Level adjustment for each of the engineers - and cost according to the level he is at
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
            s_dal!.Engineer.Create(newEng);
        }
    }
    private static void createTask()
    {
        int i = 0;
        Random rnd = new Random();

        string[] nameTasks =
        {
            "Define the purpose and scope of the project","Create a schedule for the project","Decide on a budget for the project", "Assign resources to the project",
            "Perform risk analysis and develop risk management strategies","Determine internal and public communication and protocols","Divide the project into small tasks","Attach an engineer to each task",
            "Follow the progress of the project against the schedule","Receiving data from the client","Writing code in the data layer","Trying to run and test the code for the data layer",
            "Writing code in the logical layer","Trying to run and test the code for the logical layer","Designed the interface","Attempt to run and test the code for the interface",
            "Attempt to run the code","Sending the interface to the client and focus groups","Correction of recent errors","Submission of the project",
        };
        string[] descripTasks =
        {
            "Define the boundaries and limitations of the project, specifying what is included and excluded.",
            "Develop a timeline that outlines key milestones, tasks, and deadlines for the entire project duration.",
            "Establish financial constraints and allocations for resources, materials, and other project-related expenses.",
            "Identify and allocate human, financial, and material resources needed for the successful execution of the project.",
            "Identify potential risks that may impact the project. Develop strategies to mitigate, monitor, and manage risks throughout the project lifecycle.",
            "Define communication channels and protocols for both internal team members and external stakeholders.",
            "Break down the project into smaller, manageable tasks or activities to facilitate better planning and execution.",
            "Assign qualified engineers or team members to specific tasks based on their skills and expertise.",
            "Regularly monitor and assess the project's progress in comparison to the established schedule.",
            "Collect relevant data and requirements from the client to inform the development process.",
            "Develop the code responsible for handling data storage and retrieval.",
            "Execute and test the data layer code to identify and address any issues or bugs.",
            "Develop the code responsible for implementing the business logic and processing.",
            "Execute and test the logical layer code to ensure proper functionality and identify any issues.",
            "Create the user interface (UI) design based on project requirements and user experience principles.",
            "Execute and test the interface code to ensure a seamless user experience and address any design-related issues.",
            "Execute the entire codebase to identify and fix any integration issues or bugs.",
            "Share the designed interface with the client and relevant focus groups for feedback and validation.",
            "Address and rectify any errors or issues identified during testing or feedback sessions.",
            "Present the completed and tested project to stakeholders for final approval and delivery."
        };
        foreach (string _name in nameTasks)
        {
            string? _description = descripTasks[i];
            
            DateTime _createdAtDate = DateTime.Now; //Entering the creation time - the current time at the time of creation
            DateTime? _deadlineDate = _createdAtDate.AddDays(rnd.Next(1, 60)); // deadline - random draw from another day for another two months

            TimeSpan? _requiredEffortTime=_deadlineDate-_createdAtDate; //requiredEffortTime - the end date minus the creation time

            bool _isMilestone = false;

            LevelEngineer _complexity = LevelEngineer.Beginner; //Level adjustment for each task at random
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

            int _engineerId; //Matching an appropriate engineer (by lottery) taking into account the level of the engineer required for the task
            Engineer? en;
            do
            {
                _engineerId = rnd.Next(200000000, 400000000);
                en = s_dal?.Engineer.Read(_engineerId);
            }
            while (en != null && en.Level >= _complexity);

            Task newTa = new Task(0,_name,_description,_createdAtDate,
                _requiredEffortTime,_isMilestone,_complexity,_startDate,
                _deadlineDate,_completeDate,_deliverables,_remarks,_engineerId);
            s_dal!.Task.Create(newTa);
            i++;
        }
    }
    private static void createDependency()
    {
        Dependency newDep;
        int i;
        for (i = 1; i < 20; i++) //Mission 20 depends on completing all other missions
        {
            newDep = new Dependency(0, 20, i);
            s_dal!.Dependency.Create(newDep);
        }
        for (i = 2; i <= 5; i++) //Tasks 2-5 depend on completing Task 1
        {
            newDep = new Dependency(0, i, 1);
            s_dal!.Dependency.Create(newDep);
        }
        for (int j = 4; i <= 5; i++) //Tasks 4,5 depend on completing tasks 2,3
            for (i = 2; i <= 3; i++)
            {
                newDep = new Dependency(0, j, i);
                s_dal!.Dependency.Create(newDep);
            }
        newDep = new Dependency(0, 5, 4); //Task 5 also depends on the completion of task 4
        s_dal!.Dependency.Create(newDep);
        newDep = new Dependency(0, 8, 7); //Task 8 depends on the completion of task 7
        s_dal!.Dependency.Create(newDep);
        for (i = 11; i <= 19; i += 2) //Tasks 11,13,15,17,19 depend on completing Tasks 8,10
        {
            Dependency newDep1 = new Dependency(0, i, 8);
            s_dal!.Dependency.Create(newDep1);
            Dependency newDep2 = new Dependency(0, i, 10);
            s_dal!.Dependency.Create(newDep2);
        }
        for (i = 12; i <= 18; i += 2) //Tasks 12, 14, 16 and 18 each depend on the task that precedes it chronologically
        {
            newDep = new Dependency(0, i, i - 1);
            s_dal!.Dependency.Create(newDep);
        }
    }
    public static void Do(IDal dal)
    {
       
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        createEngineer();
        createTask();
        createDependency();
    }
}

