using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class PropListResponse : JsonData
    {
        public List<ResponsePropList> response { get; set; }

        public PropListResponse(int code, string message) : base(code, message) { }
    }

    public class PropAddResponse : JsonData
    {
        public ResponsePropList response { get; set; }

        public PropAddResponse(int code, string message) : base(code, message) { }
    }

    public class ResponsePropList
    {
        public string seq { get; set; }
        public string propCode { get; set; }
        public string propType { get; set; }
        public string paramUnit { get; set; }
        public string registerUid { get; set; }
        public CreatedAt createdAt { get; set; }
        public bool deleted { get; set; }
    }
}
