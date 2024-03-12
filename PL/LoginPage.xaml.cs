using BO;
using DalApi;
using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL;

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

    private DependencyPropertyChangedEventArgs nameof(SecureString password)
    {
        throw new NotImplementedException();
    }

    private void Btn_Login_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            BO.User user = s_bl.User.Read(CurrentUser.EngineerId);
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
            if (user.Role == BO.UserRole.Manager)
            {
                new ManagerViewWindow(CurrentUser.EngineerId).ShowDialog();
            }
            else if (user.Role == BO.UserRole.Engineer)
            {
                // Open the EngineerViewWindow
                new EngineerViewWindow(CurrentUser.EngineerId).Show();
            }
            Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
