using LaunchTestV1.Model;
using LaunchTestV1.Tools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchTestV1.Repos
{
    public class TestInfo : ClassAndSubject
    {
        public string TestName
        {
            get { return this.TestType + "_" + this.TypeCount; }
        }
        public DateTime Date { get; set; }
        public string TestId { get; set; }
        public string TestType { get; set; }
        public int TypeCount { get; set; }
        public string Year
        {
            get { return Date.Year.ToString(); }
        }
        public int QueCount { get; set; }
        public int TotalMarks { get; set; }
        public string SheetName { get; set; }

        public TestInfo(DateTime testDate, string testId, string testType, int typeCount, int qCount, int totalMarks)
        {
            this.TypeCount = typeCount;
            this.TestType = testType;
            this.Date = testDate;
            this.TestId = testId;
            this.QueCount = qCount;
            this.TotalMarks = totalMarks;
        }
        public TestInfo() { }

        public void PutTestId(string testId)
        {
            this.TestId = testId;
            ClassAndSubject obj = GetClassAndSubject_ForTestID(this.TestId);
            this.Grade = obj.Grade;
            this.Subject = obj.Subject;

        }
        public static ClassAndSubject GetClassAndSubject_ForTestID(string testId)
        {
            ClassAndSubject toReturn = new ClassAndSubject();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select SClass, Subject from Questions where Qid = (select top 1 QID from TestInfo where TestID = '" + testId + "')";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    toReturn.Grade = dr.GetString(0);
                    toReturn.Subject = dr.GetString(1);
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<TestInfo> GetAllTests(string grade, string subject)
        {
            string[] getYear = GetDates(DateTime.Now).Split('-');
            string fromYear = getYear[0];
            string toYear = getYear[1];
            List<TestInfo> toReturn = new List<TestInfo>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select TestId, Grade, Subject, TestTypeOf, TestCount, TestDate, QCount, TotalMarks from TestMeta where Grade = " + grade + " and Subject = '" + subject + "' and YEAR(TestMeta.TestDate) between " + fromYear + " and " + toYear + "";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    TestInfo obj = new TestInfo();
                    obj.TestId = dr.GetString(0);
                    obj.Grade = dr.GetInt32(1).ToString();
                    obj.Subject = dr.GetString(2);
                    obj.TestType = dr.GetString(3);
                    obj.TypeCount = dr.GetInt32(4);
                    obj.Date = dr.GetDateTime(5);
                    obj.QueCount = dr.GetInt32(6);
                    obj.TotalMarks = dr.GetInt32(7);

                    toReturn.Add(obj);
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<int> GetAll_AcademicTestId(string grade, string subject)
        {
            string[] getYear = GetDates(DateTime.Now).Split('-');
            string fromYear = getYear[0];
            string toYear = getYear[1];
            List<int> toReturn = new List<int>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select Distinct(TestId) from TestMeta where Grade = " + grade + " and Subject = '" + subject + "' and YEAR(TestMeta.TestDate) between " + fromYear + "and " + toYear + "";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    toReturn.Add(Convert.ToInt32(dr.GetString(0)));
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<int> GetTestQuestionId(int testId)
        {
            List<int> toReturn = new List<int>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select QID from TestInfo where TestId = " + testId + "";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    toReturn.Add(Convert.ToInt32(dr.GetString(0)));
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<int> GetAllFileId(string grade, string subject)
        {
            List<int> toReturn = new List<int>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select distinct TestId from TestInfo where QID in (select Qid from Questions where SClass = " + grade + " and Subject = '" + subject + "')";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    toReturn.Add(Convert.ToInt32(dr.GetString(0)));
                }
                dr.Close();
            }
            toReturn.Sort();
            return toReturn;
        }
        public static List<TestSummaryBindModel> GetTestDetails(string testId)
        {
            List<TestSummaryBindModel> toReturn = new List<TestSummaryBindModel>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select Heading, Topic, Mark, PresentCount, Total from TestDetails where TestId = '" + testId + "'";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    TestSummaryBindModel obj = new TestSummaryBindModel();
                    obj.Heading = dr.GetString(0);
                    obj.Topic = dr.GetString(1);
                    obj.Mark = dr.GetInt32(2);
                    obj.Count = dr.GetInt32(3);
                    obj.Total = dr.GetInt32(4);
                    toReturn.Add(obj);
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<int> GetTestQuestions(string testId)
        {
            List<int> toReturn = new List<int>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select distinct Qid from Questions where Qid in (select QID from TestInfo where TestId = '" + testId + "') order by qid asc";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    toReturn.Add(dr.GetInt32(0));
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<int> GetUploadedQuestions(string fileId)
        {
            List<int> toReturn = new List<int>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select distinct(Qid) from QuestionOrigin where FileId = " + fileId + "";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    toReturn.Add(dr.GetInt32(0));
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<string> GetTestTypes()
        {
            List<string> toReturn = new List<string>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select Distinct TestName from TestTypes order by TestName";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    toReturn.Add(dr.GetString(0));
                }
                dr.Close();
            }
            return toReturn;
        }
        public static int GetNewTestId()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database1))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select Count(distinct(Testid)) from TestInfo";
                int nooftestid = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                return nooftestid;
            }
        }
        static string GetDates(DateTime givenDate)
        {
            StringBuilder db = new StringBuilder();
            if (givenDate.Month < 5)
            {
                db.Append(givenDate.Year - 1);
                db.Append("-");
                db.Append(givenDate.Year);
            }
            else
            {
                db.Append(givenDate.Year);
                db.Append("-");
                db.Append(givenDate.Year + 1);
            }
            return db.ToString();
        }
    }
}
