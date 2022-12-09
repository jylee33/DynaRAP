using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
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
    public partial class ParamPlotControl : DevExpress.XtraEditors.XtraUserControl
    {
        private string paramModuleSeq = null;
        private string moduleName = null;
        private List<PlotGridData> plotGridDataList = new List<PlotGridData>();
        SaveParamModuleSelectDataResponse paramModuleResponse = null;
        EquationResponse equationResponse = null;
        ParameterModuleControl parameterModuleControl = null;
        AdditionalResponse additionalResponse = null;
        List<PlotSourceResponse> plotSourceResponses = null;
        PlotModuleControl plotModuleControlBase = null;
        DataTable changePlotdt = new DataTable();
        public ParamPlotControl(ParameterModuleControl parameterModuleControl)
        {
            this.parameterModuleControl = parameterModuleControl;
            InitializeComponent();
        }

        private void ParamPlotControl_Load(object sender, EventArgs e)
        {
            InitializeGridControl();
            InitializeChangeGridControl();
        }
        private void InitializeGridControl()
        {
            GridColumn colPlotName = gridView1.Columns["plotName"];
            colPlotName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Near;
            colPlotName.OptionsColumn.FixedWidth = true;
            colPlotName.Width = 80;
            repositoryItemComboBox1.PopupFormMinSize = new System.Drawing.Size(0, 500);

            gridView1.OptionsView.ShowColumnHeaders = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            gridView1.IndicatorWidth = 40;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ColumnAutoWidth = true;

            gridView1.OptionsBehavior.ReadOnly = false;
            //gridView1.OptionsBehavior.Editable = false;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;


            gridView3.OptionsView.ShowColumnHeaders = true;
            gridView3.OptionsView.ShowGroupPanel = false;
            gridView3.OptionsView.ShowIndicator = false;
            gridView3.IndicatorWidth = 40;
            gridView3.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView3.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView3.OptionsView.ColumnAutoWidth = true;

            gridView3.OptionsBehavior.ReadOnly = false;
            //gridView1.OptionsBehavior.Editable = false;

            gridView3.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
        }
        private void InitializeChangeGridControl()
        {
            //gridView2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            //gridView2.OptionsView.ShowColumnHeaders = false;
            gridView2.OptionsView.ShowGroupPanel = false;
            gridView2.OptionsView.ShowIndicator = false;
            gridView2.IndicatorWidth = 40;
            gridView2.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView2.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView2.OptionsView.ColumnAutoWidth = true;

            gridView2.OptionsBehavior.ReadOnly = true;
            gridView2.OptionsBehavior.Editable = false;

            gridView2.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView2.OptionsMenu.EnableColumnMenu = false;


        }
        private void GridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private void RepositoryItemComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var combo = sender as ComboBoxEdit;
            //if (combo.SelectedIndex != -1)
            //{
            //    string fieldName = gridView1.FocusedColumn.FieldName;
            //    fieldName = fieldName.Insert(4, "Seq");
            //    string seq = "";
            //    if (combo.SelectedItem.ToString() != "")
            //    {
            //        seq = plotGridSourceDataList.Find(x => x.itemName == combo.SelectedItem.ToString()).seq;
            //    }
         
            //    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, fieldName, seq);
            //}
        }
         
        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            //MainForm mainForm = this.ParentForm as MainForm;

            //PlotGridData selectGridData = (PlotGridData)gridView1.GetFocusedRow();
            //if(selectGridData.PlotSeq != null && selectGridData.PlotSeq != "")
            //{
            //    string sendData = string.Format(@"
            //    {{
            //    ""command"":""plot-data"",
            //    ""moduleSeq"": ""{0}"",
            //    ""plotSeq"" : ""{1}""
            //    }}", paramModuleSeq, selectGridData.PlotSeq);

            //    string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            //    if (responseData != null)
            //    {
            //        PlotDataResponse plotDataResponse = JsonConvert.DeserializeObject<PlotDataResponse>(responseData);
            //        if (plotDataResponse.response != null && plotDataResponse.response.Count() != 0)
            //        {
            //            //dt = GetChartValues(evaluationResponse.response);
            //            //AddChartData(selectGridData.eqName);
            //            DataTable dt = new DataTable();

            //            if (dt != null)
            //            {
            //                //MainForm mainForm = this.ParentForm as MainForm;

            //                //if (chartControl != null)
            //                //{
            //                //    chartControl.Dispose();
            //                //    chartControl = null;
            //                //}

            //                //DXChartControl chartControl = new DXChartControl(plotDataResponse, plotGridSourceDataList);
            //                //if (mainForm.PlotModuleControl == null)
            //                //{
            //                //    mainForm.PlotModuleControl = new PlotModuleControl();
            //                //    DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = mainForm.TabbedView1.AddDocument(mainForm.PlotModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
            //                //    doc.Caption = "PLOT";
            //                //    mainForm.TabbedView1.ActivateDocument(mainForm.PlotModuleControl);
            //                //}
            //                //else
            //                //{
            //                //    mainForm.TabbedView1.ActivateDocument(mainForm.PlotModuleControl);
            //                //}

            //                //mainForm.PlotModuleControl.AddDocument(chartControl);


                            

            //                PlotModuleControl plotModuleControl = new PlotModuleControl(plotDataResponse, plotGridSourceDataList, this);
            //                DXChartControl chartControl = new DXChartControl(plotDataResponse, plotGridSourceDataList, plotModuleControl);

            //                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = mainForm.TabbedView1.AddDocument(plotModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
            //                    doc.Caption = "PLOT";
            //                plotModuleControl.AddDocument(chartControl);
            //                mainForm.TabbedView1.ActivateDocument(plotModuleControl);
                     


            //                //return;

            //                return;
            //                //DockPanel panelChart = null;

            //                //if (panelChart == null)
            //                //{
            //                //    panelChart = new DockPanel();
            //                //    panelChart = mainForm.DockManager1.AddPanel(DockingStyle.Float);
            //                //    panelChart.FloatLocation = new Point(500, 100);
            //                //    panelChart.FloatSize = new Size(1058, 528);
            //                //    //panelChart.Name = this.sb.SbName;
            //                //    //panelChart.Text = this.sb.SbName;
            //                //    chartControl.Dock = DockStyle.Fill;
            //                //    panelChart.Controls.Add(chartControl);
            //                //    //panelChart.ClosedPanel += PanelChart_ClosedPanel;
            //                //}
            //                //else
            //                //{
            //                //    //panelChart.Name = this.sb.SbName;
            //                //    //panelChart.Text = this.sb.SbName;
            //                //    //panelChart.Controls.Clear();
            //                //    chartControl.Dock = DockStyle.Fill;
            //                //    panelChart.Controls.Add(chartControl);
            //                //    panelChart.Show();
            //                //    panelChart.Focus();
            //                //}
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("PLOT을 먼저 저장 후 보기를 눌러주세요.");
            //}

            //if (MessageBox.Show(Properties.Resources.StringDelete, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //{
            //    return;
            //}

            //if (luePresetList.GetColumnValue("PresetName") == null)
            //    return;

            //string presetPack = String.Empty;
            //if (luePresetList.GetColumnValue("PresetPack") != null)
            //    presetPack = luePresetList.GetColumnValue("PresetPack").ToString();

            //if (string.IsNullOrEmpty(presetPack) == false)
            //{
            //    bool bResult = ParamRemove(presetPack);

            //    if (bResult)
            //    {
            //        gridView1.DeleteRow(gridView1.FocusedRowHandle);
            //    }
            //}
        }
        private void btnSavePlot_Click(object sender, EventArgs e)
        {
            SetPlotSaveRequest("save");
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            //if (plotGridDataList.Count() != 0 && MessageBox.Show("선택된 데이터가 전체 삭제됩니다. 삭제하시겠습니까?", "전체삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
            if (MessageBox.Show("선택된 데이터가 전체 삭제됩니다. 삭제하시겠습니까?", "전체삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                plotGridDataList.Clear();
                this.gridControl1.DataSource = plotGridDataList;
                gridView1.RefreshData();
                SetPlotSaveRequest("deleteAll");
            }
        }

        public void SetSelectDataSource(string paramModuleSeq, string moduleName)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("PLOT 데이터를 불러오는 중입니다.. 잠시만 기다려주십시오.");
            this.paramModuleSeq = paramModuleSeq;
            this.moduleName = moduleName;
            //GetSelectDataList(paramModuleSeq);
            GetSelectPlotDataList(paramModuleSeq);
            panelTag.Controls.Clear();
            mainForm.HideSplashScreenManager();
        }
        public void GetSelectPlotDataList(string paramModuleSeq)
        {
            changePlotdt = new DataTable();
            changePlotdt.Columns.Clear();
            changePlotdt.Columns.Add("PlotName", typeof(string));
            changePlotdt.Columns.Add("SeriesName", typeof(string));
            string sendData = string.Format(@"
                {{
                ""command"":""plot-list"",
                ""moduleSeq"": ""{0}""
                }}", paramModuleSeq);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                plotGridDataList.Clear();
                PlotResponse plotListResponse = JsonConvert.DeserializeObject<PlotResponse>(responseData);
                this.additionalResponse = plotListResponse.additionalResponse;
                this.plotSourceResponses = plotListResponse.response;
                foreach (var list in plotListResponse.response)
                {
                    var plotGridSeriesList = changeSeriesData(Utils.base64StringDecoding(list.plotName), list.plotSeries, list.dataProp.tags);
                    if (plotGridSeriesList != null)
                    {
                        plotGridDataList.Add(new PlotGridData(Utils.base64StringDecoding(list.plotName), list.plotType, list.dataProp.tags, plotGridSeriesList));
                    }
                }
            }
            this.gridControl1.DataSource = plotGridDataList;
            this.gridControl2.DataSource = changePlotdt;

            gridView1.RefreshData();
        }

        private List<PlotGridSeries> changeSeriesData(string plotName , List<PlotSeries> plotSeries,string tag)
        {
            List<PlotGridSeries> plotGridSeriesList = new List<PlotGridSeries>();
            foreach (var plotData in plotSeries)
            {
                PlotGridSeries plotGridSeries = new PlotGridSeries(plotData, tag);
                plotGridSeries.seriesName = Utils.base64StringDecoding(plotData.seriesName);
                if (plotData.xAxisSourceType == "eq")
                {
                    EQList eQList = additionalResponse.eqlist.Find(x => x.seq == plotData.xAxisSourceSeq);
                    if (eQList != null)
                    {
                        string itemName = Utils.base64StringDecoding(eQList.eqName);
                        plotGridSeries.xAxis = itemName;
                    }
                }
                else
                {
                    SourceList sourceList = additionalResponse.sourceList.Find(x => x.seq == plotData.xAxisSourceSeq);
                    if (sourceList != null)
                    {
                        string itemName = string.Format(@"{0}-{1}", Utils.base64StringDecoding(sourceList.sourceName), sourceList.paramKey);
                        plotGridSeries.xAxis = itemName;
                        plotGridSeries.xParamKey = sourceList.paramKey;
                    }
                }
                if (plotData.yAxisSourceType == "eq")
                {
                    EQList eQList = additionalResponse.eqlist.Find(x => x.seq == plotData.yAxisSourceSeq);
                    if (eQList != null)
                    {
                        string itemName = Utils.base64StringDecoding(eQList.eqName);
                        plotGridSeries.yAxis = itemName;
                    }
                }
                else
                {
                    SourceList sourceList = additionalResponse.sourceList.Find(x => x.seq == plotData.yAxisSourceSeq);
                    if (sourceList != null)
                    {
                        string itemName = string.Format(@"{0}-{1}", Utils.base64StringDecoding(sourceList.sourceName), sourceList.paramKey);
                        plotGridSeries.yAxis = itemName;
                        plotGridSeries.yParamKey = sourceList.paramKey;

                    }
                }
                if(plotData.chartType == "Cross Plot")
                {
                    if (string.IsNullOrEmpty(plotGridSeries.xAxis) && string.IsNullOrEmpty(plotGridSeries.yAxis))
                    {
                        changePlotdt.Rows.Add(plotName, plotGridSeries.seriesName);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(plotGridSeries.yAxis)|| (plotData.xAxisSourceType != null && string.IsNullOrEmpty(plotGridSeries.xAxis)))
                    {
                        changePlotdt.Rows.Add(plotName,plotGridSeries.seriesName);
                    }
                }

                plotGridSeriesList.Add(plotGridSeries);
            }
            if (plotSeries.Count() != plotGridSeriesList.Count())
            {
                plotGridSeriesList = null;

            }
            return plotGridSeriesList;
        }

        private void btnAddPlot_Click(object sender, EventArgs e)
        {
            //if(paramModuleSeq == null)
            //{
            //    MessageBox.Show("파라미터모듈을 먼저 선택 후 진행해주세요.");
            //}
            //else
            //{
            //    if (plotGridDataList == null)
            //    {
            //        plotGridDataList = new List<PlotGridData>();
            //    }
            //    plotGridDataList.Add(new PlotGridData("", "", "", "", "", ""));
            //    this.gridControl1.DataSource = plotGridDataList;
            //    gridView1.RefreshData();
            //}
        }

        private void GetSelectDataList(string paramModuleSeq)
        {
            //repositoryItemComboBox1.Items.Clear();
            //repositoryItemComboBox1.Items.Add("");
            //plotGridSourceDataList = new List<PlotGridSourceData>();
            //string sendData = string.Format(@"
            //    {{
            //    ""command"":""source-list"",
            //    ""moduleSeq"": ""{0}""
            //    }}", paramModuleSeq);
            //string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            //if (responseData != null)
            //{
            //    paramModuleResponse = new SaveParamModuleSelectDataResponse();
            //    paramModuleResponse = JsonConvert.DeserializeObject<SaveParamModuleSelectDataResponse>(responseData);
            //    if (paramModuleResponse.response != null && paramModuleResponse.response.Count() != 0)
            //    {
            //        foreach (var list in paramModuleResponse.response)
            //        {
            //            string itemName = string.Format(@"{0}-{1}", Utils.base64StringDecoding(list.sourceName), list.paramKey);
            //            plotGridSourceDataList.Add(new PlotGridSourceData(itemName, list.sourceType, list.seq));
            //            repositoryItemComboBox1.Items.Add(itemName);
            //        }
            //    }
            //    //this.gridControl2.DataSource = selectParamDataList;

            //    //MessageBox.Show(responseData);
            //}

            //sendData = string.Format(@"
            //    {{
            //    ""command"":""eq-list"",
            //    ""moduleSeq"": ""{0}""
            //    }}", paramModuleSeq);
            //responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            //if (responseData != null)
            //{
            //    equationResponse = new EquationResponse();
            //    equationResponse = JsonConvert.DeserializeObject<EquationResponse>(responseData);
            //    if (equationResponse.response != null && equationResponse.response.Count() != 0)
            //    {
            //        foreach (var list in equationResponse.response)
            //        {
            //            list.eqName = Utils.base64StringDecoding(list.eqName);
            //            plotGridSourceDataList.Add(new PlotGridSourceData(list.eqName, "eq", list.seq));
            //            repositoryItemComboBox1.Items.Add(list.eqName);
            //        }
            //    }
            //}
        }

        private void SetPlotSaveRequest(string type)
        {
            //GetSelectDataList(paramModuleSeq);
            //PlotRequest plotRequest = new PlotRequest();
            //plotRequest.command = "save-plot";
            //plotRequest.moduleSeq = paramModuleSeq;
            //plotRequest.plots = new List<Plot>();
            //List<PlotGridData> gridDataList = (List<PlotGridData>)this.gridControl1.DataSource;

            //foreach (var list in gridDataList)
            //{
            //    List<SaveParamModuleSelectDataSource> itemSourceList = new List<SaveParamModuleSelectDataSource>();
            //    if(list.Item1 != null && list.Item1 != ""){
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item1);
            //        if(findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if(findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //    }
            //    if (list.Item2 != null && list.Item2 != "")
            //    {
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item2);
            //        if (findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if (findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //        //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item2).seq));
            //    }
            //    if (list.Item3 != null && list.Item3 != "")
            //    {
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item3);
            //        if (findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if (findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //        //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item3).seq));
            //    }
            //    if (list.Item4 != null && list.Item4 != "")
            //    {
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item4);
            //        if (findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if (findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //        //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item4).seq));
            //    }
            //    if (list.Item5 != null && list.Item5 != "")
            //    {
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item5);
            //        if (findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if (findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //        //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item5).seq));
            //    }

            //    Plot plot = new Plot();
            //    plot.sources = new List<PlotSources>();
            //    if(list.PlotName == null || list.PlotName == "")
            //    {
            //        MessageBox.Show("PLOT이름 중 비어있는 항목이 있습니다. \nPLOT이름 입력 후 저장해주세요.");
            //        return;
            //    }
            //    byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(list.PlotName);
            //    string encName = Convert.ToBase64String(basebyte);
            //    plot.plotName = encName;
            //    foreach (var sourceList in itemSourceList)
            //    {
            //        if (sourceList != null)
            //        {
            //            plot.sources.Add(new PlotSources(sourceList.sourceType, sourceList.seq));
            //            //plot.sources.Add(new PlotSources(sourceList.sourceType, sourceList.seq, sourceList.paramPack, sourceList.paramSeq, sourceList.julianStartAt, sourceList.julianEndAt, sourceList.offsetStartAt, sourceList.offsetEndAt));
            //        }
            //    }
            //    plot.dataProp = new DataProps();
            //    plot.dataProp.key = "value";
            //    plot.dataProp.tags = list.tags;
            //    plot.plotType = list.PlotType;
            //    plotRequest.plots.Add(plot);
            //}
            //var json = JsonConvert.SerializeObject(plotRequest);

            //string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], json);
            //if (responseData != null)
            //{
            //    JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
            //    if (result.code == 200)
            //    {
            //        MessageBox.Show(type == "save" ? "저장 성공" : "전체삭제 성공");
            //        GetSelectDataList(paramModuleSeq);
            //        GetSelectPlotDataList(paramModuleSeq);
            //    }
            //    else
            //    {
            //        MessageBox.Show(type == "save" ? "저장 실패" : "전체삭제 실패");
            //    }
            //}
        }

        public void PlotSaveRequest()
        {
            if (plotModuleControlBase != null)
            {
                MainForm mainForm = this.ParentForm as MainForm;
                mainForm.ShowSplashScreenManager("PLOT 데이터를 저장 중입니다.. 잠시만 기다려주십시오.");
                plotModuleControlBase.SavePlotData("outSide");
                mainForm.HideSplashScreenManager();
            }
            //GetSelectDataList(paramModuleSeq);
            //PlotRequest plotRequest = new PlotRequest();
            //plotRequest.command = "save-plot";
            //plotRequest.moduleSeq = paramModuleSeq;
            //plotRequest.plots = new List<Plot>();
            //List<PlotGridData> gridDataList = (List<PlotGridData>)this.gridControl1.DataSource;

            //foreach (var list in gridDataList)
            //{
            //    List<SaveParamModuleSelectDataSource> itemSourceList = new List<SaveParamModuleSelectDataSource>();
            //   if(list.Item1 != null && list.Item1 != ""){
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item1);
            //        if(findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if(findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //    }
            //    if (list.Item2 != null && list.Item2 != "")
            //    {
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item2);
            //        if (findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if (findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //        //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item2).seq));
            //    }
            //    if (list.Item3 != null && list.Item3 != "")
            //    {
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item3);
            //        if (findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if (findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //        //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item3).seq));
            //    }
            //    if (list.Item4 != null && list.Item4 != "")
            //    {
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item4);
            //        if (findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if (findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //        //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item4).seq));
            //    }
            //    if (list.Item5 != null && list.Item5 != "")
            //    {
            //        var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item5);
            //        if (findData.sourceType == "eq")
            //        {
            //            var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
            //            if (findEq != null)
            //            {
            //                itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
            //            }
            //        }
            //        else
            //        {
            //            itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
            //        }
            //        //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item5).seq));
            //    }
            //    Plot plot = new Plot();
            //    plot.sources = new List<PlotSources>();
            //    if (list.PlotName == null || list.PlotName == "")
            //    {
            //        MessageBox.Show("PLOT이름 중 비어있는 항목이 있습니다. \nPLOT이름 입력 후 저장해주세요.");
            //        return false;
            //    }
            //    byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(list.PlotName);
            //    string encName = Convert.ToBase64String(basebyte);
            //    plot.plotName = encName;
            //    foreach (var sourceList in itemSourceList)
            //    {
            //        if (sourceList != null)
            //        {
            //            plot.sources.Add(new PlotSources(sourceList.sourceType, sourceList.seq));
            //            //plot.sources.Add(new PlotSources(sourceList.sourceType, sourceList.seq, sourceList.paramPack, sourceList.paramSeq, sourceList.julianStartAt, sourceList.julianEndAt, sourceList.offsetStartAt, sourceList.offsetEndAt));
            //        }
            //    }
            //    plot.dataProp = new DataProps();
            //    plot.dataProp.key = "value";
            //    plot.dataProp.tags = list.tags;
            //    plot.plotType = list.PlotType;
            //    plotRequest.plots.Add(plot);
            //}
            //var json = JsonConvert.SerializeObject(plotRequest);

            //string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], json);
            //if (responseData != null)
            //{
            //    JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
            //    if (result.code == 200)
            //    {
            //        repositoryItemComboBox1.Items.Clear();
            //        repositoryItemComboBox1.Items.Add("");
            //        plotGridSourceDataList = new List<PlotGridSourceData>();
            //        paramModuleResponse = new SaveParamModuleSelectDataResponse();
                    
            //        plotGridDataList.Clear();
            //        this.gridControl1.DataSource = plotGridDataList;
            //        gridView1.RefreshData();
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //return false;
        }


        private void edtTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                PlotGridData selectGridData = (PlotGridData)gridView1.GetFocusedRow();
                if (selectGridData.tags != null && selectGridData.tags != "")
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", selectGridData.tags + "|" + me.Text);
                }
                else
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", me.Text);

                }
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }

        private void edtTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                PlotGridData selectGridData = (PlotGridData)gridView1.GetFocusedRow();
                if (selectGridData.tags != null && selectGridData.tags != "")
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", selectGridData.tags + "|" + me.Text);
                }
                else
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", me.Text);

                }
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }

        private void addTag(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            ButtonEdit btn = new ButtonEdit();
            btn.Properties.Buttons[0].Kind = ButtonPredefines.Close;
            btn.BorderStyle = BorderStyles.Simple;
            btn.ForeColor = Color.White;
            btn.Properties.Appearance.BorderColor = Color.White;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
            btn.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            //btn.ReadOnly = true;
            btn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btn.Properties.AllowFocused = false;
            //btn.ButtonClick += removeTag_ButtonClick;
            btn.Text = name;
            panelTag.Controls.Add(btn);
        }

        private void removeTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit btn = sender as ButtonEdit;
            PlotGridData selectGridData = (PlotGridData)gridView1.GetFocusedRow();
            List<string> tagList = new List<string>();
            if(selectGridData.tags!= null && selectGridData.tags != "")
            {
                tagList =  selectGridData.tags.Split('|').ToList();
            }
            tagList.Remove(btn.Text);
            string tags = "";
            foreach(var tag in tagList)
            {
                tags += (tag + "|");
            }
            if(tags != "")
            {
                tags = tags.Substring(0, tags.Length - 1);
            }
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", tags);
            panelTag.Controls.Remove(btn);

        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            PlotGridData selectGridData = (PlotGridData)gridView1.GetFocusedRow();
            if(selectGridData.tags != null && selectGridData.tags != "")
            {
                string[] tagList = selectGridData.tags.Split('|');

                panelTag.Controls.Clear();
                foreach (string value in tagList)
                {
                    addTag(value);
                }
            }
            else
            {
                panelTag.Controls.Clear();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("PLOT 데이터를 가져오는 중입니다.. 잠시만 기다려주십시오.");
            if (paramModuleSeq == null)
            {
                MessageBox.Show("파라미터모듈을 먼저 선택 후 진행해주세요.");
            }
            else
            {
                if (additionalResponse != null)
                {
                    if ((additionalResponse.sourceList != null && additionalResponse.sourceList.Count != 0 )|| ( additionalResponse.eqlist != null && additionalResponse.eqlist.Count != 0))
                    {
                        PlotModuleControl plotModuleControl = new PlotModuleControl(additionalResponse, plotSourceResponses, this, paramModuleSeq);
                        //DXChartControl chartControl = new DXChartControl(plotDataResponse, plotGridSourceDataList, plotModuleControl);

                        DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = mainForm.TabbedView1.AddDocument(plotModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                        if (moduleName != null)
                        {
                            doc.Caption = moduleName + " PLOT";
                        }
                        else
                        {
                            doc.Caption = "PLOT";
                        }
                        //plotModuleControl.AddDocument(chartControl);
                        mainForm.TabbedView1.ActivateDocument(plotModuleControl);
                        this.plotModuleControlBase = plotModuleControl;
                    }
                    else
                    {
                        MessageBox.Show("데이터나 수식을 먼저 선택 후 진행해주세요.");
                    }
                }
            }
            mainForm.HideSplashScreenManager();

        }

        private void gridView3_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            PlotGridSeries selectGridData = (PlotGridSeries)gridView3.GetFocusedRow();
            if (selectGridData.tags != null && selectGridData.tags != "")
            {
                string[] tagList = selectGridData.tags.Split('|');

                panelTag.Controls.Clear();
                foreach (string value in tagList)
                {
                    addTag(value);
                }
            }
            else
            {
                panelTag.Controls.Clear();
            }
        }

        public void RefreshPlot()
        {
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("PLOT 데이터를 Refresh 중입니다.. 잠시만 기다려주십시오.");
            if (plotModuleControlBase != null)
            {
                plotModuleControlBase.RefreshPlot(this.additionalResponse);
            }
            mainForm.HideSplashScreenManager();

        }
    }
}
