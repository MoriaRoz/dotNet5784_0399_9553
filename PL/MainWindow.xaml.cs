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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
            //Uri iconUri = new Uri("pack:dotNet5784_0399_9553/PL/LogoIcon.ico", UriKind.RelativeOrAbsolute);
            //this.Icon = BitmapFrame.Create(iconUri);
        }

        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
            //new TaskListWindow().Show();
        }

        private void btnInitDB_Click(object sender, RoutedEventArgs e)
        {

            var result = MessageBox.Show("Initialize Database?", "Confirm Initialization", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
               s_bl.InitializeDB();
            }

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Reset Database?", "Confirm Initialization", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
        }
    }
}