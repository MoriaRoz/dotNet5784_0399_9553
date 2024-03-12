using BO;
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
    /// <summary>
    /// Interaction logic for EngineerViewWindow.xaml
    /// </summary>
    public partial class EngineerViewWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public EngineerViewWindow(int Id)
        {
            InitializeComponent();
            try { CurrentEngineer = s_bl.Engineer.Read(Id); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK); }
            ListTasks = new List<BO.TaskInEngineer>();

            if (CurrentEngineer.Task == null)
            {
                foreach (BO.TaskInList task in s_bl.Task.ReadAll())
                {
                    BO.Task t = s_bl.Task.Read(task.Id);
                    if (t.Status == BO.Statuses.Scheduled && t.Complexity <= CurrentEngineer.Level)
                        ListTasks.Add(new BO.TaskInEngineer { Id = t.Id, Alias = t.Alias });
                }
            }
        }

        #region Property
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerViewWindow), new PropertyMetadata(null));

        public List<BO.TaskInEngineer> ListTasks
        {
            get { return (List<BO.TaskInEngineer>)GetValue(ListTaskProperty); }
            set { SetValue(ListTaskProperty, value); }
        }

        public static readonly DependencyProperty ListTaskProperty =
            DependencyProperty.Register("ListTasks", typeof(List<BO.TaskInEngineer>), typeof(EngineerViewWindow), new PropertyMetadata(null));

        public BO.TaskInEngineer SelectedTask
        {
            get { return (BO.TaskInEngineer)GetValue(SelectedTaskProperty); }
            set { SetValue(SelectedTaskProperty, value); }
        }

        public static readonly DependencyProperty SelectedTaskProperty =
            DependencyProperty.Register("SelectedTask", typeof(BO.TaskInEngineer), typeof(EngineerViewWindow), new PropertyMetadata(null));
        #endregion

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            new LoginPage().Show();
            Close();
        }

        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            BO.Task selectTask = s_bl.Task.Read(SelectedTask.Id);
            var result = MessageBox.Show($"Do you want to work on task Id:{selectTask.Id}?", "choosing a task", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (selectTask != null)
                {
                    selectTask.Engineer = new EngineerInTask { Id = CurrentEngineer.Id, Name = CurrentEngineer.Name };
                    selectTask.Status = BO.Statuses.Started;
                    selectTask.StartDate = s_bl.Clock;
                    s_bl.Task.Update(selectTask);
                    CurrentEngineer.Task = new TaskInEngineer { Id = selectTask.Id, Alias = selectTask.Alias };
                    s_bl.Engineer.Update(CurrentEngineer);
                }
            }
            RefreshWindow();
        }

        private void Btn_Done_Click(object sender, RoutedEventArgs e)
        {
            BO.Task doneTask = s_bl.Task.Read(CurrentEngineer.Task.Id);
            if (doneTask != null) 
            { 
                doneTask.Status = BO.Statuses.Done;
                doneTask.CompleteDate = s_bl.Clock;
                CurrentEngineer.Task = null;
                s_bl.Task.Update(doneTask);
                s_bl.Engineer.Update(CurrentEngineer);
            }
            RefreshWindow();
        }

        private void RefreshWindow()
        {
            CurrentEngineer = s_bl.Engineer.Read(CurrentEngineer.Id);
            ListTasks = new List<BO.TaskInEngineer>();

            foreach (BO.TaskInList task in s_bl.Task.ReadAll())
            {
                BO.Task t = s_bl.Task.Read(task.Id);
                if (t.Status == BO.Statuses.Scheduled && t.Complexity <= CurrentEngineer.Level)
                    ListTasks.Add(new BO.TaskInEngineer { Id = t.Id, Alias = t.Alias });
            }
        }

        #region progresBar
        private double _projectProgress;

        public double ProjectProgress
        {
            get { return _projectProgress; }
            set
            {
                //if (_projectProgress != value)
                //{
                //    _projectProgress = value;
                //    OnPropertyChanged(nameof(ProjectProgress));
                //}
            }
        }

        private void CalculateProjectProgress()
        {
            var tasks = s_bl.Task.ReadAll();
            int totalTasks = tasks.Count();
            int numOfDoneTasks = tasks.Count(t => t.Status == BO.Statuses.Done);
            double progressPercentage = (numOfDoneTasks / (double)totalTasks) * 100;
            //    ProjectProgress = progressPercentage;
            //}

        }
        #endregion
    }
}

