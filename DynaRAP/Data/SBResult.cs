using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class SBResult
    {
        public SBResult()
        {
        }
        public SBResult(string frequency, string psd)
        {
            Frequency = frequency;
            Psd = psd;
        }
        public string Frequency { get; set; }
        public string Psd { get; set; }
    }

    public class SBResult1
    {
        public SBResult1()
        {
        }
        public SBResult1(string zarray)
        {
            Zarray = zarray;
        }
        public string Zarray { get; set; }
    }
}
