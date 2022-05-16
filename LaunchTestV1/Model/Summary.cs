using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchTestV1.Model
{
    public class Summary
    {
        public string Heading { get; set; }
        public List<SubSummary> SubSummaryList { get; set; }
    }
    public class SubSummary
    {
        public string Topic { get; set; }
        public int QuestionCount { get; set; }
        public int Mark { get; set; }
        public int TotalMark { get; set; }
    }
}
