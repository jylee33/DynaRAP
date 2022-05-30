using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DynaRAP.Data;
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
    public partial class BinModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        List<ResponsePreset> presetList = null;
        List<ResponseParam> paramList = null;
        List<PresetParamData> gridList = null;

        public BinModuleControl()
        {
            InitializeComponent();
        }

        private void BinModuleControl_Load(object sender, EventArgs e)
        {
            InitializeSBParamComboList();

            //DateTime dtNow = DateTime.Now;
            //string strNow = string.Format("{0:yyyy-MM-dd}", dtNow);
            //dateScenario.Text = strNow;

            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.VerticalScroll.Visible = true;

            btnAddParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddParameter.Properties.AllowFocused = false;

            paramList = GetParamList();
           
            InitializeGridControl();
        }

        private List<ResponseParam> GetParamList()
        {
            string url = ConfigurationManager.AppSettings["UrlParam"];
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
            ListParamJsonData result = JsonConvert.DeserializeObject<ListParamJsonData>(responseText);

            return result.response;

        }

        private void InitializeGridControl()
        {
            //paramList
            repositoryItemComboBox1.TextEditStyle = TextEditStyles.DisableTextEditor;

            foreach (ResponseParam param in paramList)
            {
                repositoryItemComboBox1.Items.Add(param.paramKey);
            }

            //gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

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

            GridColumn colType = gridView1.Columns["ParamKey"];
            colType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colType.OptionsColumn.FixedWidth = true;
            colType.Width = 240;
            colType.Caption = "Parameter Name";

            GridColumn colDel = gridView1.Columns["Del"];
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
            gridView1.DeleteRow(gridView1.FocusedRowHandle);
        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private void InitializeSBParamComboList()
        {
            cboSBParameter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboSBParameter.SelectedIndexChanged += CboSBParameter_SelectedIndexChanged;

            presetList = GetPresetList();

            cboSBParameter.Properties.Items.Clear();

            foreach(ResponsePreset preset in presetList)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(preset.presetName);
                string decName = Encoding.UTF8.GetString(byte64);
                cboSBParameter.Properties.Items.Add(decName);
            }

            cboSBParameter.SelectedIndex = -1;
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
            ComboBoxEdit cbo = sender as ComboBoxEdit;
            string presetPack = String.Empty;
            paramList = null;

            if (cbo != null)
            {
                //Encoding
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(cbo.Text);
                string encName = Convert.ToBase64String(basebyte);

                ResponsePreset preset = presetList.Find(x => x.presetName.Equals(encName));

                if(preset != null)
                {
                    paramList = GetPresetParamList(preset.presetPack);

                    if (paramList != null)
                    {
                        gridList = new List<PresetParamData>();
                        foreach (ResponseParam param in paramList)
                        {
                            //AddParameter(param);
                            gridList.Add(new PresetParamData(param.paramKey, param.adamsKey, param.zaeroKey, param.grtKey, param.fltpKey, param.fltsKey, param.propInfo.propType, param.partInfo, 1));
                        }

                        this.gridControl1.DataSource = gridList;
                    }
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

        private void btnAddParameter_ButtonClick(object sender, EventArgs e)
        {
            if (gridList == null)
            {
                gridList = new List<PresetParamData>();
            }
            gridList.Add(new PresetParamData("", "", "", "", "", "", "", "", 1));
            this.gridControl1.DataSource = gridList;
            //gridControl1.Update();
            gridView1.RefreshData();
        }

        private void hyperlinkBrowseSB_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;

            mainForm.PanelBinSbList.Show();

        }

        private void btnCreateBIN_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;

            mainForm.PanelBinTable.Show();
        }
    }

    
}
