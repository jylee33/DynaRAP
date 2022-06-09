using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using DynaRAP.UControl;
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
    public partial class TestTab1Form : DevExpress.XtraEditors.XtraForm
    {
        public TestTab1Form()
        {
            InitializeComponent();
        }

        private void TestTab1Form_Load(object sender, EventArgs e)
        {
            xtraTabControl1.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover;
            xtraTabControl1.CloseButtonClick += XtraTabControl1_CloseButtonClick;
            AddTabPage("TEST1");
            AddTabPage("TEST2");
            AddTabPage("TEST3");
            AddTabPage("TEST4");
            AddTabPage("TEST5");
        }

        private void XtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
            (arg.Page as XtraTabPage).PageVisible = false;
        }

        private void AddTabPage(string tabName)
        {
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = tabName;
            tabPage.Text = tabName;
            //tabPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;

            DllParamControl paramControl = new DllParamControl();
            paramControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(paramControl);
        }
    }
}