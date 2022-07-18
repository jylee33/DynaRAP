using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DynaRAP.Data;
using DynaRAP.UTIL;
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
    public partial class DllParamControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        string dllSeq = string.Empty;
        string dllParamSeq = string.Empty;
        string paramType = string.Empty;

        ResponseDLLParamData dllParamDataList = new ResponseDLLParamData();
        //List<DllParamData> dllDataGridList = new List<DllParamData>();

        public event EventHandler AddDel_Succeeded;

        public DllParamControl()
        {
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        public DllParamControl(string dllSeq, string dllParamSeq, string paramType) : this()
        {
            this.dllSeq = dllSeq;
            this.dllParamSeq = dllParamSeq;
            this.paramType = paramType;
        }

        private void DllParamControl_Load(object sender, EventArgs e)
        {
            InitializeGridControl1();
        }

        private void InitializeGridControl1()
        {

            //gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            //gridControl1.UseEmbeddedNavigator = true;
            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
           
            RefreshGridControl();
            gridView1.InitNewRow += GridView1_InitNewRow;

            gridView1.OptionsView.ShowColumnHeaders = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = true;
            gridView1.IndicatorWidth = 40;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ColumnAutoWidth = true;
            
            gridView1.OptionsBehavior.ReadOnly = false;
            //gridView1.OptionsBehavior.Editable = false;
            
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            
            gridView1.OptionsSelection.MultiSelect = true;
            //gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            gridView1.OptionsClipboard.CopyColumnHeaders = DevExpress.Utils.DefaultBoolean.False;

            gridView1.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.Update;
            
            gridView1.CustomDrawRowIndicator += GridView1_CustomDrawRowIndicator;
            //gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.True;

            GridColumn colData = gridView1.Columns["Data"];
            colData.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            //colData.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colData.OptionsColumn.FixedWidth = true;
            //colData.Width = 240;
            //colData.Caption = "UnmappedParamName";
            //colData.OptionsColumn.ReadOnly = true;
            
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
            
            //gridView1.BestFitColumns();
        }

        public void RefreshGridControl()
        {
            dllParamDataList = GetDllParamDataList();

            if (dllParamDataList == null)
                return;

            //dllDataGridList.Clear();
            //foreach (string key in dllParamDataList.data.Keys)
            //{
            //    if (key.Equals(this.dllParamSeq))
            //    {
            //        foreach (string val in dllParamDataList.data[key])
            //        {
            //            dllDataGridList.Add(new DllParamData(val, 1));
            //        }
            //    }
            //}
            //gridControl1.DataSource = dllDataGridList;

            DataTable dt = new DataTable();

            dt.Columns.Add("Data", typeof(string));
            dt.Columns.Add("Del", typeof(int));

            foreach (string key in dllParamDataList.data.Keys)
            {
                if (key.Equals(this.dllParamSeq))
                {
                    foreach (string val in dllParamDataList.data[key])
                    {
                        dt.Rows.Add(val, 1);
                    }
                }
            }

            gridControl1.DataSource = dt;
            gridView1.RefreshData();

        }

        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.StringDelete, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            DllParamDataResponse result = RemoveDll(dllSeq);
            if (result.code == 200)
            {
                gridView1.DeleteRow(gridView1.FocusedRowHandle);
                gridView1.RefreshData();

                if (this.AddDel_Succeeded != null)
                {
                    this.AddDel_Succeeded(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DllParamDataResponse RemoveDll(string dllSeq)
        {
            DllParamDataResponse result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlDLL"];
                string sendData = string.Format(@"
                {{
                ""command"":""data-remove"",
                ""dllSeq"":""{0}"",
                ""dllRowRange"":[{1}, {1}]
                }}"
                , dllSeq, gridView1.FocusedRowHandle + 1);

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
                result = JsonConvert.DeserializeObject<DllParamDataResponse>(responseText);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

            return result;

        }

        private void btnAddNewRow_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();
#if null
            string dataName = string.Empty;
            if (this.paramType.Equals("data"))
            {
                dataName = Prompt.ShowDialog("데이터 이름", "데이터 추가", true);
            }
            else
            {
                dataName = Prompt.ShowDialog("데이터 이름", "데이터 추가");
            }

            if (string.IsNullOrEmpty(dataName))
            {
                //MessageBox.Show("데이터를 입력하세요", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DllParamDataResponse result = AddDllParamData(dataName);
            if (result != null)
            {
                if (result.code == 200)
                {
                    if (this.paramType.Equals("data"))
                    {
                        dllDataGridList.Add(new DllParamData(result.response.paramVal.ToString(), 1));
                    }
                    else
                    {
                        dllDataGridList.Add(new DllParamData(result.response.paramValStr.ToString(), 1));
                    }
                    gridControl1.DataSource = dllDataGridList;
                    //gridControl1.Update();
                    gridView1.RefreshData();

                    if(this.AddDel_Succeeded != null)
                    {
                        this.AddDel_Succeeded(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
#endif
        }

        private DllParamDataResponse AddDllParamData(string dataName)
        {
            DllParamDataResponse result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlDLL"];
                string sendData = string.Empty;

                if (this.paramType.Equals("data"))
                {
                    sendData = string.Format(@"
                    {{
                    ""command"":""data-add"",
                    ""dllSeq"":""{0}"",
                    ""paramSeq"":""{1}"",
                    ""paramVal"":{2}
                    }}"
                    , dllSeq, dllParamSeq, dataName);
                }
                else
                {
                    sendData = string.Format(@"
                    {{
                    ""command"":""data-add"",
                    ""dllSeq"":""{0}"",
                    ""paramSeq"":""{1}"",
                    ""paramValStr"":{2}
                    }}"
                     , dllSeq, dllParamSeq, dataName);
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
                result = JsonConvert.DeserializeObject<DllParamDataResponse>(responseText);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

            return result;

        }

        private void GridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gridView = sender as GridView;
            // Set the new row cell value
            //gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Data"], "test");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Del"], 1);
        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private ResponseDLLParamData GetDllParamDataList()
        {
            DllParamDataListResponse result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlDLL"];
                string sendData = string.Format(@"
                {{
                ""command"":""data-list"",
                ""dllSeq"":""{0}"",
                ""dllParamSeq"":""{1}""
                }}"
                , dllSeq, dllParamSeq);

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
                result = JsonConvert.DeserializeObject<DllParamDataListResponse>(responseText);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

            return result.response;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddDllParamDataBulk();
        }

        private bool AddDllParamDataBulk()
        {
            try
            {
                DllParamDataAddBulkRequest req = new DllParamDataAddBulkRequest();

                req.command = "data-add-bulk";
                req.dllSeq = this.dllSeq;
                req.paramSeq = this.dllParamSeq;
                req.data = new List<string>();

                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    string gridData = gridView1.GetRowCellValue(i, "Data") == null ? "" : gridView1.GetRowCellValue(i, "Data").ToString();

                    req.data.Add(gridData);
                }

                string url = ConfigurationManager.AppSettings["UrlDLL"];

                var json = JsonConvert.SerializeObject(req);

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
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;

        }

    }
}
