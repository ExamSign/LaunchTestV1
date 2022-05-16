//using EasyWordDocument.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EasyWordDocument.Repos
//{
//    public class DBAction
//    {
//        static string ConnectionString = Properties.Settings.Default.Database;

//        public static void RemoveItem(int qid)
//        {
//            RemoveQuestionItem(qid);
//            RemoveImageItem(qid);
//            RemoveXpsItem(qid);
//            RemoveTestReference(qid);
//        }

//        public static void RemoveQuestionItem(int qid)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "delete from Questions where Qid = '" + qid + "'";
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static void RemoveImageItem(int qid)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "delete from imagetable where Qid = '" + qid + "'";
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static void RemoveXpsItem(int qid)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "delete from Xpstable where Qid = '" + qid + "'";
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static void RemoveTestReference(int qid)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "delete from TestInfo where Qid = '" + qid + "'";
//                cmd.ExecuteNonQuery();
//            }
//        }


//        public static void UpdateQuestionItem(MyQuestions questionItem)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "update Questions set Question = '" + questionItem.Question + "' where Qid = " + questionItem.Qno + "";
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static void InsertQuestionWithImages(int newQuestionId, MyQuestions question)
//        {
//            var questionimage = question.ImagesList.Where(temp => temp.Qid == question.Qno).ToList();

//            foreach (var image in questionimage)
//            {
//                using (SqlConnection con = new SqlConnection(ConnectionString))
//                {
//                    SqlCommand cmd = new SqlCommand();
//                    con.Open();
//                    cmd.Connection = con;
//                    cmd.CommandText = "insert into imagetable values(@Qid,@imagenumber,@ImageByte)";
//                    cmd.Parameters.AddWithValue("@Qid", newQuestionId);
//                    cmd.Parameters.AddWithValue("@imagenumber", image.Imagenumber);
//                    cmd.Parameters.AddWithValue("@ImageByte", image.Imagebyte);
//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }
//        public static void InsertQuestionItem(int newQuestionId, MyQuestions question)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "insert into Questions (Qid, Question, Hasimage, Subject, SClass, QType, Marks,QDesc, Topic, SubTopic, SheetType, QName) values(@Qid, @Question, @Hasimage, @Subject, @SClass, @QType, @Marks,@QDesc, @Topic, @SubTopic, @SheetType, @QName)";
//                cmd.Parameters.AddWithValue("@Qid", newQuestionId);
//                cmd.Parameters.AddWithValue("@Question", question.Question);
//                cmd.Parameters.AddWithValue("@Hasimage", question.HasImage);
//                cmd.Parameters.AddWithValue("@Subject", question.Subject);
//                cmd.Parameters.AddWithValue("@SClass", question.Sclass);
//                cmd.Parameters.AddWithValue("@QType", question.QType);
//                cmd.Parameters.AddWithValue("@QDesc", question.Heading);
//                cmd.Parameters.AddWithValue("@Marks", question.Marks);
//                cmd.Parameters.AddWithValue("@Topic", RemoveEndDot(question.Topic));
//                cmd.Parameters.AddWithValue("@SubTopic", question.SubTopic);
//                cmd.Parameters.AddWithValue("@SheetType", question.SheetType);
//                cmd.Parameters.AddWithValue("@QName", question.QName);
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static string RemoveEndDot(string givenString)
//        {
//            if (givenString[givenString.Length - 1] == '.')
//                return givenString.Substring(0, givenString.Length - 2).Trim();
//            else
//                return givenString.Trim();
//        }
//        public static int GetNewQuestionId()
//        {
//            int NewQuestionId = 0;
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "select MAX(Qid)  from Questions";
//                NewQuestionId = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
//            }
//            return NewQuestionId;
//        }

//        public static bool IsSheetNameExists(TestMeta testMetaData)
//        {
//            int result = 0;
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "select COUNT(*) from TestMeta where Grade = "+testMetaData.Grade+" and Subject = '"+testMetaData.Subject+"' and TestTypeOf = '"+testMetaData.SheetType+"' and SheetNo = "+testMetaData.SheetNumber+"";
//                result = Convert.ToInt32(cmd.ExecuteScalar());
//            }
//            return result >= 1 ? true : false;
//        }

//        public static void InsertIntoNewTestInfo(string newTestId, string grade, string subject, int qid)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "insert into TestInfo(TestId, Grade, Subject, Qid) values(@TestId, @Grade, @Subject, @Qid)";
//                cmd.Parameters.AddWithValue("@TestId", newTestId);
//                cmd.Parameters.AddWithValue("@Grade", grade);
//                cmd.Parameters.AddWithValue("@Subject", subject);
//                cmd.Parameters.AddWithValue("@Qid", qid);
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static bool InsertWordFileToDB(string testId, string grade, string subject, string fileName, byte[] fileBytes)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "insert into TestWordFiles(TestId, Grade, SubjectName, FileTitle, WordBytes) values(@TestId, @Grade,@SubjectName, @FileTitle, @WordBytes)";
//                cmd.Parameters.AddWithValue("@TestId", testId);
//                cmd.Parameters.AddWithValue("@Grade", grade);
//                cmd.Parameters.AddWithValue("@SubjectName", subject);
//                cmd.Parameters.AddWithValue("@FileTitle", fileName);
//                cmd.Parameters.AddWithValue("@WordBytes", fileBytes);
//                try
//                {
//                    cmd.ExecuteNonQuery();
//                    return true;
//                }
//                catch (Exception ex) { return false; }
//            }
//        }
//        public static bool BackUpQuestionFile(string fileId, int grade, string subject, string fileName, byte[] fileBytes)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "insert into UploadedQuestionFile (FileId, Grade, Subject, FileTittle, WordBytes) values (@FileId, @Grade, @Subject, @FileTittle, @WordBytes)";
//                cmd.Parameters.AddWithValue("@FileId", GetNewQuestionFileId());
//                cmd.Parameters.AddWithValue("@Grade", grade);
//                cmd.Parameters.AddWithValue("@Subject", subject);
//                cmd.Parameters.AddWithValue("@FileTittle", fileName);
//                cmd.Parameters.AddWithValue("@WordBytes", fileBytes);
//                try
//                {
//                    cmd.ExecuteNonQuery();
//                    return true;
//                }
//                catch (Exception ex) { return false; }
//            }
//        }
//        public static bool InsertQuestionOrigin(int fileId, int qid)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "insert into QuestionOrigin(FileId, Qid) values (@FileId, @Qid)";
//                cmd.Parameters.AddWithValue("@FileId", fileId);
//                cmd.Parameters.AddWithValue("@Qid", qid);
//                try
//                {
//                    cmd.ExecuteNonQuery();
//                    return true;
//                }
//                catch (Exception ex) { return false; }
//            }
//        }
//        public static int GetNewQuestionFileId()
//        {
//            int toReturn = 0;
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "select distinct top 1 FileId from QuestionOrigin order by FileId desc";
//                SqlDataReader dr = cmd.ExecuteReader();
//                while(dr.Read())
//                {   
//                    toReturn = dr.GetInt32(0);
//                    break;
//                }
//            }
//            return (toReturn + 1);
//        }
//        public static void InsertXpsItem(int newQuestionId,DocumentModel question)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "insert into Xpstable (Qid, XpsFile) values(@Qid, @XpsFile)";
//                cmd.Parameters.AddWithValue("@Qid", newQuestionId);
//                cmd.Parameters.AddWithValue("@XpsFile", question.ByteData);
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static void UpdateXpsItem(int Qid, byte[] newByteData)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "update Xpstable set XpsFile = @newXpsData where Qid = @Qid";
//                cmd.Parameters.AddWithValue("@Qid", Qid);
//                cmd.Parameters.AddWithValue("@newXpsData", newByteData);
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static bool InsertSummaryDetails(string testID, string heading, subsummary testItem)
//        {
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "insert into TestDetails(TestId, Heading, Topic, Mark, PresentCount, Total)values(@TestId, @Heading, @Topic, @Mark, @PresentCount, @Total)";
//                cmd.Parameters.AddWithValue("@TestId", testID);
//                cmd.Parameters.AddWithValue("@Heading", heading);
//                cmd.Parameters.AddWithValue("@Topic", testItem.Topic);
//                cmd.Parameters.AddWithValue("@Mark", testItem.Mark);
//                cmd.Parameters.AddWithValue("@PresentCount", testItem.QuestionCount);
//                cmd.Parameters.AddWithValue("@Total", testItem.TotalMark);
//                try
//                {
//                    cmd.ExecuteNonQuery();
//                    return true;
//                }
//                catch (Exception ex) { return false; }
//            }
//        }
//        public static bool InsertIntoTestMetaNew(string newTestId, string grade, string subject, string testType, DateTime dateOn, int qCount, int totalMarks, int sheetNo, int duration)
//        {
//            int TestSheetCount = GetCountForTestType(grade, subject, testType) + 1;
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "insert into TestMeta(TestId, Grade, Subject, TestTypeOf, TestCount, TestDate, QCount, TotalMarks, SheetNo, Duration) values (@TestId, @Grade, @Subject, @TestTypeOf, @TestCount, @TestDate, @QCount, @TotalMarks, @SheetNo, @Duration)";
//                cmd.Parameters.AddWithValue("@TestId", newTestId);
//                cmd.Parameters.AddWithValue("@Grade", grade);
//                cmd.Parameters.AddWithValue("@Subject", subject);
//                cmd.Parameters.AddWithValue("@TestTypeOf", testType);
//                cmd.Parameters.AddWithValue("@TestCount", TestSheetCount);
//                cmd.Parameters.AddWithValue("@TestDate", dateOn);
//                cmd.Parameters.AddWithValue("@QCount", qCount);
//                cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
//                cmd.Parameters.AddWithValue("@SheetNo", sheetNo);
//                cmd.Parameters.AddWithValue("@Duration", duration);

//                try { cmd.ExecuteNonQuery(); return true; }
//                catch (Exception ex)
//                { return false; }
//            }
//        }
//        public static int GetNewBluePrintId()
//        {
//            int newId = 0;
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "select count(distinct Bid) from BluePrint";
//                newId = Convert.ToInt32(cmd.ExecuteScalar());
//            }
//            return newId + 1;
//        }
//        static int GetCountForTestType(string grade, string subject, string testType)
//        {
//            int totalTestTypeCount = 0;
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "select Count(*) from TestMeta where Grade = " + grade + " and Subject = '" + subject + "' and TestTypeOf = '" + testType + "'";
//                totalTestTypeCount = Convert.ToInt32(cmd.ExecuteScalar());
//            }
//            return totalTestTypeCount;
//        }


//        // BluePrint Insert and Retrive
//        // =====================================
//        public static TestMeta GetBluePrintMeta(SqlConnection globalConnection, int bluePrintId)
//        {
//            TestMeta toReturn = new TestMeta();
//            SqlCommand cmd = new SqlCommand();
//            cmd.Connection = globalConnection;
//            cmd.CommandText = "select Grade, Subject, QCount, Marks, Period from BluePrint where Bid = " + bluePrintId + "";
//            SqlDataReader dr = cmd.ExecuteReader();
//            while (dr.Read())
//            {
//                toReturn.Grade = dr.GetInt32(0).ToString();
//                toReturn.Subject = dr.GetString(1);
//                toReturn.QuestionCount = dr.GetInt32(2);
//                toReturn.TotalMarks = dr.GetInt32(3);
//                break;
//            }
//            return toReturn;
//        }
//        public static List<int> GetBluePrintIdList(TestMeta testMetaData)
//        {
//            List<int> toReturn = new List<int>();
//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "select distinct Bid from BluePrint where Grade = " + testMetaData.Grade + " and Subject = '" + testMetaData.Subject + "'";
//                SqlDataReader dr = cmd.ExecuteReader();
//                while (dr.Read())
//                {
//                    toReturn.Add(dr.GetInt32(0));
//                }
//            }
//            return toReturn;
//        }
//        public static void InsertIntoBluePrint(SqlConnection globalConnection, TestMeta testMetaData, int bluePrintId, DateTime period)
//        {
//            SqlCommand cmd = new SqlCommand();
//            cmd.Connection = globalConnection;
//            cmd.CommandText = "insert into BluePrint (Bid, Grade, Subject,QCount, Marks, Period) values(@Bid, @Grade, @Subject, @QCount,@Marks, @Period)";
//            cmd.Parameters.AddWithValue("@Bid", bluePrintId);
//            cmd.Parameters.AddWithValue("@Grade", testMetaData.Grade);
//            cmd.Parameters.AddWithValue("@Subject", testMetaData.Subject);
//            cmd.Parameters.AddWithValue("@QCount", testMetaData.QuestionCount);
//            cmd.Parameters.AddWithValue("@Marks", testMetaData.TotalMarks);
//            cmd.Parameters.AddWithValue("@Period", period);
//            cmd.ExecuteNonQuery();
//        }
//        public static void InsertIntoBluePrintMark(SqlConnection globalConnection, int newBluePrintId, int marks, int count)
//        {
//            SqlCommand cmd = new SqlCommand();
//            cmd.Connection = globalConnection;
//            cmd.CommandText = "insert into MarkBluePrint(Bid, Mark,Count, Total) values (@Bid, @Marks,@Count, @Total)";
//            cmd.Parameters.AddWithValue("@Bid", newBluePrintId);
//            cmd.Parameters.AddWithValue("@Marks", marks);
//            cmd.Parameters.AddWithValue("@Count", count);
//            cmd.Parameters.AddWithValue("@Total", (marks * count));
//            cmd.ExecuteNonQuery();
//        }
//        public static void InsertIntoBluePrintTopic(SqlConnection globalConnection, int newBluePrintId, string topic, int total)
//        {
//            SqlCommand cmd = new SqlCommand();
//            cmd.Connection = globalConnection;
//            cmd.CommandText = "insert into TopicBluePrint(Bid, Topic, Total) values (@Bid, @Topic, @Total)";
//            cmd.Parameters.AddWithValue("@Bid", newBluePrintId);
//            cmd.Parameters.AddWithValue("@Topic", topic);
//            cmd.Parameters.AddWithValue("@Total", total);
//            cmd.ExecuteNonQuery();
//        }
//        public static void InsertIntoBluePrintCompetencies(SqlConnection globalConnection, int newBluePrintId, string competencies,int percentage, int total)
//        {
//            SqlCommand cmd = new SqlCommand();
//            cmd.Connection = globalConnection;
//            cmd.CommandText = "insert into CompetencyBluePrint(Bid, Competencies, Percentage, Total) values (@Bid, @Competencies, @Percentage, @Total)";
//            cmd.Parameters.AddWithValue("@Bid", newBluePrintId);
//            cmd.Parameters.AddWithValue("@Competencies", competencies);
//            cmd.Parameters.AddWithValue("@Percentage", percentage);
//            cmd.Parameters.AddWithValue("@Total", total);
//            cmd.ExecuteNonQuery();
//        }
//        public static List<MarkDenom> GetMarkDenomination(SqlConnection globalConnection, int bluePrintId)
//        {
//            List<MarkDenom> denom = new List<MarkDenom>();
//            SqlCommand cmd = new SqlCommand();
//            cmd.Connection = globalConnection;
//            cmd.CommandText = "select Mark, Count from MarkBluePrint where Bid = " + bluePrintId + "";
//            SqlDataReader dr = cmd.ExecuteReader();
//            while(dr.Read())
//            {
//                MarkDenom markDenom = new MarkDenom();
//                markDenom.Mark = dr.GetInt32(0);
//                int count  = dr.GetInt32(1);
//                markDenom.Count = count;
//                denom.Add(markDenom);
//            }
//            dr.Close();
//            return denom;
//        }
//        public static List<TopicWeight> GetTopicWeight(SqlConnection globalConnection, int bluePrintId)
//        {
//            List<TopicWeight> topicsList = new List<TopicWeight>(); 
//            SqlCommand cmd = new SqlCommand();
//            cmd.Connection = globalConnection;
//            cmd.CommandText = "select Topic, Total from TopicBluePrint where Bid = " + bluePrintId + "";
//            SqlDataReader dr = cmd.ExecuteReader();
//            while(dr.Read())
//            {
//                TopicWeight topic = new TopicWeight();
//                topic.TopicName = dr.GetString(0);
//                topic.MarksNeed = dr.GetInt32(1);
//                topicsList.Add(topic);
//            }
//            dr.Close();
//            return topicsList;
//        }
//        public static List<CompetencyWeight> GetCompetencyWeights(SqlConnection globalConnection, int bluePrintId)
//        {
//            List<CompetencyWeight> competencyWeight = new List<CompetencyWeight>();
//            SqlCommand cmd = new SqlCommand();
//            cmd.Connection = globalConnection;
//            cmd.CommandText = "select Competencies, Percentage, Total from CompetencyBluePrint where Bid = " + bluePrintId + "";
//            SqlDataReader dr = cmd.ExecuteReader();
//            while(dr.Read())
//            {
//                CompetencyWeight cw = new CompetencyWeight();
//                string[] cwList = dr.GetString(0).Split(',');
//                foreach (string item in cwList)
//                    if (item.Length > 0)
//                        cw.C_Names.Add(item);

//                cw.PercentValue = dr.GetInt32(1);
//                cw.MarksToFill = dr.GetInt32(2);
//                competencyWeight.Add(cw);
//            }
//            dr.Close();
//            return competencyWeight;
//        }

//        public static void RevertQuestionInsert(string fileId)
//        {
//            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;

//                // Delete all question items
//                cmd.CommandText = "delete from Questions where Qid in (select Qid from QuestionOrigin where FileId = " + fileId + ")";
//                cmd.ExecuteNonQuery();
//                // Delete all xps files
//                cmd.CommandText = "delete from Xpstable where Qid in (select Qid from QuestionOrigin where FileId = " + fileId + ")";
//                cmd.ExecuteNonQuery();
//                // Delete all images
//                cmd.CommandText = "delete from imagetable where Qid in (select Qid from QuestionOrigin where FileId = "+fileId+")";
//                cmd.ExecuteNonQuery();
//                // Delete all images
//                cmd.CommandText = "delete from TestLetMapings where BaseQId in (select Qid from QuestionOrigin where FileId = " + fileId + ")";
//                cmd.ExecuteNonQuery();
//                // Delete IsHeadingUpdate table data
//                cmd.CommandText = "delete from IsHeadingUpdate where FileId = " + fileId + "";
//                cmd.ExecuteNonQuery();
//                // Delete Question address
//                cmd.CommandText = "delete from QuestionOrigin where FileId = " + fileId + "";
//                cmd.ExecuteNonQuery();
//                // Delete Question word file
//                cmd.CommandText = "delete from UploadedQuestionFile where FileId = " + fileId + "";
//                cmd.ExecuteNonQuery();
//            }
//        }
//        public static List<int> GetClass()
//        {
//            List<int> toReturn = new List<int>();
//            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "select distinct SClass from Questions order by SClass asc";
//                SqlDataReader dr = cmd.ExecuteReader();
//                while (dr.Read())
//                {
//                    toReturn.Add(Convert.ToInt32(dr.GetString(0)));
//                }
//                dr.Close();
//            }
//            return toReturn;
//        }
//        public static List<string> GetSubjects(string grade)
//        {
//            List<string> toReturn = new List<string>();
//            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "select distinct Subject from Questions where SClass = " + grade + " order by Subject";
//                SqlDataReader dr = cmd.ExecuteReader();
//                while (dr.Read())
//                {
//                    toReturn.Add(dr.GetString(0));
//                }
//                dr.Close();
//            }
//            return toReturn;
//        }

//        public static void ModifyQuestionType(DocumentModel item)
//        {
//            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database))
//            {
//                SqlCommand cmd = new SqlCommand();
//                con.Open();
//                cmd.Connection = con;
//                cmd.CommandText = "update Questions set QType = '" + item.Qtype + "' where Qid = '" + item.DocNo + "'";
//                cmd.ExecuteNonQuery();
//            }
//        }
//    }
//}
