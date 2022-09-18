using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DynaRAP.Data;
using DynaRAP.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class ParamDataSelectControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ParamDataSelectionData> paramDataList = null;
        private List<ParamDataSelectionData> selectParamDataList = null;
        private Dictionary<string, RepositoryItemComboBox> comboDic = null;
        private Dictionary<string, RepositoryItemComboBox> selectComboDic = null;
        private Dictionary<string, List<GridParam>> gridPramDic = new Dictionary<string, List<GridParam>>();
        private string paramModuleSeq = null;
        private List<double> chartData = null;
        ChartControl chartControl = null;
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now;
        DataTable dt = null;
        DockPanel panelChart = null;

        public ParamDataSelectControl()
        {
            InitializeComponent();
        }

        private void ParamDataSelectControl_Load(object sender, EventArgs e)
        {
            //comboBoxEdit1.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            comboBoxEdit1.Properties.Items.Add("PART");
            comboBoxEdit1.Properties.Items.Add("SHORTBLOCK");
            comboBoxEdit1.Properties.Items.Add("DLL");
            comboBoxEdit1.Properties.Items.Add("PARAMMODULE");

            selectParamDataList = new List<ParamDataSelectionData>();
            selectComboDic = new Dictionary<string, RepositoryItemComboBox>();

            InitializeGridControl1();
            InitializeGridControl2();
        }
        private void InitializeGridControl1()
        {
            GridColumn colStart = gridView1.Columns["SelectionStart"];
            colStart.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colStart.OptionsColumn.FixedWidth = true;
            colStart.Width = 140;
            colStart.OptionsColumn.ReadOnly = true;

            GridColumn colEnd = gridView1.Columns["SelectionEnd"];
            colEnd.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colEnd.OptionsColumn.FixedWidth = true;
            colEnd.Width = 140;
            colEnd.OptionsColumn.ReadOnly = true;

            GridColumn colAdd = gridView1.Columns["Add"];
            colAdd.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colAdd.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colAdd.OptionsColumn.FixedWidth = true;
            colAdd.Width = 40;
            colAdd.Caption = "추가";
            colAdd.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;
            
            gridView1.CustomRowCellEditForEditing += (sender, e) =>
            {
                GridView view = sender as GridView;

                if (e.Column.FieldName == "ParamsCombo")
                {
                   
                    ParamDataSelectionData paramData = (ParamDataSelectionData)gridView1.GetFocusedRow();
                    this.gridColumn3.ColumnEdit = comboDic[paramData.SourceSeq];
                    e.RepositoryItem = comboDic[paramData.SourceSeq];

                    //방법2
                    //RepositoryItemLookUpEdit riLookup1 = new RepositoryItemLookUpEdit();

                    //riLookup1.DisplayMember = "paramKey";

                    //riLookup1.ValueMember = "paramKey";

                    //riLookup1.DataSource = gridPramDic[paramData.SourceSeq];

                    //e.RepositoryItem = riLookup1;
                }

            };
        }
        private void InitializeGridControl2()
        {
            chartData = new List<double>();
            GridColumn colStart = gridView2.Columns["SelectionStart"];
            colStart.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colStart.OptionsColumn.FixedWidth = true;
            colStart.Width = 140;
            colStart.OptionsColumn.ReadOnly = true;

            GridColumn colEnd = gridView2.Columns["SelectionEnd"];
            colEnd.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colEnd.OptionsColumn.FixedWidth = true;
            colEnd.Width = 140;
            colEnd.OptionsColumn.ReadOnly = true;

            GridColumn colDel = gridView2.Columns["Del"];
            colDel.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.OptionsColumn.FixedWidth = true;
            colDel.Width = 40;
            colDel.Caption = "삭제";
            colDel.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox2.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox2.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox2.Click += RepositoryItemImageComboBox2_Click;

            GridColumn colView = gridView2.Columns["View"];
            colView.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colView.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colView.OptionsColumn.FixedWidth = true;
            colView.Width = 40;
            colView.Caption = "보기";
            colView.OptionsColumn.ReadOnly = true;
            this.repositoryItemImageComboBox3.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox3.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox3.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox3.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox3.Click += RepositoryItemImageComboBox3_Click;

            gridView2.CustomRowCellEditForEditing += (sender, e) =>
            {

                GridView view = sender as GridView;

                if (e.Column.FieldName == "ParamsCombo")
                {
                    ParamDataSelectionData paramData = (ParamDataSelectionData)gridView2.GetFocusedRow();
                    e.RepositoryItem = selectComboDic[paramData.SourceSeq];
                }
            };
        }

        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            if(paramModuleSeq == null)
            {
                MessageBox.Show("파라미터모듈을 먼저 선택 후 추가해주세요.");
                return;
            }
            ParamDataSelectionData selectData = (ParamDataSelectionData)gridView1.GetFocusedRow();
            //selectParamDataList.Find(selectData)
            if (selectParamDataList.FindIndex(x => x.SourceSeq == selectData.SourceSeq) != -1)
            {
                MessageBox.Show("이미 추가된 데이터 입니다.");
            }
            else 
            {
                selectParamDataList.Add(selectData);

                //RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
                //foreach (var parameter in selectData.Parameter)
                //{
                //    repositoryItemComboBox.Items.Add(parameter.paramKey);
                //}
                //selectComboDic.Add(selectData.SourceSeq, repositoryItemComboBox);
                this.gridControl2.DataSource = selectParamDataList;
                gridView2.RefreshData();
            }//this.gridView
           
            //     (ListParamModule)gridView1.GetFocusedRow()
            ////gridView2.DeleteRow(gridView2.FocusedRowHandle);
            ////lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, intervalList.Count);
            //MessageBox.Show("test");
        }

        private void RepositoryItemImageComboBox2_Click(object sender, EventArgs e)
        {
            selectParamDataList.Remove((ParamDataSelectionData)gridView2.GetFocusedRow());
            this.gridControl2.DataSource = selectParamDataList;
            //gridView2.DeleteRow(gridView2.FocusedRowHandle);
            gridView2.RefreshData();
        }
        private void RepositoryItemImageComboBox3_Click(object sender, EventArgs e)
        {
            AddChartData("Shear RH Wing BL3405");
        }

        //RepositoryItemComboBox UserCombo = new RepositoryItemComboBox();
        //UserCombo.Items.Add("1");
        //    UserCombo.Items.Add("2");
            
        //    UserCombo.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
        //    gridView1.Columns["Id"].ColumnEdit = UserCombo;
        private void edtSearch_Click(object sender, ButtonPressedEventArgs e)
        {
            if (comboBoxEdit1.Text.Equals("") || edtSearch.Text.Equals(""))
            {
                MessageBox.Show("구분 값이나 검색어가 없습니다.");
                return;
            }
            if (e.Button.Kind.ToString() == "Search")
            {
                string sendData = string.Format(@"
                {{
                ""command"":""search"",
                ""sourceType"": ""{0}"",
                ""keyword"": ""{1}""
                }}" , comboBoxEdit1.Text.ToLower(), edtSearch.Text);
                string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
                if (responseData != null)
                {
                    SearchParamModuleResponse paramModuleResponse = JsonConvert.DeserializeObject<SearchParamModuleResponse>(responseData);
                    comboDic = new Dictionary<string, RepositoryItemComboBox>();
                    paramDataList = new List<ParamDataSelectionData>();
                    switch (comboBoxEdit1.Text)
                    {
                        case "PART": 
                            foreach(var list in paramModuleResponse.response)
                            {
                                List<GridParam> paramList = new List<GridParam>();
                                DateTime? startTime = null;
                                DateTime? endTime = null;
                                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
                                repositoryItemComboBox.SelectedIndexChanged += RepositoryItemComboBox_SelectedIndexChanged;
                                repositoryItemComboBox.ReadOnly = false;
                                repositoryItemComboBox.ReadOnly = false;
                                repositoryItemComboBox.AllowDropDownWhenReadOnly = DefaultBoolean.True;
                                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
                                foreach (var parameter in list.@params)
                                {
                                    GridParam param = new GridParam();
                                    param.seq = parameter.seq;
                                    param.paramKey = parameter.paramKey;
                                    paramList.Add(param);
                                    repositoryItemComboBox.Items.Add(param.paramKey);
                                }
                                gridPramDic.Add(list.seq, paramList);
                                gridControl1.RepositoryItems.Add(repositoryItemComboBox);
                                comboDic.Add(list.seq, repositoryItemComboBox);
                                if(list.julianStartAt != "")
                                {
                                    startTime = Utils.GetDateFromJulian(list.julianStartAt);
                                }
                                if(list.julianEndAt != "")
                                {
                                    endTime = Utils.GetDateFromJulian(list.julianEndAt);
                                }
                                //paramDataList.Add(new ParamDataSelectionData("part", Utils.base64StringDecoding(list.partName), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", startTime), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", endTime) , "",list.seq,1));
                                paramDataList.Add(new ParamDataSelectionData("part", Utils.base64StringDecoding(list.partName), list.julianStartAt, list.julianEndAt, "",list.seq,1));
                            }
                            break;
                        case "SHORTBLOCK":
                            foreach (var list in paramModuleResponse.response)
                            {
                                List<GridParam> paramList = new List<GridParam>();
                                DateTime? startTime = null;
                                DateTime? endTime = null;
                                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
                                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
                                foreach (var parameter in list.@params)
                                {
                                    GridParam param = new GridParam();
                                    param.seq = parameter.seq;
                                    param.paramKey = parameter.paramKey;
                                    paramList.Add(param);
                                    repositoryItemComboBox1.Items.Add(param.paramKey);
                                }
                                comboDic.Add(list.seq, repositoryItemComboBox);

                                if (list.julianStartAt != "")
                                {
                                    startTime = Utils.GetDateFromJulian(list.julianStartAt);
                                }
                                if (list.julianEndAt != "")
                                {
                                    endTime = Utils.GetDateFromJulian(list.julianEndAt);
                                }
                                //paramDataList.Add(new ParamDataSelectionData("shortblock", Utils.base64StringDecoding(list.blockName), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", startTime), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", endTime), "", list.seq,1));
                                paramDataList.Add(new ParamDataSelectionData("shortblock", Utils.base64StringDecoding(list.blockName), list.julianStartAt, list.julianEndAt, "", list.seq,1));
                            }
                            break;
                        case "DLL":
                            foreach (var list in paramModuleResponse.response)
                            {
                                paramDataList.Add(new ParamDataSelectionData("shortblock", Utils.base64StringDecoding(list.dataSetName),"","", "", list.seq,0));
                            }
                            break;
                        case "PARAMMODULE":
                            foreach (var list in paramModuleResponse.response)
                            {
                                paramDataList.Add(new ParamDataSelectionData("parammodule", Utils.base64StringDecoding(list.moduleName),"","", "", list.seq,0));
                            }
                            break;
                    }
                    this.gridControl1.DataSource = paramDataList;
                }
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            if (paramModuleSeq == null)
            {
                MessageBox.Show("파라미터모듈을 먼저 선택 후 추가해주세요.");
                return;
            }
            if (paramDataList != null)
            {
                foreach (var param in paramDataList)
                {
                    if (selectParamDataList.FindIndex(x => x.SourceSeq == param.SourceSeq) == -1)
                    {
                        selectParamDataList.Add(param);
                    }
                }
            }
            this.gridControl2.DataSource = selectParamDataList;
            gridView2.RefreshData();
        }

        private void btnListSave_Click(object sender, EventArgs e)
        {
            if (paramModuleSeq == null)
            {
                MessageBox.Show("파라미터모듈을 먼저 선택 후 추가해주세요.");
                return;
            }
            SaveParamModuleSelectDataRequest requestParam = new SaveParamModuleSelectDataRequest();
            requestParam.sources = new List<SaveParamModuleSelectDataSource>();
            requestParam.command = "save-source";
            requestParam.moduleSeq = paramModuleSeq;
            List<ParamDataSelectionData> selectDataList = (List<ParamDataSelectionData>)this.gridControl2.DataSource;
            foreach(var list in selectDataList)
            {   
                switch (list.DataType)
                {
                    case "parammodule":
                        requestParam.sources.Add(new SaveParamModuleSelectDataSource(list.DataType, list.SourceSeq));
                        break;
                    case "dll":
                        requestParam.sources.Add(new SaveParamModuleSelectDataSource(list.DataType, list.SourceSeq));
                        break;
                }
            }
            var json = JsonConvert.SerializeObject(requestParam);

            //string sendData = string.Format(@"
            //    {{
            //    ""command"":""save-source"",
            //    ""moduleSeq"": ""{0}"",
            //    ""keyword"": ""{1}""
            //    }}", comboBoxEdit1.Text.ToLower(), edtSearch.Text);

            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], json);
            if (responseData != null)
            {;
                MessageBox.Show(responseData);
            }

        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            AddChartData("Shear RH Wing BL3405");
        }

        private void RepositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ParamDataSelectionData temp = (ParamDataSelectionData)gridView2.GetFocusedRow();
            //comboDic[temp.SourceSeq] = comboDic[temp.SourceSeq].
            //gridView1.Columns["Parameter"].ColumnEdit = "tesst";
            //Columns.ColumnByFieldName(sColumnNameReplace).ColumnEdit = Temp
            var combo = sender as ComboBoxEdit;
            if (combo.SelectedIndex != -1)
            {
                string paramKey = combo.SelectedItem as string;
                var test = comboDic;
                //gridView1.Columns.
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DataCnt", paramKey);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ParamsCombo", paramKey);
                //gridView1.RefreshData();
            }
            //gridView1.Columns["Parameter"].ColumnEdit = combo;
        }

        public void SetSelectDataSource(string paramModuleSeq)
        {
            this.paramModuleSeq = paramModuleSeq;
            GetSelectDataList(paramModuleSeq);
        }

        private void GetSelectDataList(string paramModuleSeq)
        {
            string sendData = string.Format(@"
                {{
                ""command"":""source-list"",
                ""moduleSeq"": ""{0}""
                }}", paramModuleSeq);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                selectParamDataList = new List<ParamDataSelectionData>();
                SaveParamModuleSelectDataResponse paramModuleResponse = JsonConvert.DeserializeObject<SaveParamModuleSelectDataResponse>(responseData); 
                if(paramModuleResponse.response != null && paramModuleResponse.response.Count() != 0)
                {
                    foreach (var list in paramModuleResponse.response)
                    {
                        selectParamDataList.Add(new ParamDataSelectionData(list.sourceType, list.sourceType, list.julianStartAt, list.julianEndAt, "", list.sourceSeq, 1));
                    }
                }
                this.gridControl2.DataSource = selectParamDataList;

                //MessageBox.Show(responseData);
            }
        }
        
        private ResponsePartInfo GetPartInfo(string partSeq)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlPart"];

                string sendData = string.Format(@"
            {{
            ""command"":""row-data"",
            ""partSeq"":""{0}"",
            ""julianRange"":["""", """"]
            }}"
                , partSeq);

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                PartInfoResponse result = JsonConvert.DeserializeObject<PartInfoResponse>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        return null;
                    }
                    else
                    {
                        return result.response;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }
            return null;

        }


        private void AddChartData(string strKey)
        {
            MainForm mainForm = this.ParentForm as MainForm;

            if (chartControl != null)
            {
                chartControl.Dispose();
                chartControl = null;
            }

            chartControl = new ChartControl();

            chartControl.Series.Clear();

            Series series = new Series("Series1", ViewType.Line);
            chartControl.Series.Add(series);

            dt = GetChartValues(strKey, GetPartInfo("9c98259720fbac084d0e4ceb28fc0ec822138353257f3a986f77961aedf67ded"));
            series.DataSource = dt;

            series.ArgumentScaleType = ScaleType.DateTime;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            //((XYDiagram)chartControl.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chartControl.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            XYDiagram diagram = (XYDiagram)chartControl.Diagram;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.AutoGrid = false;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffffff}";
            //diag.AxisX.Label.TextPattern = "{A:MMM-dd HH}";

            //this.rangeControl1.Client = chartControl;
            //rangeControl1.RangeChanged += RangeControl1_RangeChanged;
            //rangeControl1.ShowLabels = true;
            diagram.RangeControlDateTimeGridOptions.GridMode = ChartRangeControlClientGridMode.Manual;
            diagram.RangeControlDateTimeGridOptions.GridOffset = 1;
            diagram.RangeControlDateTimeGridOptions.GridSpacing = 60;
            diagram.RangeControlDateTimeGridOptions.LabelFormat = "HH:mm:ss.fff";
            diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Millisecond;

            //rangeControl1.SelectedRange = new RangeControlRange(minValue, maxValue);

            if (panelChart == null)
            {
                panelChart = new DockPanel();
                panelChart = mainForm.DockManager1.AddPanel(DockingStyle.Float);
                panelChart.FloatLocation = new Point(500, 100);
                panelChart.FloatSize = new Size(1058, 528);
                panelChart.Name = strKey;
                panelChart.Text = strKey;
                chartControl.Dock = DockStyle.Fill;
                panelChart.Controls.Add(chartControl);
                //panelChart.ClosedPanel += PanelChart_ClosedPanel;
            }
            else
            {
                panelChart.Name = strKey;
                panelChart.Text = strKey;
                //panelChart.Controls.Clear();
                chartControl.Dock = DockStyle.Fill;
                panelChart.Controls.Add(chartControl);
                panelChart.Show();
                panelChart.Focus();
            }
        }

        private DataTable GetChartValues(string strParam, ResponsePartInfo partInfo)
        {
            // Create an empty table.
            DataTable table = new DataTable("Table1");

            // Add two columns to the table.
            //table.Columns.Add("Argument", typeof(Int32));
            table.Columns.Add("Argument", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));

            DataRow row = null;
            int i = 0;
            chartData.Clear();

            for (i = 0; i < partInfo.paramSet.Count; i++)
            {
                if (partInfo.paramSet[i].paramKey.Equals(strParam))
                {
                    int j = 0;
                    foreach (List<double> dataArr in partInfo.data)
                    {
                        row = table.NewRow();
                        string day = partInfo.julianSet[0][j];
                        DateTime dt = Utils.GetDateFromJulian(day);

                        if (j == 0)
                        {
                            this.startTime = dt;
                        }
                        this.endTime = dt;

                        double data = dataArr[i];
                        chartData.Add(data);
                        row["Argument"] = dt;
                        //row["Argument"] = i;
                        row["Value"] = data;
                        table.Rows.Add(row);

                        j++;
                    }
                    break;
                }
            }
            Console.WriteLine(string.Format("StartTime : {0}, EndTime : {1}", string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", startTime), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", endTime)));

            return table;
        }
    }
}
