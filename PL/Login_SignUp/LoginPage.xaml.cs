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
            Id = "";
            Password = "";
            InitializeComponent();
        }

        #region Property
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public static readonly DependencyProperty IdProperty =
        DependencyProperty.Register("Id", typeof(string), typeof(LoginPage), new PropertyMetadata(null));
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(LoginPage), new PropertyMetadata(null));
        #endregion
        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (Id == "" || Password == "")
            {
                MessageBox.Show("No ID or password was entered", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                BO.User user = s_bl.User.Read(int.Parse(Id));
                if (user == null)
                {
                    MessageBox.Show("User with the provided ID does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string providedPassword = Password;
                if (user.Password != providedPassword)
                {
                    MessageBox.Show("Incorrect password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if ((BO.UserRole)user.Role == BO.UserRole.Manager)
                {
                    new View.ManagerViewWindow(int.Parse(Id)).ShowDialog();
                }
                else if ((BO.UserRole)user.Role == BO.UserRole.Engineer)
                {
                    new View.EngineerViewWindow(int.Parse(Id)).Show();
                }

                Close();
            }
            catch (Exception ex){MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);}
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
            new Login_SingUP.SingUpWindow().Show();
            Close();
        }
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
