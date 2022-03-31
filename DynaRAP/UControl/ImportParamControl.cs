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

namespace DynaRAP.UControl
{
    public partial class ImportParamControl : DevExpress.XtraEditors.XtraUserControl
    {

        public string Title
        {
            set
            {
                labelControl1.Text = value;
            }
        }

        public ImportParamControl()
        {
            InitializeComponent();
        }

        private void ImportParamControl_Load(object sender, EventArgs e)
        {
            labelControl1.Visible = false;
        }
    }
}
