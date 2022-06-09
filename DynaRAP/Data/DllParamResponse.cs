using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DllParamResponse : JsonData
    {
        public ResponseDLLParam response { get; set; }

        public DllParamResponse(int code, string message) : base(code, message) { }
    }

}
