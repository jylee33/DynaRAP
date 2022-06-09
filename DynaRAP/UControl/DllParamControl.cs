using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
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

namespace DynaRAP.UControl
{
    public partial class DllParamControl : DevExpress.XtraEditors.XtraUserControl
    {
        string dllSeq = string.Empty;
        string dllParamSeq = string.Empty;

        List<ResponseDLLParamData> dllParamDataList = new List<ResponseDLLParamData>();
        List<DllParamData> dllDataGridList = new List<DllParamData>();

        public DllParamControl()
        {
            InitializeComponent();
        }

        public DllParamControl(string dllSeq, string dllParamSeq) : this()
        {
            this.dllSeq = dllSeq;
            this.dllParamSeq = dllParamSeq;
        }

        private void DllParamControl_Load(object sender, EventArgs e)
        {
            InitializeGridControl1();
        }

        private void InitializeGridControl1()
        {

            //gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            dllParamDataList = GetDllParamDataList();

            dllDataGridList.Clear();
            foreach (ResponseDLLParamData data in dllParamDataList)
            {
                dllDataGridList.Add(new DllParamData(data.seq, data.paramVal.ToString()));
            }
            gridControl1.DataSource = dllDataGridList;

            //gridControl1.UseEmbeddedNavigator = true;
            gridControl1.PreviewKeyDown += GridControl1_PreviewKeyDown;

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
            gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.True;

            GridColumn colData = gridView1.Columns["Data"];
            colData.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            //colData.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colData.OptionsColumn.FixedWidth = true;
            //colData.Width = 240;
            //colData.Caption = "UnmappedParamName";
            //colData.OptionsColumn.ReadOnly = true;

            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
            //gridView1.InitNewRow += GridView1_InitNewRow;

        }

        private void btnAddNewRow_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();
        }

        private void GridControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter && gridView1.ActiveEditor != null && gridView1.FocusedRowHandle == GridControl.NewItemRowHandle)
            //{
            //    gridView1..CommitEditing();
            //}
        }

        private void GridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            // Set the new row cell value
            view.SetRowCellValue(e.RowHandle, view.Columns["Seq"], "");
            view.SetRowCellValue(e.RowHandle, view.Columns["Data"], "asdfasdf");
        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private List<ResponseDLLParamData> GetDllParamDataList()
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
