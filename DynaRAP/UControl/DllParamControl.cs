using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
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
    public partial class DllParamControl : DevExpress.XtraEditors.XtraUserControl
    {
        string dllSeq = string.Empty;
        string dllParamSeq = string.Empty;
        string paramType = string.Empty;

        ResponseDLLParamData dllParamDataList = new ResponseDLLParamData();
        List<DllParamData> dllDataGridList = new List<DllParamData>();

        public event EventHandler AddDel_Succeeded;

        public DllParamControl()
        {
            InitializeComponent();
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
            //gridView1.InitNewRow += GridView1_InitNewRow;
            
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
            //gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.True;

            GridColumn colData = gridView1.Columns["Data"];
            colData.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            //colData.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colData.OptionsColumn.FixedWidth = true;
            //colData.Width = 240;
            //colData.Caption = "UnmappedParamName";
            colData.OptionsColumn.ReadOnly = true;

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

            RefreshGridControl();
            //gridView1.BestFitColumns();
        }

        public void RefreshGridControl()
        {
            dllParamDataList = GetDllParamDataList();

            dllDataGridList.Clear();
            foreach (string key in dllParamDataList.data.Keys)
            {
                if (key.Equals(this.dllParamSeq))
                {
                    foreach (string val in dllParamDataList.data[key])
                    {
                        dllDataGridList.Add(new DllParamData(val, 1));
                    }
                }
            }
            gridControl1.DataSource = dllDataGridList;
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
            string url = ConfigurationManager.AppSettings["UrlDLL"];
            string sendData = string.Format(@"
                {{
                ""command"":""data-remove"",
                ""dllSeq"":""{0}"",
                ""dllRowRange"":[{1}, {1}]
                }}"
                , dllSeq, gridView1.FocusedRowHandle + 1);


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
            DllParamDataResponse result = JsonConvert.DeserializeObject<DllParamDataResponse>(responseText);

            return result;

        }

        private void btnAddNewRow_Click(object sender, EventArgs e)
        {
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
        }

        private DllParamDataResponse AddDllParamData(string dataName)
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
            DllParamDataResponse result = JsonConvert.DeserializeObject<DllParamDataResponse>(responseText);

            return result;

        }

        private void GridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            // Set the new row cell value
            view.SetRowCellValue(e.RowHandle, view.Columns["Seq"], "");
            view.SetRowCellValue(e.RowHandle, view.Columns["Data"], "test");
        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private ResponseDLLParamData GetDllParamDataList()
        {
            string url = ConfigurationManager.AppSettings["UrlDLL"];
            string sendData = string.Format(@"
            {{
            ""command"":""data-list"",
            ""dllSeq"":""{0}"",
            ""dllParamSeq"":""{1}""
            }}"
            , dllSeq, dllParamSeq);

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
            DllParamDataListResponse result = JsonConvert.DeserializeObject<DllParamDataListResponse>(responseText);

            return result.response;

        }

    }
}
