using LaunchTestV1.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchTestV1.Repos
{
    public class GMeetRepo
    {
        public static List<GradeAndSection> GetClass()
        {
            List<GradeAndSection> toReturn = new List<GradeAndSection>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database2))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select distinct Sclass, Section from StudentDetails";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    GradeAndSection item = new GradeAndSection();
                    item.Grade = dr.GetString(0);
                    item.Section = dr.GetString(1);
                    if (item.Section.Length == 1)
                        toReturn.Add(item);
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<string> GetSubjects(string grade)
        {
            List<string> toReturn = new List<string>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database2))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select distinct Subject from TimeTableNew where Grade = " + grade + "";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    toReturn.Add(dr.GetString(0));
                }
                dr.Close();
            }
            return toReturn;
        }
        public static List<StudentMeta> GetStudents(string grade)
        {
            List<StudentMeta> toReturn = new List<StudentMeta>();
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.Database2))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select PPSNo, Name, Sclass, Section, displayNAME, displayPPSNO, UserId from StudentDetails where Sclass = " + grade + "";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    StudentMeta item = new StudentMeta();
                    item.PPSNo = dr.GetString(0);
                    item.StudentName = dr.GetString(1);
                    item.Grade = dr.GetString(2);
                    item.Section = dr.GetString(3);
                    item.StudentDisplayName = dr.GetString(4);
                    item.StudentDisplayPPSNo = dr.GetString(5);
                    item.UserID = dr.GetString(6);
                    toReturn.Add(item);
                }
                dr.Close();
            }
            return toReturn;
        }
    }
    public class GradeAndSection
    {
        public string Grade { get; set; }
        public string Section { get; set; }
    }
}
