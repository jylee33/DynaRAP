using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class PartInfoResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public ResponsePartInfo response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class ParamSet
    {
        public string seq { get; set; }
        public string paramPack { get; set; }
        public string paramGroupSeq { get; set; }
        public string paramName { get; set; }
        public string paramKey { get; set; }
        public string adamsKey { get; set; }
        public string zaeroKey { get; set; }
        public string grtKey { get; set; }
        public string fltpKey { get; set; }
        public string fltsKey { get; set; }
        public string paramUnit { get; set; }
        public string registerUid { get; set; }
        public int presetParamSeq { get; set; }
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
    }

    public class ResponsePartInfo
    {
        public List<ParamSet> paramSet { get; set; }
        public List<List<string>> julianSet { get; set; }
        public List<List<double>> data { get; set; }
    }

}
