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
using System.Reflection.Emit;

namespace PL.Login_SingUP;
/// <summary>
/// Code-behind Sing up window
/// </summary>
public partial class SingUpWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public SingUpWindow()
    {
        Id = "";
        Name = "";
        Password = "";
        Role = BO.UserRole.Engineer;
        InitializeComponent();
    }

    #region Property
    public string Id
    {
        get { return (string)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }
    public static readonly DependencyProperty IdProperty =
    DependencyProperty.Register("Id", typeof(string), typeof(SingUpWindow), new PropertyMetadata(null));

    public string Password
    {
        get { return (string)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(SingUpWindow), new PropertyMetadata(null));

    public string Name
    {
        get { return (string)GetValue(NameProperty); }
        set { SetValue(NameProperty, value); }
    }
    public static readonly DependencyProperty NameProperty =
    DependencyProperty.Register("Name", typeof(string), typeof(SingUpWindow), new PropertyMetadata(null));

    public BO.UserRole Role
    {
        get { return (BO.UserRole)GetValue(RoleProperty); }
        set { SetValue(RoleProperty, value); }
    }
    public static readonly DependencyProperty RoleProperty =
        DependencyProperty.Register("Role", typeof(BO.UserRole), typeof(SingUpWindow), new PropertyMetadata(null));
    #endregion

    private void BtnCreate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Id == "" || Name == "" || Password == "")
            {
                MessageBox.Show("One or more details were not entered", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int id = int.Parse(Id);
            BO.User user = new BO.User()
            {
                Id = id,
                Name = Name,
                Password = Password,
                Role = Role
            };
            s_bl?.User.Create(user);
            MessageBox.Show("User created successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            if (Role == BO.UserRole.Engineer)
                new View.EngineerViewWindow(id).Show();
            if (Role == BO.UserRole.Manager)
                new View.ManagerViewWindow(id).ShowDialog();
        }
        catch (Exception ex){ MessageBox.Show($"Error creating user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
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
    private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Role = Role;
    }
    private void BtnBackLogin_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}
