using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DllParamDataListResponse : JsonData
    {
        public ResponseDLLParamData response { get; set; }

        public DllParamDataListResponse(int code, string message) : base(code, message) { }
    }

    public class ResponseDLLParamData
    {
        public string seq { get; set; }
        public string dllSeq { get; set; }
        public string paramSeq { get; set; }
        public int rowNo { get; set; }
        public double paramVal { get; set; }
        public string paramValStr { get; set; }

        public List<string> dllParamSet { get; set; }
        public List<long> dllRowRange { get; set; }
        public Dictionary<string, List<string>> data { get; set; }
    }

    public class DllParamData
    {
        public DllParamData(string data, int del)
        {
            Data = data;
            Del = del;
        }
        public string Data { get; set; }
        public int Del { get; set; }
    }
}
