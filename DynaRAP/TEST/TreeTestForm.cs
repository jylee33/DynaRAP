using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.StyleFormatConditions;
using DynaRAP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.TEST
{
    public partial class TreeTestForm : DevExpress.XtraEditors.XtraForm
    {
        public TreeTestForm()
        {
            InitializeComponent();
        }

        private void TreeTestForm_Load(object sender, EventArgs e)
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
            treeListProject.ParentFieldName = "RegionID";
            //Allow the treelist to create columns bound to the fields the KeyFieldName and ParentFieldName properties specify.
            treeListProject.OptionsBehavior.PopulateServiceColumns = true;
            //Specify the data source.
            treeListProject.DataSource = SalesDataGenerator.CreateData();
            //The treelist automatically creates columns for the public fields found in the data source. 
            //You do not need to call the TreeList.PopulateColumns method unless the treeList1.OptionsBehavior.AutoPopulateColumns option is disabled.

            //Change the row height.
            treeListProject.RowHeight = 23;

            //Access the automatically created columns.
            TreeListColumn colRegion = treeListProject.Columns["Region"];
            TreeListColumn colMarchSales = treeListProject.Columns["MarchSales"];
            TreeListColumn colSeptemberSales = treeListProject.Columns["SeptemberSales"];
            TreeListColumn colMarchSalesPrev = treeListProject.Columns["MarchSalesPrev"];
            TreeListColumn colSeptemberSalesPrev = treeListProject.Columns["SeptemberSalesPrev"];
            TreeListColumn colMarketShare = treeListProject.Columns["MarketShare"];

            //Hide the key columns. An end-user can access them from the Customization Form.
            treeListProject.Columns[treeListProject.KeyFieldName].Visible = false;
            treeListProject.Columns[treeListProject.ParentFieldName].Visible = false;

            //Format column headers and cell values.
            colMarchSalesPrev.Caption = "<i>Previous <b>March</b> Sales</i>";
            colSeptemberSalesPrev.Caption = "<i>Previous <b>September</b> Sales</i>";
            treeListProject.OptionsView.AllowHtmlDrawHeaders = true;
            colMarchSalesPrev.AppearanceCell.Font = new System.Drawing.Font(colMarchSalesPrev.AppearanceCell.Font, System.Drawing.FontStyle.Italic);
            colSeptemberSalesPrev.AppearanceCell.Font = new System.Drawing.Font(colSeptemberSalesPrev.AppearanceCell.Font, System.Drawing.FontStyle.Italic);

            //Create two hidden unbound columns that calculate their values from expressions.
            TreeListColumn colUnboundMarchChange = treeListProject.Columns.AddField("FromPrevMarchChange");
            colUnboundMarchChange.Caption = "Change from prev March";
            colUnboundMarchChange.UnboundDataType = typeof(decimal);
            colUnboundMarchChange.UnboundExpression = "[MarchSales]-[MarchSalesPrev]";
            TreeListColumn colUnboundSeptemberChange = treeListProject.Columns.AddField("FromPrevSepChange");
            colUnboundSeptemberChange.Caption = "Change from prev September";
            colUnboundSeptemberChange.UnboundDataType = typeof(decimal);
            colUnboundSeptemberChange.UnboundExpression = "[SeptemberSales]-[SeptemberSalesPrev]";
            colUnboundMarchChange.OptionsColumn.ShowInCustomizationForm = false;
            colUnboundSeptemberChange.OptionsColumn.ShowInCustomizationForm = false;

            //Make the Region column read-only.
            colRegion.OptionsColumn.ReadOnly = true;

            //Sort data against the Region column
            colRegion.SortIndex = 0;

            //Apply a filter.
            treeListProject.ActiveFilterString = "[MarchSales] > 10000";

            //Calculate two total summaries against root nodes.
            colMarchSales.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            colMarchSales.SummaryFooterStrFormat = "Total={0:c0}";
            colMarchSales.AllNodesSummary = false;
            colSeptemberSales.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            colSeptemberSales.SummaryFooterStrFormat = "Total={0:c0}";
            colSeptemberSales.AllNodesSummary = false;
            treeListProject.OptionsView.ShowSummaryFooter = true;

            //Use a 'SpinEdit' in-place editor for the *Sales columns.
            RepositoryItemSpinEdit riSpinEdit = new RepositoryItemSpinEdit();
            riSpinEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            riSpinEdit.DisplayFormat.FormatString = "c0";
            treeListProject.RepositoryItems.Add(riSpinEdit);
            colMarchSales.ColumnEdit = riSpinEdit;
            colMarchSalesPrev.ColumnEdit = riSpinEdit;
            colSeptemberSales.ColumnEdit = riSpinEdit;
            colSeptemberSalesPrev.ColumnEdit = riSpinEdit;

            //Apply Excel-style formatting: display predefined 'Up Arrow' and 'Down Arrow' icons based on the unbound column values.
            TreeListFormatRule rule1 = new TreeListFormatRule();
            rule1.Rule = createThreeTrianglesIconSetRule();
            rule1.Column = colUnboundMarchChange;
            rule1.ColumnApplyTo = colMarchSales;
            TreeListFormatRule rule2 = new TreeListFormatRule();
            rule2.Rule = createThreeTrianglesIconSetRule();
            rule2.Column = colUnboundSeptemberChange;
            rule2.ColumnApplyTo = colSeptemberSales;
            treeListProject.FormatRules.Add(rule1);
            treeListProject.FormatRules.Add(rule2);

            //Do not stretch columns to the treelist width.
            treeListProject.OptionsView.AutoWidth = false;

            //Locate a node by a value it contains.
            TreeListNode node1 = treeListProject.FindNodeByFieldValue("Region", "North America");
            //Focus and expand this node.
            treeListProject.FocusedNode = node1;
            node1.Expanded = true;

            //Locate a node by its key field value and expand it.
            TreeListNode node2 = treeListProject.FindNodeByKeyID(32);//Node 'Asia'
            node2.Expand();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate {
                treeListProject.BestFitColumns();
            }));
        }

        FormatConditionRuleIconSet createThreeTrianglesIconSetRule()
        {
            FormatConditionRuleIconSet ruleIconSet = new FormatConditionRuleIconSet();
            FormatConditionIconSet iconSet = ruleIconSet.IconSet = new FormatConditionIconSet();
            FormatConditionIconSetIcon icon1 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon icon2 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon icon3 = new FormatConditionIconSetIcon();
            //Choose predefined icons. 
            icon1.PredefinedName = "Triangles3_3.png";
            icon2.PredefinedName = "Triangles3_2.png";
            icon3.PredefinedName = "Triangles3_1.png";
            //Specify the type of threshold values. 
            iconSet.ValueType = FormatConditionValueType.Number;
            //Define ranges to which icons are applied by setting threshold values. 
            icon1.Value = Decimal.MinValue;
            icon1.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            icon2.Value = 0;
            icon2.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            icon3.Value = 0;
            icon3.ValueComparison = FormatConditionComparisonType.Greater;
            //Add icons to the icon set. 
            iconSet.Icons.Add(icon1);
            iconSet.Icons.Add(icon2);
            iconSet.Icons.Add(icon3);
            return ruleIconSet;
        }

    }

    
}