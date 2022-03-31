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
    public partial class SBIntervalControl : DevExpress.XtraEditors.XtraUserControl
    {
        public string Title
        {
            set
            {
                edtFlying.Text = value;
            }
        }

        public SBIntervalControl()
        {
            InitializeComponent();
        }

        private void SBIntervalControl_Load(object sender, EventArgs e)
        {

        }
    }
}
