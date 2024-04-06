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

namespace PL
{
    /// <summary>
    /// Interaction logic for StartDateWindow.xaml
    /// </summary>
    
    public partial class StartDateWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public StartDateWindow()
        {
            InitializeComponent();
        }
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDate != null)
            {
                try
                {
                    s_bl.CreateSchedule(SelectedDate.Value);
                }
                catch (Exception ex){MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);}
            
            }
            else
                MessageBox.Show("No date entered");
            this.Close();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(StartDateWindow), new PropertyMetadata(null));
    }
}
