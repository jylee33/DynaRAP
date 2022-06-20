using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using DynaRAP.Common;
using DynaRAP.Data;
using DynaRAP.EventData;
using DynaRAP.Forms;
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
    public partial class ImportDLLControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        List<ResponseDLL> dllList = new List<ResponseDLL>();
        List<DllData> dllDataList = new List<DllData>();
        List<ResponseDLLParam> dllParamList = new List<ResponseDLLParam>();
        string dllSeq = string.Empty;

        List<DllParamControl> paramControlList = new List<DllParamControl>();

        public ImportDLLControl()
        {
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void ImportDLLControl_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.VerticalScroll.Visible = true;

            btnAddDLL.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddDLL.Properties.AllowFocused = false;

            InitializeGridControl1();
            lblDLLCount.Text = string.Format(Properties.Resources.StringDLLCount, dllDataList.Count);

            xtraTabControl1.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover;
            xtraTabControl1.CloseButtonClick += XtraTabControl1_CloseButtonClick;

            btnAddDllParam.Properties.TextEditStyle |= DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddDllParam.Properties.AllowFocused = false;
            btnAddDllParam.Enabled = false;
        }

        private void XtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.StringDelete, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
            XtraTabPage tabPage = arg.Page as XtraTabPage;

            DllParamResponse result = RemoveDllParam(tabPage.Name);

            if (result.code == 200)
            {
                tabPage.PageVisible = false;
            }
            else
            {
                MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DllParamResponse RemoveDllParam(string paramSeq)
        {
            DllParamResponse result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlDLL"];

                string sendData = string.Format(@"
                {{
                ""command"":""param-remove"",
                ""seq"":""{0}""
                }}"
                , paramSeq);

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
                result = JsonConvert.DeserializeObject<DllParamResponse>(responseText);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return result;

        }

        private void AddTabPage(string tabName, string paramSeq, string paramType)
        {
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = paramSeq;
            tabPage.Text = tabName;
            //tabPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;

            DllParamControl paramControl = new DllParamControl(dllSeq, paramSeq, paramType);
            paramControl.AddDel_Succeeded += ParamControl_DataAddDel_Succeeded;
            paramControl.Dock = DockStyle.Fill;
            paramControlList.Add(paramControl);
            tabPage.Controls.Add(paramControl);
        }

        private void ParamControl_DataAddDel_Succeeded(object sender, EventArgs e)
        {
            DllParamControl me = sender as DllParamControl;

            foreach (DllParamControl ctrl in paramControlList)
            {
                if (ctrl == me)
                    continue;

                ctrl.RefreshGridControl();
            }
        }

        private void InitializeGridControl1()
        {

            //gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            dllList = GetDllList();

            dllDataList.Clear();
            foreach(ResponseDLL dll in dllList)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(dll.dataSetName);
                string decName = Encoding.UTF8.GetString(byte64);

                dllDataList.Add(new DllData(dll.seq, dll.dataSetCode, decName, dll.dataVersion, dll.createdAt.dateTime, 1, 1));
            }
            gridControl1.DataSource = dllDataList;

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

            GridColumn colCode = gridView1.Columns["DataSetCode"];
            colCode.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            //colCode.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colCode.OptionsColumn.FixedWidth = true;
            //colCode.Width = 240;
            //colCode.Caption = "UnmappedParamName";
            colCode.OptionsColumn.ReadOnly = true;

            GridColumn colName = gridView1.Columns["DataSetName"];
            colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colName.OptionsColumn.ReadOnly = true;

            GridColumn colVersion = gridView1.Columns["DataVersion"];
            colVersion.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colVersion.OptionsColumn.ReadOnly = true;

            GridColumn colTime = gridView1.Columns["RegTime"];
            colTime.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colTime.OptionsColumn.ReadOnly = true;

            GridColumn colDel = gridView1.Columns["Del"];
            colDel.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.OptionsColumn.FixedWidth = true;
            colDel.Width = 40;
            colDel.Caption = "삭제";
            colDel.OptionsColumn.ReadOnly = true;

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

            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));
            this.repositoryItemImageComboBox2.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox2.Buttons[0].Visible = false;
            this.repositoryItemImageComboBox2.Click += RepositoryItemImageComboBox2_Click;
        }

        private List<ResponseDLL> GetDllList()
        {
            DllListResponse result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlDLL"];
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
                result = JsonConvert.DeserializeObject<DllListResponse>(responseText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return result.response;

        }

        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            string dllName = string.Format("DLL - [{0}]{1}",
                gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DataSetCode").ToString()
                , gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DataSetName").ToString());

            lblDllName.Text = dllName;

            this.xtraTabControl1.TabPages.Clear();
            this.paramControlList.Clear();
            dllSeq = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Seq").ToString();
            dllParamList = GetDllParamList(dllSeq);
            foreach (ResponseDLLParam param in dllParamList)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(param.paramName);
                string decName = Encoding.UTF8.GetString(byte64);

                AddTabPage(decName, param.seq, param.paramType);
            }
            btnAddDllParam.Enabled = true;
        }

        private List<ResponseDLLParam> GetDllParamList(string seq)
        {
            DllParamListResponse result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlDLL"];
                string sendData = string.Format(@"
                {{
                ""command"":""param-list"",
                ""dllSeq"":""{0}""
                }}"
                , seq);

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
                result = JsonConvert.DeserializeObject<DllParamListResponse>(responseText);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return result.response;

        }

        private void RepositoryItemImageComboBox2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.StringDelete, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            dllSeq = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Seq").ToString();
            DllResponse result = RemoveDll(dllSeq);
            if (result.code == 200)
            {
                gridView1.DeleteRow(gridView1.FocusedRowHandle);
                lblDLLCount.Text = string.Format(Properties.Resources.StringSplitCount, dllDataList.Count);
            }
            else
            {
                MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DllResponse RemoveDll(string seq)
        {
            DllResponse result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlDLL"];

                string sendData = string.Format(@"
                {{
                ""command"":""remove"",
                ""seq"":""{0}""
                }}"
                , seq);

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
                result = JsonConvert.DeserializeObject<DllResponse>(responseText);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return result;

        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private void btnAddDLL_ButtonClick(object sender, EventArgs e)
        {
            AddDllForm form = new AddDllForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (dllDataList == null)
                {
                    dllDataList = new List<DllData>();
                }
                
                ResponseDLL resp = form.Response.response;

                //Decoding
                byte[] byte64 = Convert.FromBase64String(resp.dataSetName);
                string decName = Encoding.UTF8.GetString(byte64);

                dllDataList.Add(new DllData(resp.seq, resp.dataSetCode, decName, resp.dataVersion, resp.createdAt.dateTime, 1, 1));
                this.gridControl1.DataSource = dllDataList;
                //gridControl1.Update();
                gridView1.RefreshData();

                lblDLLCount.Text = string.Format(Properties.Resources.StringDLLCount, dllDataList.Count);
            }
        }

        private void btnAddDllParam_ButtonClick(object sender, EventArgs e)
        {
            AddDllParamForm form = new AddDllParamForm(dllSeq);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (dllDataList == null)
                {
                    dllDataList = new List<DllData>();
                }

                ResponseDLLParam resp = form.Response.response;

                //Decoding
                byte[] byte64 = Convert.FromBase64String(resp.paramName);
                string decName = Encoding.UTF8.GetString(byte64);

                AddTabPage(decName, resp.seq, resp.paramType);
            }
        }
    }

    
}
