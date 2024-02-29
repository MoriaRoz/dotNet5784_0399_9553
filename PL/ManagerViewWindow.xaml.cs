using BlApi;
using BO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ManagerViewWindow.xaml
    /// </summary>
    public partial class ManagerViewWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();
        public ManagerViewWindow(int id)
        {
            InitializeComponent();

        }
        private void btnTask_Click(object sender, RoutedEventArgs e)
        {
            Task.TaskListWindow taskListWindow = new Task.TaskListWindow();
            taskListWindow.Show();
        }
        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        {
            Engineer.EngineerListWindow engineerListWindow = new Engineer.EngineerListWindow();
            engineerListWindow.Show();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            Close();
        }
        public class BooleanToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return s_bl.GetProjectStartDate() != null ? Visibility.Visible : Visibility.Collapsed;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        { 
        }
    }
}
