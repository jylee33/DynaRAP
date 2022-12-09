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
        public UnmappedParamData(string unmappedParamName, string paramKey, string mapping)
        {
            UnmappedParamName = unmappedParamName;
            ParamKey = paramKey;
            Mapping = mapping;
        }
        public string UnmappedParamName { get; set; }
        public string ParamKey { get; set; }
        public string Mapping { get; set; }
    }
}
