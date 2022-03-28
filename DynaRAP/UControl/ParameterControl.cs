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
    public partial class ParameterControl : DevExpress.XtraEditors.XtraUserControl
    {

        public string Title
        {
            set
            {
                labelControl1.Text = value;
            }
        }

        public ParameterControl()
        {
            InitializeComponent();
        }

        private void ParameterControl_Load(object sender, EventArgs e)
        {

        }
    }
}
