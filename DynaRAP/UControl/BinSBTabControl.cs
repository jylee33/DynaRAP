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
    public partial class BinSBTabControl : DevExpress.XtraEditors.XtraUserControl
    {
        private string idxValue = string.Empty;

        public string IdxValue
        {
            get { return idxValue; }
            set { idxValue = value; }
        }

        public BinSBTabControl()
        {
            InitializeComponent();
        }

        private void BinSBTabControl_Load(object sender, EventArgs e)
        {
            AddTabPage("SB1");
            AddTabPage("SB2");
            AddTabPage("SB3");
            AddTabPage("SB4");
            AddTabPage("SB5");
        }

        private void AddTabPage(string tabName)
        {
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = tabName;
            tabPage.Text = tabName;

            BinSBInfoControl sbInfoControl = new BinSBInfoControl();
            sbInfoControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(sbInfoControl);
        }
    }
}
