using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ParamDataSelectionData
    {
        string dataType = string.Empty;
        string dataName = string.Empty;
        List<GridParam> paramList = null;
        string selectionStart;
        string selectionEnd;
        string dataCnt = string.Empty;
        int add = 1;
        int del = 1;
        int view = 1;
        //string sourceType = string.Empty;
        string sourceSeq = string.Empty;
        //string paramPack = string.Empty;
        //string paramSeq = string.Empty;
        //string julianStartAt = string.Empty;
        //string julianEndAt = string.Empty;
        //string offsetStartAt = string.Empty;
        //string offsetEndAt = string.Empty;

        public string DataType { get => dataType; set => dataType = value; }
        public string DataName { get => dataName; set => dataName = value; }
        public List<GridParam> ParamList { get => paramList; set => paramList = value; }
        public string SelectionStart { get => selectionStart; set => selectionStart = value; }
        public string SelectionEnd { get => selectionEnd; set => selectionEnd = value; }
        public string DataCnt { get => dataCnt; set => dataCnt = value; }
        public string sourceType { get; set; }
        public string SourceSeq { get => sourceSeq; set => sourceSeq = value; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public DateTime julianStartAt { get; set; }
        public DateTime julianEndAt { get; set; }
        public string offsetStartAt { get; set; }
        public string offsetEndAt { get; set; }
        public int Add { get => add; set => add = value; }
        public int Del { get => del; set => del = value; }
        public int View { get => view; set => view = value; }

        public ParamDataSelectionData()
        {
        }

        public ParamDataSelectionData(string dataType, string dataName, string selectionStart, string selectionEnd, string dataCnt, string sourceSeq, int view = 1, List<GridParam> paramList = null)
        {
            this.dataType = dataType;
            this.dataName = dataName;
            //this.parameter = parameter;
            this.selectionStart = selectionStart;
            this.selectionEnd = selectionEnd;
            this.dataCnt = dataCnt;
            this.sourceSeq = sourceSeq;
            this.view = view;
            this.paramList = paramList;
    }

        public class GridParam
        {
            public string paramKey { get; set; }
            public string seq { get; set; }
        }
    }
}
