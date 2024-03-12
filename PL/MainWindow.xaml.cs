using PL.Engineer;
using PL.Task;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL;
/// <summary>
/// Code-behind Main window
/// </summary>
public partial class MainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        new LoginPage().Show();
    }

    private void btnInitDB_Click(object sender, RoutedEventArgs e)
    {

        var result = MessageBox.Show("Initialize Database?", "Confirm Initialization", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes) { s_bl.InitializeDB(); }
    }

    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Reset Database?", "Confirm Initialization", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes) { s_bl.ResetDB(); }
    }

    //Temporary buttons
    private void Button_e_Click(object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().Show();
    }

    private void Button_t_Click(object sender, RoutedEventArgs e)
    {
        new TaskListWindow().Show();
    }

    private void Button_Me_Click(object sender, RoutedEventArgs e)
    {
        new EngineerViewWindow(123).Show();
    }

    private void Button_Mt_Click(object sender, RoutedEventArgs e)
    {
        new ManagerViewWindow(0).ShowDialog();
    }
}