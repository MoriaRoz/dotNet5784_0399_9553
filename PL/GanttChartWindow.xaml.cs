using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for GanttChartWindow.xaml
    /// </summary>
    public partial class GanttChartWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public GanttChartWindow()
        {
            DateTime start = s_bl.Clock;
            if (s_bl.GetProjectStartDate() != null)
                start = s_bl.GetProjectStartDate() ?? s_bl.Clock;
            DateTime end = s_bl.Clock;

            var tasks = s_bl.Task.ReadAll();
            foreach (BO.TaskInList t in tasks)
            {
                BO.Task task = s_bl.Task.Read(t.Id);

                DateTime startT = task.ScheduledDate ?? s_bl.Clock;
                DateTime endT = task.ForecastDate ?? s_bl.Clock;

                if (task.StartDate != null)
                    startT = task.StartDate ?? s_bl.Clock;
                if (startT < start)
                    start = startT;

                if (endT > end)
                    end = endT;

                BO.TaskGantt tGantt = new BO.TaskGantt()
                {
                    TaskId = task.Id,
                    TaskAlias = task.Alias,
                    TaskStart = startT,
                    TaskEnd = endT,
                };
                Gantt.Add(tGantt);
            }
            foreach (BO.TaskGantt t in Gantt)
            {
                t.TimeFromStart = (t.TaskStart - start).Days;
                t.TimeToEnd = (end - t.TaskEnd).Days;
            }
            InitializeComponent();
        }
        public List<BO.TaskGantt> Gantt { get; set; }

        public static readonly DependencyProperty GanttProperty =
            DependencyProperty.Register("Gantt", typeof(List<BO.TaskGantt>), typeof(GanttChartWindow), new PropertyMetadata(null));
    }
}