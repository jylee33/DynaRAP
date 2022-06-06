using DevExpress.XtraEditors;
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
    public partial class TestSplitContainerForm : DevExpress.XtraEditors.XtraForm
    {
        public TestSplitContainerForm()
        {
            InitializeComponent();
        }

        private void TestSplitContainerForm_Load(object sender, EventArgs e)
        {
            splitContainerControl1.FixedPanel = SplitFixedPanel.Panel2;
            splitContainerControl1.CollapsePanel = SplitCollapsePanel.Panel2;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            splitContainerControl1.Collapsed = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            splitContainerControl1.Collapsed = false;
        }
    }
}