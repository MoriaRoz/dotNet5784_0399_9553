using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    //public partial class TaskListWindow : Window
    //{
    //    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    //    public TaskListWindow()
    //    {
    //        InitializeComponent();
    //        var TaskList = s_bl?.Task.ReadAll()!;
    //    }

    //    public IEnumerable<BO.Task> TaskList
    //    {
    //        get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
    //        set { SetValue(TaskListProperty, value); }
    //    }

    //    public static readonly DependencyProperty TaskListProperty =
    //        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));

    //    public BO.Statuses status { get; set; } = BO.Statuses.Unscheduled;

    //    //private void EngLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //    //{
    //    //    TaskList = (status == BO.Statuses.Unscheduled) ?
    //    //        s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Status == status)!;

    //    //}

    //    private void btnAddEng_Click(object sender, RoutedEventArgs e)
    //    {
    //        new TaskWindow().ShowDialog();
    //        var EngineerList = s_bl.Task.ReadAll();
    //    }

    //    private void ListView_UpdateEng_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    //    {
    //        BO.Task? eng = (sender as ListView)?.SelectedItem as BO.Task;
    //        if (eng != null)
    //            new TaskWindow(eng.Id).ShowDialog();
    //        var EngineerList = s_bl.Task.ReadAll();
    //    }
    //}
}
