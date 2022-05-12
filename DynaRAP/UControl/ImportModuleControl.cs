using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DynaRAP.Data;
using DynaRAP.EventData;
using DynaRAP.UTIL;
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
        string selectedFuselage = string.Empty;
        Dictionary<string, List<string>> dicData = new Dictionary<string, List<string>>();
        List<ImportParamControl> paramList = new List<ImportParamControl>();
        List<ImportIntervalControl> splitList = new List<ImportIntervalControl>();
        List<ResponsePreset> presetList = null;
        List<PresetData> pComboList = null;

        string csvFilePath = string.Empty;
        object minValue = null;
        object maxValue = null;

        public ImportModuleControl()
        {
            InitializeComponent();
        }

        private void ImportModuleControl_Load(object sender, EventArgs e)
        {
            //InitializeSplittedRegionList();

            luePresetList.Properties.DisplayMember = "PresetName";
            luePresetList.Properties.ValueMember = "PresetPack";
            luePresetList.Properties.NullText = "";

            InitializePresetList();

            DateTime dtNow = DateTime.Now;
            string strNow = string.Format("{0:yyyy-MM-dd}", dtNow);
            //dateScenario.Text = strNow;

            panelData.AutoScroll = true;
            panelData.WrapContents = false;
            panelData.HorizontalScroll.Visible = false;
            panelData.VerticalScroll.Visible = true;

            btnViewData.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddSplittedInterval.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnSaveSplittedInterval.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            btnViewData.Properties.AllowFocused = false;
            btnAddParameter.Properties.AllowFocused = false;
            btnAddSplittedInterval.Properties.AllowFocused = false;
            btnSaveSplittedInterval.Properties.AllowFocused = false;

            btnAddParameter.Enabled = false;
            
            lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, splitList.Count);

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

        int paramIndex = 9;

        private void AddParameter()
        {
            ImportParamControl ctrl = new ImportParamControl();
            ctrl.Title = "Parameter " + paramIndex.ToString();
            ctrl.DicData = dicData;
            ctrl.OnSelectedRange += ChartControl_OnSelectedRange;
            ctrl.Dock = DockStyle.Fill;
            panelData.Controls.Add(ctrl);
            panelData.Controls.SetChildIndex(ctrl, paramIndex++);

            paramList.Add(ctrl);

        }
        private void ChartControl_OnSelectedRange(object sender, SelectedRangeEventArgs e)
        {
            ImportParamControl me = sender as ImportParamControl;
            minValue = e.MinValue;
            maxValue = e.MaxValue;

            Debug.Print(string.Format("-----> MinValue : {0}, MaxValue : {1}", minValue, maxValue));

            foreach(ImportParamControl ctrl in paramList)
            {
                if (ctrl == me)
                    continue;

                ctrl.SelectRegion(minValue, maxValue);
            }
        }

        private void btnAddSplittedInterval_ButtonClick(object sender, EventArgs e)
        {
            AddSplittedInterval();

            lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, splitList.Count);
        }

        int intervalIndex = 6;

        private void AddSplittedInterval()
        {
            if(minValue == null || maxValue == null)
            {
                if(paramList.Count == 0)
                {
                    MessageBox.Show(Properties.Resources.StringAddParameter);
                    return;
                }
                else
                {
                    ImportParamControl paramCtrl =  paramList[0] as ImportParamControl;
                    paramCtrl.Sync();
                }
            }

            if (minValue == null || maxValue == null)
            {
                MessageBox.Show(Properties.Resources.StringNoSelectedRegion);
                return;
            }
            
            ImportIntervalControl ctrl = new ImportIntervalControl(minValue, maxValue);
            ctrl.Title = "flight#" + (paramIndex + intervalIndex).ToString();
            ctrl.DeleteBtnClicked += new EventHandler(Interval_DeleteBtnClicked);
            panelData.Controls.Add(ctrl);
            panelData.Controls.SetChildIndex(ctrl, paramIndex + intervalIndex);
            splitList.Add(ctrl);

            intervalIndex++;

        }

        void Interval_DeleteBtnClicked(object sender, EventArgs e)
        {
            ImportIntervalControl ctrl = sender as ImportIntervalControl;
            panelData.Controls.Remove(ctrl);
            splitList.Remove(ctrl);
            ctrl.Dispose();
            
            intervalIndex--;

            lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, splitList.Count);
        }

        private void btnSaveSplittedInterval_ButtonClick(object sender, EventArgs e)
        {
            bool bResult = Import();

            if (bResult)
            {
                MessageBox.Show(Properties.Resources.StringSuccessImport, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool Import()
        {
            ImportRequest import = new ImportRequest();
            import.command = "upload";
            import.sourcePath = csvFilePath;
            import.flightAt = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            import.dataType = "flight";
            import.parts = new List<Part>();

            string presetPack = String.Empty;
            if (luePresetList.GetColumnValue("PresetPack") != null)
                presetPack = luePresetList.GetColumnValue("PresetPack").ToString();

            import.presetPack = presetPack;

            foreach (ImportIntervalControl ctrl in splitList)
            {
                //Encoding
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(ctrl.PartName);
                string partName = Convert.ToBase64String(basebyte);

                string t1 = Utils.GetJulianFromDate(ctrl.Min);
                string t2 = Utils.GetJulianFromDate(ctrl.Max);

                import.parts.Add(new Part(partName, t1, t2));
            }

            var json = JsonConvert.SerializeObject(import);
            Console.WriteLine(json);

            string url = ConfigurationManager.AppSettings["UrlImport"];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 60 * 60 * 1000;   // 1시간 timeout
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
                }
            }
            return true;
        }

        private void lblFlyingData_Click(object sender, EventArgs e)
        {
            btnAddParameter.Enabled = false;
            
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "C:\\";
            dlg.Filter = "Excel files (*.xls, *.xlsx)|*.xls; *.xlsx|Comma Separated Value files (CSV)|*.csv|모든 파일 (*.*)|*.*";
            //dlg.Filter = "Comma Separated Value files (CSV)|*.csv";

#if !DEBUG
            if (dlg.ShowDialog() == DialogResult.OK)
#endif
            {
#if DEBUG
                csvFilePath = @"C:\temp\a.xls";
                lblFlyingData.Text = @"C:\temp\a.xls";
                StreamReader sr = new StreamReader(csvFilePath);
#else
                csvFilePath = dlg.FileName;
                lblFlyingData.Text = csvFilePath;
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
            }
        }

        private void luePresetList_EditValueChanged(object sender, EventArgs e)
        {

        }
    }

    
}
