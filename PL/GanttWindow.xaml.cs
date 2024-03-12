//using DalTest;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace PL;

///// <summary>
///// Interaction logic for GantWindow.xaml
///// </summary>
//public partial class GanttWindow : Window
//{
//    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//    public GanttWindow()
//    {
//        InitializeComponent();

//        DateTime start = DateTime.Now;
//        if (s_bl.GetProjectStartDate() != null)
//            start = s_bl.GetProjectStartDate() ?? DateTime.Now;
//        DateTime end = DateTime.Now;


//        var tasks = s_bl.Task.ReadAll();
//        foreach (BO.TaskInList t in tasks)
//        {
//            BO.Task task = s_bl.Task.Read(t.Id);

//            DateTime startT = task.ScheduledDate ?? DateTime.Now;
//            DateTime endT = task.ForecastDate ?? DateTime.Now;

//            if (task.StartDate != null)
//                startT = task.StartDate ?? DateTime.Now;
//            if (startT < start)
//                start = startT;

//            if (endT > end)
//                end = endT;

//            BO.TaskGantt tGantt = new BO.TaskGantt()
//            {
//                TaskId = task.Id,
//                TaskAlias = task.Alias,
//                TaskStart = startT,
//                TaskEnd = endT,
//            };
//            Gantt.Add(tGantt);
//        }
//        foreach (BO.TaskGantt t in Gantt)
//        {
//            t.TimeFromStart = (t.TaskStart - start).Days;
//            t.TimeToEnd = (end - t.TaskEnd).Days;
//        }
//    }
//EnumerableExecutor
//    public List<BO.TaskGantt> Gantt
//    {
//        get { return (List<BO.TaskGantt>)GetValue(GanttProperty); }
//        set { SetValue(GanttProperty, value); }
//    }

//    public static readonly DependencyProperty GanttProperty =
//        DependencyProperty.Register("Gantt", typeof(List<BO.TaskGantt>), typeof(GanttWindow), new PropertyMetadata(null));
//}
