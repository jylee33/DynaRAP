using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class CreateShortBlockResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public ResponseSB response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class CreateRequest
    {
        public string blockMetaSeq { get; set; }
        public string partSeq { get; set; }
        public double sliceTime { get; set; }
        public double overlap { get; set; }
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
        public List<Parameter> parameters { get; set; }
        public List<ShortBlock> shortBlocks { get; set; }
        public bool forcedCreate { get; set; }
    }

    public class PartInfo
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
    }

    public class ResponseSB
    {
        public string seq { get; set; }
        public string partSeq { get; set; }
        public string selectedPresetPack { get; set; }
        public string selectedPresetSeq { get; set; }
        public double overlap { get; set; }
        public double sliceTime { get; set; }
        public string registerUid { get; set; }
        public CreatedAt createdAt { get; set; }
        public bool createDone { get; set; }
        public CreateRequest createRequest { get; set; }
        public string status { get; set; }
        public string statusMessage { get; set; }
        public int fetchCount { get; set; }
        public int totalFetchCount { get; set; }
        public PartInfo partInfo { get; set; }
        public List<ShortBlockParamList> shortBlockParamList { get; set; }
    }

    public class ShortBlockParamList
    {
        public string seq { get; set; }
        public string blockMetaSeq { get; set; }
        public int paramNo { get; set; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public string paramName { get; set; }
        public string paramKey { get; set; }
        public string adamsKey { get; set; }
        public string zaeroKey { get; set; }
        public string grtKey { get; set; }
        public string fltpKey { get; set; }
        public string fltsKey { get; set; }
        public string paramUnit { get; set; }
    }


}
