using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DynaRAP.Common;
using DynaRAP.Data;
using DynaRAP.EventData;
using DynaRAP.UTIL;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class ImportModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        string selectedFuselage = string.Empty;
        Dictionary<string, List<string>> dicData = new Dictionary<string, List<string>>();
        List<ImportParamControl> paramControlList = new List<ImportParamControl>();
        List<ResponseParam> paramList = new List<ResponseParam>();
        //List<ImportIntervalControl> splitList = new List<ImportIntervalControl>();
        List<ResponsePreset> presetList = null;
        List<PresetData> pComboList = null;
        List<ImportIntervalData> intervalList = null;
        string csvFilePath = string.Empty;
        object minValue = null;
        object maxValue = null;
        string headerRow = string.Empty;

        ImportType importType = ImportType.FLYING;

        public ImportModuleControl()
        {
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        public ImportModuleControl(ImportType importType) : this()
        {
            this.importType = importType;
        }

        private void ImportModuleControl_Load(object sender, EventArgs e)
        {
            cboImportType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            if (importType == ImportType.FLYING)
            {
                cboImportType.Properties.Items.Add("GRT");
                cboImportType.Properties.Items.Add("FLTP");
                cboImportType.Properties.Items.Add("FLTS");
            }
            else
            {
                cboImportType.Properties.Items.Add("ADAMS");
                cboImportType.Properties.Items.Add("ZAERO");
            }

            //InitializeSplittedRegionList();

            luePresetList.Properties.DisplayMember = "PresetName";
            luePresetList.Properties.ValueMember = "PresetPack";
            luePresetList.Properties.NullText = "";

            InitializePresetList();

            DateTime dtNow = DateTime.Now;
            string strNow = string.Format("{0:yyyy-MM-dd}", dtNow);
            //dateScenario.Text = strNow;

            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.VerticalScroll.Visible = true;

            btnViewData.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddSplittedInterval.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnSaveSplittedInterval.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            btnViewData.Properties.AllowFocused = false;
            btnAddParameter.Properties.AllowFocused = false;
            btnAddSplittedInterval.Properties.AllowFocused = false;
            btnSaveSplittedInterval.Properties.AllowFocused = false;

            btnAddParameter.Enabled = false;
            
            if(intervalList == null)
            {
                intervalList = new List<ImportIntervalData>();
            }
            lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, intervalList.Count);

            InitializeGridControl1();
            InitializeGridControl2();

            edtLPFn.Text = "10";
            edtLPFcutoff.Text = "0.4";
            cboLPFbtype.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboLPFbtype.Properties.Items.Add("");
            cboLPFbtype.Properties.Items.Add("low");
            cboLPFbtype.Properties.Items.Add("high");
            cboLPFbtype.SelectedIndex = 0;

            edtHPFn.Text = "10";
            edtHPFcutoff.Text = "0.02";
            cboHPFbtype.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboHPFbtype.Properties.Items.Add("");
            cboHPFbtype.Properties.Items.Add("low");
            cboHPFbtype.Properties.Items.Add("high");
            cboHPFbtype.SelectedIndex = 2;
        }

        private void InitializeGridControl1()
        {

            //gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            repositoryItemComboBox1.TextEditStyle = TextEditStyles.DisableTextEditor;
            repositoryItemComboBox1.SelectedIndexChanged += RepositoryItemComboBox1_SelectedIndexChanged;
            repositoryItemComboBox1.BeforePopup += RepositoryItemComboBox1_BeforePopup;
            repositoryItemComboBox1.PopupFormMinSize = new System.Drawing.Size(0, 500);

            paramList = GetParamList();

            repositoryItemComboBox1.Items.Clear();
            repositoryItemComboBox1.Items.Add("skip");
            foreach (ResponseParam param in paramList)
            {
                repositoryItemComboBox1.Items.Add(param.paramKey);
            }

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

            GridColumn colName = gridView1.Columns["UnmappedParamName"];
            //colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            //colName.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colName.OptionsColumn.FixedWidth = true;
            colName.Width = 240;
            colName.Caption = "UnmappedParamName";
            colName.OptionsColumn.ReadOnly = true;
        }

        private List<ResponseParam> GetParamList()
        {
            ListParamJsonData result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlParam"];
                //string sendData = @"
                //{
                //""command"":""list"",
                //""pageNo"":1,
                //""pageSize"":3000,
                //""resultDataType"": ""map""
                //}";
                string sendData = @"
                {
                ""command"":""list"",
                ""pageNo"":1,
                ""pageSize"":3000
                }";

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
                result = JsonConvert.DeserializeObject<ListParamJsonData>(responseText);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return result.response;

        }

        private void InitializeGridControl2()
        {

            //gridView2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            repositoryItemComboBox2.TextEditStyle = TextEditStyles.DisableTextEditor;
            repositoryItemComboBox2.SelectedIndexChanged += RepositoryItemComboBox2_SelectedIndexChanged;
            repositoryItemComboBox2.BeforePopup += RepositoryItemComboBox2_BeforePopup;

            string importType = ConfigurationManager.AppSettings["ImportType"];

            string[] types = importType.Split(',');

            repositoryItemComboBox2.Items.Clear();
            foreach (string type in types)
            {
                repositoryItemComboBox2.Items.Add(type);
            }

            gridView2.OptionsView.ShowColumnHeaders = true;
            gridView2.OptionsView.ShowGroupPanel = false;
            gridView2.OptionsView.ShowIndicator = false;
            gridView2.IndicatorWidth = 40;
            gridView2.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView2.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView2.OptionsView.ColumnAutoWidth = true;

            gridView2.OptionsBehavior.ReadOnly = false;
            //gridView2.OptionsBehavior.Editable = false;

            gridView2.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;

            gridView2.CustomDrawRowIndicator += GridView2_CustomDrawRowIndicator;

            GridColumn colType = gridView2.Columns["ImportType"];
            colType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colType.OptionsColumn.FixedWidth = true;
            colType.Width = 240;
            colType.Caption = "기동 이름";

            GridColumn colDel = gridView2.Columns["Del"];
            colDel.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.OptionsColumn.FixedWidth = true;
            colDel.Width = 40;
            colDel.Caption = "삭제";
            colDel.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;
        }

        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            gridView2.DeleteRow(gridView2.FocusedRowHandle);
            lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, intervalList.Count);
        }

        private void RepositoryItemComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxEdit;
            if (combo.SelectedIndex != -1)
            {
                string paramKey = combo.SelectedItem as string;
                if (string.IsNullOrEmpty(paramKey) == false)
                {
                    bool bFind = false;

                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        string paramKey2 = gridView1.GetRowCellValue(i, "ParamKey") == null ? "" : gridView1.GetRowCellValue(i, "ParamKey").ToString();

                        if (i == gridView1.FocusedRowHandle || paramKey.Equals("skip"))
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(paramKey2) == false && paramKey2.Equals("skip") == false && paramKey2.Equals(paramKey))
                        {
                            bFind = true;
                            break;
                        }
                    }

                    if (bFind)
                    {
                        MessageBox.Show("항목의 중복이 허용되지 않습니다.");
                        combo.SelectedIndex = prevSelected;
                    }

                }
            }
        }

        int prevSelected = -1;
        private void RepositoryItemComboBox1_BeforePopup(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxEdit;
            prevSelected = combo.SelectedIndex;
        }

        private void RepositoryItemComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void RepositoryItemComboBox2_BeforePopup(object sender, EventArgs e)
        {
        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private void GridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private void InitializePresetList()
        {
            try
            {
                luePresetList.Properties.DataSource = null;

                presetList = GetPresetList();

                if (presetList != null)
                {
                    pComboList = new List<PresetData>();

                    foreach (ResponsePreset list in presetList)
                    {
                        //Decoding
                        byte[] byte64 = Convert.FromBase64String(list.presetName);
                        string decName = Encoding.UTF8.GetString(byte64);

                        pComboList.Add(new PresetData(decName, list.presetPack));
                    }
                    luePresetList.Properties.DataSource = pComboList;
#if !DEBUG
            luePresetList.Properties.PopulateColumns();
            luePresetList.Properties.ShowHeader = false;
            luePresetList.Properties.Columns["PresetPack"].Visible = false;
            luePresetList.Properties.ShowFooter = false;
#else
                    luePresetList.Properties.PopulateColumns();
                    luePresetList.Properties.Columns["PresetName"].Width = 800;
#endif

                    //luePresetList.EditValue = edtParamName.Text;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private List<ResponsePreset> GetPresetList()
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlPreset"];
                string sendData = @"
                {
                ""command"":""list"",
                ""pageNo"":1,
                ""pageSize"":3000
                }";

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
                ListPresetJsonData result = JsonConvert.DeserializeObject<ListPresetJsonData>(responseText);

                return result.response;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        private void btnViewData_ButtonClick(object sender, EventArgs e)
        {
            if (File.Exists(csvFilePath) == false)
            {
                MessageBox.Show(Properties.Resources.FileNotExist);
                return;
            }

            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.PanelImportViewCsv.Show();
            mainForm.CsvTableControl.CsvFilePath = this.csvFilePath;
            mainForm.CsvTableControl.FillGrid();
        }


        private void btnAddParameter_ButtonClick(object sender, EventArgs e)
        {
            AddParameter();
        }

        const int START_PARAM_INDEX = 0;
        const int PARAM_HEIGHT = 140;
        const int MAX_CHART_CNT = 3;
        int paramIndex = START_PARAM_INDEX;

        private void AddParameter()
        {
            ImportParamControl ctrl = new ImportParamControl();
            ctrl.Title = "Parameter " + paramIndex.ToString();
            ctrl.DeleteBtnClicked += new EventHandler(ImportParamControl_DeleteBtnClicked);
            ctrl.DicData = dicData;
            ctrl.OnSelectedRange += ChartControl_OnSelectedRange;
            //ctrl.Dock = DockStyle.Fill;
            flowLayoutPanel3.Controls.Add(ctrl);
            flowLayoutPanel3.Controls.SetChildIndex(ctrl, paramIndex);
            paramControlList.Add(ctrl);

            paramIndex++;
            if (paramIndex <= MAX_CHART_CNT)
            {
                flowLayoutPanel3.Height += PARAM_HEIGHT;
            }
        }

        void ImportParamControl_DeleteBtnClicked(object sender, EventArgs e)
        {
            ImportParamControl ctrl = sender as ImportParamControl;
            flowLayoutPanel3.Controls.Remove(ctrl);
            paramControlList.Remove(ctrl);
            ctrl.Dispose();

            paramIndex--;
            if (paramIndex < MAX_CHART_CNT)
            {
                flowLayoutPanel3.Height -= PARAM_HEIGHT;
            }
        }

        private void ChartControl_OnSelectedRange(object sender, SelectedRangeEventArgs e)
        {
            ImportParamControl me = sender as ImportParamControl;
            minValue = e.MinValue;
            maxValue = e.MaxValue;

            Debug.Print(string.Format("-----> MinValue : {0}, MaxValue : {1}", minValue, maxValue));

            foreach(ImportParamControl ctrl in paramControlList)
            {
                if (ctrl == me)
                    continue;

                ctrl.SelectRegion(minValue, maxValue);
            }
        }

        private void btnAddSplittedInterval_ButtonClick(object sender, EventArgs e)
        {
            AddSplittedInterval();

            lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, intervalList.Count);
        }

        const int START_SPLIT_INDEX = 0;
        const int SPLIT_HEIGHT = 24;
        const int MAX_SPLIT_CNT = 10;
        int intervalIndex = START_SPLIT_INDEX;

        private void AddSplittedInterval()
        {
            if(minValue == null || maxValue == null)
            {
                if(paramControlList.Count == 0)
                {
                    MessageBox.Show(Properties.Resources.StringAddParameter);
                    return;
                }
                else
                {
                    ImportParamControl paramCtrl =  paramControlList[0] as ImportParamControl;
                    paramCtrl.Sync();
                }
            }

            if (minValue == null || maxValue == null)
            {
                MessageBox.Show(Properties.Resources.StringNoSelectedRegion);
                return;
            }

            DataTable dt = null;
            int recordCnt = 0;

            if(paramControlList != null && paramControlList.Count > 0)
            {
                //DateTime sTime = Convert.ToDateTime(minValue.ToString());
                //DateTime eTime = Convert.ToDateTime(maxValue.ToString());

                DateTime sTime = (DateTime)minValue;
                DateTime eTime = (DateTime)maxValue;

                dt = GetIntervalData(paramControlList[0].Dt, sTime, eTime);
                recordCnt = dt.Rows.Count;
            }

            /*ImportIntervalControl ctrl = new ImportIntervalControl(minValue, maxValue, recordCnt);
            //ctrl.Title = "flight#" + (paramIndex + intervalIndex).ToString();
            ctrl.DeleteBtnClicked += new EventHandler(Interval_DeleteBtnClicked);
            flowLayoutPanel4.Controls.Add(ctrl);
            flowLayoutPanel4.Controls.SetChildIndex(ctrl, intervalIndex);
            splitList.Add(ctrl);

            intervalIndex++;
            if (intervalIndex <= MAX_SPLIT_CNT)
            {
                flowLayoutPanel4.Height += SPLIT_HEIGHT;
            }*/
            if(intervalList == null)
            {
                intervalList = new List<ImportIntervalData>();
            }
            DateTime min = (DateTime)minValue;
            DateTime max = (DateTime)maxValue;

            intervalList.Add(new ImportIntervalData("", "", min.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), max.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), recordCnt.ToString(), 1));
            this.gridControl2.DataSource = intervalList;
            gridView2.RefreshData();

        }

        private DataTable GetIntervalData(DataTable curDataTable, DateTime sTime, DateTime eTime)
        {
            DataRow[] result = curDataTable.Select(String.Format("Argument >= #{0}# AND Argument <= #{1}#", sTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), eTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff")));

            DataTable table = new DataTable("Table1");
            table.Columns.Add("Argument", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));

            foreach (DataRow row in result)
            {
                table.ImportRow(row);
            }
            return table;
        }

        void Interval_DeleteBtnClicked(object sender, EventArgs e)
        {
            /*ImportIntervalControl ctrl = sender as ImportIntervalControl;
            flowLayoutPanel1.Controls.Remove(ctrl);
            splitList.Remove(ctrl);
            ctrl.Dispose();

            intervalIndex--;
            if (intervalIndex <= MAX_SPLIT_CNT)
            {
                flowLayoutPanel4.Height -= SPLIT_HEIGHT;
            }

            lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, splitList.Count);*/
        }

        private void btnSaveSplittedInterval_ButtonClick(object sender, EventArgs e)
        {
            if(cboImportType.SelectedIndex < 0)
            {
                MessageBox.Show("입력타입을 선택하세요.");
                return;
            }

            if (luePresetList.GetColumnValue("PresetPack") == null)
            {
                MessageBox.Show("매칭테이블을 선택하세요.");
                return;
            }

            if (gridView2.RowCount <= 0)
            {
                MessageBox.Show("분할 구간이 없습니다.");
                return;
            }

            bool bResult = Import();

            if (bResult)
            {
                //MessageBox.Show(Properties.Resources.StringSuccessImport, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool Import()
        {
            try
            {
                ImportRequest import = new ImportRequest();
                import.command = "upload";
                import.sourcePath = csvFilePath;
                import.flightAt = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                import.dataType = cboImportType.Text.ToLower();
                import.forcedImport = chkForcedImport.Checked;
                import.lpfOption = new LpfOption();
                import.hpfOption = new HpfOption();
                import.tempMappingParams = new Dictionary<string, string>();
                import.parts = new List<Part>();

                string presetPack = String.Empty;
                if (luePresetList.GetColumnValue("PresetPack") != null)
                    presetPack = luePresetList.GetColumnValue("PresetPack").ToString();

                import.presetPack = presetPack;

                import.lpfOption.n = edtLPFn.Text;
                import.lpfOption.cutoff = edtLPFcutoff.Text;
                import.lpfOption.btype = cboLPFbtype.Text;

                import.hpfOption.n = edtHPFn.Text;
                import.hpfOption.cutoff = edtHPFcutoff.Text;
                import.hpfOption.btype = cboHPFbtype.Text;

                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    string paramName = gridView1.GetRowCellValue(i, "UnmappedParamName") == null ? "" : gridView1.GetRowCellValue(i, "UnmappedParamName").ToString();
                    string paramKey = gridView1.GetRowCellValue(i, "ParamKey") == null ? "skip" : gridView1.GetRowCellValue(i, "ParamKey").ToString();
                    if (string.IsNullOrEmpty(paramKey))
                    {
                        paramKey = "skip";
                    }
                    import.tempMappingParams.Add(paramName, paramKey);
                }

                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    string splitName = gridView2.GetRowCellValue(i, "SplitName") == null ? "" : gridView2.GetRowCellValue(i, "SplitName").ToString();
                    string startTime = gridView2.GetRowCellValue(i, "StartTime") == null ? "" : gridView2.GetRowCellValue(i, "StartTime").ToString();
                    string endTime = gridView2.GetRowCellValue(i, "EndTime") == null ? "" : gridView2.GetRowCellValue(i, "EndTime").ToString();
                    //Encoding
                    byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(splitName);
                    string partName = Convert.ToBase64String(basebyte);

                    string t1 = string.Empty;
                    string t2 = string.Empty;
                    string t3 = string.Empty;
                    string t4 = string.Empty;
                    if (importType == ImportType.FLYING)
                    {
                        t1 = Utils.GetJulianFromDate(startTime);
                        t2 = Utils.GetJulianFromDate(endTime);
                    }
                    else
                    {
                        t3 = Utils.GetJulianFromDate(startTime);
                        t4 = Utils.GetJulianFromDate(endTime);

                        t3 = t3.Substring(t3.Length - 9, 9);
                        t4 = t4.Substring(t4.Length - 9, 9);
                    }

                    import.parts.Add(new Part(partName, t1, t2, t3, t4));
                }

                string url = ConfigurationManager.AppSettings["UrlImport"];

                var json = JsonConvert.SerializeObject(import);

                //Console.WriteLine(json);
                log.Info("url : " + url);
                log.Info(json);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(json);
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
                ImportResponse result = JsonConvert.DeserializeObject<ImportResponse>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        // progress 확인
                        //ImportProgressForm form = new ImportProgressForm("116a1460354a7065cb1393aa94a529e14221be82a5bae3bbccc8b1a5b6b59680"); // test
                        ImportProgressForm form = new ImportProgressForm(result.response.seq);
                        if (form.ShowDialog() == DialogResult.Cancel)
                        {
                            List<UnmappedParamData> unmappedList = new List<UnmappedParamData>();
                            foreach (string type in form.NotMappedParams)
                            {
                                unmappedList.Add(new UnmappedParamData(type, "skip"));
                            }
                            this.gridControl1.DataSource = unmappedList;
                            gridView1.RefreshData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void lblFlyingData_Click(object sender, EventArgs e)
        {
            btnAddParameter.Enabled = false;
            headerRow = string.Empty;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "C:\\";
            dlg.Filter = "Excel files (*.xls, *.xlsx)|*.xls; *.xlsx|Comma Separated Value files (CSV)|*.csv|모든 파일 (*.*)|*.*";
            //dlg.Filter = "Comma Separated Value files (CSV)|*.csv";

#if !DEBUG
            if (dlg.ShowDialog() == DialogResult.OK)
#endif
            {
#if DEBUG
                // screen init
                luePresetList.EditValue = "";
                
                gridControl1.DataSource = null;
                gridView1.RefreshData();

                gridControl2.DataSource = null;
                gridView2.RefreshData();
                intervalList.Clear();
                lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, intervalList.Count);

                paramIndex = START_PARAM_INDEX;
                flowLayoutPanel3.Height -= PARAM_HEIGHT * flowLayoutPanel3.Controls.Count;
                flowLayoutPanel3.Controls.Clear();

                edtTag.Text = "";
                panelTag.Controls.Clear();

                // screen init

                if (importType == ImportType.FLYING)
                {
                    csvFilePath = @"C:\temp\TEST_GRT_SL30_03_1st_FT_Load_1_edit220617.csv";
                    lblFlyingData.Text = csvFilePath;
                }
                else
                {
                    csvFilePath = @"C:\temp\NL01-BFP23-06025-P002_GRIDG_SOF.dat";
                    lblFlyingData.Text = csvFilePath;
                }
                StreamReader sr = new StreamReader(csvFilePath);
#else
                csvFilePath = dlg.FileName;
                lblFlyingData.Text = csvFilePath;
                StreamReader sr = new StreamReader(dlg.FileName);
#endif

                if (importType == ImportType.FLYING) // 비행데이터 import
                {
                    int idx = 0;

                    // 스트림의 끝까지 읽기
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] data = line.Split(',');

                        if (string.IsNullOrEmpty(data[0]))
                            continue;

                        int i = 0;
                        if (idx == 0)
                        {
                            this.headerRow = line;
                            headerRow = headerRow.Substring(0, headerRow.LastIndexOf(','));
                            dicData.Clear();
                            for (i = 0; i < data.Length; i++)
                            {
                                if (dicData.ContainsKey(data[i]) == false)
                                {
                                    if (string.IsNullOrEmpty(data[i]) == false)
                                        dicData.Add(data[i], new List<string>());
                                }
                            }
                            idx++;
                            continue;
                        }

                        i = 0;
                        foreach (string key in dicData.Keys)
                        {
                            if (dicData.ContainsKey(key))
                            {
                                if (string.IsNullOrEmpty(data[i]) == false)
                                    dicData[key].Add(data[i++]);
                            }
                        }
                    }
                }
                else // 해석데이터 import
                {
                    dicData.Clear();

                    Dictionary<string, List<string>> tempData = new Dictionary<string, List<string>>();

                    // 스트림의 끝까지 읽기
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        line = line.Trim();
                        string[] data = line.Split(' ');

                        if (string.IsNullOrEmpty(data[0]))
                            continue;

                        double dVal;
                        bool isNumber = double.TryParse(data[0], out dVal);
                        int i = 0;

                        if (isNumber == false)
                        {
                            foreach (string key in tempData.Keys)
                            {
                                if (dicData.ContainsKey(key) == false)
                                {
                                    dicData.Add(key, tempData[key]);
                                }
                            }

                            if (data[0].Equals("UNITS"))
                            {
                                tempData.Clear();

                                if (tempData.ContainsKey("DATE") == false)
                                {
                                    tempData.Add("DATE", new List<string>());
                                }
                                for (i = 1; i < data.Length; i++)
                                {
                                    if (tempData.ContainsKey(data[i]) == false)
                                    {
                                        if (string.IsNullOrEmpty(data[i]) == false)
                                        {
                                            tempData.Add(data[i], new List<string>());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            data = data.Where((source, index) => string.IsNullOrEmpty(source) == false).ToArray();

                            if(data[0].StartsWith("-"))
                            {
                                continue;
                            }

                            i = 0;
                            foreach (string key in tempData.Keys)
                            {
                                if (tempData.ContainsKey(key))
                                {
                                    if (string.IsNullOrEmpty(data[i]) == false)
                                        tempData[key].Add(data[i++]);
                                }
                            }
                        }
                    }
                }

                //foreach (KeyValuePair<string, List<string>> kv in dicData)
                //{
                //    Console.Write("{0} : ", kv.Key);
                //    foreach (string val in kv.Value)
                //    {
                //        Console.Write("{0} ", val);
                //    }
                //    Console.WriteLine();
                //}

                btnAddParameter.Enabled = true;

                CheckParam();
            }
        }

        private bool CheckParam()
        {
            try
            {
                string presetPack = String.Empty;
                string dataType = cboImportType.Text.ToLower();

                if (luePresetList.GetColumnValue("PresetPack") != null)
                    presetPack = luePresetList.GetColumnValue("PresetPack").ToString();

                if (string.IsNullOrEmpty(presetPack)
                    || string.IsNullOrEmpty(dataType)
                    || (importType == ImportType.FLYING && string.IsNullOrEmpty(this.headerRow))
                    )
                {
                    return false;
                }

                string url = ConfigurationManager.AppSettings["UrlImport"];
                string sendData = String.Empty;
                csvFilePath = csvFilePath.Replace("\\", "\\\\");
                if (importType == ImportType.FLYING) // 비행데이터 import
                {
                    sendData = string.Format(@"
                {{
                ""command"":""check-param"",
                ""presetPack"":""{0}"",
                ""presetSeq"":null,
                ""dataType"":""{1}"",
                ""headerRow"":""{2}""
                }}"
                    , presetPack, dataType, this.headerRow);
                }
                else // 해석데이터 import
                {
                    sendData = string.Format(@"
                {{
                ""command"":""check-param"",
                ""presetPack"":""{0}"",
                ""presetSeq"":null,
                ""dataType"":""{1}"",
                ""importFilePath"":""{2}""
                }}"
                    , presetPack, dataType, this.csvFilePath);
                }

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
                ImportResponse result = JsonConvert.DeserializeObject<ImportResponse>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        List<UnmappedParamData> unmappedList = new List<UnmappedParamData>();
                        foreach (string type in result.response.notMappedParams)
                        {
                            unmappedList.Add(new UnmappedParamData(type, "skip"));
                        }
                        this.gridControl1.DataSource = unmappedList;
                        gridView1.RefreshData();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void luePresetList_EditValueChanged(object sender, EventArgs e)
        {
            CheckParam();
        }

        private void edtTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
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
            panelTag.Controls.Remove(btn);

        }

        private void cboImportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckParam();
        }
    }

    
}
