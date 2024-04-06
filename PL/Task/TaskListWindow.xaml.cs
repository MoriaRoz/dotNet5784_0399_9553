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

namespace PL.Task;

/// <summary>
/// Code-behind List Task window
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public TaskListWindow()
    {
        ProjectStatus=s_bl.GetProjectStatus();
        InitializeComponent();
    }

    #region Properties
    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
     
    public BO.ProjectStatus ProjectStatus
    {
        get { return (BO.ProjectStatus)GetValue(ProjectStatusProperty); }
        set { SetValue(ProjectStatusProperty, value); }
    }
    public static readonly DependencyProperty ProjectStatusProperty =
        DependencyProperty.Register("ProjectStatus", typeof(BO.ProjectStatus), typeof(TaskListWindow), new PropertyMetadata(null));
    #endregion

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
        try
        {
            MenuItem? statusSelect = e.OriginalSource as MenuItem;
            BO.Statuses status = (BO.Statuses)statusSelect.Header;
            TaskList = s_bl.Task.ReadAll(item => item.Status == status);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    private void selectComplexity_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            MenuItem? complexitySelect = e.OriginalSource as MenuItem;
            BO.LevelEngineer complexity = (BO.LevelEngineer)complexitySelect.Header;
            if (complexity == BO.LevelEngineer.None)
                TaskList = s_bl.Task.ReadAll();
            else
                TaskList = s_bl.Task.ReadAll(item => item.Complexity == complexity);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    private void ActivatedRefresh(object sender, EventArgs e)
    {
        try
        {
            TaskList = s_bl.Task.ReadAll();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    private void Back_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
