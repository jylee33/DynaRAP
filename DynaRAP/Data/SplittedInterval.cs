using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class SplittedInterval
    {
        public SplittedInterval()
        {
        }
        public SplittedInterval(string intervalName, string startTime, string endTime, int view)
        {
            IntervalName = intervalName;
            StartTime = startTime;
            EndTime = endTime;
            View = view;
        }
        public string IntervalName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int View { get; set; }
    }
}
