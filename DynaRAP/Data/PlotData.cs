using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class PlotResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<PlotSourceResponse> response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class PlotRequest
    {
        public string command { get; set; }
        public string moduleSeq { get; set; }
        public List<Plot> plots { get; set; }

    }

    public class Plot
    {
        public string plotName { get; set; }
        public string plotType { get; set; }
        public List<PlotSources> sources { get; set; }
        public DataProps dataProp { get; set; }
    }
    
    public class PlotSources
    {
        public string sourceType { get; set; }
        public string sourceSeq { get; set; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
        public PlotSources(string sourceType , string sourceSeq, string paramPack, string paramSeq, string julianStartAt, string julianEndAt, double offsetStartAt, double offsetEndAt)
        {
            this.sourceType = sourceType;
            this.sourceSeq = sourceSeq;
            this.paramPack = paramPack;
            this.paramSeq = paramSeq;
            this.julianStartAt = julianStartAt;
            this.julianEndAt = julianEndAt;
            this.offsetStartAt = offsetStartAt;
            this.offsetEndAt = offsetEndAt;
        }
        public PlotSources(string sourceType, string sourceSeq)
        {
            this.sourceType = sourceType;
            this.sourceSeq = sourceSeq;
        }
    }
    public class PlotSourceResponse
    {
        public string seq { get; set; }
        public string moduleSeq { get; set; }
        public string plotName { get; set; }
        public string plotType { get; set; }
        public CreatedAt createdAt { get; set; }
        public int plotOrder { get; set; }
        public DataProps dataProp { get; set; }
        public List<PlotSourceList> plotSourceList { get; set; }
        public List<Sources> plotSources { get; set; }
    }

    public class PlotSourceList
    {
        public string seq { get; set; }
        public string moduleSeq { get; set; }
        public string plotSeq { get; set; }
        public string sourceType { get; set; }
        public string sourceSeq { get; set; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
    }
    public class Sources
    {
        public string seq { get; set; }
        public string sourceType { get; set; }
        public string sourceSeq { get; set; }
    }
    public class PlotGridData
    {
        string plotName = string.Empty;
        string plotType = string.Empty;
        string item1 = string.Empty;
        string item2 = string.Empty;
        string item3 = string.Empty;
        string item4 = string.Empty;
        string item5 = string.Empty;
        string itemSeq1 = string.Empty;
        string itemSeq2 = string.Empty;
        string itemSeq3 = string.Empty;
        string itemSeq4 = string.Empty;
        string itemSeq5 = string.Empty;

        public string PlotName { get => plotName; set => plotName = value; }
        public string PlotType { get => plotType; set => plotType = value; }
        public string Item1 { get => item1; set => item1 = value; }
        public string Item2 { get => item2; set => item2 = value; }
        public string Item3 { get => item3; set => item3 = value; }
        public string Item4 { get => item4; set => item4 = value; }
        public string Item5 { get => item5; set => item5 = value; }
        public string ItemSeq1 { get => itemSeq1; set => itemSeq1 = value; }
        public string ItemSeq2 { get => itemSeq2; set => itemSeq2 = value; }
        public string ItemSeq3 { get => itemSeq3; set => itemSeq3 = value; }
        public string ItemSeq4 { get => itemSeq4; set => itemSeq4 = value; }
        public string ItemSeq5 { get => itemSeq5; set => itemSeq5 = value; }
        public int View { get; set; }
        public string tags{ get; set; } 
        public string PlotSeq { get; set; }
        public PlotGridData(string plotName, string item1, string item2, string item3, string item4, string item5)
        {
            this.plotName = plotName;
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
            this.item5 = item5;
            this.View = 1;
        }

        public PlotGridData(string plotName,string plotType,string plotSeq, string item1, string itemSeq1, string item2, string itemSeq2, string item3, string itemSeq3, string item4, string itemSeq4, string item5, string itemSeq5, string tags)
        {
            this.plotName = plotName;
            this.plotType = plotType;
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
            this.item5 = item5;
            this.itemSeq1 = itemSeq1;
            this.itemSeq2 = itemSeq2;
            this.itemSeq3 = itemSeq3;
            this.itemSeq4 = itemSeq4;
            this.itemSeq5 = itemSeq5;
            this.tags = tags;
            this.PlotSeq = plotSeq;
            this.View = 1;
        }
    }

    public class PlotGridSourceData
    {
        public string itemName { get; set; }
        public string sourceType { get; set; }
        public string seq { get; set; }
        public PlotGridSourceData(string itemName, string sourceType, string seq) 
        {
            this.itemName = itemName;
            this.sourceType = sourceType;
            this.seq = seq;
        }
    }

    public class PlotDataResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<List<double>> response { get; set; }
        public PlotAddtitional additionalResponse { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class PlotAddtitional
    {
        public string seq { get; set; }
        public string moduleSeq { get; set; }
        public string plotName { get; set; }
        public string plotType { get; set; }
        public CreatedAt createdAt { get; set; }
        public int plotOrder { get; set; }
        public List<PlotSource> plotSourceList { get; set; }
        public List<plotSources> plotSources { get; set; }
        public DataProps dataProp { get; set; }
    }
    public class PlotSource 
    {
        public string seq { get; set; }
        public string moduleSeq { get; set; }
        public string plotSeq { get; set; }
        public string sourceType { get; set; }
        public string sourceSeq { get; set; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
    }
    public class plotSources
    {
        public string seq { get; set; }
        public string sourceType { get; set; }
        public string sourceSeq { get; set; }
    }

    public class dxGridData
    {   
        public dxGridData()
        {
            this.delete = 1;
        }
        //public dxGridData(string seriesName, string chartType ,string xAxis, string yAxis = null)
        //{
        //    this.seriesName = seriesName;
        //    this.chartType = chartType;
        //    this.xAxis = xAxis;
        //    this.yAxis = yAxis;
        //}

        public string chartType { get; set; }
        public string seriesColor { get; set; }
        public string bordLength { get; set; }
        public string seriesLength { get; set; }
        public string bordType { get; set; }
        public int delete { get; set; }
        public string seriesName { get; set; }
        public string xAxis { get; set; }
        public string yAxis { get; set; }
    }
}
