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
    public static List<BO.TaskInEngineer> TaskList { get; set; }
    public TaskSelectionWindow(BO.Engineer currentEng)
    {
        InitializeComponent();
        
        TaskList =new List<BO.TaskInEngineer>();
        var tasks = (from BO.TaskInList task in s_bl.Task.ReadAll()
                     let taskList = s_bl.Task.Read(task.Id)
                     where taskList.Complexity <= currentEng.Level
                     select new BO.TaskInEngineer
                     {
                         Id = taskList.Id,
                         Alias = taskList.Alias,
                     });
        foreach (BO.TaskInEngineer task in tasks)
            TaskList.Add(task);
    }
    public BO.Engineer CurrentEng
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("CurrentEng", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));
    public BO.TaskInEngineer TaskSelect { get; set; } = null;
    private void EngLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        CurrentEng.Task = TaskSelect;
    }
}
