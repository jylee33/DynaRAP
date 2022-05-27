using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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
    public partial class MgmtLRPExtraControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler DeleteBtnClicked;

        string extraKey = string.Empty;
        string extraVal = string.Empty;

        public string Title
        {
            set
            {
            }
        }

        public string ExtraKey { get => extraKey; set => extraKey = value; }
        public string ExtraVal { get => extraVal; set => extraVal = value; }

        public MgmtLRPExtraControl()
        {
            InitializeComponent();
        }

        public MgmtLRPExtraControl(string extraKey, string extraVal) : this()
        {
            this.extraKey = extraKey;
            this.extraVal = extraVal;
        }

        private void MgmtLRPExtraControl_Load(object sender, EventArgs e)
        {
            edtKey.Text = extraKey;
            edtValue.Text = extraVal;
        }

        private void CboParamType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.DeleteBtnClicked != null)
                this.DeleteBtnClicked(this, new EventArgs());
        }
    }
}
