using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchTestV1.Model
{
    public class TestSummaryBindModel
    {
        public string Heading { get; set; }
        public string Topic { get; set; }
        public int Mark { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public TestSummaryBindModel(string heading, string topic, int mark, int count, int total)
        {
            this.Heading = heading;
            this.Topic = topic;
            this.Mark = mark;
            this.Count = count;
            this.Total = total;
        }
        public TestSummaryBindModel()
        {

        }
    }
}
