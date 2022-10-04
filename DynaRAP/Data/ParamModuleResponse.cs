using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ListParamModuleResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<ListParamModule> response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class ListParamModule
    {
        public string seq { get; set; }
        public string moduleName { get; set; }
        public string copyFromSeq { get; set; }
        public bool referenced { get; set; }
        public DataProps dataProp { get; set; }
        public CreatedAt createdAt { get; set; }

    }

    public class SaveParamModuleSelectDataRequest
    {
        public string command { get; set; }
        public string moduleSeq { get; set; }
        public List<SaveParamModuleSelectDataSource> sources { get; set; }

    }
    
    public class SaveParamModuleSelectDataResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<SaveParamModuleSelectDataSource> response { get; set; }
    }
    public class SaveParamModuleSelectDataSource
    {
        public string seq { get; set; }
        public string sourceType { get; set; }
        public string sourceName { get; set; }
        public string sourceSeq { get; set; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public string paramKey { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
        public string moduleSeq { get; set; }
        public string dataCount { get; set; }
        public string useTime { get; set; }
        public string sourceNo { get; set; }
        public SaveParamModuleSelectDataSource(string sourceType, string sourceSeq, string seq, string paramPack = null, string paramSeq = null, string julianStartAt = null,string julianEndAt= null, double offsetStartAt = 0.0, double offsetEndAt = 0.0)
        {
            this.sourceType = sourceType;
            this.sourceSeq = sourceSeq;
            this.paramPack = paramPack;
            this.paramSeq = paramSeq;
            this.julianStartAt = julianStartAt;
            this.julianEndAt = julianEndAt;
            this.offsetStartAt = offsetStartAt;
            this.offsetEndAt = offsetEndAt;
            this.seq = seq;
        }
        public SaveParamModuleSelectDataSource()
        {
        }
    }
    public class SearchParamModuleResponse //parts
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<SearchResponseParamModule> response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }
    public class SearchResponseParamModule
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
        public bool lpfDone { get; set; }
        public bool hpfDone { get; set; }
        public string blockMetaSeq { get; set; }
        public string partSeq { get; set; }
        public int blockNo { get; set; }
        public string blockName { get; set; }
        public string dataSetCode { get; set; }
        public string dataSetName { get; set; }
        public string dataVersion { get; set; }
        public string tags { get; set; }
        public string moduleName { get; set; }
        public string copyFromSeq { get; set; }
        public bool referenced { get; set; }
        public string dataCount { get; set; }


        public List<Equation> equations { get; set; }
        public CreatedAt createdAt { get; set; }
        public List<SearchParams> @params { get; set; }
        public string useTime { get; set; }
    }
    public class SearchParams
    { 
        public string seq { get; set; }
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public string blockMetaSeq { get; set; }
        public int paramNo { get; set; }
        public int unionParamSeq { get; set; }
        public string paramKey { get; set; }
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
        public string registerUid { get; set; }
        public Extras extras { get; set; }
        public string tags { get; set; }
        public int referenceSeq { get; set; }
        public PropInfo propInfo { get; set; }
        public string dllSeq { get; set; }
        public string paramName { get; set; }
        public string paramType { get; set; }
        public PartInfos paramInfo { get; set; }
    }

    public class PartInfos
    {
        public string seq { get; set; }
        public string paramPack { get; set; }
        public string propSeq { get; set; }
        public string paramKey { get; set; }
        public string adamsKey { get; set; }
        public string zaeroKey { get; set; }
        public string grtKey { get; set; }
        public string fltpKey { get; set; }
        public string fltsKey { get; set; }
        public string partInfo { get; set; }
        public string partInfoSub { get; set; }
        public string registerUid { get; set; }
    }

    public class Equation
    {

    }
}
