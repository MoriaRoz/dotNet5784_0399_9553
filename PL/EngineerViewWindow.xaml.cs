using BO;
using DalApi;
using DO;
using PL.Engineer;
using PL.Task;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    public partial class EngineerViewWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private bool _engHasTask;
        private bool _engDoesntHasTask;
        private double _projectProgress;
        public EngineerViewWindow(int engineerId = 0)
        {
            InitializeComponent();
            try { CurrentEngineer = engineerId != 0 ? s_bl.Engineer.Read(engineerId) : null; }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK); }
        }

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }
        public static readonly DependencyProperty EngineerProperty =
         DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));
        public bool EngHasTask 
        {
            get { return _engHasTask; }
        }

        public bool EngDoesntHasTask
        {
            get { return _engDoesntHasTask; }
        }
        private void checkIfEngHasTask()
        {
            BO.TaskInEngineer engTask = CurrentEngineer.Task;
            if (engTask != null)
            {
                _engHasTask = true;
                _engDoesntHasTask = false;
            }
            else
            {
                _engHasTask = false;
                _engDoesntHasTask = true;
            }
        }
        private IEnumerable<BO.TaskInList>? engineerTasks;

        private void LoadEngineerTasks(int engineerId)
        {
            engineerTasks = s_bl.Task.ReadAll(task => task.Complexity <= s_bl.Engineer.Read(engineerId).Level && task.Engineer == null);
        }
        private void ListView_ChooseTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("choosing a task", "Do you want to work on this task?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                BO.TaskInList taskinlist  = (sender as ListView)?.SelectedItem as BO.TaskInList;
                if (taskinlist != null)
                {
                    BO.Task? currentTask = s_bl.Task.Read(taskinlist.Id);
                    if (currentTask != null)
                    {
                        currentTask.Engineer = new EngineerInTask { Id = CurrentEngineer.Id, Name = CurrentEngineer.Name };
                        s_bl.Task.Update(currentTask); 
                        LoadEngineerTasks(CurrentEngineer.Id); 
                        _engDoesntHasTask = false;
                        _engHasTask = true;
                    }
                }
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            Close();
        }
        //processbur:
        private void CalculateProjectProgress()
        {
            var tasks = s_bl.Task.ReadAll();
            int totalTasks = tasks.Count();
            int numOfDoneTasks = tasks.Count(t => t.Status == BO.Statuses.Done);
            double progressPercentage = (numOfDoneTasks / (double)totalTasks) * 100;
            _projectProgress = progressPercentage;
        }
        public double ProjectProgress
        {
            get { return _projectProgress; }
        }
    }
}
