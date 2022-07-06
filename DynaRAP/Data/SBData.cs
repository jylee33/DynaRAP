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
        public SBData(string blockName, string julianStartAt, string julianEndAt, string seq, int view, int download)
        {
            BlockName = blockName;
            JulianStartAt = julianStartAt;
            JulianEndAt = julianEndAt;
            Seq = seq;
            View = view;
            Download = download;
        }
        public string BlockName { get; set; }
        public string Seq { get; set; }
        public string JulianStartAt { get; set; }
        public string JulianEndAt { get; set; }
        public int View { get; set; }
        public int Download { get; set; }
    }
}
