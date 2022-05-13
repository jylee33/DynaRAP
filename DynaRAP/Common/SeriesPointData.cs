using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Common
{
    #region 1D SeriesPoint Data
    public class SeriesPointData
    {
        public string SeriesName { get; private set; }
        public DateTime Argument { get; private set; }
        public double Value { get; private set; }

        public SeriesPointData(string name, DateTime arg, double val)
        {
            SeriesName = name;
            Argument = arg;
            Value = val;
        }
    }
    #endregion

}
