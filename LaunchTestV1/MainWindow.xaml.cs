using LaunchTestV1.Model;
using LaunchTestV1.Repos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ClassAndSubject ContextClass = new ClassAndSubject();
        string SelectedTestId = string.Empty;
        string SelectedTestType = string.Empty;
        string SelectedSection = "ALL";

        List<StudentMeta> StudentMasterList = new List<StudentMeta>();
        ObservableCollection<StudentMeta> ContextStudentList = new ObservableCollection<StudentMeta>();

        List<StudentMeta> SelectedStudentList = new List<StudentMeta>();
        ObservableCollection<string> SelectedSectionList = new ObservableCollection<string>(); 
        List<TestInfo> TestItemMaster = new List<TestInfo>();
        List<GradeAndSection> GradeAndSectionList = new List<GradeAndSection>();
        List<string> SectionList = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            GradeAndSectionList = GMeetRepo.GetClass();
            xSelectedSectionList.ItemsSource = SelectedSectionList;
            xClassCmb.ItemsSource = GradeAndSectionList.Select(o => int.Parse(o.Grade)).Distinct().OrderBy(o => o).ToList();
        }

        private void xClassCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xClassCmb.SelectedIndex >= 0)
            {
                ResetToDefault();
                ContextClass.Grade = xClassCmb.SelectedItem.ToString();
                xSubjectCmb.ItemsSource = GMeetRepo.GetSubjects(ContextClass.Grade);
                xSection.ItemsSource = GetSection(ContextClass.Grade);
                StudentMasterList = GMeetRepo.GetStudents(ContextClass.Grade);
                GetStudentList();
            }
        }
        private void xSubjectCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xSubjectCmb.SelectedIndex != -1)
            {
                ContextClass.Subject = xSubjectCmb.SelectedItem.ToString();
                TestItemMaster = TestInfo.GetAllTests(ContextClass.Grade, ContextClass.Subject);
                xTestTypeCmb.ItemsSource = TestItemMaster.Select(o => o.TestType).Distinct().ToList();
            }
        }
        private void xTestTypeCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(xTestTypeCmb.SelectedIndex >= 0)
            {
                SelectedTestType = xTestTypeCmb.SelectedItem.ToString();
                List<string> contextTestIdList = TestItemMaster.
                    Where(o => o.Grade == ContextClass.Grade && o.Subject == ContextClass.Subject && o.TestType == SelectedTestType).Select(o => o.TestId).ToList();
                xTestIDCmb.ItemsSource = contextTestIdList;
            }
        }
        private void xTestIDCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xTestIDCmb.SelectedIndex >= 0)
            {
                SelectedTestId = xTestIDCmb.SelectedItem.ToString();
                xTestSummary.ItemsSource = GetTestDetails(SelectedTestId);
            }
        }
        //
        private void xSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xSection.SelectedIndex >= 0)
            {
                SelectedSection = xSection.SelectedItem.ToString();
                SectionSelectionProcess();
                if (SelectedSection == "ALL")
                    xTestForCB.IsChecked = false;

                xSection.SelectedIndex = -1;
            }
        }
        private void xSelectedSectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xSelectedSectionList.SelectedIndex >= 0)
            {
                string removeItem = xSelectedSectionList.SelectedItem.ToString();
                SelectedSectionList.Remove(removeItem);

                ContextStudentList.Clear();
                foreach (var item in StudentMasterList)
                {
                    if (SelectedSectionList.Contains(item.Section))
                        ContextStudentList.Add(item);
                }

                if (SelectedSectionList.Count == 1)
                    xRollNumberPanel.Visibility = Visibility.Visible;
                else
                    xRollNumberPanel.Visibility = Visibility.Collapsed;
            }
        }
        //
        private void xTestForCB_Checked(object sender, RoutedEventArgs e)
        {
            xTestForCB.IsChecked = true;

            if (xTestForCB.IsChecked == true)
                xSearchStudentPanel.Visibility = Visibility.Visible;
            else
                xSearchStudentPanel.Visibility = Visibility.Collapsed;
        }
        private void xTestForCB_Unchecked(object sender, RoutedEventArgs e)
        {
            xTestForCB.IsChecked = false;
            xSearchStudentPanel.Visibility = Visibility.Collapsed;
        }
        //
        private void xRollNoTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(xRollNoTB.Text.Length > 0)
            {
                //GetStudentList();
            }
        }
        

        void GetStudentList()
        {
            xStudentList.ItemsSource = ContextStudentList;
        }
        List<Summary> GetTestDetails(string testID)
        {
            List<TestSummaryBindModel> TestDetails = TestInfo.GetTestDetails(testID);

            string[] distinctHeading = TestDetails.Select(o => o.Heading).Distinct().ToArray();
            List<Summary> toReturn = new List<Summary>();
            foreach (string heading in distinctHeading)
            {
                string[] distinctTopic = TestDetails.Where(o => o.Heading == heading).Select(o => o.Topic).ToArray();
                Summary headingSummary = new Summary();
                headingSummary.Heading = heading;
                List<SubSummary> subList = new List<SubSummary>();
                foreach (string topic in distinctTopic)
                {
                    SubSummary obj = new SubSummary();
                    obj.Topic = topic;
                    obj.Mark = TestDetails.Where(o => o.Heading == heading && o.Topic == topic).Select(o => o.Mark).FirstOrDefault();
                    obj.QuestionCount = TestDetails.Where(o => o.Heading == heading && o.Topic == topic).Select(o => o.Count).FirstOrDefault();
                    obj.TotalMark = obj.Mark * obj.QuestionCount;
                    subList.Add(obj);
                }
                headingSummary.SubSummaryList = subList;
                toReturn.Add(headingSummary);
            }
            return toReturn;
        }
        List<string> GetSection(string grade)
        {
            SectionList = GradeAndSectionList.Where(o => o.Grade == grade).Select(o => o.Section).ToList();
            SectionList.Insert(0, "ALL");
            return SectionList;
        }
        void ResetToDefault()
        {
            xSubjectCmb.ItemsSource = null;
            xTestTypeCmb.ItemsSource = null;
            xTestIDCmb.ItemsSource = null;
            xTestSummary.ItemsSource = null;
            xSelectedStudentList.ItemsSource = null;

            xRollNoTB.Text = string.Empty;
        }
        void SectionSelectionProcess()
        {
            if(SelectedSection == "ALL")
            {
                List<string> section = GetSection(ContextClass.Grade);
                section.RemoveAt(0);

                SelectedSectionList = new ObservableCollection<string>(section);
                xSelectedSectionList.ItemsSource = SelectedSectionList;
                //
                ContextStudentList.Clear();
                foreach (var item in StudentMasterList)
                    ContextStudentList.Add(item);
            }
            else
            {
                if (!SelectedSectionList.Contains(SelectedSection))
                    SelectedSectionList.Add(SelectedSection);

                //
                ContextStudentList.Clear();
                foreach (var item in StudentMasterList)
                {
                    if (SelectedSectionList.Contains(item.Section))
                        ContextStudentList.Add(item);
                }
            }
            if (SelectedSectionList.Count == 1)
                xRollNumberPanel.Visibility = Visibility.Visible;
            else
                xRollNumberPanel.Visibility = Visibility.Collapsed;

            xSelectedStudentList.ItemsSource = ContextStudentList;
        }        
    }
}
