using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking2010.Views.Widget;
using DevExpress.XtraEditors;
using DynaRAP.Data;
using DynaRAP.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class PlotModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        AdditionalResponse additionalResponse = null;
        List<PlotSourceResponse> plotSourceResponses = null;
        ParamPlotControl paramPlotControl = null;
        string paramModuleSeq = null;
        public PlotModuleControl()
        {
            InitializeComponent();
        }
        public PlotModuleControl(AdditionalResponse additionalResponse, List<PlotSourceResponse> plotSourceResponses, ParamPlotControl paramPlotControl,string paramModuleSeq)
        {
            this.additionalResponse = additionalResponse;
            this.paramPlotControl = paramPlotControl;
            this.paramModuleSeq = paramModuleSeq;
            this.plotSourceResponses = plotSourceResponses;
            InitializeComponent();
        }

        private void PlotModuleControl_Load(object sender, EventArgs e)
        {
            AddDocumentManager();
            //for (int i = 0; i < 4; i++)
            //{
            //    AddDocuments();
            //}
            ////Adding Documents to group1 is not necessary, since all newly created Documents are automatically placed in the first StackGroup.
            //group1.Items.AddRange(new Document[] { view.Documents[0] as Document, view.Documents[1] as Document });
            //view.Controller.Dock(view.Documents[2] as Document, group2);
            //view.Controller.Dock(view.Documents[3] as Document, group3);

            if(plotSourceResponses != null)
            {
                foreach (var sourceData in plotSourceResponses)
                {
                    List<dxGridData> dxGridDataList = new List<dxGridData>();
                    string plotName = Utils.base64StringDecoding(sourceData.plotName);
                    foreach (var plotData in sourceData.plotSeries)
                    {
                        string seriesName = Utils.base64StringDecoding(plotData.seriesName);
                        dxGridDataList.Add(new dxGridData(plotData, seriesName));
                    }
                    string tagValue = string.Empty;
                    if(sourceData.dataProp != null)
                    {
                        tagValue = sourceData.dataProp.tags;
                    }
                    DXChartControl chartControl = new DXChartControl(additionalResponse, this, dxGridDataList, tagValue);
                    this.AddDocument(chartControl, plotName);
                }
            }
        }

        WidgetView view;

        StackGroup group1, group2, group3;
        void AddDocumentManager()
        {
            DocumentManager dM = new DocumentManager(components);
            view = new WidgetView();
            dM.View = view;
            view.AllowDocumentStateChangeAnimation = DevExpress.Utils.DefaultBoolean.True;
            group1 = new StackGroup();
            group2 = new StackGroup();
            group3 = new StackGroup();
            group1.Length.UnitType = LengthUnitType.Star;
            group1.Length.UnitValue = 1;
            view.StackGroups.AddRange(new StackGroup[] { group1, group2, group3 });
            dM.ContainerControl = this;
        }

        int count = 0;

        private void btnAddPlot_Click(object sender, EventArgs e)
        {
            string plotName = Prompt.ShowDialogCancel("PLOT 이름 입력", "New PLOT");
            if (plotName.Equals("Cancel123"))
                return;
            if (string.IsNullOrEmpty(plotName))
            {
                if (plotName == string.Empty)
                {
                    MessageBox.Show("PLOT이름이 비어 있거나 입력되지 않았습니다. \n다시 확인해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            DXChartControl chartControl = new DXChartControl(additionalResponse,this);
            this.AddDocument(chartControl, plotName);
        }

        void AddDocuments()
        {
            Document document = view.AddDocument(new MyLineChart()) as Document;
            document.MaximizedControl = new MyLineChart();
            count++;
        }

        private void btnPlotSave_Click(object sender, EventArgs e)
        {
            SavePlotData("inSide");
        }
       
        public void SavePlotData(string location)
        {
            PlotRequest plotRequest = new PlotRequest();
            plotRequest.command = "save-plot";
            plotRequest.moduleSeq = paramModuleSeq;
            plotRequest.plots = new List<Plot>();

            for (int i = 0; i < view.Documents.Count; i++)
            {
                DXChartControl dXChartControl = (DXChartControl)view.Documents[i].Control;

                Plot plot = new Plot();
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(view.Documents[i].Caption); ;
                string encName = Convert.ToBase64String(basebyte);
                plot.plotName = encName;
                plot.plotOrder = i.ToString() ;
                plot.plotType = "1D-Time History";

                plot.dataProp = new DataProps();
                plot.dataProp.key = "value";
                plot.dataProp.tags = dXChartControl.getTagValue();
                plot.plotSeries = new List<PlotSeries>();
                List<dxGridData> seriesDataList = dXChartControl.getSeriesInfo();
                foreach(var seriesData in seriesDataList)
                {
                    byte[] byteName = System.Text.Encoding.UTF8.GetBytes(seriesData.seriesName); ;
                    string seriesName = Convert.ToBase64String(byteName);
                    PlotSeries plotSereis = new PlotSeries(seriesData, seriesName);
                    plot.plotSeries.Add(plotSereis);
                }
                plotRequest.plots.Add(plot);
            }
            var json = JsonConvert.SerializeObject(plotRequest);

            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], json);
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (location == "inSide")
                {
                    if (result.code == 200)
                    {
                        MessageBox.Show("저장 성공");
                        paramPlotControl.GetSelectPlotDataList(paramModuleSeq);
                    }
                    else
                    {
                        MessageBox.Show("저장 실패");
                    }
                }
            }
        }


        public void AddDocument(UserControl ctrl, string name = null)
        {
            Document document = view.AddDocument(ctrl) as Document;
            //document.MaximizedControl = new MyLineChart();
            document.Caption = "PLOT #" + count.ToString();
            if(name != null)
            {
                document.Caption = name;
            }

            switch (count % view.StackGroups.Count)
            {
                case 0:
                    view.Controller.Dock(document);
                    break;
                case 1:
                    view.Controller.Dock(document, group2);
                    break;
                case 2:
                    view.Controller.Dock(document, group3);
                    break;
                default:
                    view.Controller.Dock(document);
                    break;
            }

            count++;
        }

        public void GetDocument()
        {
            Document document = view.ActiveDocument as Document;
            string plotName = Prompt.ShowDialogCancel("PLOT 이름 입력", "Change PLOT Name", document.Caption);
            if (plotName.Equals("Cancel123"))
                return;
            if (string.IsNullOrEmpty(plotName))
            {
                if (plotName == string.Empty)
                {
                    MessageBox.Show("PLOT이름이 비어 있거나 입력되지 않았습니다. \n다시 확인해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }
            document.Caption = plotName;
        }
    }
}
