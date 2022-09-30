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
        public SBParameter(string parameterType, string parameterName, double min, double max, double avg)
        {
            ParameterType = parameterType;
            ParameterName = parameterName;
            Min = min;
            Max = max;
            Avg = avg;
        }
        public string ParameterType { get; set; }
        public string ParameterName { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Avg { get; set; }
    }
}
