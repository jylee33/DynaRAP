using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class CsvTableControl : DevExpress.XtraEditors.XtraUserControl
    {
        string csvFilePath = string.Empty;

        public string CsvFilePath
        {
            set { csvFilePath = value; }
        }

        public CsvTableControl()
        {
            InitializeComponent();
        }

        public CsvTableControl(string csvFilePath) : this()
        {
            this.csvFilePath = csvFilePath;
        }

        private void CsvTableControl_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        public void FillGrid()
        {
            if (File.Exists(csvFilePath) == false)
            {
                MessageBox.Show(Properties.Resources.FileNotExist);
                return;
            }

            DataTable dt = LoadCSV(this.csvFilePath, true);

            gridView1.Columns.Clear();

            gridControl1.DataSource = null;
            gridControl1.DataSource = dt;

            //gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView1.OptionsView.ShowColumnHeaders = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            //gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ColumnAutoWidth = false;

            //gridView1.OptionsBehavior.ReadOnly = true;
            //gridView1.OptionsBehavior.Editable = false;

            gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            //gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            //gridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
            //gridView1.OptionsSelection.EnableAppearanceHideSelection = false;

            //gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            //gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

            //GridColumn colType = gridView1.Columns[0];
            //colType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            //colType.OptionsColumn.FixedWidth = true;
            //colType.Width = 120;
        }

        private DataTable LoadCSV(string path, bool hasHeader)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(path))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }


            return dt;
        }
    }
}
