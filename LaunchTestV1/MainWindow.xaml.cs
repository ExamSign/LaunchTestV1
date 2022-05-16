using LaunchTestV1.Repos;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaunchTestV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            xClassCmb.ItemsSource = GMeetRepo.GetClass().Select(o => int.Parse(o.Grade)).Distinct().OrderBy(o => o).ToList();
        }

        private void xClassCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xClassCmb.SelectedIndex >= 0)
            {
                string grade = xClassCmb.SelectedItem.ToString();
                xSubjectCmb.ItemsSource = GMeetRepo.GetSubjects(grade);
            }
        }

        private void xSubjectCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xSubjectCmb.SelectedIndex != -1)
            {
                //ContextClass.Subject = xSubjectCmb.SelectedItem.ToString();
                //TestItemMaster = TestInfo.GetAllTests(ContextClass.Grade, ContextClass.Subject);
                //xTestItemList.ItemsSource = TestItemMaster;
            }
        }

        private void xTestTypeCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void xTestFor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void xRollNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
