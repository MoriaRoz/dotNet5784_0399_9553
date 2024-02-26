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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public EngineerWindow(int Id=0)
        {
            InitializeComponent();
            if (Id == 0)
                CurrentEngineer = new();
            else
                try { CurrentEngineer = s_bl.Engineer.Read(Id); }
                catch(Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK); }
        }
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        public BO.LevelEngineer Level { get; set; } = BO.LevelEngineer.None;

        private void EngLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = s_bl.Task.ReadAll(Level);
        }
        private void BtnAddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button? btn = sender! as Button;
            if (btn != null)
            {
                try
                {
                    if (btn.Content.ToString() == "Add")
                    {
                        s_bl.Engineer.Create(CurrentEngineer);
                        MessageBox.Show($"Engineer with Id:{CurrentEngineer.Id} added successfully");
                    }
                    if (btn.Content.ToString() == "Update")
                    {
                        s_bl.Engineer.Update(CurrentEngineer);
                        MessageBox.Show($"Engineer with Id:{CurrentEngineer.Id} successfully updated");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Close();
            }
        }
    }
}