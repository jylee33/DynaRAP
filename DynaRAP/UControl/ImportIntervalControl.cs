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
    public partial class ImportIntervalControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler DeleteBtnClicked;

        public string Title
        {
            set
            {
                edtFlying.Text = value;
            }
        }

        public ImportIntervalControl()
        {
            InitializeComponent();
        }

        public ImportIntervalControl(object min, object max) : this()
        {
            edtStartTime.Text = min.ToString();
            edtEndTime.Text = max.ToString();
        }

        private void ImportIntervalControl_Load(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.DeleteBtnClicked != null)
                this.DeleteBtnClicked(this, new EventArgs());
        }
    }
}
