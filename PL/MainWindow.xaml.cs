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
        CurrentDate= s_bl.Clock;
        InitializeComponent();
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

    public DateTime CurrentDate
    {
        get { return (DateTime)GetValue(CurrentDateProperty); }
        set { SetValue(CurrentDateProperty, value); }
    }

    public static readonly DependencyProperty CurrentDateProperty =
        DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

    #region Temporary buttons
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
        new EngineerViewWindow(212940399).Show();
    }

    private void Button_Mt_Click(object sender, RoutedEventArgs e)
    {
        new ManagerViewWindow(0).ShowDialog();
    }
    #endregion

    private void Btn_addDay_Click(object sender, RoutedEventArgs e)
    {
        s_bl.addDayToClock();
        CurrentDate = s_bl.Clock;
    }

    private void Btn_addHour_Click(object sender, RoutedEventArgs e)
    {
        s_bl.addHourToClock();
        CurrentDate=s_bl.Clock;
    }

    private void Btn_resetClock_Click(object sender, RoutedEventArgs e)
    {
        s_bl.restartClock();
        CurrentDate = s_bl.Clock;
    }
}