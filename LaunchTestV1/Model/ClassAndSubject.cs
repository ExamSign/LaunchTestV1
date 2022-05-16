using LaunchTestV1.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchTestV1.Model
{
    public class ClassAndSubject : BindableBase
    {
        public string Grade { get; set; }
        public string Subject { get; set; }
        public ClassAndSubject(string grade, string subject)
        {
            this.Grade = grade;
            this.Subject = subject;
        }
        public ClassAndSubject()
        {

        }
    }
}
