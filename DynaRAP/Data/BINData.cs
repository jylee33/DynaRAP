using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class BinTableData
    {
        public int code { get;set; }
        public string message { get;set; }
        public List<BinTableResponse> response { get;set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class BinTableResponse
    {
        public string seq { get; set; }
        public string metaName { get; set; }
        public CreatedAt createdAt { get; set; }
        public List<PartSB> parts { get; set; }
        public List<PartSB> selectedShortBlocks { get; set; }
        public DataProps dataProps { get; set; }
        public List<PickUpParam> pickUpParams { get; set; }
    }

    public class PartSB
    {
        public string seq { get; set; }
        public string binMetaSeq { get; set; }
        public string dataFrom { get; set; }
        public string refSeq { get; set; }
    }
    public class BinTableSaveRequest
    {
        public string command { get; set; }
        public string metaName { get; set; }
        public string binMetaSeq { get; set; }
        public List<string> parts { get; set; }
        public List<string> selectedShortBlocks { get; set; }
        public DataProps dataProps { get; set; }
        public List<PickUpParam> pickUpParams { get; set; }
    }

    public class DataProps
    {
        public string key { get; set; }
        public string key2 { get; set; }
        public string tags { get; set; }
    }

    public class PickUpParam
    {
        public string paramSeq { get; set; }
        public string paramPack { get; set; }
        public string fieldType { get; set; }
        public string fieldPropSeq { get; set; }
        public string paramKey { get; set; }
        public string adamsKey { get; set; }
        public string zaeroKey { get; set; }
        public string grtKey { get; set; }
        public string fltpKey { get; set; }
        public string fltsKey { get; set; }
        public List<UserParamTable> userParamTable { get; set; }

        public PickUpParam(ParamDatas paramDatas)
        {
            this.paramSeq = paramDatas.seq;
            this.paramPack = paramDatas.paramPack;
            this.fieldPropSeq = paramDatas.propSeq;
            this.paramKey = paramDatas.paramKey;
            this.adamsKey = paramDatas.adamsKey;
            this.zaeroKey = paramDatas.zaeroKey;
            this.grtKey = paramDatas.grtKey;
            this.fltpKey = paramDatas.fltpKey;
            this.fltsKey = paramDatas.fltsKey;
            this.fieldType = string.Format("{0}({1})", paramDatas.propInfo.propCode, paramDatas.propInfo.paramUnit);
        }
       public PickUpParam()
        {

        }
    }

    public class UserParamTable
    {
        public double nominal { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }

    public class BinGridData
    {
        public string seq { get; set; }
        public string metaName { get; set; }
        public string tags { get; set; }

    }

    public class TreeData
    {
        public TreeData()
        {
        }
        public TreeData(int id, int parentId, string treeName,string seq, string type, bool? check)
        {
            ID = id;
            ParentID = parentId;
            TreeName = treeName;
            Check = check;
            Seq = seq;
            Type = type;
        }
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string TreeName { get; set; }
        public bool? Check { get; set; }
        public string Seq { get; set; }
        public string partSeq { get; set; }
        public string blockMetaSeq { get; set; }
        public string Type { get; set; }
    }

    public class ResponseParamList
    {
        public int code { get; set; }
        public string message { get; set; }
        public ParamResponse response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class ParamResponse
    {
        public List<string> paramSet { get; set; }
        public List<ParamDatas> paramData { get; set; }
    }


    public class ParamDatas
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
        public double lrpX { get; set; }
        public double lrpY { get; set; }
        public double lrpZ { get; set; }
        public string registerUid { get; set; }
        public Dictionary<string, string> extras { get; set; }
        public string tags { get; set; }
        public int referenceSeq { get; set; }
        public string paramSearchSeq { get; set; }
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
        public PropInfo propInfo { get; set; }
        public ParamValueMap paramValueMap { get; set; }
    }

    public class ParamValueMap
    {
        public string seq { get; set; }
        public string blockMetaSeq { get; set; }
        public string blockSeq { get; set; }
        public int unionParamSeq { get; set; }
        public double blockMin { get; set; }
        public double blockMax { get; set; }
        public double blockAvg { get; set; }
        public List<string> psd { get; set; }
        public List<string> bpfPsd { get; set; }
        public List<string> bpfZarray { get; set; }
        public List<string> bpfFrequency { get; set; }

        public double rms { get; set; }
        public double n0 { get; set; }
        public List<string> peak { get; set; }
        public List<string> zarray { get; set; }
        public double zPeak { get; set; }
        public double zValley { get; set; }
        public double lpfRms { get; set; }
        public double lpfN0 { get; set; }
        public double hpfRms { get; set; }
        public double hpfN0 { get; set; }
        public double bpfRms { get; set; }
        public double bpfN0 { get; set; }
        public double blockLpfMin { get; set; }
        public double blockLpfMax { get; set; }
        public double blockLpfAvg { get; set; }
        public double blockHpfMin { get; set; }
        public double blockHpfMax { get; set; }
        public double blockHpfAvg { get; set; }
        public double blockBpfMin { get; set; }
        public double blockBpfMax { get; set; }
        public double blockBpfAvg { get; set; }
    }


    public class propTypesCombo
    {
        public string paramKey { get; set; }
        public string paramUnit { get; set; }
        public string propType { get; set; }
        public string viewName { get; set; }
        public string seq { get; set; }
        public propTypesCombo(string paramUnit, string paramKey,string propType, string viewName)
        {
            this.paramUnit = paramUnit;
            this.paramKey = paramKey;
            this.propType = propType;
            this.viewName = viewName;
        }
        public propTypesCombo(string paramKey,string seq)
        {
            this.seq = seq;
            this.paramKey = paramKey;
        }
    }

    public class MinMaxRagne
    {
        public double min { get; set; }
        public double max { get; set; }
        public string range { get; set; }
        public MinMaxRagne(double min, double max)
        {
            this.min = min;
            this.max = max;
            this.range = string.Format("{0}-{1}", min, max);
        }
    }

    public class BINSBSummary
    {
        public int code { get;set;}
        public string message { get;set;}
        public List<SummaryResponse> response { get; set; }
    }
    public class SummaryResponse
    {
        public string seq { get; set; }
        public string binMetaSeq { get; set; }
        public List<int> factorIndexes { get; set; }
        public List<Summary> summary { get; set; }
    }
    public class Summary
    {
        public SummaryData normal { get; set; }
        public SummaryData lpf { get; set; }
        public SummaryData hpf { get; set; }
        public SummaryData bpf { get; set; }
    }
    public class SummaryData
    {
        public double min { get; set; }
        public double avg { get; set; }
        public double max { get; set; }
        public List<double> psd { get; set; }
        public List<double> frequency { get; set; }
        public List<double> rms { get; set; }
        public double avg_rms { get; set; }
        public List<double> zeta { get; set; }
        public double burstFactor { get; set; }
        public List<double> n0 { get; set; }
        public double avg_n0 { get; set; }

        public List<double> rmsToPeak { get; set; }
        public double maxRmsToPeak { get; set; }
        public double maxLoadAccel { get; set; }
    }

    public class BINSummary
    {
        public string key { get; set; }
        public string value { get; set; }
        public BINSummary(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        public BINSummary()
        {

        }
    }

    public class BINSummaryList
    {
        public string key { get; set; }
        public List<string> valueList { get; set; }
        public BINSummaryList(string key, List<string> valueList)
        {
            this.key = key;
            this.valueList = valueList;
        }
    }

    public class BINParamCombo
    {
        public string paramKey { get; set; }
        public string seq { get; set; }
    }

    public class BINMetaData
    {
        public BINMetaData()
        {
            this.shortblcokSeqList = new List<string>();
        }
        public int[] index { get; set; }
        public List<string> shortblcokSeqList { get; set; }
        public dynamic jsonResult { get; set; }
    }
}
