using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DynaRAP.UControl
{
    public partial class SBModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        string selectedFuselage = string.Empty;
        Series series1 = new Series();
        ChartArea myChartArea = new ChartArea("LineChartArea");
        List<SBIntervalControl> sbIntervalList = new List<SBIntervalControl>();

        Dictionary<string, List<string>> dicData = new Dictionary<string, List<string>>();
        string selKey = String.Empty;
        List<double> chartData = new List<double>();

        List<ResponsePreset> presetList = null;
        List<ResponseParam> presetParamList = null;
        List<PresetData> pComboList = null;

        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now;

        Dictionary<string, List<string>> uploadList = new Dictionary<string, List<string>>();

        public SBModuleControl()
        {
            InitializeComponent();
        }

        private void SBModuleControl_Load(object sender, EventArgs e)
        {
            luePresetList.Properties.DisplayMember = "PresetName";
            luePresetList.Properties.ValueMember = "PresetPack";
            luePresetList.Properties.NullText = "";

            GetUploadList();
            InitializePreviewChart();
            InitializeUploadTypeList();
            InitializePresetList();

            //DateTime dtNow = DateTime.Now;
            //string strNow = string.Format("{0:yyyy-MM-dd}", dtNow);
            //dateScenario.Text = strNow;

            panelData.AutoScroll = true;
            panelData.WrapContents = false;
            panelData.HorizontalScroll.Visible = false;
            panelData.VerticalScroll.Visible = true;

            edtSBLength.Properties.AutoHeight = false;
            edtOverlap.Properties.AutoHeight = false;
            edtSBLength.Dock = DockStyle.Fill;
            edtOverlap.Dock = DockStyle.Fill;

            edtSBLength.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            edtSBLength.Properties.Mask.EditMask = @"f2";
            //edtSBLength.Properties.Mask.PlaceHolder = '0';
            //edtSBLength.Properties.Mask.SaveLiteral = true;
            //edtSBLength.Properties.Mask.ShowPlaceHolders = true;
            edtSBLength.Properties.Mask.UseMaskAsDisplayFormat = true;

            edtOverlap.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            edtOverlap.Properties.Mask.EditMask = @"d2";
            edtOverlap.Properties.Mask.UseMaskAsDisplayFormat = true;

            edtSBLength.Text = "1";
            edtOverlap.Text = "10";

            btnAddParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnSaveSplittedParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            btnAddParameter.Properties.AllowFocused = false;
            btnSaveSplittedParameter.Properties.AllowFocused = false;

            lblValidSBCount.Text = string.Format(Properties.Resources.StringValidSBCount, sbIntervalList.Count);

        }

        private bool GetUploadList()
        {
            string url = ConfigurationManager.AppSettings["UrlImport"];
            string sendData = @"
            {
            ""command"":""upload-list""
            }";

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
            UploadListResponse result = JsonConvert.DeserializeObject<UploadListResponse>(responseText);
            uploadList.Clear();

            if (result != null)
            {
                if (result.code != 200)
                {
                    return false;
                }
                else
                {
                    foreach (ResponseImport res in result.response)
                    {
                        if (uploadList.ContainsKey(res.dataType) == false)
                        {
                            uploadList.Add(res.dataType, new List<string>());
                        }

                        //Decoding
                        byte[] byte64 = Convert.FromBase64String(res.uploadName);
                        string decName = Encoding.UTF8.GetString(byte64);
                        uploadList[res.dataType].Add(decName);
                    }
                }
            }
            return true;

        }

        private void InitializePreviewChart()
        {
            myChartArea.CursorX.IsUserEnabled = true;
            myChartArea.CursorX.IsUserSelectionEnabled = true;
            myChartArea.AxisX.ScaleView.Zoomable = false;
            myChartArea.BackColor = Color.FromArgb(37, 37, 38);
            myChartArea.AxisX.LabelStyle.ForeColor = Color.White;
            myChartArea.AxisY.LabelStyle.ForeColor = Color.White;
            myChartArea.AxisX.MajorGrid.Enabled = false;
            myChartArea.AxisX.MinorGrid.Enabled = false;
            myChartArea.AxisY.MajorGrid.Enabled = false;
            myChartArea.AxisY.MinorGrid.Enabled = false;
            ////

            chart1.ChartAreas.RemoveAt(0);
            chart1.ChartAreas.Add(myChartArea);

            /*
            //chartPreview.BackColor = Color.FromArgb(45, 45, 48);

            chartPreview.ChartAreas.RemoveAt(0);
            chartPreview.ChartAreas.Add(myChartArea);
            chartPreview.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartPreview.ChartAreas[0].AxisY.LabelStyle.Enabled = false;


            series1.ChartType = SeriesChartType.Line;
            series1.Name = "VAS";
            series1.XValueType = ChartValueType.DateTime;
            series1.IsValueShownAsLabel = false;
            //series1.IsVisibleInLegend = false;
            series1.LabelForeColor = Color.Red;
            series1.MarkerStyle = MarkerStyle.None;
            series1.MarkerSize = 3;
            series1.MarkerColor = Color.Red;

            SettingMyData();
            chartPreview.Series.Add(series1);
            */
        }

        private void InitializeUploadTypeList()
        {
            cboUploadType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboUploadType.SelectedIndexChanged += cboUploadType_SelectedIndexChanged;

            cboFlying.Properties.Items.Clear();
            foreach (string str in uploadList.Keys)
            {
                cboUploadType.Properties.Items.Add(str);
            }

            cboUploadType.SelectedIndex = 0;
        }

        private void cboUploadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                InitializeFlyingList(combo.Text);
            }
        }

        private void InitializeFlyingList(string uploadType)
        {
            cboFlying.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboFlying.SelectedIndexChanged += CboFlying_SelectedIndexChanged;

            cboFlying.Properties.Items.Clear();

            foreach (string str in uploadList[uploadType])
            {
                cboFlying.Properties.Items.Add(str);
            }

            cboFlying.SelectedIndex = 0;

        }

        private void CboFlying_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                ReadCsvFile();
                InitComboParamList();
            }
        }

        private void ReadCsvFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "C:\\";
            dlg.Filter = "Excel files (*.xls, *.xlsx)|*.xls; *.xlsx|Comma Separated Value files (CSV)|*.csv|모든 파일 (*.*)|*.*";
            //dlg.Filter = "Comma Separated Value files (CSV)|*.csv";

#if !DEBUG
            if (dlg.ShowDialog() == DialogResult.OK)
#endif
            {
#if DEBUG
                StreamReader sr = new StreamReader(@"C:\temp\a.xls");
#else
                StreamReader sr = new StreamReader(dlg.FileName);
#endif

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
        }

        private void InitComboParamList()
        {
            cboParameter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboParameter.Properties.DropDownRows = 15;
            cboParameter.SelectedIndexChanged += cboParameter_SelectedIndexChanged;

            List<string> paramList = dicData.Keys.ToList();
            //cboParameter.Properties.Items.Add("SIN");
            //cboParameter.Properties.Items.Add("COS");
            cboParameter.Properties.Items.AddRange(paramList);
            cboParameter.Properties.Items.Remove("DATE");

            cboParameter.SelectedIndex = -1;
        }

        private void cboParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboParameter.EditValue != null)
            {
                selKey = cboParameter.EditValue.ToString();
                AddChartData(selKey);
            }
        }

        private void AddChartData(string strKey)
        {
            chart1.Series.Clear();

            Series series1 = new Series("Series1");
            series1.ChartType = SeriesChartType.Line;
            series1.Name = strKey;
            series1.XValueType = ChartValueType.DateTime;
            //series1.IsValueShownAsLabel = true;
            series1.IsVisibleInLegend = false;
            series1.LabelForeColor = Color.Red;
            series1.MarkerStyle = MarkerStyle.Square;
            series1.MarkerSize = 3;
            series1.MarkerColor = Color.Black;

            series1.XValueMember = "Argument";
            series1.YValueMembers = "Value";

            chart1.Series.Add(series1);

            chart1.DataSource = GetChartValues(strKey);
            chart1.BackColor = Color.FromArgb(37, 37, 38);
            chart1.DataBind();

            AddIntervalList();
            AddStripLines();

        }


        private DataTable GetChartValues(string strKey)
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
            foreach (string value in dicData[selKey])
            {
                row = table.NewRow();
                string day = dicData["DATE"][i];
                DateTime dt = Utils.GetDateFromJulian(day);

                if (i == 0)
                {
                    this.startTime = dt;
                }
                this.endTime = dt;

                double data = double.Parse(value);
                chartData.Add(data);
                row["Argument"] = dt;
                //row["Argument"] = i;
                row["Value"] = data;
                table.Rows.Add(row);
                i++;
            }
            Console.WriteLine(string.Format("StartTime : {0}, EndTime : {1}", string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", startTime), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", endTime)));

            return table;
        }

        double sbLen = 1;
        double overlap = 10;

        private void AddIntervalList()
        {
            sbIntervalList.Clear();

            intervalIndex = startIntervalIndex;
            int reducedHeight = (paramHeight * flowLayoutPanel2.Controls.Count);
            flowLayoutPanel2.Height -= reducedHeight;
            flowLayoutPanel2.Controls.Clear();

            double.TryParse(edtSBLength.Text, out sbLen);
            double.TryParse(edtOverlap.Text, out overlap);

            overlap *= 0.01;

            //sbLen = 0.1;//test

            DateTime t1 = startTime;
            DateTime t2 = t1.AddSeconds(sbLen);
            int i = 0;
            while (t1 < endTime)
            {
                //Console.WriteLine(i + string.Format(" - StartTime : {0}, EndTime : {1}", string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", t1), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", t2)));
                SplittedSB sb = new SplittedSB(string.Format("ShortBlock#{0}", i), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", t1), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", t2), 0);
                i++;

                AddSplittedInterval(sb);

                t1 = t2.AddSeconds(-(sbLen * overlap));
                t2 = t1.AddSeconds(sbLen);
            }

            lblValidSBCount.Text = string.Format(Properties.Resources.StringValidSBCount, sbIntervalList.Count);

        }
        private void AddStripLines()
        {
            double.TryParse(edtSBLength.Text, out sbLen);
            double.TryParse(edtOverlap.Text, out overlap);

            //sbLen *= 0.00001;
            //sbLen *= 0.1;
            overlap *= 0.01;

            if (sbLen <= 0 || overlap <= 0)
                return;

            //Axis ax = chart1.ChartAreas[0].AxisX;
            Axis ax = myChartArea.AxisX;
            List<Color> colors = new List<Color>()  {   Color.FromArgb(75, 44, 44), Color.FromArgb(98, 41, 41)
                                                        , Color.FromArgb(64, Color.LightSeaGreen), Color.FromArgb(64, Color.LightGoldenrodYellow)};

            double hrange = ax.Maximum - ax.Minimum;

            if (double.IsNaN(hrange))
                return;

            TimeSpan spanStart = new TimeSpan(startTime.Day, startTime.Hour, startTime.Minute, startTime.Second, startTime.Millisecond);
            TimeSpan spanEnd = new TimeSpan(endTime.Day, endTime.Hour, endTime.Minute, endTime.Second, endTime.Millisecond);
            TimeSpan spanGap = spanEnd.Subtract(spanStart);

            sbLen = sbLen * hrange / spanGap.TotalSeconds;

            ax.StripLines.Clear();

            StripLine sl = new StripLine();
            sl.Interval = hrange;
            sl.StripWidth = hrange;            // width, 너비
            sl.IntervalOffset = 0;  // x-position, 시작점
            sl.BackColor = colors[0];
            ax.StripLines.Add(sl);

            double offset = sbLen * (1 - overlap);
            //double startTime = 0;
            while (offset < hrange)
            {
                StripLine sl2 = new StripLine();
                sl2.Interval = hrange;
                sl2.IntervalOffset = offset;    // 시작점
                sl2.StripWidth = sbLen * overlap;   // 너비
                sl2.BackColor = colors[1];
                ax.StripLines.Add(sl2);

                //AddSplittedInterval(new SplittedSB("", startTime, endTime, 0));
                //Console.WriteLine(string.Format("starttime : {0}, endtime : {1}", string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", startTime), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", endTime)));
                
                offset += sbLen;
            }
        }

        private void InitializePresetList()
        {
            luePresetList.Properties.DataSource = null;

            presetList = GetPresetList();
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

        private List<ResponsePreset> GetPresetList()
        {
            string url = ConfigurationManager.AppSettings["UrlPreset"];
            string sendData = @"
            {
            ""command"":""list"",
            ""pageNo"":1,
            ""pageSize"":3000
            }";

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

        private void CboSBParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnAddParameter_ButtonClick(object sender, EventArgs e)
        {
            AddParameter(null);
        }

        const int startParamIndex = 0;
        int paramIndex = startParamIndex;
        const int paramHeight = 22;

        private void AddParameter(ResponseParam param)
        {
            SBParamControl ctrl = new SBParamControl(param);
            ctrl.Title = "Parameter " + (paramIndex- startParamIndex).ToString();
            flowLayoutPanel1.Controls.Add(ctrl);
            flowLayoutPanel1.Controls.SetChildIndex(ctrl, paramIndex++);

            flowLayoutPanel1.Height += paramHeight;
        }

        const int startIntervalIndex = 0;
        int intervalIndex = startIntervalIndex;

        private void AddSplittedInterval(SplittedSB sb)
        {
            SBIntervalControl ctrl = new SBIntervalControl(sb);
            ctrl.DeleteBtnClicked += new EventHandler(InvalidSB_DeleteBtnClicked);
            flowLayoutPanel2.Controls.Add(ctrl);
            flowLayoutPanel2.Controls.SetChildIndex(ctrl, intervalIndex++);
            sbIntervalList.Add(ctrl);

            flowLayoutPanel2.Height += paramHeight;
        }

        void InvalidSB_DeleteBtnClicked(object sender, EventArgs e)
        {
            SBIntervalControl ctrl = sender as SBIntervalControl;
            flowLayoutPanel2.Controls.Remove(ctrl);
            sbIntervalList.Remove(ctrl);
            ctrl.Dispose();

            intervalIndex--;

            lblValidSBCount.Text = string.Format(Properties.Resources.StringValidSBCount, sbIntervalList.Count);
        }

        private void btnSaveSplittedParameter_ButtonClick(object sender, ButtonPressedEventArgs e)
        {

        }

        private void btnSaveSplittedParameter_ButtonClick(object sender, EventArgs e)
        {

        }

        private void edtSBLength_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            edtSBLength.Text = String.Empty;
        }

        private void edtOverlap_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            edtOverlap.Text = String.Empty;
        }

        private void edtSBLength_EditValueChanged(object sender, EventArgs e)
        {
            AddIntervalList();
            AddStripLines();
        }

        private void edtOverlap_EditValueChanged(object sender, EventArgs e)
        {
            AddIntervalList();
            AddStripLines();
        }

        private void luePresetList_EditValueChanged(object sender, EventArgs e)
        {
            paramIndex = startParamIndex;
            int reducedHeight = (paramHeight * flowLayoutPanel1.Controls.Count);
            flowLayoutPanel1.Height -= reducedHeight;
            flowLayoutPanel1.Controls.Clear();

            string presetPack = String.Empty;
            if (luePresetList.GetColumnValue("PresetPack") != null)
                presetPack = luePresetList.GetColumnValue("PresetPack").ToString();

            presetParamList = null;
            ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(presetPack));

            string presetName = String.Empty;

            if (preset != null)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(preset.presetName);
                string decName = Encoding.UTF8.GetString(byte64);

                presetName = decName;

                presetParamList = GetPresetParamList(preset.presetPack);
            }

            if (presetParamList != null)
            {
                foreach (ResponseParam param in presetParamList)
                {
                    AddParameter(param);
                }
            }
        }

        private List<ResponseParam> GetPresetParamList(string presetPack)
        {
            string url = ConfigurationManager.AppSettings["UrlPreset"];
            string sendData = string.Format(@"
            {{
            ""command"":""param-list"",
            ""presetPack"":""{0}"",
            ""presetSeq"":"""",
            ""paramPack"":"""",
            ""paramSeq"":"""",
            ""pageNo"":1,
            ""pageSize"":3000
            }}"
            , presetPack);

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
            ListParamJsonData result = JsonConvert.DeserializeObject<ListParamJsonData>(responseText);

            return result.response;

        }

    }

    
}
