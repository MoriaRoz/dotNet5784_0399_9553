using BO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Login_SingUP
{
    /// <summary>
    /// Code-behind Login window
    /// </summary>
    public partial class LoginPage : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public LoginPage()
        {
            InitializeComponent();
        }

        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public static readonly DependencyProperty UserProperty =
                DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(LoginPage), new PropertyMetadata(null));

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                MessageBox.Show("There is no user in the system that matches this ID, to create a user click on the appropriate button.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                BO.User user = s_bl.User.Read(CurrentUser.EngineerId);
                if (user == null)
                {
                    MessageBox.Show("User with the provided ID does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string providedPassword = CurrentUser.Password;
                if (user.Password != providedPassword)
                {
                    MessageBox.Show("Incorrect password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.Role == BO.UserRole.Manager)
                {
                    new View.ManagerViewWindow(CurrentUser.EngineerId).ShowDialog();
                }
                else if (user.Role == BO.UserRole.Engineer)
                {
                    new View.EngineerViewWindow(CurrentUser.EngineerId).Show();
                }

                Close();
            }
            catch (Exception ex){MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);}
        }
        private void Create_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            new Login_SingUP.SingUpWindow().Show();
        }
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
