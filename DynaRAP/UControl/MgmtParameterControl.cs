using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DynaRAP.Data;
using DynaRAP.UTIL;
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
    public partial class MgmtParameterControl : DevExpress.XtraEditors.XtraUserControl
    {
        public MgmtParameterControl()
        {
            InitializeComponent();
        }

        private void MgmtParameterControl_Load(object sender, EventArgs e)
        {
            InitializeFlyingDataList();
        }

        private void InitializeFlyingDataList()
        {
            //TreeList treeList1 = new TreeList();
            treeList1.Parent = this.splitContainer1.Panel1;
            treeList1.Dock = DockStyle.Fill;
            //Specify the fields that arrange underlying data as a hierarchy.
            treeList1.KeyFieldName = "ID";
            treeList1.ParentFieldName = "ParentID";
            //Allow the treelist to create columns bound to the fields the KeyFieldName and ParentFieldName properties specify.
            treeList1.OptionsBehavior.PopulateServiceColumns = true;

            //Specify the data source.
            //treeList1.DataSource = null;
            treeList1.DataSource = CreateData();
            //The treelist automatically creates columns for the public fields found in the data source. 
            //You do not need to call the TreeList.PopulateColumns method unless the treeList1.OptionsBehavior.AutoPopulatefColumns option is disabled.
            treeList1.ForceInitialize();

            treeList1.RowHeight = 23;
            treeList1.OptionsView.ShowColumns = false;
            treeList1.OptionsView.ShowHorzLines = false;
            treeList1.OptionsView.ShowVertLines = false;
            treeList1.OptionsView.ShowIndicator = false;
            treeList1.OptionsView.ShowTreeLines = DevExpress.Utils.DefaultBoolean.False;
            treeList1.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            treeList1.OptionsView.ShowSummaryFooter = false;
            treeList1.OptionsView.AutoWidth = false;

            treeList1.OptionsFilter.AllowFilterEditor = false;

            treeList1.OptionsSelection.MultiSelect = false;

            //Access the automatically created columns.
            TreeListColumn colName = treeList1.Columns["FlyingName"];

            //Hide the key columns. An end-user can access them from the Customization Form.
            treeList1.Columns[treeList1.KeyFieldName].Visible = false;
            treeList1.Columns[treeList1.ParentFieldName].Visible = false;

            //Make the Project column read-only.
            colName.OptionsColumn.ReadOnly = true;
            colName.OptionsColumn.AllowEdit = false;

            //Sort data against the Project column
            colName.SortIndex = -1;// 0;

            //treeList1.OptionsView.ShowCheckBoxes = true; // 제일 앞에 checkBox 붙이는 옵션
            treeList1.PopupMenuShowing += TreeList1_PopupMenuShowing;

            treeList1.ExpandAll();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate
            {
                treeList1.BestFitColumns();
            }));



        }

        private void TreeList1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            // Check if a node's indicator cell is clicked.
            TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(e.Point);
            TreeListNode node = null;
            //if (hitInfo.HitInfoType == HitInfoType.RowIndicator)
            {
                node = hitInfo.Node;
            }
            if (node == null) return;

            // Create the Add Node command.
            DXMenuItem menuItemAdd = new DXMenuItem("Add Node", this.addNodeMenuItemClick);
            menuItemAdd.Tag = node;
            e.Menu.Items.Add(menuItemAdd);

            // Create the Delete Node command.
            DXMenuItem menuItemDelete = new DXMenuItem("Delete Node", this.deleteNodeMenuItemClick);
            menuItemDelete.Tag = node;
            e.Menu.Items.Add(menuItemDelete);
        }

        private void addNodeMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            TreeListNode node = item.Tag as TreeListNode;
            if (node == null) return;

            string flyingName = Prompt.ShowDialog("Parameter Name", "Add Parameter");

            TreeListNode newNode = treeList1.AppendNode(new object[] { "" }, node);
            newNode.SetValue("FlyingName", flyingName);

            //treeList1.ExpandAll();
            node.Expand();
            //treeList1.Selection.Set(newNode);
            treeList1.FocusedNode = newNode;
        }

        private void deleteNodeMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            TreeListNode node = item.Tag as TreeListNode;
            if (node == null) return;
            node.TreeList.DeleteNode(node);
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            treeList1.PostEditor();
        }

        private BindingList<FlyingData> CreateData()
        {
            BindingList<FlyingData> list = new BindingList<FlyingData>();
            //list.Add(new FlyingData(0, -1, "ImportModuleScenarioName", null));
            list.Add(new FlyingData(1, 0, "비행데이터", null));
            list.Add(new FlyingData(2, 0, "버펫팅데이터", null));
            list.Add(new FlyingData(3, 0, "Short Block", null));

            FlyingData data = new FlyingData();
            data.ParentID = 1;
            data.FlyingName = "2022-03-03_형상A_1호기.bin";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 1;
            data.FlyingName = "2022-03-03_형상B_3호기.bin";
            data.Check = false;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 1;
            data.FlyingName = "2022-03-03_형상A_2호기.bin";
            data.Check = false;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 2;
            data.FlyingName = "버펫팅_01.bpt";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 2;
            data.FlyingName = "버펫팅_02.bpt";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 2;
            data.FlyingName = "버펫팅_03.bpt";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 2;
            data.FlyingName = "버펫팅_04.bpt";
            data.Check = false;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_01.sbl";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_02.sbl";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_03.sbl";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_04.sbl";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_05.sbl";
            data.Check = false;
            list.Add(data);

            int pid = data.ID;

            data = new FlyingData();
            data.ParentID = pid;
            data.FlyingName = "ShortBlock_06.sbl";
            data.Check = false;
            list.Add(data);

            pid = data.ID;

            data = new FlyingData();
            data.ParentID = pid;
            data.FlyingName = "ShortBlock_07.sbl";
            data.Check = false;
            list.Add(data);

            return list;
        }

    }
}
