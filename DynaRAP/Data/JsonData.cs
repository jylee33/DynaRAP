using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class JsonData
    {
        public int code { get; set; }
        public string message { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }

        public JsonData(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }

    public class DirJsonData : JsonData
    {
        public Response response { get; set; }

        public DirJsonData(int code, string message) : base(code, message) { }
    }

    public class AddParamJsonData : JsonData
    {
        public ResponseParam response { get; set; }

        public AddParamJsonData(int code, string message) : base(code, message) { }
    }

    public class ListParamJsonData : JsonData
    {
        public List<ResponseParam> response { get; set; }
        
        public ListParamJsonData(int code, string message) : base(code, message) { }
    }

    public class AddPresetJsonData : JsonData
    {
        public ResponsePreset response { get; set; }

        public AddPresetJsonData(int code, string message) : base(code, message) { }
    }

    public class ListPresetJsonData : JsonData
    {
        public List<ResponsePreset> response { get; set; }

        public ListPresetJsonData(int code, string message) : base(code, message) { }
    }

    public class CreatedAt
    {
        public long timestamp { get; set; }
        public string dateFormat { get; set; }
        public string dateTime { get; set; }
    }

    public class Pool
    {
        public int seq { get; set; }
        public int parentDirSeq { get; set; }
        public string uid { get; set; }
        public string dirName { get; set; }
        public string dirType { get; set; }
        public string dirIcon { get; set; }
        public CreatedAt createdAt { get; set; }
        public string refSeq { get; set; }
        public string refSubSeq { get; set; }
    }

    public class Presentation
    {
        public int dirKey { get; set; }
        public List<object> subTree { get; set; }
    }

    public class Response
    {
        public List<Pool> pools { get; set; }
        public List<Presentation> presentation { get; set; }
        public int seq { get; set; }
    }

    public class ResponseParam
    {
        public string seq { get; set; }
        public string paramPack { get; set; }
        public string paramGroupSeq { get; set; }
        public string paramName { get; set; }
        public string paramKey { get; set; }
        public string paramSpec { get; set; }
        public string adamsKey { get; set; }
        public string zaeroKey { get; set; }
        public string grtKey { get; set; }
        public string fltpKey { get; set; }
        public string fltsKey { get; set; }
        public string partInfo { get; set; }
        public string partInfoSub { get; set; }
        public double lrpX { get; set; }
        public double lrpY { get; set; }
        public double lrpZ { get; set; }
        public string paramUnit { get; set; }
        public double domainMin { get; set; }
        public double domainMax { get; set; }
        public double specified { get; set; }
        public string registerUid { get; set; }
    }

    public class ResponsePreset
    {
        public string seq { get; set; }
        public string presetPack { get; set; }
        public string presetName { get; set; }
        public string presetPackFrom { get; set; }
        public string registerUid { get; set; }
    }

    public class ResponseAt
    {
        public long timestamp { get; set; }
        public string dateFormat { get; set; }
        public string dateTime { get; set; }
    }

    







}
