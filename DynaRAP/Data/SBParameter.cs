using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class SBParameter
    {
        public SBParameter()
        {
        }
        public SBParameter(string parameterType, string parameterName, int min, int max, int avg, int del)
        {
            ParameterType = parameterType;
            ParameterName = parameterName;
            Min = min;
            Max = max;
            Avg = avg;
            Del = del;
        }
        public string ParameterType { get; set; }
        public string ParameterName { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Avg { get; set; }
        public int Del { get; set; }
    }
}
