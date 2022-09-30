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
        public SBResult(double psd, double rms, double n0, double rmsToPeek, double resultValue)
        {
            Psd = psd;
            Rms = rms;
            N0 = n0;
            RmsToPeek = rmsToPeek;
            ResultValue = resultValue;
        }
        public double Psd { get; set; }
        public double Rms { get; set; }
        public double N0 { get; set; }
        public double RmsToPeek { get; set; }
        public double ResultValue { get; set; }
    }
}
