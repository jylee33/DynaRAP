using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DynaRAP.TEST;
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
    public partial class InitialViewControl : DevExpress.XtraEditors.XtraUserControl
    {
        public InitialViewControl()
        {
            InitializeComponent();
        }

        private void InitialViewControl_Load(object sender, EventArgs e)
        {
            InitializeRecentProjectList();
        }

        private void InitializeRecentProjectList()
        {
            List<RecentProject> list = new List<RecentProject>();
            DateTime dtNow = DateTime.Now;
            string strNow = string.Format("{0:yyyy/MM/dd HH:mm:ss}", dtNow);

            list.Add(new RecentProject(strNow, "Scenario Name 1 Scenario Name 1 Scenario Name 1 Scenario Name 1"));
            list.Add(new RecentProject(strNow, "Scenario Name 2"));
            list.Add(new RecentProject(strNow, "Scenario Name 3"));

            this.gridControl1.DataSource = list;

            gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView1.OptionsView.ShowColumnHeaders = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ColumnAutoWidth = false;

            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

            GridColumn colDate = gridView1.Columns["Date"];
            GridColumn colProjectName = gridView1.Columns["ProjectName"];
            colDate.OptionsColumn.FixedWidth = true;
            colProjectName.OptionsColumn.FixedWidth = true;
            colDate.Width = 130;
            colProjectName.Width = 250;

        }

        private void hyperlinkLabelControl1_Click(object sender, EventArgs e)
        {
            TreeTestForm form = new TreeTestForm();
            form.Show();
        }
    }

    public class RecentProject
    {
        public RecentProject()
        {
        }
        public RecentProject(string date, string projectName)
        {
            Date = date;
            ProjectName = projectName;
        }
        public string Date { get; set; }
        public string ProjectName { get; set; }
    }

    
}
