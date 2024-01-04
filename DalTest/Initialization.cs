namespace DalTest;
using DalApi;
using DO;
public static class Initialization
    {
        private static IEngineer? s_dalEngineer; //stage 1
        private static IDependency? s_dalDependency; //stage 1
        private static ITask? s_dalTask; //stage 1

        private static readonly Random s_rand = new();
        private static void createSEngineer()
        {
            string[] engineerNames =
            {
            "Dani Levi", "Eli Amar", "Yair Cohen",
            "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

            foreach (var _name in engineerNames)
            {
                int _id;
                do
                    _id = s_rand.Next(MIN_ID, MAX_ID);
                while (s_dalStudent!.Read(_id) != null);


                DateTime start = new DateTime(1995, 1, 1);
                int range = (DateTime.Today - start).Days;
                DateTime _bdt = start.AddDays(s_rand.Next(range));

                string? _alias = (_id % 2) == 0 ? _name + "ALIAS" : null;

                Student newStu = new(_id, _name, _alias, _b, _bdt);

                s_dalStudent!.Create(newStu);
            }
        }

}

