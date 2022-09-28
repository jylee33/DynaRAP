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
            this.fieldType = string.Format("{0}({1})", paramDatas.propInfo.propType, paramDatas.propInfo.paramUnit);
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
    }


    public class propTypesCombo
    {
        public string paramKey { get; set; }
        public string paramUnit { get; set; }
        public string propType { get; set; }
        public string viewName { get; set; }
        public string seq { get; set; }
        public propTypesCombo(string paramUnit, string paramKey, string propType, string viewName)
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
}
