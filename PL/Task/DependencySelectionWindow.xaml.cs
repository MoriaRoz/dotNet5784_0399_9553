using BO;
using PL.Engineer;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for DependencySelectionWindow.xaml
    /// </summary>
    public partial class DependencySelectionWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DependencySelectionWindow(BO.Task task)
        {
            CurrentTask = task;
            ListTasks = new List<TaskInList>();
            SelectedTasks = new List<TaskInList>();
            if (CurrentTask!=null)
            {
                try
                {

                    bool addDep = true;
                    foreach (BO.TaskInList t in s_bl.Task.ReadAll())
                    {
                        foreach (BO.TaskInList taskDep in CurrentTask.Dependencies)
                        {
                            if (t.Id == taskDep.Id)
                                addDep = false;
                        }
                        if (addDep)
                            ListTasks.Add(t);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
            InitializeComponent();
        }

        #region Property
        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(DependencySelectionWindow), new PropertyMetadata(null));
        
        public List<BO.TaskInList> ListTasks
        {
            get { return (List<BO.TaskInList>)GetValue(ListTaskProperty); }
            set { SetValue(ListTaskProperty, value); }
        }
        public static readonly DependencyProperty ListTaskProperty =
            DependencyProperty.Register("ListTasks", typeof(List<BO.TaskInList>), typeof(DependencySelectionWindow), new PropertyMetadata(null));

        public List<BO.TaskInList> SelectedTasks
        {
            get { return (List<BO.TaskInList>)GetValue(SelectedTaskProperty); }
            set { SetValue(SelectedTaskProperty, value); }
        }
        public static readonly DependencyProperty SelectedTaskProperty =
            DependencyProperty.Register("SelectedTasks", typeof(List<BO.TaskInList>), typeof(DependencySelectionWindow), new PropertyMetadata(null));
        #endregion

        private void Btn_Done_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentTask.Dependencies.Union(SelectedTasks).ToList();
                s_bl.Task.Update(CurrentTask);
                MessageBox.Show($"Task {CurrentTask.Id} dependencies added successfully");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
            Close();
        }
    }
}
