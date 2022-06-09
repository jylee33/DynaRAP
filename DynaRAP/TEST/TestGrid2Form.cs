using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class TestGrid2Form : DevExpress.XtraEditors.XtraForm
    {
        public TestGrid2Form()
        {
            InitializeComponent();
        }

        private void TestGrid2Form_Load(object sender, EventArgs e)
        {
            InitializeSBParamList();
        }

        private void InitializeSBParamList()
        {
            List<TestData> list = new List<TestData>();

            repositoryItemComboBox1.TextEditStyle = TextEditStyles.DisableTextEditor;
            repositoryItemComboBox1.Items.Add("MACH");
            repositoryItemComboBox1.Items.Add("동압");
            repositoryItemComboBox1.Items.Add("고도");
            repositoryItemComboBox1.Items.Add("AOA");
            repositoryItemComboBox1.Items.Add("AOS");
            repositoryItemComboBox1.Items.Add("NZ");
            repositoryItemComboBox1.Items.Add("ROLL RATE");
            repositoryItemComboBox1.Items.Add("PITCH RATE");

            repositoryItemComboBox2.TextEditStyle = TextEditStyles.DisableTextEditor;
            repositoryItemComboBox2.Items.Add("SW903_NM RATE");
            repositoryItemComboBox2.Items.Add("SW904_NM RATE");
            repositoryItemComboBox2.Items.Add("SW905_NM RATE");
            repositoryItemComboBox2.Items.Add("SW906_NM RATE");

            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("동압", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("고도", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("AOA", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 1));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 0));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 0));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 0));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 0));
            list.Add(new TestData("MACH", "SW903_NM", 0, 0, 0, 1));

            this.gridControl1.DataSource = list;

            gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView1.OptionsView.ShowColumnHeaders = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = true;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ColumnAutoWidth = true;

            gridView1.OptionsBehavior.ReadOnly = false;
            //gridView1.OptionsBehavior.Editable = false;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

            GridColumn colType = gridView1.Columns["ParameterType"];
            colType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colType.OptionsColumn.FixedWidth = true;
            colType.Width = 120;
            colType.Caption = "파라미터 구분";

            GridColumn colName = gridView1.Columns["ParameterName"];
            colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colName.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colName.OptionsColumn.FixedWidth = true;
            colName.Width = 150;
            colName.Caption = "파라미터 이름";

            GridColumn colMin = gridView1.Columns["Min"];
            colMin.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colMin.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colMin.OptionsColumn.FixedWidth = true;
            colMin.Width = 60;
            colMin.Caption = "MIN";
            colMin.OptionsColumn.ReadOnly = true;

            GridColumn colMax = gridView1.Columns["Max"];
            colMax.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colMax.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colMax.OptionsColumn.FixedWidth = true;
            colMax.Width = 60;
            colMax.Caption = "MAX";
            colMax.OptionsColumn.ReadOnly = true;

            GridColumn colAvg = gridView1.Columns["Avg"];
            colAvg.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colAvg.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colAvg.OptionsColumn.FixedWidth = true;
            colAvg.Width = 60;
            colAvg.Caption = "AVG";
            colAvg.OptionsColumn.ReadOnly = true;

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
            RepositoryItemImageComboBox combo = sender as RepositoryItemImageComboBox;
        }
    }
}