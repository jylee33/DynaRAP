using DevExpress.XtraEditors;
using DynaRAP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.TEST
{
    public partial class TestGridForm : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt = new DataTable();
       
        public TestGridForm()
        {
            InitializeComponent();
        }

        private void TestGridForm_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("AOA");
            dt.Columns.Add("50-150");
            dt.Columns.Add("150-250");
            dt.Columns.Add("250-350");

            dt.Rows.Add(10, 0, 0, 0);
            dt.Rows.Add(11, 0, 0, 0);
            dt.Rows.Add(12, 0, 0, 0);
            dt.Rows.Add(13, 0, 0, 0);
            dt.Rows.Add(14, 0, 0, 0);
            dt.Rows.Add(15, 0, 0, 0);
            dt.Rows.Add(16, 0, 0, 0);
            dt.Rows.Add(17, 0, 0, 0);
            dt.Rows.Add(18, 0, 0, 0);

            dt.AcceptChanges();
            gridControl1.DataSource = dt;

            //bandedGridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            bandedGridView1.OptionsView.ShowColumnHeaders = true;
            bandedGridView1.OptionsView.ShowGroupPanel = false;
            bandedGridView1.OptionsView.ShowIndicator = false;
            //bandedGridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //bandedGridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            //bandedGridView1.OptionsView.ColumnAutoWidth = false;

            bandedGridView1.OptionsBehavior.ReadOnly = true;
            //bandedGridView1.OptionsBehavior.Editable = false;

            bandedGridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            bandedGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

            bandedGridView1.ColumnPanelRowHeight = 40;
            bandedGridView1.IndicatorWidth = 100;

            gridBand1.Caption = "";
            gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridBand1.Columns.Add(bandedGridView1.Columns["AOA"]);

            gridBand2.Caption = "AOS";
            gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridBand2.Columns.Add(bandedGridView1.Columns["50-150"]);
            gridBand2.Columns.Add(bandedGridView1.Columns["150-250"]);
            gridBand2.Columns.Add(bandedGridView1.Columns["250-350"]);

            bandedGridView1.CustomDrawRowIndicator += bandedGridView1_CustomDrawRowIndicator;
            bandedGridView1.CustomDrawBandHeader += BandedbandedGridView1_CustomDrawBandHeader;
            bandedGridView1.CustomDrawColumnHeader += BandedbandedGridView1_CustomDrawColumnHeader;

        }

        private void BandedbandedGridView1_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
        }

        private void BandedbandedGridView1_CustomDrawBandHeader(object sender, DevExpress.XtraGrid.Views.BandedGrid.BandHeaderCustomDrawEventArgs e)
        {
        }

        private void bandedGridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            //e.Info.Appearance.Font = new Font("Tahoma", 18);
            //e.Info.Appearance.Options.UseFont = true;
            //if(e.RowHandle < 0)
            //{
            //    //e.Info.DisplayText = "AOA";
            //    return;
            //}
            //e.Info.DisplayText = e.RowHandle.ToString();
        }
    }
}