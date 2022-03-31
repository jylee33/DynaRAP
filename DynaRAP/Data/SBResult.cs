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
        public SBResult(int psd, int rms, int n0, int rmsToPeek, int resultValue)
        {
            Psd = psd;
            Rms = rms;
            N0 = n0;
            RmsToPeek = rmsToPeek;
            ResultValue = resultValue;
        }
        public int Psd { get; set; }
        public int Rms { get; set; }
        public int N0 { get; set; }
        public int RmsToPeek { get; set; }
        public int ResultValue { get; set; }
    }
}
