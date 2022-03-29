using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class SplittedSB
    {
        public SplittedSB()
        {
        }
        public SplittedSB(string sbName, string startTime, string endTime, int view)
        {
            SbName = sbName;
            StartTime = startTime;
            EndTime = endTime;
            View = view;
        }
        public string SbName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int View { get; set; }
    }
}
