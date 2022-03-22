using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            // Handling the QueryControl event that will populate all automatically generated Documents
            this.tabbedView1.QueryControl += tabbedView1_QueryControl;
        }

        void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            if (e.Document == userControl1Document)
                e.Control = new DynaRAP.UserControl1();
            if (e.Document == userControl2Document)
                e.Control = new DynaRAP.UserControl2();
            if (e.Control == null)
                e.Control = new System.Windows.Forms.Control();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("test");
        }
    }
}
