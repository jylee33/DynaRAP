using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DllResponse : JsonData
    {
        public ResponseDLL response { get; set; }

        public DllResponse(int code, string message) : base(code, message) { }
    }

}
