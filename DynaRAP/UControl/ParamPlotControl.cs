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
        private List<PlotGridData> plotGridDataList = new List<PlotGridData>();
        SaveParamModuleSelectDataResponse paramModuleResponse = null;
        EquationResponse equationResponse = null;
        ParameterModuleControl parameterModuleControl = null;
        private List<PlotGridSourceData> plotGridSourceDataList = null;
        public ParamPlotControl(ParameterModuleControl parameterModuleControl)
        {
            this.parameterModuleControl = parameterModuleControl;
            InitializeComponent();
        }

        private void ParamPlotControl_Load(object sender, EventArgs e)
        {
            InitializeGridControl();
        }
        private void InitializeGridControl()
        {
            GridColumn colPlotName = gridView1.Columns["PlotName"];
            colPlotName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colPlotName.OptionsColumn.FixedWidth = true;
            colPlotName.Width = 80;

            GridColumn colView = gridView1.Columns["View"];
            colView.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colView.OptionsColumn.FixedWidth = true;
            colView.Width = 40;
            colView.Caption = "보기";
            colView.OptionsColumn.ReadOnly = true;

            repositoryItemComboBox1.PopupFormMinSize = new System.Drawing.Size(0, 500);

            gridView1.OptionsView.ShowColumnHeaders = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = true;
            gridView1.IndicatorWidth = 40;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ColumnAutoWidth = true;

            gridView1.OptionsBehavior.ReadOnly = false;
            //gridView1.OptionsBehavior.Editable = false;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

            gridView1.CustomDrawRowIndicator += GridView1_CustomDrawRowIndicator;


            repositoryItemComboBox2.PopupFormMinSize = new System.Drawing.Size(0, 500);

            repositoryItemComboBox2.Items.Add("1D-Time");
            repositoryItemComboBox2.Items.Add("1D-Min, Max");
            repositoryItemComboBox2.Items.Add("2D-Potato");
            this.repositoryItemComboBox1.SelectedIndexChanged += RepositoryItemComboBox1_SelectedIndexChanged;


            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;
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
            MainForm mainForm = this.ParentForm as MainForm;

            PlotGridData selectGridData = (PlotGridData)gridView1.GetFocusedRow();
            if(selectGridData.PlotSeq != null && selectGridData.PlotSeq != "")
            {
                string sendData = string.Format(@"
                {{
                ""command"":""plot-data"",
                ""moduleSeq"": ""{0}"",
                ""plotSeq"" : ""{1}""
                }}", paramModuleSeq, selectGridData.PlotSeq);

                string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
                if (responseData != null)
                {
                    PlotDataResponse plotDataResponse = JsonConvert.DeserializeObject<PlotDataResponse>(responseData);
                    if (plotDataResponse.response != null && plotDataResponse.response.Count() != 0)
                    {
                        //dt = GetChartValues(evaluationResponse.response);
                        //AddChartData(selectGridData.eqName);
                        DataTable dt = new DataTable();

                        if (dt != null)
                        {
                            //MainForm mainForm = this.ParentForm as MainForm;

                            //if (chartControl != null)
                            //{
                            //    chartControl.Dispose();
                            //    chartControl = null;
                            //}

                            DXChartControl chartControl = new DXChartControl(plotDataResponse, plotGridSourceDataList);
                            DockPanel panelChart = null;

                            if (panelChart == null)
                            {
                                panelChart = new DockPanel();
                                panelChart = mainForm.DockManager1.AddPanel(DockingStyle.Float);
                                panelChart.FloatLocation = new Point(500, 100);
                                panelChart.FloatSize = new Size(1058, 528);
                                //panelChart.Name = this.sb.SbName;
                                //panelChart.Text = this.sb.SbName;
                                chartControl.Dock = DockStyle.Fill;
                                panelChart.Controls.Add(chartControl);
                                //panelChart.ClosedPanel += PanelChart_ClosedPanel;
                            }
                            else
                            {
                                //panelChart.Name = this.sb.SbName;
                                //panelChart.Text = this.sb.SbName;
                                //panelChart.Controls.Clear();
                                chartControl.Dock = DockStyle.Fill;
                                panelChart.Controls.Add(chartControl);
                                panelChart.Show();
                                panelChart.Focus();
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("PLOT을 먼저 저장 후 보기를 눌러주세요.");
            }

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

        public void SetSelectDataSource(string paramModuleSeq)
        {
            this.paramModuleSeq = paramModuleSeq;
            GetSelectDataList(paramModuleSeq);
            GetSelectPlotDataList(paramModuleSeq);
            panelTag.Controls.Clear();
        }
        public void GetSelectPlotDataList(string paramModuleSeq)
        {
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
                foreach (var list in plotListResponse.response)
                {
                    string item1 = null;
                    string item2 = null;
                    string item3 = null;
                    string item4 = null;
                    string item5 = null;
                    string itemSeq1 = null;
                    string itemSeq2 = null;
                    string itemSeq3 = null;
                    string itemSeq4 = null;
                    string itemSeq5 = null;
                    if (plotGridSourceDataList != null && plotGridSourceDataList.Count() != 0)
                    {
                        foreach (var sourceList in list.plotSourceList.Select((value, index) => new { value, index }))
                        {
                            var plotData = plotGridSourceDataList.Find(x => x.seq == sourceList.value.sourceSeq);
                            if (plotData != null)
                            {
                                switch (sourceList.index)
                                {
                                    case 0:
                                        item1 = plotData.itemName;
                                        itemSeq1 = sourceList.value.sourceSeq;
                                        break;
                                    case 1:
                                        item2 = plotData.itemName;
                                        itemSeq2 = sourceList.value.sourceSeq;
                                        break;
                                    case 2:
                                        item3 = plotData.itemName;
                                        itemSeq3 = sourceList.value.sourceSeq;
                                        break;
                                    case 3:
                                        item4 = plotData.itemName;
                                        itemSeq4 = sourceList.value.sourceSeq;
                                        break;
                                    case 4:
                                        item5 = plotData.itemName;
                                        itemSeq5 = sourceList.value.sourceSeq;
                                        break;
                                }
                            }
                        }
                    }
                    plotGridDataList.Add(new PlotGridData(Utils.base64StringDecoding(list.plotName),list.plotType,list.seq, item1, itemSeq1, item2, itemSeq2, item3, itemSeq3, item4, itemSeq4, item5, itemSeq5, list.dataProp.tags));
                }
            }
            this.gridControl1.DataSource = plotGridDataList;
            gridView1.RefreshData();
        }

        private void btnAddPlot_Click(object sender, EventArgs e)
        {
            if(paramModuleSeq == null)
            {
                MessageBox.Show("파라미터모듈을 먼저 선택 후 진행해주세요.");
            }
            else
            {
                if (plotGridDataList == null)
                {
                    plotGridDataList = new List<PlotGridData>();
                }
                plotGridDataList.Add(new PlotGridData("", "", "", "", "", ""));
                this.gridControl1.DataSource = plotGridDataList;
                gridView1.RefreshData();
            }
        }

        private void GetSelectDataList(string paramModuleSeq)
        {
            repositoryItemComboBox1.Items.Clear();
            repositoryItemComboBox1.Items.Add("");
            plotGridSourceDataList = new List<PlotGridSourceData>();
            string sendData = string.Format(@"
                {{
                ""command"":""source-list"",
                ""moduleSeq"": ""{0}""
                }}", paramModuleSeq);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                paramModuleResponse = new SaveParamModuleSelectDataResponse();
                paramModuleResponse = JsonConvert.DeserializeObject<SaveParamModuleSelectDataResponse>(responseData);
                if (paramModuleResponse.response != null && paramModuleResponse.response.Count() != 0)
                {
                    foreach (var list in paramModuleResponse.response)
                    {
                        string itemName = string.Format(@"{0}-{1}", Utils.base64StringDecoding(list.sourceName), list.paramKey);
                        plotGridSourceDataList.Add(new PlotGridSourceData(itemName, list.sourceType, list.seq));
                        repositoryItemComboBox1.Items.Add(itemName);
                    }
                }
                //this.gridControl2.DataSource = selectParamDataList;

                //MessageBox.Show(responseData);
            }

            sendData = string.Format(@"
                {{
                ""command"":""eq-list"",
                ""moduleSeq"": ""{0}""
                }}", paramModuleSeq);
            responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                equationResponse = new EquationResponse();
                equationResponse = JsonConvert.DeserializeObject<EquationResponse>(responseData);
                if (equationResponse.response != null && equationResponse.response.Count() != 0)
                {
                    foreach (var list in equationResponse.response)
                    {
                        list.eqName = Utils.base64StringDecoding(list.eqName);
                        plotGridSourceDataList.Add(new PlotGridSourceData(list.eqName, "eq", list.seq));
                        repositoryItemComboBox1.Items.Add(list.eqName);
                    }
                }
            }
        }

        private void SetPlotSaveRequest(string type)
        {
            GetSelectDataList(paramModuleSeq);
            PlotRequest plotRequest = new PlotRequest();
            plotRequest.command = "save-plot";
            plotRequest.moduleSeq = paramModuleSeq;
            plotRequest.plots = new List<Plot>();
            List<PlotGridData> gridDataList = (List<PlotGridData>)this.gridControl1.DataSource;

            foreach (var list in gridDataList)
            {
                List<SaveParamModuleSelectDataSource> itemSourceList = new List<SaveParamModuleSelectDataSource>();
                if(list.Item1 != null && list.Item1 != ""){
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item1);
                    if(findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if(findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                }
                if (list.Item2 != null && list.Item2 != "")
                {
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item2);
                    if (findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if (findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                    //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item2).seq));
                }
                if (list.Item3 != null && list.Item3 != "")
                {
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item3);
                    if (findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if (findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                    //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item3).seq));
                }
                if (list.Item4 != null && list.Item4 != "")
                {
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item4);
                    if (findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if (findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                    //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item4).seq));
                }
                if (list.Item5 != null && list.Item5 != "")
                {
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item5);
                    if (findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if (findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                    //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item5).seq));
                }

                Plot plot = new Plot();
                plot.sources = new List<PlotSources>();
                if(list.PlotName == null || list.PlotName == "")
                {
                    MessageBox.Show("PLOT이름 중 비어있는 항목이 있습니다. \nPLOT이름 입력 후 저장해주세요.");
                    return;
                }
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(list.PlotName);
                string encName = Convert.ToBase64String(basebyte);
                plot.plotName = encName;
                foreach (var sourceList in itemSourceList)
                {
                    if (sourceList != null)
                    {
                        plot.sources.Add(new PlotSources(sourceList.sourceType, sourceList.seq));
                        //plot.sources.Add(new PlotSources(sourceList.sourceType, sourceList.seq, sourceList.paramPack, sourceList.paramSeq, sourceList.julianStartAt, sourceList.julianEndAt, sourceList.offsetStartAt, sourceList.offsetEndAt));
                    }
                }
                plot.dataProp = new DataProps();
                plot.dataProp.key = "value";
                plot.dataProp.tags = list.tags;
                plot.plotType = list.PlotType;
                plotRequest.plots.Add(plot);
            }
            var json = JsonConvert.SerializeObject(plotRequest);

            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], json);
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    MessageBox.Show(type == "save" ? "저장 성공" : "전체삭제 성공");
                    GetSelectDataList(paramModuleSeq);
                    GetSelectPlotDataList(paramModuleSeq);
                }
                else
                {
                    MessageBox.Show(type == "save" ? "저장 실패" : "전체삭제 실패");
                }
            }
        }

        public bool PlotSaveRequest()
        {
            GetSelectDataList(paramModuleSeq);
            PlotRequest plotRequest = new PlotRequest();
            plotRequest.command = "save-plot";
            plotRequest.moduleSeq = paramModuleSeq;
            plotRequest.plots = new List<Plot>();
            List<PlotGridData> gridDataList = (List<PlotGridData>)this.gridControl1.DataSource;

            foreach (var list in gridDataList)
            {
                List<SaveParamModuleSelectDataSource> itemSourceList = new List<SaveParamModuleSelectDataSource>();
               if(list.Item1 != null && list.Item1 != ""){
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item1);
                    if(findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if(findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                }
                if (list.Item2 != null && list.Item2 != "")
                {
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item2);
                    if (findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if (findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                    //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item2).seq));
                }
                if (list.Item3 != null && list.Item3 != "")
                {
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item3);
                    if (findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if (findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                    //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item3).seq));
                }
                if (list.Item4 != null && list.Item4 != "")
                {
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item4);
                    if (findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if (findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                    //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item4).seq));
                }
                if (list.Item5 != null && list.Item5 != "")
                {
                    var findData = plotGridSourceDataList.Find(y => y.itemName == list.Item5);
                    if (findData.sourceType == "eq")
                    {
                        var findEq = equationResponse.response.Find(x => x.seq == findData.seq);
                        if (findEq != null)
                        {
                            itemSourceList.Add(new SaveParamModuleSelectDataSource("eq", "", findEq.seq));
                        }
                    }
                    else
                    {
                        itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == findData.seq));
                    }
                    //itemSourceList.Add(paramModuleResponse.response.Find(x => x.seq == plotGridSourceDataList.Find(y => y.itemName == list.Item5).seq));
                }
                Plot plot = new Plot();
                plot.sources = new List<PlotSources>();
                if (list.PlotName == null || list.PlotName == "")
                {
                    MessageBox.Show("PLOT이름 중 비어있는 항목이 있습니다. \nPLOT이름 입력 후 저장해주세요.");
                    return false;
                }
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(list.PlotName);
                string encName = Convert.ToBase64String(basebyte);
                plot.plotName = encName;
                foreach (var sourceList in itemSourceList)
                {
                    if (sourceList != null)
                    {
                        plot.sources.Add(new PlotSources(sourceList.sourceType, sourceList.seq));
                        //plot.sources.Add(new PlotSources(sourceList.sourceType, sourceList.seq, sourceList.paramPack, sourceList.paramSeq, sourceList.julianStartAt, sourceList.julianEndAt, sourceList.offsetStartAt, sourceList.offsetEndAt));
                    }
                }
                plot.dataProp = new DataProps();
                plot.dataProp.key = "value";
                plot.dataProp.tags = list.tags;
                plot.plotType = list.PlotType;
                plotRequest.plots.Add(plot);
            }
            var json = JsonConvert.SerializeObject(plotRequest);

            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], json);
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    repositoryItemComboBox1.Items.Clear();
                    repositoryItemComboBox1.Items.Add("");
                    plotGridSourceDataList = new List<PlotGridSourceData>();
                    paramModuleResponse = new SaveParamModuleSelectDataResponse();
                    
                    plotGridDataList.Clear();
                    this.gridControl1.DataSource = plotGridDataList;
                    gridView1.RefreshData();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
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
            btn.ButtonClick += removeTag_ButtonClick;
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
    }
}
