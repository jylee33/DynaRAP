using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DynaRAP.Data;
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
    public partial class StartScreenControl : DevExpress.XtraEditors.XtraUserControl
    {
        public StartScreenControl()
        {
            InitializeComponent();
        }

        private void StartScreenControl_Load(object sender, EventArgs e)
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

        private void lblNewScenario_Click(object sender, EventArgs e)
        {
            NewSenarioForm form = new NewSenarioForm();
            form.ShowDialog();
        }

        private void lblOpenScenario_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            //mainForm.PanelScenario.Show();
            //mainForm.TabbedView1.RemoveDocument(mainForm.StartControl);
            //mainForm.TabbedView1.RemoveDocument(mainForm.ImportModuleControl);
            //mainForm.TabbedView1.RemoveDocument(mainForm.SbModuleControl);
            //mainForm.TabbedView1.RemoveDocument(mainForm.BinModuleControl);
        }
    }

    

    
}
