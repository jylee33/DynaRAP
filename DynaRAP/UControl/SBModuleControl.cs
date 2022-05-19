using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DynaRAP.Data;
using DynaRAP.Forms;
using DynaRAP.TEST;
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
        ChartArea myChartArea = new ChartArea("LineChartArea");
        List<SBParamControl> sbParamList = new List<SBParamControl>();
        List<SBIntervalControl> sbIntervalList = new List<SBIntervalControl>();

        Dictionary<string, List<string>> dicData = new Dictionary<string, List<string>>();
        string selParam = String.Empty;
        List<double> chartData = new List<double>();

        List<ResponsePreset> presetList = null;
        List<ResponseParam> presetParamList = null;
        List<PresetData> pComboList = null;
        ResponsePartInfo partInfo = null;

        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now;

        //Dictionary<string, List<string>> uploadList = new Dictionary<string, List<string>>();
        List<ResponseImport> uploadList = new List<ResponseImport>();
        List<ResponsePart> partList = new List<ResponsePart>();

        double sbLen = 1;
        double overlap = 10;

        public SBModuleControl()
        {
            InitializeComponent();
        }

        private void SBModuleControl_Load(object sender, EventArgs e)
        {
            cboFlying.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboFlying.SelectedIndexChanged += CboFlying_SelectedIndexChanged;

            cboPart.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboPart.SelectedIndexChanged += CboPart_SelectedIndexChanged;

            cboParameter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboParameter.Properties.DropDownRows = 15;
            cboParameter.SelectedIndexChanged += cboParameter_SelectedIndexChanged;

            edtSBLength.Text = "1";
            edtOverlap.Text = "10";
  
            luePresetList.Properties.DisplayMember = "PresetName";
            luePresetList.Properties.ValueMember = "PresetPack";
            luePresetList.Properties.NullText = "";

            uploadList = GetUploadList();
            InitializePreviewChart();
            InitializeFlyingList();
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

            btnAddParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnSaveSplittedParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            btnAddParameter.Properties.AllowFocused = false;
            btnSaveSplittedParameter.Properties.AllowFocused = false;

            lblValidSBCount.Text = string.Format(Properties.Resources.StringValidSBCount, sbIntervalList.Count);

        }

        private List<ResponseImport> GetUploadList()
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
            return null;

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

            myChartArea.InnerPlotPosition.Auto = false;
            myChartArea.InnerPlotPosition.Width = 100;
            myChartArea.InnerPlotPosition.Height = 100;

            myChartArea.Position.X = 0;
            myChartArea.Position.Y = 0;
            myChartArea.Position.Width = 100;
            myChartArea.Position.Height = 100;

            chart1.ChartAreas.RemoveAt(0);
            chart1.ChartAreas.Add(myChartArea);

            /*
            chartPreview.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartPreview.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            */
        }

        private void InitializeFlyingList()
        {
            cboFlying.Properties.Items.Clear();

            foreach (ResponseImport list in uploadList)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(list.uploadName);
                string decName = Encoding.UTF8.GetString(byte64);
                cboFlying.Properties.Items.Add(decName);
            }

            cboFlying.SelectedIndex = 0;

        }

        private void CboFlying_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                InitializePartList(combo.Text);
            }
        }

        private void InitializePartList(string flyingName)
        {
            cboPart.Properties.Items.Clear();
            cboPart.Text = String.Empty;

            partList = null;
            partList = GetPartList(flyingName);

            foreach (ResponsePart part in partList)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(part.partName);
                string decName = Encoding.UTF8.GetString(byte64);

                cboPart.Properties.Items.Add(decName);
            }

            cboPart.SelectedIndex = 0;

        }

        private List<ResponsePart> GetPartList(string flyingName)
        {
            string url = ConfigurationManager.AppSettings["UrlPart"];

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(flyingName);
            string encName = Convert.ToBase64String(basebyte);

            string uploadSeq = "";
            ResponseImport import = uploadList.Find(x => x.uploadName.Equals(encName));
            if (import != null)
            {
                uploadSeq = import.seq;
            }

            string sendData = string.Format(@"
            {{
            ""command"":""list"",
            ""registerUid"":"""",
            ""uploadSeq"":""{0}"",
            ""pageNo"":1,
            ""pageSize"":3000
            }}"
            , uploadSeq);

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
            PartListResponse result = JsonConvert.DeserializeObject<PartListResponse>(responseText);

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
            return null;

        }

        private void CboPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                //ReadCsvFile();
                //InitComboParamList();

                InitializePartInfoList(combo.Text);
            }
        }

        private void InitializePartInfoList(string partName)
        {
            cboParameter.Properties.Items.Clear();
            cboParameter.Text = String.Empty;

            partInfo = null;
            partInfo = GetPartInfo(partName);

            if (partInfo != null)
            {
                foreach (ParamSet param in partInfo.paramSet)
                {
                    cboParameter.Properties.Items.Add(param.paramKey);
                }

                cboParameter.SelectedIndex = 0;
            }

        }

        private ResponsePartInfo GetPartInfo(string seq)
        {
            string url = ConfigurationManager.AppSettings["UrlPart"];

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(seq);
            string encName = Convert.ToBase64String(basebyte);

            string partSeq = "";
            ResponsePart part = partList.Find(x => x.partName.Equals(encName));
            if (part != null)
            {
                partSeq = part.seq;
            }

            string sendData = string.Format(@"
            {{
            ""command"":""row-data"",
            ""partSeq"":""{0}"",
            ""julianRange"":["""", """"]
            }}"
            , partSeq);

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
            return null;

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
                selParam = cboParameter.EditValue.ToString();
                AddChartData(selParam);
            }
        }

        private void AddChartData(string strParam)
        {
            chart1.Series.Clear();

            Series series1 = new Series("Series1");
            series1.ChartType = SeriesChartType.Line;
            series1.Name = strParam;
            series1.XValueType = ChartValueType.DateTime;
            series1.IsValueShownAsLabel = false;
            series1.IsVisibleInLegend = false;
            series1.LabelForeColor = Color.Red;
            //series1.MarkerStyle = MarkerStyle.Square;
            //series1.MarkerSize = 3;
            //series1.MarkerColor = Color.Black;

            series1.XValueMember = "Argument";
            series1.YValueMembers = "Value";

            chart1.Series.Add(series1);

            chart1.DataSource = GetChartValues(strParam);
            chart1.BackColor = Color.FromArgb(37, 37, 38);
            chart1.DataBind();

            chart1.Update();
            chart1.ChartAreas[0].RecalculateAxesScale();

            AddIntervalList();
            AddStripLines();

        }


        private DataTable GetChartValues(string strParam)
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

            for(i = 0; i < partInfo.paramSet.Count; i++)
            {
                if(partInfo.paramSet[i].paramKey.Equals(strParam))
                {
                    int j = 0;
                    foreach(List<double> dataArr in partInfo.data)
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
            ctrl.DeleteBtnClicked += new EventHandler(SBParam_DeleteBtnClicked);
            flowLayoutPanel1.Controls.Add(ctrl);
            flowLayoutPanel1.Controls.SetChildIndex(ctrl, paramIndex++);
            sbParamList.Add(ctrl);

            flowLayoutPanel1.Height += paramHeight;
        }

        private void SBParam_DeleteBtnClicked(object sender, EventArgs e)
        {
            SBParamControl ctrl = sender as SBParamControl;
            flowLayoutPanel1.Controls.Remove(ctrl);
            sbParamList.Remove(ctrl);
            ctrl.Dispose();

            flowLayoutPanel1.Height -= paramHeight;
            intervalIndex--;
        }

        const int startIntervalIndex = 0;
        int intervalIndex = startIntervalIndex;

        private void AddSplittedInterval(SplittedSB sb)
        {
            SBIntervalControl ctrl = new SBIntervalControl(sb);
            ctrl.ViewBtnClicked += new EventHandler(SB_ViewBtnClicked);
            flowLayoutPanel2.Controls.Add(ctrl);
            flowLayoutPanel2.Controls.SetChildIndex(ctrl, intervalIndex++);
            sbIntervalList.Add(ctrl);

            flowLayoutPanel2.Height += paramHeight;
        }

        void SB_ViewBtnClicked(object sender, EventArgs e)
        {
            SBIntervalControl ctrl = sender as SBIntervalControl;

            string strKey = selParam;
            DateTime sTime = DateTime.ParseExact(ctrl.Sb.StartTime, "yyyy-MM-dd HH:mm:ss.ffffff", null);
            DateTime eTime = DateTime.ParseExact(ctrl.Sb.EndTime, "yyyy-MM-dd HH:mm:ss.ffffff", null);

            DataTable dt = GetShortBlockData(ctrl.Sb.SbName, sTime, eTime);

            if (dt != null)
            {
                SBViewForm form = new SBViewForm(dt);
                form.Text = strKey;
                form.Show();
            }

            //TestChartForm2 form2 = new TestChartForm2(dt);
            //form2.Text = strKey;
            //form2.Show();

        }

        private DataTable GetShortBlockData(string strKey, DateTime sTime, DateTime eTime)
        {
            string t1 = Utils.GetJulianFromDate(sTime);
            string t2 = Utils.GetJulianFromDate(eTime);

            string url = ConfigurationManager.AppSettings["UrlPart"];

            string seq = cboPart.Text;
            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(seq);
            string encName = Convert.ToBase64String(basebyte);

            string partSeq = "";
            ResponsePart part = partList.Find(x => x.partName.Equals(encName));
            if (part != null)
            {
                partSeq = part.seq;
            }

            string sendData = string.Format(@"
            {{
            ""command"":""row-data"",
            ""partSeq"":""{0}"",
            ""julianRange"":[""{1}"", ""{2}""]
            }}"
            , partSeq, t1, t2);

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
                    DataTable table = new DataTable("Table1");
                    table.Columns.Add("Argument", typeof(DateTime));
                    table.Columns.Add("Value", typeof(double));

                    DataRow row = null;
                    int i = 0;

                    for (i = 0; i < partInfo.paramSet.Count; i++)
                    {
                        if (partInfo.paramSet[i].paramKey.Equals(cboParameter.Text))
                        {
                            int j = 0;
                            foreach (List<double> dataArr in partInfo.data)
                            {
                                row = table.NewRow();
                                string day = partInfo.julianSet[0][j];
                                DateTime dt = Utils.GetDateFromJulian(day);

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
                    return table;
                }
            }
            return null;

        }
        //private DataTable GetShortBlockData(string strKey, string sTime, string eTime)
        //{
        //    // Create an empty table.
        //    DataTable table = new DataTable("Table1");

        //    // Add two columns to the table.
        //    //table.Columns.Add("Argument", typeof(Int32));
        //    table.Columns.Add("Argument", typeof(DateTime));
        //    table.Columns.Add("Value", typeof(double));

        //    DataRow row = null;
        //    int i = 0;
        //    chartData.Clear();
        //    foreach (string value in dicData[strKey])
        //    {
        //        row = table.NewRow();
        //        string day = dicData["DATE"][i];
        //        DateTime dt = Utils.GetDateFromJulian(day);

        //        int result1 = DateTime.Compare(dt, sTime);
        //        int result2 = DateTime.Compare(dt, eTime);

        //        if(result1 < 0 || result2 > 0)
        //        {
        //            continue;
        //        }

        //        double data = double.Parse(value);
        //        chartData.Add(data);
        //        row["Argument"] = dt;
        //        //row["Argument"] = i;
        //        row["Value"] = data;
        //        table.Rows.Add(row);
        //        i++;
        //    }

        //    return table;
        //}

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
            CreateShortBlock();
        }

        private bool CreateShortBlock()
        {
            double.TryParse(edtSBLength.Text, out sbLen);
            double.TryParse(edtOverlap.Text, out overlap);

            CreateShortBlockRequest req = new CreateShortBlockRequest();
            req.command = "create-shortblock";
            req.blockMetaSeq = "";
            req.partSeq = "";
            req.sliceTime = sbLen;
            req.overlap = overlap;

            string presetPack = string.Empty;
            string presetSeq = string.Empty;
            if (luePresetList.GetColumnValue("PresetPack") != null)
            {
                presetPack = luePresetList.GetColumnValue("PresetPack").ToString();
                ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(presetPack));

                if (preset != null)
                {
                    presetSeq = preset.seq;
                }
            }

            req.presetPack = presetPack;
            req.presetSeq = presetSeq;
            
            req.parameters = new List<Parameter>();
            foreach (SBParamControl ctrl in sbParamList)
            {
                ////Encoding
                //byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(ctrl.PartName);
                //string partName = Convert.ToBase64String(basebyte);

                //string t1 = Utils.GetJulianFromDate(ctrl.Min);
                //string t2 = Utils.GetJulianFromDate(ctrl.Max);

                //req.parts.Add(new Part(partName, t1, t2));
                Parameter param = new Parameter();
                ResponseParam resParam = ctrl.Param;
                param.paramPack = resParam.paramPack;
                param.paramSeq = resParam.seq;
                param.paramName = resParam.paramName;
                param.paramKey = resParam.paramKey;
                param.adamsKey = resParam.adamsKey;
                param.zaeroKey = resParam.zaeroKey;
                param.grtKey = resParam.grtKey;
                param.fltpKey = resParam.fltpKey;
                param.fltsKey = resParam.fltsKey;
                param.paramUnit = resParam.paramUnit;

                req.parameters.Add(param);
            }

            req.shortBlocks = new List<ShortBlock>();
            int i = 1;
            foreach(SBIntervalControl ctrl in sbIntervalList)
            {
                ShortBlock sb = new ShortBlock();
                SplittedSB splitSb = ctrl.Sb;
                sb.blockNo = i++;
                sb.blockName = splitSb.SbName;
                sb.julianStartAt = splitSb.StartTime;
                sb.julianEndAt = splitSb.EndTime;

                req.shortBlocks.Add(sb);

            }

            var json = JsonConvert.SerializeObject(req);
            Console.WriteLine(json);

            string url = ConfigurationManager.AppSettings["UrlPart"];

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
            CreateShortBlockResponse result = JsonConvert.DeserializeObject<CreateShortBlockResponse>(responseText);

            if (result != null)
            {
                if (result.code != 200)
                {
                    return false;
                }
                else
                {
                    //MessageBox.Show("Success");
                    CreateSBProgressForm form = new CreateSBProgressForm(result.response.seq);
                    form.ShowDialog();
                }
            }
            return true;

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

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart1.HitTest(pos.X, pos.Y, false,
                                            ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;
                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                        // check if the cursor is really close to the point (2 pixels around the point)
                        if (Math.Abs(pos.X - pointXPixel) < 2 &&
                            Math.Abs(pos.Y - pointYPixel) < 2)
                        {
                            DateTime dt1 = DateTime.FromOADate(prop.XValue);
                            tooltip.Show("X=" + dt1 + ", Y=" + prop.YValues[0], this.chart1,
                                            pos.X, pos.Y - 15);
                            //Console.WriteLine(string.Format("X = {0}", prop.XValue));
                            //Console.WriteLine(string.Format("X-time = {0}", dt1));
                        }
                    }
                }
            }
        }

    }

    
}
