using PL.Engineer;
using PL.Task;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for EngineerViewWindow.xaml
    /// </summary>
    public partial class EngineerViewWindow : Window, INotifyPropertyChanged
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public EngineerViewWindow(int Id)
        {
            InitializeComponent();
            //        try {
            //        //TaskWindow taskWindow = new TaskWindow(s_bl.Task.Read(s_bl.Engineer.Read(Id).Task.Id).Id);
            //        //taskWindow.Show();
            //            }
            //        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK); }
        }
        private double _projectProgress;

        public double ProjectProgress
        {
            get { return _projectProgress; }
            set
            {
                if (_projectProgress != value)
                {
                    _projectProgress = value;
                    OnPropertyChanged(nameof(ProjectProgress));
                }
            }
        }

        private void CalculateProjectProgress()
        {
                var tasks = s_bl.Task.ReadAll();
                int totalTasks = tasks.Count();
                int numOfDoneTasks = tasks.Count(t => t.Status == BO.Statuses.Done);
                double progressPercentage = (numOfDoneTasks / (double)totalTasks) * 100;
                ProjectProgress = progressPercentage;
        }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginWindow = new LoginPage();
            loginWindow.Show();
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public class EnumToStringConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null)
                    return null;
                return ((Enum)value).ToString();
            }
        }
    }
}
