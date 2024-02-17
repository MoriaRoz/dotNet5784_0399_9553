using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }

        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        public BO.LevelEngineer Level { get; set; } = BO.LevelEngineer.None;

        private void EngLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Level == BO.LevelEngineer.None) ?
                s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Level)!;

        }

        private void btnAddEng_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
            EngineerList = s_bl.Engineer.ReadAll();
        }

        private void ListView_UpdateEng_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? eng = (sender as ListView)?.SelectedItem as BO.Engineer;
            new EngineerWindow(eng.Id).ShowDialog();
            EngineerList = s_bl.Engineer.ReadAll();
        }
    }
}
