using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class SBParamListResponse : JsonData
    {
        public ResponseSBParam response { get; set; }

        public SBParamListResponse(int code, string message) : base(code, message) { }
    }

    public class ResponseSBParam
    {
        public List<string> paramSet { get; set; }
        public List<object> paramData { get; set; }
    }

}
