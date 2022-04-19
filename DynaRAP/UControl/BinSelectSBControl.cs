using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
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
    public partial class BinSelectSBControl : DevExpress.XtraEditors.XtraUserControl
    {
        public BinSelectSBControl()
        {
            InitializeComponent();
        }

        private void SelectSBControl_Load(object sender, EventArgs e)
        {
            InitializeFlyingDataList();
        }

        private void InitializeFlyingDataList()
        {
            //TreeList treeList1 = new TreeList();
            treeList1.Parent = this;
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

            //Access the automatically created columns.
            TreeListColumn colName = treeList1.Columns["FlyingName"];
            TreeListColumn colCheck = treeList1.Columns["Check"];

            //Hide the key columns. An end-user can access them from the Customization Form.
            treeList1.Columns[treeList1.KeyFieldName].Visible = false;
            treeList1.Columns[treeList1.ParentFieldName].Visible = false;

            //Make the Project column read-only.
            colName.OptionsColumn.ReadOnly = true;
            colCheck.OptionsColumn.ReadOnly = false;

            colName.OptionsColumn.AllowEdit = false;

            //Sort data against the Project column
            colName.SortIndex = -1;// 0;

            repositoryItemCheckEdit1.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.CheckBox;
            repositoryItemCheckEdit1.EditValueChanged += repositoryItemCheckEdit1_EditValueChanged;
            //treeList1.OptionsView.ShowCheckBoxes = true; // 제일 앞에 checkBox 붙이는 옵션
            treeList1.CellValueChanged += treeList1_CellValueChanged;

            treeList1.ExpandAll();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate
            {
                treeList1.BestFitColumns();
            }));



        }

        private void treeList1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            foreach (TreeListNode node in e.Node.Nodes)
                node[e.Column] = e.Value;
            TreeListNode parent = e.Node.ParentNode;
            if (parent != null) // not a root node
            {
                bool checkedValue = false;
                if (e.Value != null)
                    checkedValue = (bool)e.Value;
                foreach (TreeListNode node in parent.Nodes)
                {
                    if ((bool)node[e.Column] != checkedValue)
                    {
                        parent[e.Column] = null;
                        break;
                    }
                    else
                        parent[e.Column] = checkedValue;
                }
            }
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

            return list;
        }
    }
}
