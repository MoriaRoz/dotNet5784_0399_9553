using BO;
using DalApi;
using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
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

        private SecureString _password;

        public SecureString Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(LoginPage), new PropertyMetadata(null));

        private void Button_CreateUser_Click(object sender, RoutedEventArgs e)
        {
            CreateUserWindow createUserWindow = new CreateUserWindow();
            createUserWindow.Show();
            Close();
        }

        private DependencyPropertyChangedEventArgs nameof(SecureString password)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = s_bl?.User.Read(CurrentUser.EngineerId);
                if (user == null)
                {
                    MessageBox.Show("User with the provided ID does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var passwordBox = sender as PasswordBox;
                string enteredPassword = passwordBox.Password;
                var securePassword = user.Password;
                string storedPassword = new System.Net.NetworkCredential(string.Empty, securePassword).Password;

                if (enteredPassword != storedPassword)
                {
                    MessageBox.Show("Incorrect password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.Rool == BO.UserRole.Manager)
                {
                    ManagerViewWindow managerViewWindow = new ManagerViewWindow(CurrentUser.EngineerId);
                    managerViewWindow.Show();
                }
                else if (user.Rool == BO.UserRole.Engineer)
                {
                    EngineerViewWindow engineerViewWindow = new EngineerViewWindow(CurrentUser.EngineerId);
                    engineerViewWindow.Show();
                }

                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SecureString securePassword = new SecureString();
                foreach (char c in passwordBox.Password)
                {
                    securePassword.AppendChar(c);
                }

                Password = securePassword;
            }
        }
    }
}
