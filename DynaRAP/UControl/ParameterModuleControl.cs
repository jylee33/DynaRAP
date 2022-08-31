using DevExpress.XtraEditors;
using DevExpress.XtraTab;
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
    public partial class ParameterModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        ParamDataSelectControl dataSelectionControl = null;
        ParamExpressionControl expressionControl = null;
        ParamPlotControl plotControl = null;

        public ParameterModuleControl()
        {
            InitializeComponent();

            dataSelectionControl = new ParamDataSelectControl();
            expressionControl = new ParamExpressionControl();
            plotControl = new ParamPlotControl();
        }

        private void ParameterModuleControl_Load(object sender, EventArgs e)
        {
            //xtraTabControl1.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover;
            //xtraTabControl1.CloseButtonClick += XtraTabControl1_CloseButtonClick;

            AddTabPage("ParamDataSelect", "데이터 선택", dataSelectionControl);
            AddTabPage("ParamExpression", "수식 관리", expressionControl);
            AddTabPage("ParamPlot", "PLOT 구성", plotControl);

        }

        private void XtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
        }

        private void AddTabPage(string tabName, string tabTitle, XtraUserControl ctrl)
        {
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = tabName;
            tabPage.Text = tabTitle;
            //tabPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;

            ctrl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(ctrl);
        }

    }
}
