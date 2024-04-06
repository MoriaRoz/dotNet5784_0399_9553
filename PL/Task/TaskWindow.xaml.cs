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

namespace PL.Task;

/// <summary>
/// Code-behind Task window
/// </summary>
public partial class TaskWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public TaskWindow(int id = 0)
    {
        try
        {
            if (id == 0)
                CurrentTask = new();
            else
                try { CurrentTask = s_bl.Task.Read(id) ?? new BO.Task(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK); }
            Dependencies = CurrentTask.Dependencies ?? new List<BO.TaskInList>();
            ProjectStatus = s_bl.GetProjectStatus();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        InitializeComponent();
    }

    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(TaskProperty); }
        set { SetValue(TaskProperty, value); }
    }
    public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));
    
    public List<BO.TaskInList> Dependencies
    {
        get { return (List<BO.TaskInList>)GetValue(DependenciesProperty); }
        set { SetValue(DependenciesProperty, value); }
    }
    public static readonly DependencyProperty DependenciesProperty =
        DependencyProperty.Register("Dependencies", typeof(List<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

    public BO.ProjectStatus ProjectStatus
    {
        get { return (BO.ProjectStatus)GetValue(ProjectStatusProperty); }
        set { SetValue(ProjectStatusProperty, value); }
    }
    public static readonly DependencyProperty ProjectStatusProperty =
        DependencyProperty.Register("ProjectStatus", typeof(BO.ProjectStatus), typeof(TaskWindow), new PropertyMetadata(null));


    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    private void BtnAddOrUpdate_Click(object sender, RoutedEventArgs e)
    {
        Button? btn = sender! as Button;
        if (btn != null)
        {
            try
            {
                if (btn.Content.ToString() == "Add")
                {
                    s_bl.Task.Create(CurrentTask);
                    MessageBox.Show($"Task with Id:{CurrentTask.Id} added successfully");
                }
                if (btn.Content.ToString() == "Update")
                {
                    s_bl.Task.Update(CurrentTask);
                    MessageBox.Show($"Task with Id:{CurrentTask.Id} successfully updated");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

            if (CurrentTask.Status <= BO.Statuses.Started && s_bl.GetProjectStatus() == BO.ProjectStatus.Inlanning)
            {
                MessageBoxResult result = MessageBox.Show($"You want to add dependencies to a task {CurrentTask.Id}?",
                            "Message", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    new DependencySelectionWindow(CurrentTask).ShowDialog();
                }
            }
            Close();
        }
    }
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext != null)
        {
            BO.TaskInList itemToRemove = (BO.TaskInList)button.DataContext;
            if (CurrentTask.Dependencies != null && CurrentTask.Dependencies.Contains(itemToRemove))
            {
                CurrentTask.Dependencies.Remove(itemToRemove);
                try
                { 
                    s_bl.Task.Update(CurrentTask);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }
        InitializeComponent();
    }
}
