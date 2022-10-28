using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DynaRAP.Data;
using DynaRAP.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class BinSBSummary : DevExpress.XtraEditors.XtraUserControl
    {
        string paramSeq = null;
        string shortBlockSeq = null;
        string partSeq = null;
        List<string> paramNameList = null;
        string binTableName = string.Empty;
        string binMetaSeq = string.Empty;
        List<BINSummaryList> psdList = new List<BINSummaryList>();
        List<BINSummary> rmsList = new List<BINSummary>();
        List<BINSummary> rmsToPeakList = new List<BINSummary>();
        SummaryData selectSummaryData = null;

        public BinSBSummary(SummaryData selectSummaryData, string binTableName, string binMetaSeq)
        {
            this.binTableName = binTableName;
            this.binMetaSeq = binMetaSeq;
            this.selectSummaryData = selectSummaryData;
            InitializeComponent();
        }

        private void BinSBSummary_Load(object sender, EventArgs e)
        {
            binNameLabel.Text = binTableName;
            if (selectSummaryData != null)
            {
                maxRMStoPeakLabel.Text = selectSummaryData.maxRmsToPeak.ToString();
            }
        }

        private void InitializeBInMinMax()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MIN");
            dt.Columns.Add("MAX");
            dt.Columns.Add("AVG");
            DataRow dataRow = dt.NewRow();
            dataRow["MIN"] = selectSummaryData.min;
            dataRow["MAX"] = selectSummaryData.max;
            dataRow["AVG"] = selectSummaryData.avg;
            dt.Rows.Add(dataRow);
            //string partName = null;
            //string updateName = null;
            //string sendData = string.Format(@"
            //    {{
            //    ""command"":""info"",
            //    ""partSeq"":""{0}""
            //    }}", partSeq);
            //string responseData = Utils.GetPostData(System.Configuration.ConfigurationManager.AppSettings["UrlPart"], sendData);
            //if (responseData != null)
            //{
            //    SBPartInfoResponse responseParam = JsonConvert.DeserializeObject<SBPartInfoResponse>(responseData);
            //    if (responseParam.code == 200)
            //    {

            //        byte[] byte64 = Convert.FromBase64String(responseParam.response.partName);
            //        partName = Encoding.UTF8.GetString(byte64);


            //        string uploadSendData = string.Format(@"
            //        {{
            //        ""command"":""progress"",
            //        ""uploadSeq"":""{0}""
            //        }}", responseParam.response.uploadSeq);
            //        string responseText = Utils.GetPostData(System.Configuration.ConfigurationManager.AppSettings["UrlImport"], uploadSendData);
            //        if (responseText != null)
            //        {
            //            ImportResponse result = JsonConvert.DeserializeObject<ImportResponse>(responseText);
            //            if (result.code == 200)
            //            {
            //                byte[] updateByte64 = Convert.FromBase64String(result.response.uploadName);
            //                updateName = Encoding.UTF8.GetString(updateByte64);
            //            }
            //        }


            //    }
            //}

            //dt.Rows.Add("비행데이터", updateName);
            //dt.Rows.Add("비행분할데이터", partName) ;

            dt.AcceptChanges();

            gridControl1.DataSource = dt;

            gridView1.OptionsView.ShowColumnHeaders = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            gridView1.OptionsView.ColumnAutoWidth = true;

            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;

            gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
            gridView1.OptionsSelection.EnableAppearanceHideSelection = false;

            gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

        }

        private void InitializePSD()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Frequency");
            dt.Columns.Add("PSD");
            foreach (var keyValue in psdList)
            {
                if (dt.Columns.Contains(keyValue.key))
                {
                    dt.Columns.Add(keyValue.key+ "_"+1);
                    keyValue.key = keyValue.key + "_" + 1;
                }
                dt.Columns.Add(keyValue.key);
            }

            if (psdList.Count != 0)
            {
                for (int i = 0; i < psdList[0].valueList.Count; i++)
                {
                    DataRow dataRow = dt.NewRow();
                    dataRow["Frequency"] = selectSummaryData.frequency[i];
                    dataRow["PSD"] = selectSummaryData.psd[i];
                    for (int j = 0; j < psdList.Count(); j++)
                    {
                        dataRow[psdList[j].key] = psdList[j].valueList[i];
                    }
                    dt.Rows.Add(dataRow);
                }
            }

            dt.AcceptChanges();

            this.gridControl2.DataSource = dt;



            gridView2.OptionsView.ShowColumnHeaders = true;
            gridView2.OptionsView.ShowGroupPanel = false;
            gridView2.OptionsView.ShowIndicator = false;
            //gridView4.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView4.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView2.OptionsView.ColumnAutoWidth = true;

            gridView2.OptionsBehavior.ReadOnly = true;
            gridView2.OptionsBehavior.Editable = false;

            gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView2.OptionsSelection.EnableAppearanceFocusedRow = false;
            gridView2.OptionsSelection.EnableAppearanceHideSelection = false;

            gridView2.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
        }


        private void InitializeRMS()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AVG_RMS");
            foreach(var keyValue in rmsList)
            {
                if (dt.Columns.Contains(keyValue.key))
                {
                    dt.Columns.Add(keyValue.key + "_" + 1);
                    keyValue.key = keyValue.key + "_" + 1;
                }
                dt.Columns.Add(keyValue.key);
            }

            DataRow dataRow = dt.NewRow();
            dataRow["AVG_RMS"] = selectSummaryData.avg_rms;
            for (int i = 0; i < rmsList.Count(); i++)
            {
                dataRow[rmsList[i].key] = rmsList[i].value;
            }
            dt.Rows.Add(dataRow);

            dt.AcceptChanges();

            this.gridControl3.DataSource = dt;



            gridView3.OptionsView.ShowColumnHeaders = true;
            gridView3.OptionsView.ShowGroupPanel = false;
            gridView3.OptionsView.ShowIndicator = false;
            //gridView4.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView4.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView3.OptionsView.ColumnAutoWidth = true;

            gridView3.OptionsBehavior.ReadOnly = true;
            gridView3.OptionsBehavior.Editable = false;

            gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView3.OptionsSelection.EnableAppearanceFocusedRow = false;
            gridView3.OptionsSelection.EnableAppearanceHideSelection = false;

            gridView3.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
        }


        private void InitializeZeta()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Burst Factor");

            foreach (var keyValue in rmsList)
            {
                if (dt.Columns.Contains(keyValue.key))
                {
                    dt.Columns.Add(keyValue.key + "_" + 1);
                    keyValue.key = keyValue.key + "_" + 1;
                }
                dt.Columns.Add(keyValue.key);
            }


            //for (int i=0; i< selectSummaryData.zeta.Count; i++)
            //{
            //    dt.Columns.Add(i.ToString());
            //}

            DataRow dataRow = dt.NewRow();
            dataRow["Burst Factor"] = selectSummaryData.burstFactor;
            for (int i = 0; i < selectSummaryData.zeta.Count; i++)
            {
                dataRow[rmsList[i].key] = selectSummaryData.zeta[i];
            }
            dt.Rows.Add(dataRow);

            dt.AcceptChanges();

            gridControl4.DataSource = dt;
            
            gridView4.OptionsView.ShowColumnHeaders = true ;
            gridView4.OptionsView.ShowGroupPanel = false;
            gridView4.OptionsView.ShowIndicator = false;
            gridView4.OptionsView.ColumnAutoWidth = true;

            gridView4.OptionsBehavior.ReadOnly = true;
            gridView4.OptionsBehavior.Editable = false;

            gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView4.OptionsSelection.EnableAppearanceFocusedRow = false;
            gridView4.OptionsSelection.EnableAppearanceHideSelection = false;

            gridView4.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            gridView4.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;

            gridView4.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;

        }
        private void InitializeRMSToPeak()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RMS-to-Peak");

            foreach (var rmsToPeak in selectSummaryData.rmsToPeak)
            {

                dt.Rows.Add(rmsToPeak);
            }
            
            dt.AcceptChanges();

            this.gridControl5.DataSource = dt;



            gridView5.OptionsView.ShowColumnHeaders = false;
            gridView5.OptionsView.ShowGroupPanel = false;
            gridView5.OptionsView.ShowIndicator = false;
            //gridView4.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView4.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView5.OptionsView.ColumnAutoWidth = true;

            gridView5.OptionsBehavior.ReadOnly = true;
            gridView5.OptionsBehavior.Editable = false;

            gridView5.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView5.OptionsSelection.EnableAppearanceFocusedRow = false;
            gridView5.OptionsSelection.EnableAppearanceHideSelection = false;

            gridView5.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
        }


        public void psdListAdd(BINSummaryList bINSummary)
        {
            this.psdList.Add(bINSummary);
        }
        public void rmsListAdd(BINSummary bINSummary)
        {
            this.rmsList.Add(bINSummary);
        }
        public void rmsToPeakListAdd(BINSummary bINSummary)
        {
            this.rmsToPeakList.Add(bINSummary);
        }

        public void initGridControl()
        {
            if (selectSummaryData != null)
            {
                InitializeBInMinMax();
                InitializePSD();
                InitializeRMS();
                InitializeZeta();
                InitializeRMSToPeak();
            }
        }
    }
}
