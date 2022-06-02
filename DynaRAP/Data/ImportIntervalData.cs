using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ImportIntervalData
    {
        public ImportIntervalData()
        {
        }
        public ImportIntervalData(string importType, string splitName, string startTime, string endTime, string dataCount, int del)
        {
            ImportType = importType;
            SplitName = splitName;
            StartTime = startTime;
            EndTime = endTime;
            DataCount = dataCount;
            Del = del;
        }
        public string ImportType { get; set; }
        public string SplitName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DataCount { get; set; }
        public int Del { get; set; }
    }
}
