using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

namespace PL.Engineer;

/// <summary>
/// Code-behind selecting a task for an engineer window
/// </summary>
public partial class TaskSelectionWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public TaskSelectionWindow(BO.Engineer eng)
    {
        InitializeComponent();
        CurrentEng = eng;
        ListTasks = new List<TaskInEngineer>();
        if (CurrentEng.Level != BO.LevelEngineer.None)
        {
            foreach (BO.TaskInList task in s_bl.Task.ReadAll())
            {
                BO.Task? t = s_bl.Task.Read(task.Id);
                if (t != null)
                {
                    if (t.Status == BO.Statuses.Scheduled && t.Complexity <= CurrentEng.Level)
                        ListTasks.Add(new BO.TaskInEngineer { Id = t.Id, Alias = t.Alias });
                }
            }
        }
    }
    public BO.Engineer CurrentEng
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("CurrentEng", typeof(BO.Engineer), typeof(TaskSelectionWindow), new PropertyMetadata(null));
    public List<BO.TaskInEngineer> ListTasks
    {
        get { return (List<BO.TaskInEngineer>)GetValue(ListTaskProperty); }
        set { SetValue(ListTaskProperty, value); }
    }

    public static readonly DependencyProperty ListTaskProperty =
        DependencyProperty.Register("ListTasks", typeof(List<BO.TaskInEngineer>), typeof(TaskSelectionWindow), new PropertyMetadata(null));

    public BO.TaskInEngineer SelectedTask
    {
        get { return (BO.TaskInEngineer)GetValue(SelectedTaskProperty); }
        set { SetValue(SelectedTaskProperty, value); }
    }

    public static readonly DependencyProperty SelectedTaskProperty =
        DependencyProperty.Register("SelectedTask", typeof(BO.TaskInEngineer), typeof(TaskSelectionWindow), new PropertyMetadata(null));

    private void Btn_Done_Click(object sender, RoutedEventArgs e)
    {
        CurrentEng.Task = SelectedTask;
        s_bl.Engineer.Update(CurrentEng);
        MessageBox.Show($"{CurrentEng.Name} was assigned to the task {SelectedTask.Id}");
        Close();
    }
}
