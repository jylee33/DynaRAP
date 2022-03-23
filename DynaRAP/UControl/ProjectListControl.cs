using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using System;
using System.Collections.Generic;
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

            //Sort data against the Project column
            colProjectName.SortIndex = -1;// 0;

            treeListProject.ExpandAll();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate
            {
                treeListProject.BestFitColumns();
            }));
        }


    }

    public class ProjectData
    {
        static int UniqueID = 4;
        public ProjectData()
        {
            ID = UniqueID++;
        }
        public ProjectData(int id, int parentId, string projectName, decimal link)
        {
            ID = id;
            ParentID = parentId;
            ProjectName = projectName;
            Link = link;
        }
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string ProjectName { get; set; }
        public decimal Link { get; set; }
    }

    public class ProjectDataGenertor
    {
        public static List<ProjectData> CreateData()
        {
            List<ProjectData> sales = new List<ProjectData>();
            sales.Add(new ProjectData(0, -1, "Project Name", 30540));
            sales.Add(new ProjectData(1, 0, "비행데이터", 22000));
            sales.Add(new ProjectData(2, 0, "버펫팅데이터", 22000));
            sales.Add(new ProjectData(3, 0, "Short Block", 22000));

            ProjectData data = new ProjectData();
            data.ParentID = 1;
            data.ProjectName = "2022-03-03_형상A_1호기.bin";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 1;
            data.ProjectName = "2022-03-03_형상B_3호기.bin";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 1;
            data.ProjectName = "2022-03-03_형상A_2호기.bin";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 2;
            data.ProjectName = "버펫팅_01.bpt";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 2;
            data.ProjectName = "버펫팅_02.bpt";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 2;
            data.ProjectName = "버펫팅_03.bpt";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 2;
            data.ProjectName = "버펫팅_04.bpt";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_01.sbl";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_02.sbl";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_03.sbl";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_04.sbl";
            data.Link = 10000;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_05.sbl";
            data.Link = 10000;
            sales.Add(data);

            return sales;
        }
    }

}
