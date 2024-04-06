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

namespace PL
{
    /// <summary>
    /// Code-behind Login window
    /// </summary>
    public partial class LoginPage : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        #region Property
        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.User), typeof(LoginPage), new PropertyMetadata(null));
        public BO.UserRole role
        {
            get { return (BO.UserRole)GetValue(RoleProperty); }
            set { SetValue(RoleProperty, value); }
        }
        public static readonly DependencyProperty RoleProperty =
            DependencyProperty.Register("Role", typeof(BO.UserRole), typeof(LoginPage), new PropertyMetadata(null));
        #endregion
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                MessageBox.Show("There is no user in the system that matches this ID, to create a user click on the appropriate button.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                BO.User user = s_bl.User.Read(CurrentUser.Id);
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
                    new ManagerViewWindow(CurrentUser.Id).ShowDialog();
                }
                else if (user.Role == BO.UserRole.Engineer)
                {
                    // Open the EngineerViewWindow
                    new EngineerViewWindow(CurrentUser.Id).Show();
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
            else
            {
                TextBox textBox = sender as TextBox;
                string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

                if (!int.TryParse(newText, out _)) 
                {
                    e.Handled = true;
                }
                else if (newText.Length > 9)
                {
                    e.Handled = true;
                }
            }
        }
        private void Create_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            new SingUpWindow().Show();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
