﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ParamDataSelectionData
    {
        string dataView = string.Empty;
        string dataType = string.Empty;
        string dataName = string.Empty;
        string selectionStart;
        string selectionEnd;
        string dataCnt = string.Empty;
        int add = 1;
        int del = 1;
        int view = 1;
        //string sourceType = string.Empty;
        string sourceSeq = string.Empty;
        string sourceName = string.Empty;
        string seq = null;
        //string paramPack = string.Empty;
        //string paramSeq = string.Empty;
        //string julianStartAt = string.Empty;
        //string julianEndAt = string.Empty;
        //string offsetStartAt = string.Empty;
        //string offsetEndAt = string.Empty;

        public string partSeq { get; set; }
        public string DataType { get => dataType; set => dataType = value; }
        public string DataView { get => dataView; set => dataView = value; }
        public string DataName { get => dataName; set => dataName = value; }
        public string SelectionStart { get => selectionStart; set => selectionStart = value; }
        public string SelectionEnd { get => selectionEnd; set => selectionEnd = value; }
        public string DataCnt { get => dataCnt; set => dataCnt = value; }
        public string sourceType { get; set; }
        public string SourceSeq { get => sourceSeq; set => sourceSeq = value; }
        public string SourceName { get => sourceName; set => sourceName = value; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public string paramKey { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public string offsetStartAt { get; set; }
        public string offsetEndAt { get; set; }
        public string useTime { get; set; }
        public int Add { get => add; set => add = value; }
        public int Del { get => del; set => del = value; }
        public int View { get => view; set => view = value; }
        public string Seq { get => seq; set => seq = value; }

        public ParamDataSelectionData()
        {
        }

        public ParamDataSelectionData(string dataView, string dataType, string dataName, string selectionStart, string selectionEnd, string dataCnt, string sourceSeq, int view = 1, string partSeq = null)
        {
            this.dataView = dataView;
            this.dataType = dataType;
            this.dataName = dataName;
            //this.parameter = parameter;
            this.selectionStart = selectionStart;
            this.selectionEnd = selectionEnd;
            this.dataCnt = dataCnt;
            this.sourceSeq = sourceSeq;
            this.view = view;
            this.partSeq = partSeq;
        }
        public ParamDataSelectionData(string dataView, string dataType, string dataName, string selectionStart, string selectionEnd, string dataCnt, string sourceSeq, int view, string useTime, string julianStartAt,string julianEndAt,string offsetStartAt,string offsetEndAt)
        {
            this.dataView = dataView;
            this.dataType = dataType;
            this.dataName = dataName;
            //this.parameter = parameter;
            this.selectionStart = selectionStart;
            this.selectionEnd = selectionEnd;
            this.dataCnt = dataCnt;
            this.sourceSeq = sourceSeq;
            this.view = view;
            this.partSeq = partSeq;
            this.julianEndAt = julianEndAt;
            this.julianStartAt = julianStartAt;
            this.offsetEndAt = offsetEndAt;
            this.offsetStartAt = offsetStartAt;
            this.useTime = useTime;
        }
        
        public ParamDataSelectionData(string dataView, string dataType, string dataName, string paramKey, string selectionStart, string selectionEnd, string dataCnt, string sourceSeq,string useTime, string seq, int view = 1, string paramSeq = null)
        {
            this.dataView = dataView;
            this.dataType = dataType;;
            this.dataName = dataName;
            this.selectionStart = selectionStart;
            this.selectionEnd = selectionEnd;
            this.paramSeq = paramSeq;
            this.paramKey = paramKey;
            this.dataCnt = dataCnt;
            this.sourceSeq = sourceSeq;
            this.view = view;
            this.partSeq = partSeq;
            this.useTime = useTime;
            this.seq = seq;
        }
        public ParamDataSelectionData DeepCopy()
        {
            ParamDataSelectionData newCopy = new ParamDataSelectionData();
            newCopy.dataView = this.dataView;
            newCopy.partSeq = this.partSeq;
            newCopy.DataType = this.DataType;
            newCopy.DataName = this.DataName;
            newCopy.SelectionStart = this.SelectionStart;
            newCopy.SelectionEnd = this.SelectionEnd;
            newCopy.DataCnt = this.DataCnt;
            newCopy.sourceType = this.sourceType;
            newCopy.SourceSeq = this.SourceSeq;
            newCopy.SourceName = this.SourceName;
            newCopy.paramPack = this.paramPack;
            newCopy.paramSeq = this.paramSeq;
            newCopy.paramKey = this.paramKey;
            newCopy.julianStartAt = this.julianStartAt;
            newCopy.julianEndAt = this.julianEndAt;
            newCopy.offsetStartAt = this.offsetStartAt;
            newCopy.offsetEndAt = this.offsetEndAt;
            newCopy.useTime = this.useTime;
            newCopy.Add = this.Add;
            newCopy.Del = this.Del;
            newCopy.View = this.View;
            newCopy.Seq = this.Seq;

            return newCopy;
        }
    }
    public class GridParam
    {
        public string paramKey { get; set; }
        public string seq { get; set; }
    }
}
