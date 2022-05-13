using DevExpress.XtraEditors;
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

namespace DynaRAP.UControl
{
    public partial class SBIntervalControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler ViewBtnClicked;
        SplittedSB sb;


        public string Title
        {
            set
            {
                edtSbName.Text = value;
            }
        }

        public SBIntervalControl()
        {
            InitializeComponent();
        }

        public SBIntervalControl(SplittedSB sb) : this()
        {
            this.sb = sb;
        }

        private void SBIntervalControl_Load(object sender, EventArgs e)
        {
            edtSbName.Text = sb.SbName;
            edtStartTime.Text = sb.StartTime;
            edtEndTime.Text = sb.EndTime;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (this.ViewBtnClicked != null)
                this.ViewBtnClicked(this, new EventArgs());
        }
    }
}
