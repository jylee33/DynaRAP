using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DllParamDataResponse : JsonData
    {
        public ResponseDLLParamData response { get; set; }

        public DllParamDataResponse(int code, string message) : base(code, message) { }
    }

}
