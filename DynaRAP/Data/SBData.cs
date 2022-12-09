using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class SBData
    {
        public SBData()
        {
        }
        public SBData(string blockName, string julianStartAt, string julianEndAt, string seq, int view, int download0 , int download1, int download2, int download3, int download4)
        {
            BlockName = blockName;
            JulianStartAt = julianStartAt;
            JulianEndAt = julianEndAt;
            Seq = seq;
            View = view;
            Download0 = download0;
            Download1 = download1;
            Download2 = download2;
            Download3 = download3;
            Download4 = download4;
        }
        public string BlockName { get; set; }
        public string Seq { get; set; }
        public string JulianStartAt { get; set; }
        public string JulianEndAt { get; set; }
        public int View { get; set; }
        public int Download0 { get; set; }
        public int Download1 { get; set; }
        public int Download2 { get; set; }
        public int Download3 { get; set; }
        public int Download4 { get; set; }
    }
}
