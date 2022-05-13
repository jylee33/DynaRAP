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

namespace DynaRAP
{
    public partial class SelectSBForm : DevExpress.XtraEditors.XtraForm
    {
        public SelectSBForm()
        {
            InitializeComponent();
        }

        private void SelectSBForm_Load(object sender, EventArgs e)
        {
            this.Text = "ImportModuleScenarioName";
        }

    }
}