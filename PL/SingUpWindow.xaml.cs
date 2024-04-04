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
using DalTest;
using PL.Engineer;

namespace PL;
/// <summary>
/// Code-behind Sing up window
/// </summary>
public partial class SingUpWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public SingUpWindow()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(SingUpWindow), new PropertyMetadata(null));
    public BO.User CurrentUser
    {
        get => (BO.User)GetValue(UserProperty);
        set { SetValue(UserProperty, value); }
    }

    private void BtnCreate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl?.User.Create(CurrentUser);
            MessageBox.Show("User created successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            if (CurrentUser.Role == BO.UserRole.Engineer)
                new EngineerViewWindow(CurrentUser.Id).Show();
            if (CurrentUser.Role == BO.UserRole.Manager)
                new ManagerViewWindow(CurrentUser.Id).ShowDialog();    
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnBackLogin_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
