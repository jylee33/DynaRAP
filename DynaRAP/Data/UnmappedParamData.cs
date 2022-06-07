using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class UnmappedParamData
    {
        public UnmappedParamData()
        {
        }
        public UnmappedParamData(string unmappedParamName, string paramKey)
        {
            UnmappedParamName = unmappedParamName;
            ParamKey = paramKey;
        }
        public string UnmappedParamName { get; set; }
        public string ParamKey { get; set; }
    }
}
