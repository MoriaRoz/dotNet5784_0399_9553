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
    /// Code-behind Manager window
    /// </summary>
    public partial class ManagerViewWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();
        public BO.User CurrentUser { get; set; }
        public ManagerViewWindow(int id)
        {
            InitializeComponent();
            //CurrentUser = s_bl.User.Read(id);
        }
        private void Btn_TaskList_Click(object sender, RoutedEventArgs e)
        {
            new Task.TaskListWindow().Show();
        }
        private void Btn_EngineerList_Click(object sender, RoutedEventArgs e)
        {
            new Engineer.EngineerListWindow().Show();
        }
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
