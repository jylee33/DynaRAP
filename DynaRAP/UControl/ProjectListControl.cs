using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DynaRAP.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class ProjectListControl : UserControl
    {
        public ProjectListControl()
        {
            InitializeComponent();
        }

        private void ProjectListControl_Load(object sender, EventArgs e)
        {
            InitializeProjectList();

        }

        private void InitializeProjectList()
        {
            //TreeList treeListProject = new TreeList();
            treeListProject.Parent = this;
            treeListProject.Dock = DockStyle.Fill;
            //Specify the fields that arrange underlying data as a hierarchy.
            treeListProject.KeyFieldName = "ID";
            treeListProject.ParentFieldName = "ParentID";
            //Allow the treelist to create columns bound to the fields the KeyFieldName and ParentFieldName properties specify.
            treeListProject.OptionsBehavior.PopulateServiceColumns = true;

            //Specify the data source.
            //treeListProject.DataSource = null;
            treeListProject.DataSource = ProjectDataGenertor.CreateData();
            //The treelist automatically creates columns for the public fields found in the data source. 
            //You do not need to call the TreeList.PopulateColumns method unless the treeList1.OptionsBehavior.AutoPopulatefColumns option is disabled.

            treeListProject.RowHeight = 23;
            treeListProject.OptionsView.ShowColumns = false;
            treeListProject.OptionsView.ShowHorzLines = false;
            treeListProject.OptionsView.ShowVertLines = false;
            treeListProject.OptionsView.ShowIndicator = false;
            treeListProject.OptionsView.ShowTreeLines = DevExpress.Utils.DefaultBoolean.False;
            treeListProject.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            treeListProject.OptionsView.ShowSummaryFooter = false;
            treeListProject.OptionsView.AutoWidth = false;

            treeListProject.OptionsFilter.AllowFilterEditor = false;

            //Access the automatically created columns.
            TreeListColumn colProjectName = treeListProject.Columns["ProjectName"];
            TreeListColumn colLink = treeListProject.Columns["Link"];

            //Hide the key columns. An end-user can access them from the Customization Form.
            treeListProject.Columns[treeListProject.KeyFieldName].Visible = false;
            treeListProject.Columns[treeListProject.ParentFieldName].Visible = false;

            //Make the Project column read-only.
            colProjectName.OptionsColumn.ReadOnly = true;
            colLink.OptionsColumn.ReadOnly = true;

            colProjectName.OptionsColumn.AllowEdit = false;

            //Sort data against the Project column
            colProjectName.SortIndex = -1;// 0;

            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;

            treeListProject.ExpandAll();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate
            {
                treeListProject.BestFitColumns();
            }));
        }

        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            RepositoryItemImageComboBox combo = sender as RepositoryItemImageComboBox;

        }
    }

    

}
