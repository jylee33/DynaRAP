using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DllParamListResponse : JsonData
    {
        public List<ResponseDLLParam> response { get; set; }

        public DllParamListResponse(int code, string message) : base(code, message) { }
    }

    public class ResponseDLLParam
    {
        public string seq { get; set; }
        public string dllSeq { get; set; }
        public string paramName { get; set; }
        public string paramType { get; set; }
        public int paramNo { get; set; }
        public string registerUid { get; set; }
    }

}
