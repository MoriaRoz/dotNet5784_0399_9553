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

            if (CurrentEngineer.Task==null)
            {
                _hasNoTask = true;
                _hasTask = false;
                foreach (BO.TaskInList task in s_bl.Task.ReadAll())
                {
                    BO.Task t = s_bl.Task.Read(task.Id);
                    if (t.Status == BO.Statuses.Scheduled && t.Complexity <= CurrentEngineer.Level)
                        ListTasks.Add(new BO.TaskInEngineer { Id = t.Id, Alias = t.Alias });
                }
            }
            else
            {
                _hasNoTask = false;
                _hasTask = true;
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
        public bool _hasTask
        {
            get { return (bool)GetValue(_hasTaskProperty); }
            set { SetValue(_hasTaskProperty, value); }
        }

        public static readonly DependencyProperty _hasTaskProperty =
            DependencyProperty.Register("_hasTask", typeof(bool), typeof(EngineerViewWindow), new PropertyMetadata(null));
        public bool _hasNoTask
        {
            get { return (bool)GetValue(_hasNoTaskProperty); }
            set { SetValue(_hasNoTaskProperty, value); }
        }

        public static readonly DependencyProperty _hasNoTaskProperty =
            DependencyProperty.Register("_hasNoTask", typeof(bool), typeof(EngineerViewWindow), new PropertyMetadata(null));
        
        public List<BO.TaskInEngineer> ListTasks
        {
            get { return (List<BO.TaskInEngineer>)GetValue(ListTaskProperty); }
            set { SetValue(ListTaskProperty, value); }
        }

        public static readonly DependencyProperty ListTaskProperty =
            DependencyProperty.Register("ListTasks", typeof(List<BO.TaskInEngineer>), typeof(EngineerViewWindow), new PropertyMetadata(null));
        #endregion

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            new LoginPage().Show();
            Close();
        }

        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Done_Click(object sender, RoutedEventArgs e)
        {

        }

        private void selectTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInEngineer selectTask = (sender as ListView)?.SelectedItem as BO.TaskInEngineer;
            var result = MessageBox.Show($"Do you want to work on task Id:{selectTask.Id}?", "choosing a task", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (selectTask != null)
                {
                    BO.Task? currentTask = s_bl.Task.Read(selectTask.Id);
                    if (currentTask != null)
                    {
                        currentTask.Engineer = new EngineerInTask { Id = CurrentEngineer.Id, Name = CurrentEngineer.Name };
                        currentTask.Status = BO.Statuses.Started;
                        currentTask.StartDate = s_bl.Clock;
                        s_bl.Task.Update(currentTask);
                        CurrentEngineer.Task=new TaskInEngineer { Id = currentTask.Id,Alias=currentTask.Alias };
                        s_bl.Engineer.Update(CurrentEngineer);
                        _hasNoTask = false;
                        _hasTask = true;
                    }
                }
            }
        }
    }
}
