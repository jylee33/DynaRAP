using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
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
            list.Add(new RecentProject(DateTime.Now, "Scenario Name 1"));
            list.Add(new RecentProject(DateTime.Now, "Scenario Name 2"));
            list.Add(new RecentProject(DateTime.Now, "Scenario Name 3"));

            this.gridControl1.DataSource = list;

            gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView1.OptionsView.ShowColumnHeaders = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;

            gridView1.OptionsBehavior.ReadOnly = true;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;

            GridColumn dateCol = gridView1.Columns["Date"];
            dateCol.Width = 20;

        }
    }

    public class RecentProject
    {
        public RecentProject()
        {
        }
        public RecentProject(DateTime date, string projectName)
        {
            Date = date;
            ProjectName = projectName;
        }
        public DateTime Date { get; set; }
        public string ProjectName { get; set; }
    }

    
}
