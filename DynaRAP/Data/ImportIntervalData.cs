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
        public ImportIntervalData(string importType, string splitName, string startTime, string endTime, string dataCount, int del, int all, int raw, int lpf, int hpf, int bpf)
        {
            ImportType = importType;
            SplitName = splitName;
            StartTime = startTime;
            EndTime = endTime;
            DataCount = dataCount;
            Del = del;
            Download_ALL = all;
            Download_BPF = bpf;
            Download_HPF = hpf;
            Download_LPF = lpf;
            Download_RAW = raw;
        }
        public string ImportType { get; set; }
        public string SplitName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DataCount { get; set; }
        public int Del { get; set; }
        public int Download_ALL { get; set; }
        public int Download_RAW { get; set; }
        public int Download_LPF { get; set; }
        public int Download_HPF { get; set; }
        public int Download_BPF { get; set; }
        public string Seq { get; set; }
    }
}
