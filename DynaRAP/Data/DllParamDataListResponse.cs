using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DllParamDataListResponse : JsonData
    {
        public List<ResponseDLLParamData> response { get; set; }

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
    }

    public class DllParamData
    {
        public DllParamData(string seq, string data, int del)
        {
            Seq = seq;
            Data = data;
            Del = del;
        }
        public string Seq { get; set; }
        public string Data { get; set; }
        public int Del { get; set; }
    }
}
