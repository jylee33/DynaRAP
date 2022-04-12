using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
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

namespace DynaRAP.UControl
{
    public partial class BinTableControl : DevExpress.XtraEditors.XtraUserControl
    {
        DockPanel binSBTabPanel = null;
        BinSBTabControl binSBTabCtrl = null;

        public BinTableControl()
        {
            InitializeComponent();
        }

        private void BinTableControl_Load(object sender, EventArgs e)
        {
            DataTable dt = GetDataTable();
            AddTabPage("AOA-Q", dt);
            AddTabPage("AOA-AOS", dt);
            AddTabPage("Q-AOS", dt);
        }

        private DataTable GetDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("AOA");
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Columns.Add("Column3");
            dt.Columns.Add("Column4");
            dt.Columns.Add("Column5");
            dt.Columns.Add("Column6");
            dt.Columns.Add("Column7");
            dt.Columns.Add("Column8");
            dt.Columns.Add("Column9");
            dt.Columns.Add("Column10");
            dt.Columns.Add("Column11");
            dt.Columns.Add("Column12");
            dt.Columns.Add("Column13");
            dt.Columns.Add("Column14");
            dt.Columns.Add("Column15");
            dt.Columns.Add("Column16");
            dt.Columns.Add("Column17");
            dt.Columns.Add("Column18");
            dt.Columns.Add("Column19");
            dt.Columns.Add("Column20");

            dt.Rows.Add(10, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            dt.Rows.Add(11, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            dt.Rows.Add(12, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            dt.Rows.Add(13, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            dt.Rows.Add(14, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            dt.Rows.Add(15, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            dt.Rows.Add(16, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            dt.Rows.Add(17, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            dt.Rows.Add(18, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);

            dt.AcceptChanges();

            return dt;
        }

        private void AddTabPage(string tabName, DataTable dt)
        {
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = tabName;
            tabPage.Text = tabName;

            GridControl gridControl = new GridControl();
            gridControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(gridControl);

            BandedGridView bandedGridView = new BandedGridView();

            gridControl.ViewCollection.Add(bandedGridView);

            gridControl.MainView = bandedGridView;
            bandedGridView.GridControl = gridControl;
            bandedGridView.Name = "gridView1";

            GridBand gridBand1 = new GridBand();
            //GridBand gridBand2 = new GridBand();

            bandedGridView.Bands.Clear();
            bandedGridView.Bands.Add(gridBand1);
            //bandedGridView.Bands.Add(gridBand2);

            //bandedGridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            bandedGridView.OptionsView.ShowColumnHeaders = true;
            bandedGridView.OptionsView.ShowGroupPanel = false;
            bandedGridView.OptionsView.ShowIndicator = false;
            //bandedGridView.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //bandedGridView.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            bandedGridView.OptionsView.ColumnAutoWidth = false;

            bandedGridView.OptionsBehavior.ReadOnly = true;
            bandedGridView.OptionsBehavior.Editable = false;

            //bandedGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            //bandedGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            bandedGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
            
            bandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandedGridView.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            // 아래처럼 LookAndFeel 제거를 해야 원하는 컬러로 세팅이 가능하다.
            //gridControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            //gridControl.LookAndFeel.UseDefaultLookAndFeel = false;
            //bandedGridView.Appearance.HeaderPanel.Options.UseBackColor = true;
            //bandedGridView.Appearance.HeaderPanel.BackColor = Color.Gray;

            bandedGridView.RowCellStyle += BandedGridView_RowCellStyle;
            bandedGridView.RowCellClick += BandedGridView_RowCellClick;

            //bandedGridView.ColumnPanelRowHeight = 40;
            //bandedGridView.IndicatorWidth = 100;

            gridControl.DataSource = dt;

            gridBand1.Caption = "AOS";
            gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gridBand1.AppearanceHeader.BackColor = Color.Gray;
            //gridBand1.Columns.Clear();

            BandedGridColumn colIndex = bandedGridView.Columns["AOA"];
            colIndex.OptionsColumn.FixedWidth = true;
            colIndex.Width = 80;
            //gridBand1.Columns.Add(colIndex);
            //colIndex.AppearanceHeader.Options.UseBackColor = true;
            //colIndex.AppearanceHeader.BackColor = Color.Gray;
            //colIndex.AppearanceCell.BackColor = Color.Gray;
            //colIndex.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //colIndex.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            colIndex.OptionsColumn.AllowFocus = false;


            //gridBand2.Caption = "AOS";
            //gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gridBand2.Columns.Clear();
            //gridBand2.Columns.Add(bandedGridView.Columns["Column1"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column2"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column3"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column4"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column5"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column6"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column7"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column8"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column9"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column10"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column11"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column12"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column13"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column14"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column15"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column16"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column17"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column18"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column19"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column20"]);


        }

        private void BandedGridView_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            BandedGridView gridView = sender as BandedGridView;

            string index = gridView.GetRowCellValue(e.RowHandle, "AOA").ToString();
            Console.WriteLine("selected " + index);

            MainForm mainForm = this.ParentForm as MainForm;

            // panel 추가
            if (binSBTabPanel == null)
            {
                binSBTabPanel = mainForm.DockManager1.AddPanel(DockingStyle.Float);
                binSBTabPanel.FloatLocation = new Point(500, 100);
                binSBTabPanel.FloatSize = new Size(466, 620);
                binSBTabPanel.Name = "ShortBlock Panel";
                binSBTabPanel.Text = "ShortBlock Panel";
                binSBTabCtrl = new BinSBTabControl();
                binSBTabCtrl.IdxValue = index;
                binSBTabCtrl.Dock = DockStyle.Fill;
                binSBTabPanel.Controls.Add(binSBTabCtrl);
                binSBTabPanel.ClosedPanel += BinSBTabPanel_ClosedPanel;
            }
            else
            {
                binSBTabCtrl.IdxValue = index;
                binSBTabPanel.Show();
            }

        }

        private void BinSBTabPanel_ClosedPanel(object sender, DockPanelEventArgs e)
        {
            this.binSBTabPanel = null;
            this.binSBTabCtrl = null;
        }

        private void BandedGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            BandedGridView gridView = sender as BandedGridView;
            //e.Appearance.BackColor = Color.Black;

            if (e.Column.FieldName == "Customer")
            {
                bool value = Convert.ToBoolean(gridView.GetRowCellValue(e.RowHandle, "Flag_Customer"));
                if (value)
                    e.Appearance.BackColor = Color.Red;
            }
            if (e.Column.FieldName == "Vendor")
            {
                bool value = Convert.ToBoolean(gridView.GetRowCellValue(e.RowHandle, "Flat_Vendor"));
                if (value)
                    e.Appearance.BackColor = Color.Red;
            }
        }
    }
}
