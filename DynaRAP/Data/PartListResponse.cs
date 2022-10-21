using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class PartListResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<ResponsePart> response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class ResponsePart
    {
        public string seq { get; set; }
        public string uploadSeq { get; set; }
        public string partName { get; set; }
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
        public string registerUid { get; set; }
        public string dataType { get; set; }
    }

}
