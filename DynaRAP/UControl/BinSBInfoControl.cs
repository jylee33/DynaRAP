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
    public partial class BinSBInfoControl : DevExpress.XtraEditors.XtraUserControl
    {
        string shortBlockSeq = null;
        public BinSBInfoControl(string shortBlockSeq)
        {
            this.shortBlockSeq = shortBlockSeq;
            InitializeComponent();
        }

        private void BinSBInfoControl_Load(object sender, EventArgs e)
        {
            InitializeSBInfo();
            InitializeSBParamList();
            InitializeSBResult();
            InitializeSBData();
        }

        private void InitializeSBInfo()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ColumnKey");
            dt.Columns.Add("ColumnValue");

            dt.Rows.Add("비행데이터", "2022-03-03_형상A_1호기.bin");
            dt.Rows.Add("비행분할데이터", "버펫팅_02.bpt");

            dt.AcceptChanges();

            gridControl1.DataSource = dt;

            //gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView1.OptionsView.ShowColumnHeaders = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            //gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
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

            GridColumn colType = gridView1.Columns["ColumnKey"];
            colType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colType.OptionsColumn.FixedWidth = true;
            colType.Width = 120;

        }

        private void InitializeSBParamList()
        {
            List<SBParameter> SBParameterList = new List<SBParameter>();
            List<SBResult> SBResultList = new List<SBResult>();


            string sendData = string.Format(@"
            {{
            ""command"":""param-list"",
            ""blockSeq"":""{0}""
            }}", shortBlockSeq);
            string responseData = Utils.GetPostData(System.Configuration.ConfigurationManager.AppSettings["UrlShortBlock"], sendData);
            if (responseData != null)
            {
                ResponseParamList responseParam = JsonConvert.DeserializeObject<ResponseParamList>(responseData);
                if (responseParam.code == 200)
                {
                    foreach(var paramList in responseParam.response.paramData)
                    {
                        SBParameterList.Add(new SBParameter( (paramList.propInfo==null? "": paramList.propInfo.propType) ,paramList.paramKey, paramList.paramValueMap.blockMin, paramList.paramValueMap.blockMax, paramList.paramValueMap.blockMax));
                        SBResultList.Add(new SBResult(paramList.paramValueMap.psd, paramList.paramValueMap.rms, paramList.paramValueMap.n0, paramList.paramValueMap.zPeak, paramList.paramValueMap.zValley));

                    }
                }
            }


            this.gridControl2.DataSource = SBParameterList;

            //gridView2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView2.OptionsView.ShowColumnHeaders = true;
            gridView2.OptionsView.ShowGroupPanel = false;
            gridView2.OptionsView.ShowIndicator = false;
            //gridView2.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView2.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView2.OptionsView.ColumnAutoWidth = true;

            gridView2.OptionsBehavior.ReadOnly = true;
            gridView2.OptionsBehavior.Editable = false;

            gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView2.OptionsSelection.EnableAppearanceFocusedRow = false;
            gridView2.OptionsSelection.EnableAppearanceHideSelection = false;

            gridView2.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;

            GridColumn colType = gridView2.Columns["ParameterType"];
            colType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colType.OptionsColumn.FixedWidth = true;
            colType.Width = 120;
            colType.Caption = "구분";
            colType.OptionsColumn.ReadOnly = true;

            GridColumn colName = gridView2.Columns["ParameterName"];
            colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colName.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colName.OptionsColumn.FixedWidth = true;
            colName.Width = 150;
            colName.Caption = "파라미터 이름";
            colName.OptionsColumn.ReadOnly = true;

            GridColumn colMin = gridView2.Columns["Min"];
            colMin.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colMin.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colMin.OptionsColumn.FixedWidth = true;
            colMin.Width = 60;
            colMin.Caption = "MIN";
            colMin.OptionsColumn.ReadOnly = true;

            GridColumn colMax = gridView2.Columns["Max"];
            colMax.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colMax.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colMax.OptionsColumn.FixedWidth = true;
            colMax.Width = 60;
            colMax.Caption = "MAX";
            colMax.OptionsColumn.ReadOnly = true;

            GridColumn colAvg = gridView2.Columns["Avg"];
            colAvg.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colAvg.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colAvg.OptionsColumn.FixedWidth = true;
            colAvg.Width = 60;
            colAvg.Caption = "AVG";
            colAvg.OptionsColumn.ReadOnly = true;


            this.gridControl3.DataSource = SBResultList;

            //gridView3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView3.OptionsView.ShowColumnHeaders = true;
            gridView3.OptionsView.ShowGroupPanel = false;
            gridView3.OptionsView.ShowIndicator = false;
            //gridView3.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView3.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView3.OptionsView.ColumnAutoWidth = true;

            gridView3.OptionsBehavior.ReadOnly = true;
            gridView3.OptionsBehavior.Editable = false;

            gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView3.OptionsSelection.EnableAppearanceFocusedRow = false;
            gridView3.OptionsSelection.EnableAppearanceHideSelection = false;

            gridView3.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            gridView3.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;

            gridView3.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;

            GridColumn colPsd = gridView3.Columns["Psd"];
            colPsd.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colPsd.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colPsd.OptionsColumn.FixedWidth = true;
            //colPsd.Width = 120;
            colPsd.Caption = "PSD";
            colPsd.OptionsColumn.ReadOnly = true;

            GridColumn colRms = gridView3.Columns["Rms"];
            colRms.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colRms.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colRms.OptionsColumn.FixedWidth = true;
            //colRms.Width = 150;
            colRms.Caption = "RMS";
            colRms.OptionsColumn.ReadOnly = true;

            GridColumn colN0 = gridView3.Columns["N0"];
            colN0.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colN0.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colN0.OptionsColumn.FixedWidth = true;
            //colN0.Width = 60;
            colN0.Caption = "N0";
            colN0.OptionsColumn.ReadOnly = true;

            GridColumn colRmsToPeek = gridView3.Columns["RmsToPeek"];
            colRmsToPeek.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colRmsToPeek.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colRmsToPeek.OptionsColumn.FixedWidth = true;
            //colRmsToPeek.Width = 60;
            colRmsToPeek.Caption = "RMS-to-Peek";
            colRmsToPeek.OptionsColumn.ReadOnly = true;

            GridColumn colResultValue = gridView3.Columns["ResultValue"];
            colResultValue.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colResultValue.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //colResultValue.OptionsColumn.FixedWidth = true;
            //colResultValue.Width = 60;
            colResultValue.Caption = "ResultValue";
            colResultValue.OptionsColumn.ReadOnly = true;
        }

        private void InitializeSBResult()
        {

           


        }

        private void InitializeSBData()
        {
            List<SBParameter> list = new List<SBParameter>();

            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("동압", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("고도", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("AOA", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));
            list.Add(new SBParameter("MACH", "SW903_NM", 0, 0, 0));

            this.gridControl4.DataSource = list;

            //gridView4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView4.OptionsView.ShowColumnHeaders = true;
            gridView4.OptionsView.ShowGroupPanel = false;
            gridView4.OptionsView.ShowIndicator = false;
            //gridView4.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView4.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView4.OptionsView.ColumnAutoWidth = true;

            gridView4.OptionsBehavior.ReadOnly = true;
            gridView4.OptionsBehavior.Editable = false;

            gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView4.OptionsSelection.EnableAppearanceFocusedRow = false;
            gridView4.OptionsSelection.EnableAppearanceHideSelection = false;

            gridView4.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;

            GridColumn colType = gridView4.Columns["ParameterType"];
            colType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colType.OptionsColumn.FixedWidth = true;
            colType.Width = 120;
            colType.Caption = "파라미터 구분";
            colType.OptionsColumn.ReadOnly = true;

            GridColumn colName = gridView4.Columns["ParameterName"];
            colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colName.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colName.OptionsColumn.FixedWidth = true;
            colName.Width = 150;
            colName.Caption = "파라미터 이름";
            colName.OptionsColumn.ReadOnly = true;

            GridColumn colMin = gridView4.Columns["Min"];
            colMin.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colMin.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colMin.OptionsColumn.FixedWidth = true;
            colMin.Width = 60;
            colMin.Caption = "MIN";
            colMin.OptionsColumn.ReadOnly = true;

            GridColumn colMax = gridView4.Columns["Max"];
            colMax.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colMax.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colMax.OptionsColumn.FixedWidth = true;
            colMax.Width = 60;
            colMax.Caption = "MAX";
            colMax.OptionsColumn.ReadOnly = true;

            GridColumn colAvg = gridView4.Columns["Avg"];
            colAvg.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colAvg.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colAvg.OptionsColumn.FixedWidth = true;
            colAvg.Width = 60;
            colAvg.Caption = "AVG";
            colAvg.OptionsColumn.ReadOnly = true;


        }

    }
}
