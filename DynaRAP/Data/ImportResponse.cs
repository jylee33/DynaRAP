using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ImportResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public ResponseImport response { get; set; }
        public ResponseAtImport responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class UploadListResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<ResponseImport> response { get; set; }
        public ResponseAtImport responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class FlightAt
    {
        public long timestamp { get; set; }
        public string dateFormat { get; set; }
        public string dateTime { get; set; }
    }

    public class MappedParam
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

    public class PartImport
    {
        public string partName { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
    }

    public class PartList
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

    public class ResponseImport
    {
        public string seq { get; set; }
        public string uploadId { get; set; }
        public string dataType { get; set; }
        public string uploadName { get; set; }
        public string storePath { get; set; }
        public int fileSize { get; set; }
        public string flightSeq { get; set; }
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
        public UploadedAt uploadedAt { get; set; }
        public FlightAt flightAt { get; set; }
        public string registerUid { get; set; }
        public bool importDone { get; set; }
        public UploadRequest uploadRequest { get; set; }
        public string status { get; set; }
        public string statusMessage { get; set; }
        public int fetchCount { get; set; }
        public int totalFetchCount { get; set; }
        public List<string> notMappedParams { get; set; }
        public List<MappedParam> mappedParams { get; set; }
        //public List<PartList> partList { get; set; }
    }

    public class ResponseAtImport
    {
        public long timestamp { get; set; }
        public string dateFormat { get; set; }
        public string dateTime { get; set; }
    }

    public class UploadedAt
    {
        public long timestamp { get; set; }
        public string dateFormat { get; set; }
        public string dateTime { get; set; }
    }

    public class UploadRequest
    {
        public string sourcePath { get; set; }
        public string presetPack { get; set; }
        public string flightAt { get; set; }
        public string dataType { get; set; }
        public List<PartImport> parts { get; set; }
        public bool forcedImport { get; set; }
        public Dictionary<string, string> tempMappingParams { get; set; }
    }
}
