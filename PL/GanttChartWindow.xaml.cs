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
            Gantt = s_bl.tasksGantt();
            Dates = s_bl.getProjectDates();
            InitializeComponent();
        }
        public List<BO.TaskGantt> Gantt { get; set; }

        public static readonly DependencyProperty GanttProperty =
            DependencyProperty.Register("Gantt", typeof(List<BO.TaskGantt>), typeof(GanttChartWindow));

        public List<DateTime?> Dates
        {
            get { return (List<DateTime?>)GetValue(DatesProperty); }
            set { SetValue(DatesProperty, value); }
        }
        public static readonly DependencyProperty DatesProperty =
            DependencyProperty.Register("Dates", typeof(List<DateTime?>), typeof(GanttChartWindow));
    }
}