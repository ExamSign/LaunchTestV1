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
        //
        ObservableCollection<StudentMeta> ContextStudentList = new ObservableCollection<StudentMeta>();
        ObservableCollection<StudentMeta> SelectedStudentList = new ObservableCollection<StudentMeta>();


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
                Process2();
                //
                if (SelectedSection == "ALL")
                    xTestForCB.IsChecked = false;
                //
                xSection.SelectedIndex = -1;
            }
        }
        private void xSelectedSectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xSelectedSectionList.SelectedIndex >= 0)
            {
                string removeItem = xSelectedSectionList.SelectedItem.ToString();
                SelectedSectionList.Remove(removeItem);
                Process2();
                //
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
            {
                xSearchStudentPanel.Visibility = Visibility.Visible;
                SelectedStudentList = new ObservableCollection<StudentMeta>();
                xSelectedStudentList.ItemsSource = SelectedStudentList;
                xTotalStudentTB.Text = SelectedStudentList.Count.ToString();
            }
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
                List<StudentMeta> searchList = ContextStudentList.Where(o => o.StudentName.ToLower().Contains(xRollNoTB.Text.ToLower())).Select(o => o).ToList();
                xStudentList.ItemsSource = searchList;
            }
            else if(xRollNoTB.Text.Length == 0)
                xStudentList.ItemsSource = ContextStudentList.Where(o => o.StudentName.ToLower().Contains(xRollNoTB.Text.ToLower())).Select(o => o).ToList();
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
            ContextClass = new ClassAndSubject();
            SelectedStudentList.Clear();

            xSubjectCmb.ItemsSource = null;
            xTestTypeCmb.ItemsSource = null;
            xTestIDCmb.ItemsSource = null;
            xTestSummary.ItemsSource = null;
            xSelectedStudentList.ItemsSource = null;
            xSelectedSectionList.ItemsSource = null;
            xTotalStudentTB.Text = SelectedStudentList.Count.ToString();
            xRollNoTB.Text = string.Empty;
        }
        void Process2()
        {
            if (SelectedSectionList.Count() == 1 && SelectedSectionList[0] == "ALL")
            {
                ContextStudentList.Clear();
                foreach (var item in StudentMasterList)
                    ContextStudentList.Add(item);
            }
            else
            {
                ContextStudentList.Clear();
                foreach (var item in StudentMasterList)
                    if (SelectedSectionList.Contains(item.Section))
                        ContextStudentList.Add(item);
            }
            xStudentList.ItemsSource = ContextStudentList;
            SelectedStudentList = ContextStudentList;
            xTotalStudentTB.Text = SelectedStudentList.Count.ToString();
            xSelectedStudentList.ItemsSource = SelectedStudentList;
        }
        void SectionSelectionProcess()
        {
            if(SelectedSection == "ALL")
            {
                List<string> section = GetSection(ContextClass.Grade);
                section.RemoveAt(0);

                SelectedSectionList = new ObservableCollection<string>(section);
                xSelectedSectionList.ItemsSource = SelectedSectionList;
            }
            else
                if (!SelectedSectionList.Contains(SelectedSection))
                    SelectedSectionList.Add(SelectedSection);
            //
            if (SelectedSectionList.Count == 1)
                xRollNumberPanel.Visibility = Visibility.Visible;
            else
                xRollNumberPanel.Visibility = Visibility.Collapsed;
        }

        private void xStudentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(xStudentList.SelectedIndex >= 0)
            {
                StudentMeta item = xStudentList.SelectedItem as StudentMeta;
                if(!SelectedStudentList.Contains(item))
                {
                    SelectedStudentList.Add(item);
                }
                xSelectedStudentList.ItemsSource = SelectedStudentList;
                xTotalStudentTB.Text = SelectedStudentList.Count.ToString();
            }
        }

        bool Validation()
        {
            string Error_Msg = "";
            if (ContextClass.Grade.Length <= 0)
            {
                Error_Msg = "Please fill the context.";
                MessageBox.Show(Error_Msg);
                return false;
            }
            else if (ContextClass.Subject.Length <= 0)
            {
                Error_Msg = "Please fill the context.";
                MessageBox.Show(Error_Msg);
                return false;
            }
            else if (SelectedTestType.Length <= 0)
            {
                Error_Msg = "Select Test Type";
                MessageBox.Show(Error_Msg);
                return false;
            }
            else if (SelectedTestId.Length <= 0)
            {
                Error_Msg = "Select the Test ID.";
                MessageBox.Show(Error_Msg);
                return false;
            }
            else if (SelectedStudentList.Count <= 0)
            {
                Error_Msg = "You have to select atleast 1 student.";
                MessageBox.Show(Error_Msg);
                return false;
            }
            return true;
        }

        private void xCreateSchedule_Click(object sender, RoutedEventArgs e)
        {
            if(Validation())
            {
                GMeetRepo.InsertScheduleData(ContextClass, SelectedTestId, SelectedStudentList.ToList());
                MessageBox.Show("Test Scheduled successfully..!");
                ResetToDefault();
            }
        }
    }
}
