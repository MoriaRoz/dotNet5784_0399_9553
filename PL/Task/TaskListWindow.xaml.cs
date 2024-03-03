using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskListWindow()
        {
            InitializeComponent();
        }
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
        }

        private void ListView_UpdateTask_MouseDoubleClick(object sender, MouseEventArgs e) 
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
                new TaskWindow(task.Id).ShowDialog();
        }
        private void selectStatus_Click(object sender, RoutedEventArgs e)
        {
            MenuItem? statusSelect = e.OriginalSource as MenuItem;
            BO.Statuses status = (BO.Statuses)statusSelect.Header;
            TaskList = s_bl.Task.ReadAll(item => item.Status == status);
        }
        private void selectComplexity_Click(object sender, RoutedEventArgs e)
        {
            MenuItem? complexitySelect = e.OriginalSource as MenuItem;
            BO.LevelEngineer complexity = (BO.LevelEngineer)complexitySelect.Header;
            TaskList = s_bl.Task.ReadAll(item => item.Complexity == complexity);
        }

        private void ActivatedRefresh(object sender, EventArgs e)
        {
            TaskList = s_bl.Task.ReadAll();
        }
    }
}
