using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DynaRAP.Data;
using log4net.Config;
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
    public partial class SBListControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
     
        List<ResponseImport> uploadList = new List<ResponseImport>();
        List<ResponsePart> partList = new List<ResponsePart>();
        List<ResponseSBList> sbList = new List<ResponseSBList>();

        List<SBData> gridList = null;

        public SBListControl()
        {
            InitializeComponent();
        
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void SBListControl_Load(object sender, EventArgs e)
        {
            cboFlying.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboFlying.SelectedIndexChanged += CboFlying_SelectedIndexChanged;

            cboPart.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboPart.SelectedIndexChanged += CboPart_SelectedIndexChanged;

            uploadList = GetUploadList();
            InitializeFlyingList();
            InitializeGridControl();

            InitializeTagGridControl();

        }

        private void InitializeTagGridControl()
        {
            //gridView2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView2.OptionsView.ShowColumnHeaders = false;
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

        private void InitializeGridControl()
        {
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

            gridView1.OptionsMenu.EnableColumnMenu = false;

            gridView1.CustomDrawRowIndicator += GridView1_CustomDrawRowIndicator;

            GridColumn colName = gridView1.Columns["BlockName"];
            colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colName.OptionsColumn.FixedWidth = true;
            colName.Width = 240;
            colName.Caption = "BLOCK 이름";

            GridColumn colView = gridView1.Columns["View"];
            colView.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colView.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colView.OptionsColumn.FixedWidth = true;
            colView.Width = 40;
            colView.Caption = "보기";
            colView.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;

            GridColumn colDownload1 = gridView1.Columns["Download1"];
            colDownload1.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDownload1.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colDownload1.OptionsColumn.FixedWidth = true;
            colDownload1.Width = 80;
            colDownload1.Caption = "CSV_RAW";
            colDownload1.OptionsColumn.ReadOnly = true;

            GridColumn colDownload2 = gridView1.Columns["Download2"];
            colDownload2.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDownload2.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colDownload2.OptionsColumn.FixedWidth = true;
            colDownload2.Width = 80;
            colDownload2.Caption = "CSV_LPF";
            colDownload2.OptionsColumn.ReadOnly = true;

            GridColumn colDownload3 = gridView1.Columns["Download3"];
            colDownload3.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDownload3.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colDownload3.OptionsColumn.FixedWidth = true;
            colDownload3.Width = 80;
            colDownload3.Caption = "CSV_HPF";
            colDownload3.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox2.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox2.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox2.Click += RepositoryItemImageComboBox2_Click;

            this.repositoryItemImageComboBox3.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox3.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox3.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox3.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox3.Click += RepositoryItemImageComboBox3_Click;

            this.repositoryItemImageComboBox4.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox4.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox4.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox4.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox4.Click += RepositoryItemImageComboBox4_Click;

#if DEBUG
            gridView1.Columns["Seq"].Visible = true;
#endif
        }

        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            int row = gridView1.FocusedRowHandle;
            string blockSeq = gridView1.GetRowCellValue(row, "Seq") == null ? "" : gridView1.GetRowCellValue(row, "Seq").ToString();
            string start = gridView1.GetRowCellValue(row, "JulianStartAt") == null ? "" : gridView1.GetRowCellValue(row, "JulianStartAt").ToString();
            string end = gridView1.GetRowCellValue(row, "JulianEndAt") == null ? "" : gridView1.GetRowCellValue(row, "JulianEndAt").ToString();

            GetSBData(blockSeq, start, end, "N");
        }

        private void RepositoryItemImageComboBox2_Click(object sender, EventArgs e)
        {
            int row = gridView1.FocusedRowHandle;
            string blockSeq = gridView1.GetRowCellValue(row, "Seq") == null ? "" : gridView1.GetRowCellValue(row, "Seq").ToString();
            string start = gridView1.GetRowCellValue(row, "JulianStartAt") == null ? "" : gridView1.GetRowCellValue(row, "JulianStartAt").ToString();
            string end = gridView1.GetRowCellValue(row, "JulianEndAt") == null ? "" : gridView1.GetRowCellValue(row, "JulianEndAt").ToString();

            // 아래 strParamSet 을 구해서 GetSBData() 호출할 때 사용하면 해당 paramSet 에 대한 SBData 만 얻어올 수 있다.
            //List<string> sbParamList = GetSBParamList(blockSeq);
            //string strParamSet = JsonConvert.SerializeObject(sbParamList);

            GetSBData(blockSeq, start, end, "N", true);
        }

        private void RepositoryItemImageComboBox3_Click(object sender, EventArgs e)
        {
            int row = gridView1.FocusedRowHandle;
            string blockSeq = gridView1.GetRowCellValue(row, "Seq") == null ? "" : gridView1.GetRowCellValue(row, "Seq").ToString();
            string start = gridView1.GetRowCellValue(row, "JulianStartAt") == null ? "" : gridView1.GetRowCellValue(row, "JulianStartAt").ToString();
            string end = gridView1.GetRowCellValue(row, "JulianEndAt") == null ? "" : gridView1.GetRowCellValue(row, "JulianEndAt").ToString();

            GetSBData(blockSeq, start, end, "L", true);
        }

        private void RepositoryItemImageComboBox4_Click(object sender, EventArgs e)
        {
            int row = gridView1.FocusedRowHandle;
            string blockSeq = gridView1.GetRowCellValue(row, "Seq") == null ? "" : gridView1.GetRowCellValue(row, "Seq").ToString();
            string start = gridView1.GetRowCellValue(row, "JulianStartAt") == null ? "" : gridView1.GetRowCellValue(row, "JulianStartAt").ToString();
            string end = gridView1.GetRowCellValue(row, "JulianEndAt") == null ? "" : gridView1.GetRowCellValue(row, "JulianEndAt").ToString();

            GetSBData(blockSeq, start, end, "H", true);
        }

        private List<string> GetSBParamList(string blockSeq)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlShortBlock"];

                string sendData = string.Format(@"
                {{
                ""command"":""param-list"",
                ""blockSeq"":""{0}""
                }}"
                , blockSeq);

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

                SBParamListResponse result = JsonConvert.DeserializeObject<SBParamListResponse>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        return null;
                    }
                    else
                    {
                        return result.response.paramSet;
                    }
                }

                //Console.WriteLine(responseText);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

            return null;

        }

        private void GetSBData(string blockSeq, string start, string end,  string filterType, bool bDownload = false)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlShortBlock"];

                if (bDownload)
                {
                    url += "/d";
                }

                string sendData = string.Format(@"
                {{
                ""command"":""row-data"",
                ""blockSeq"":""{0}"",
                ""julianRange"":[""{1}"", ""{2}""],
                ""filterType"": ""{3}""
                }}"
                , blockSeq, start, end, filterType);

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

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Comma Separated Value files (CSV)|*.csv";
                dlg.Title = "Save an Image File";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string fileName = dlg.FileName;

                    FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(responseText);
                    sw.Close();
                    fs.Close();

                }

                //Console.WriteLine(responseText);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }

        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private void InitializeFlyingList()
        {
            cboFlying.Properties.Items.Clear();

            if (uploadList == null)
                return;

            foreach (ResponseImport list in uploadList)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(list.uploadName);
                string decName = Encoding.UTF8.GetString(byte64);
                cboFlying.Properties.Items.Add(decName);
            }

            cboFlying.SelectedIndex = -1;

        }

        private void CboFlying_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                string uploadName = combo.Text;
                InitializePartList(uploadName);

                //Encoding
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(uploadName);
                string encName = Convert.ToBase64String(basebyte);

                ResponseImport upload = uploadList.Find(x => x.uploadName.Equals(encName));
                if (upload != null)
                {
                    gridControl2.DataSource = null;
                    gridView2.RefreshData();

                    string[] tagArr = GetTagList(upload.seq);

                    if (tagArr != null)
                    {
                        DataTable dt = new DataTable();

                        dt.Columns.Add("Tag", typeof(string));

                        foreach (string tag in tagArr)
                        {
                            dt.Rows.Add(tag);
                        }

                        gridControl2.DataSource = dt;
                        gridView2.RefreshData();
                    }
                }
            }
        }

        private string[] GetTagList(string uploadSeq)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlDataProp"];

                string sendData = string.Format(@"
                {{
                ""command"":""list"",
                ""referenceType"": ""upload"",
                ""referenceKey"": ""{0}""
                }}"
                , uploadSeq);

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
                DataPropResponse result = JsonConvert.DeserializeObject<DataPropResponse>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                    }
                    else
                    {
                        foreach (ResponseDataProp data in result.response)
                        {
                            // 서버에서는 List<ResponseDataProp> 로 주지만 실제로는 값이 하나임.
                            // 첫 데이터만 이용하고 빠져나감.
                            //Decoding
                            byte[] byte64 = Convert.FromBase64String(data.propValue);
                            string decName = Encoding.UTF8.GetString(byte64);
                            string[] tagArr = decName.Split(',');
                            return tagArr;
                        }
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

        private void CboPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridList = null;
            gridControl1.DataSource = null;

            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                //Encoding
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(combo.Text);
                string encName = Convert.ToBase64String(basebyte);

                ResponsePart part = partList.Find(x => x.partName.Equals(encName));
                if (part != null)
                {
                    sbList = GetSBList(part.seq);

                    if(sbList != null)
                    {
                        gridList = new List<SBData>();
                        foreach (ResponseSBList sb in sbList)
                        {
                            //Decoding
                            byte[] byte64 = Convert.FromBase64String(sb.blockName);
                            string decName = Encoding.UTF8.GetString(byte64);

                            gridList.Add(new SBData(decName, sb.julianStartAt, sb.julianEndAt, sb.seq, 1, 1, 1, 1));
                        }

                        this.gridControl1.DataSource = gridList;
                    }
                    gridView1.RefreshData();
                }
            }
        }

        private List<ResponseSBList> GetSBList(string partSeq)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlShortBlock"];

                string sendData = string.Format(@"
                {{
                ""command"":""list"",
                ""registerUid"":"""",
                ""partSeq"":""{0}"",
                ""pageNo"":1,
                ""pageSize"":3000
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
                SBListResponse result = JsonConvert.DeserializeObject<SBListResponse>(responseText);

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

        private void InitializePartList(string flyingName)
        {
            gridList = null;
            gridControl1.DataSource = null;
            gridView1.RefreshData();

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

            cboPart.SelectedIndex = -1;

        }

        private List<ResponseImport> GetUploadList()
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlImport"];
                string sendData = @"
            {
            ""command"":""upload-list""
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
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }
            return null;

        }

        private List<ResponsePart> GetPartList(string flyingName)
        {
            try
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
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }
            return null;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            uploadList = GetUploadList();
            InitializeFlyingList();

            gridList = null;
            gridControl1.DataSource = null;
            gridView1.RefreshData();

            gridControl2.DataSource = null;
            gridView2.RefreshData();
        }
    }
}
