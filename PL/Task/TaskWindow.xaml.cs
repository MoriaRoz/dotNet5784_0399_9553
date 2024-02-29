//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace PL.Task
//{

//    /// <summary>
//    /// Interaction logic for TaskWindow.xaml
//    /// </summary>
//    public partial class TaskWindow : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//        public TaskWindow()
//        {
//            InitializeComponent();
//            var EngineerList = s_bl.Engineer.ReadAll();
//            var TasksList = s_bl.Task.ReadAll();
//        }
//        public TaskWindow(int Id = 0)
//        {
//            InitializeComponent();
//            if (Id == 0)
//                CurrentTask = new();
//            else
//                try { CurrentTask = s_bl.Task.Read(Id); }
//                catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK); }
//        }
//        public BO.Task CurrentTask
//        {
//            get { return (BO.Task)GetValue(TaskProperty); }
//            set { SetValue(TaskProperty, value); }
//        }
//        public static readonly DependencyProperty TaskProperty =
//            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

//        private void BtnAddOrUpdate_Click(object sender, RoutedEventArgs e)
//        {
//            Button? btn = sender! as Button;
//            if (btn != null)
//            {
//                try
//                {
//                    if (btn.Content.ToString() == "Add")
//                    {
//                        s_bl.Task.Create(CurrentTask);
//                        MessageBox.Show($"Task with Id:{CurrentTask.Id} added successfully");
//                    }
//                    if (btn.Content.ToString() == "Update")
//                    {
//                        s_bl.Task.Update(CurrentTask);
//                        MessageBox.Show($"Task with Id:{CurrentTask.Id} successfully updated");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                }
//                Close();
//            }
//        }

//        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {

//        }
//    }
//}
