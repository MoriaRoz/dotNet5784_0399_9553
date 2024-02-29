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
using BO;
using System;
using System.Windows;

namespace PL
{
    public partial class CreateUserWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public CreateUserWindow()
        {
            InitializeComponent();
        }
        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        private void Button_CreateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                s_bl?.User.Create(CurrentUser);
                MessageBox.Show("User created successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static readonly DependencyProperty UserProperty =
   DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(LoginPage), new PropertyMetadata(null));


        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();

            // Show the login page
            loginPage.Show();

            // Close the current window (CreateUserWindow)
            Close();
        }
    }
}
