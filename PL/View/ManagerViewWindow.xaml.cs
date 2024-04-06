using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PL.View
{
    /// <summary>
    /// Code-behind Manager window
    /// </summary>
    public partial class ManagerViewWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();
        public BO.User CurrentUser { get; set; }
        public ManagerViewWindow(int id)
        {
            InitializeComponent();
            //CurrentUser = s_bl.User.Read(id);
        }
        private void Btn_TaskList_Click(object sender, RoutedEventArgs e)
        {
            new Task.TaskListWindow().Show();
        }
        private void Btn_EngineerList_Click(object sender, RoutedEventArgs e)
        {
            new Engineer.EngineerListWindow().Show();
        }
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            new Login_SingUP.LoginPage().Show();
            Close();
        }
        private void Btn_Schedule_Click(object sender, RoutedEventArgs e)
        {
            new StartDateWindow().Show();
        }
        private void Btn_Gantt_Click(object sender, RoutedEventArgs e)
        {
            new GanttChartWindow().Show();
        }
    }
}
