using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DataPropResponse : JsonData
    {
        public List<ResponseDataProp> response { get; set; }
        public DataPropResponse(int code, string message) : base(code, message) { }
    }

    public class ResponseDataProp
    {
        public string seq { get; set; }
        public string propName { get; set; }
        public string propValue { get; set; }
        public string referenceType { get; set; }
        public string referenceKey { get; set; }
        public UpdatedAt updatedAt { get; set; }
    }
}
