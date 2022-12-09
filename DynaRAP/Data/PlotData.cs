using DynaRAP.Data;
using DynaRAP.UControl;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public AdditionalResponse additionalResponse { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    //public class PlotRequest
    //{
    //    public string command { get; set; }
    //    public string moduleSeq { get; set; }
    //    public List<Plot> plots { get; set; }

    //}

    //public class Plot
    //{
    //    public string plotName { get; set; }
    //    public string plotType { get; set; }
    //    public List<PlotSources> sources { get; set; }
    //    public DataProps dataProp { get; set; }
    //}

    public class AdditionalResponse
    {
        public List<SourceList> sourceList { get; set; }
        public List<EQList> eqlist { get; set; }

    }

    public class SourceList
    {
        public string seq { get; set; }
        public string moduleSeq { get; set; }
        public string sourceType { get; set; }
        public string sourceSeq { get; set; }
        public string paramPack { get; set; }
        public string paramSeq { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public string offsetEndAt { get; set; }
        public string sourceName { get; set; }
        public string sourceNo { get; set; }
        public string paramKey { get; set; }
        public int dataCount { get; set; }
        public List<string> timeSet { get; set; }
        public List<double> data { get; set; }
        public List<double> lpfData { get; set; }
        public List<double> hpfData { get; set; }
        public PlotSourceParams @param { get; set; }
    }

    public class PlotSourceParams
    {
        public string seq { get; set; }
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
        public string paramPack { get; set; }
        public string paramKey { get; set; }
        public string adamsKey { get; set; }
        public string zaeroKey { get; set; }
        public string grtKey { get; set; }
        public string fltpKey { get; set; }
        public string fltsKey { get; set; }
        public string partInfo { get; set; }
        public string partInfoSub { get; set; }
        public string propSeq { get; set; }
        public string paramSearchSeq { get; set; }
        public double lrpX { get; set; }
        public double lrpY { get; set; }
        public double lrpZ { get; set; }
        public string registerUid { get; set; }
        public Extras extras { get; set; }
        public string tags { get; set; }
        public int referenceSeq { get; set; }
        public PropInfo propInfo { get; set; }
        public bool deleted { get; set; }
    }

    public class EQList
    {
        public string seq { get; set; }
        public string moduleSeq { get; set; }
        public string eqName { get; set; }
        public string equation { get; set; }
        public int eqOrder { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
        public int dataCount { get; set; }
        public DataProps dataProp { get; set; }
        public string eqNo { get; set; }
        public List<object> data { get; set; }
        public List<string> timeSet { get; set; }
        public List<double[]> convhData { get; set; }
    }

    public class PlotRequest
    {
        public string command { get; set; }
        public string moduleSeq { get; set; }
        public List<Plot> plots { get; set; }

    }
    public class PlotAxisInfo
    {
        public string diagramType { get; set; }
        public string xTitle { get; set; }
        public string xSpacing { get; set; }
        public string xGridAlign { get; set; }
        public string xMinRange { get; set; }
        public string xMaxRange { get; set; }
        public string yTitle { get; set; }
        public string ySpacing { get; set; }
        public string yGridAlign { get; set; }
        public string yMinRange { get; set; }
        public string yMaxRange { get; set; }
        public PlotAxisInfo(PlotSourceResponse plot)
        {
            this.diagramType = plot.diagramType;
            this.xTitle = plot.xTitle;
            this.xSpacing = plot.xSpacing;
            this.xGridAlign = plot.xGridAlign;
            this.xMinRange = plot.xMinRange;
            this.xMaxRange = plot.xMaxRange;
            this.yTitle = plot.yTitle;
            this.ySpacing = plot.ySpacing;
            this.yGridAlign = plot.yGridAlign;
            this.yMinRange = plot.yMinRange;
            this.yMaxRange = plot.yMaxRange;
        }
        public PlotAxisInfo()
        { 
        }
        public PlotAxisInfo DeepCody()
        {
            PlotAxisInfo plotAxisInfo = new PlotAxisInfo();
            plotAxisInfo.diagramType = this.diagramType;
            plotAxisInfo.xTitle = this.xTitle;
            plotAxisInfo.xSpacing = this.xSpacing;
            plotAxisInfo.xGridAlign = this.xGridAlign;
            plotAxisInfo.xMinRange = this.xMinRange;
            plotAxisInfo.xMaxRange = this.xMaxRange;
            plotAxisInfo.yTitle = this.yTitle;
            plotAxisInfo.ySpacing = this.ySpacing;
            plotAxisInfo.yGridAlign = this.yGridAlign;
            plotAxisInfo.yMinRange = this.yMinRange;
            plotAxisInfo.yMaxRange = this.yMaxRange;
            return plotAxisInfo;
        }
    }
    public class Plot
    {
        public string plotName { get; set; }
        public string plotOrder { get; set; }
        public string plotType { get; set; }
        public string diagramType { get; set; }
        public string xTitle { get; set; }
        public string xSpacing { get; set; }
        public string xGridAlign { get; set; }
        public string xMinRange { get; set; }
        public string xMaxRange { get; set; }
        public string yTitle { get; set; }
        public string ySpacing { get; set; }
        public string yGridAlign { get; set; }
        public string yMinRange { get; set; }
        public string yMaxRange { get; set; }

        public List<PlotSeries> plotSeries { get; set; }
        public List<PlotPoint> selectPoints { get; set; }
        public DataProps dataProp { get; set; }
    }

    public class PlotSeries
    {
        public string seriesName { get; set; }
        public string chartType { get; set; }
        public string xAxisSourceType { get; set; }
        public string xAxisSourceSeq { get; set; }
        public string yAxisSourceType { get; set; }
        public string yAxisSourceSeq { get; set; }
        public string lineType { get; set; }
        public string lineColor { get; set; }
        public string lineBorder { get; set; }
        public PlotSeries()
        {

        }
        public PlotSeries(dxGridData dxGridData, string seriesName)
        {
            this.seriesName = seriesName;
            this.chartType = dxGridData.chartType;
            this.xAxisSourceType = dxGridData.xAxisSourceType;
            this.xAxisSourceSeq = dxGridData.xAxisSourceSeq;
            this.yAxisSourceType = dxGridData.yAxisSourceType;
            this.yAxisSourceSeq = dxGridData.yAxisSourceSeq;
            this.lineBorder = dxGridData.lineBorder;
            this.lineColor = dxGridData.lineColor;
            this.lineType = dxGridData.lineType;
        }
    }
    public class PlotPoint
    {
        public string xValue { get; set; }
        public string xSourceSeq { get; set; }
        public string xSourceType { get; set; }
        public string yValue { get; set; }
        public string ySourceSeq { get; set; }
        public string ySourceType { get; set; }
        public string chartType { get; set; }
        public int? pointIndex { get; set; }
        public PlotPoint()
        {

        }
        public PlotPoint(PlotPointData plotPoint)
        {
            this.chartType = plotPoint.chartType;
            this.xValue = plotPoint.xAxis;
            this.xSourceSeq = plotPoint.xSourceSeq;
            this.yValue = plotPoint.yAxis;
            this.ySourceSeq = plotPoint.ySourceSeq;
            this.xSourceType = plotPoint.xSourceType;
            this.ySourceType = plotPoint.ySourceType;
            this.pointIndex = plotPoint.pointIndex;
        }
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
        public PlotSources(string sourceType, string sourceSeq, string paramPack, string paramSeq, string julianStartAt, string julianEndAt, double offsetStartAt, double offsetEndAt)
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
        public string diagramType { get; set; }
        public string xTitle { get; set; }
        public string xSpacing { get; set; }
        public string xGridAlign { get; set; }
        public string xMinRange { get; set; }
        public string xMaxRange { get; set; }
        public string yTitle { get; set; }
        public string ySpacing { get; set; }
        public string yGridAlign { get; set; }
        public string yMinRange { get; set; }
        public string yMaxRange { get; set; }
        public CreatedAt createdAt { get; set; }
        public int plotOrder { get; set; }
        public DataProps dataProp { get; set; }
        public List<PlotSourceList> plotSourceList { get; set; }
        public List<PlotSeries> plotSeries { get; set; }
        public List<Sources> plotSources { get; set; }
        public List<PlotPoint> selectPoints { get; set; }
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

        public string plotName { get; set; }
        public string plotType { get; set; }
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
        public string tags { get; set; }
        public string PlotSeq { get; set; }

        public List<PlotGridSeries> plotSeries { get; set; }
        public PlotGridData(string plotName, string plotType, string tags, List<PlotGridSeries> plotSeries)
        {
            this.plotName = plotName;
            this.plotType = plotType;
            this.tags = tags;
            this.plotSeries = plotSeries;
        }

        public PlotGridData(string plotName, string plotType, string plotSeq, string item1, string itemSeq1, string item2, string itemSeq2, string item3, string itemSeq3, string item4, string itemSeq4, string item5, string itemSeq5, string tags)
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

    public class PlotGridSeries
    {
        public string seriesName { get; set; }
        public string chartType { get; set; }
        public string xAxis { get; set; }
        public string xParamKey { get; set; }
        public string yAxis { get; set; }
        public string yParamKey { get; set; }
        public string lineType { get; set; }
        public string lineColor { get; set; }
        public string lineBorder { get; set; }
        public string tags { get; set; }
        public PlotGridSeries(PlotSeries plotSeries, string tags)
        {
            this.chartType = plotSeries.chartType;
            this.lineType = plotSeries.lineType;
            this.lineColor = plotSeries.lineColor;
            this.lineBorder = plotSeries.lineBorder;
            this.tags = tags;
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
        public dxGridData(PlotSeries plotSourceResponse, string seriesName)
        {
            this.seriesName = seriesName;
            this.chartType = plotSourceResponse.chartType;
            this.xAxisSourceType = plotSourceResponse.xAxisSourceType;
            this.xAxisSourceSeq = plotSourceResponse.xAxisSourceSeq;
            this.yAxisSourceType = plotSourceResponse.yAxisSourceType;
            this.yAxisSourceSeq = plotSourceResponse.yAxisSourceSeq;
            this.lineBorder = plotSourceResponse.lineBorder;
            this.lineColor = plotSourceResponse.lineColor;
            this.lineType = plotSourceResponse.lineType;
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
        public string lineColor { get; set; }
        public string lineBorder { get; set; }
        public string lineType { get; set; }
        public int delete { get; set; }
        public string seriesName { get; set; }
        public string xAxis { get; set; }
        public string yAxis { get; set; }
        public string xAxisSourceType { get; set; }
        public string yAxisSourceType { get; set; }
        public string xAxisSourceSeq { get; set; }
        public string yAxisSourceSeq { get; set; }
    }

    public class PlotPointData
    {
        public string chartType { get; set; }
        public string xAxis { get; set; }
        public string yAxis { get; set; }
        public string xAxisName { get; set; }
        public string yAxisName { get; set; }

        public int delete { get; set; }
        public int? pointIndex { get; set; }
        public string xSourceSeq { get; set; }
        public string xSourceType { get; set; }
        public string ySourceSeq { get; set; }
        public string ySourceType { get; set; }
        public string sourceType { get; set; }
        public string paramKey { get; set; }
        public string xOriginSeq { get; set; }
        public string yOriginSeq { get; set; }

        public PlotPointData(int pointIndex, string chartType, string xAxis, string xAxisName, string xSourceSeq, string xOriginSeq, string xSourceType, string yAxis, string yAxisName, string ySourceSeq, string yOriginSeq, string ySourceType, string sourceType, string paramKey = null)
        {
            this.chartType = chartType;
            this.xAxis = xAxis;
            this.xAxisName = xAxisName;
            this.yAxis = yAxis;
            this.yAxisName = yAxisName;
            this.xSourceSeq = xSourceSeq;
            this.xOriginSeq = xOriginSeq;
            this.ySourceSeq = ySourceSeq;
            this.yOriginSeq = yOriginSeq;
            this.delete = 1;
            this.pointIndex = pointIndex;
            this.sourceType = sourceType;
            this.paramKey = paramKey;
            this.xSourceType = xSourceType;
            this.ySourceType = ySourceType;
        }

        public PlotPointData(PlotPoint plotPoint)
        {
            this.chartType = plotPoint.chartType;
            this.xAxis = plotPoint.xValue;
            this.yAxis = plotPoint.yValue;
            this.pointIndex = plotPoint.pointIndex;
            this.delete = 1;
            if (plotPoint.xSourceType != null)
            {
                this.xSourceType = plotPoint.xSourceType;
                this.xSourceSeq = plotPoint.xSourceSeq;
            }
            if (plotPoint.ySourceType != null)
            {
                this.ySourceType = plotPoint.ySourceType;
                this.ySourceSeq = plotPoint.ySourceSeq;
            }
        }
    }

    public class PlotSeriesSource
    {
        public string sourceType { get; set; }
        public string seq { get; set; }
        public string itemName { get; set; }
        public string originSeq { get; set; }
        public PlotSeriesSource(string sourceType, string seq, string itemName, string originSeq = null)
        {
            this.sourceType = sourceType;
            this.seq = seq;
            this.itemName = itemName;
            this.originSeq = originSeq;
        }
    }

    public class PotatoChartInfo
    {
        public Color color { get; set; }
        public Cluster cluster { get; set; }
        public int lineTickness { get; set; }
        public PotatoChartInfo(Cluster cluster, Color color, int lineTickness)
        {
            this.cluster = cluster;
            this.color = color;
            this.lineTickness = lineTickness;
        }
    }

    public class FindSourceRequest
    {
        public string command { get; set; }
        public string moduleSeq { get; set; }   
        public string eqSeq { get; set; }
        public string xValue { get; set; }
        public string yValue { get; set; }
        public string chartType { get; set; }
        public int? pointIndex{ get; set; }
        public FindSourceRequest(PlotPointData eqPlotPointData)
        {
            this.command = "find-source";
            this.moduleSeq = eqPlotPointData.yOriginSeq;
            this.eqSeq = eqPlotPointData.ySourceSeq;
            this.xValue = eqPlotPointData.xAxis;
            this.yValue = eqPlotPointData.yAxis;
            this.chartType = eqPlotPointData.chartType;
            this.pointIndex = eqPlotPointData.pointIndex;
        }
    }

    public class FindSourceResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public FindSource response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }
    public class FindSource
    {
        public string partSeq { get; set; }
        public string blockSeq { get; set; }
        public List<string> timeSet { get; set; }
    }
}

