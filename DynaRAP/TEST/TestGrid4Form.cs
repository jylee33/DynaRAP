using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
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
    public partial class TestGrid4Form : DevExpress.XtraEditors.XtraForm
    {
        public TestGrid4Form()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();
        }

        private void TestGrid4Form_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = CreateData();
            //gridView1.InitNewRow += gridView1_InitNewRow;

            RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();
            //gridView1.Columns["YN"].ColumnEdit = checkEdit;
        }

        private DataTable CreateData()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("YN", typeof(bool));

            for (int i = 0; i < 5; i++)
                dt.Rows.Add(i, "Name" + i, i % 2 == 0);

            return dt;
        }

        void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            //gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["YN"], true);
        }
    }
}