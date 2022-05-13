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
    public partial class SBParamControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler DeleteBtnClicked;
       
        ResponseParam param = null;
        string title = string.Empty;
      
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public SBParamControl()
        {
            InitializeComponent();
        }

        public SBParamControl(ResponseParam param) : this()
        {
            this.param = param;
        }

        private void SBParamControl_Load(object sender, EventArgs e)
        {
            InitializeParamTypeCombo();
            InitializeParamNameCombo();

            string paramType = string.Empty;
            string paramName = string.Empty;

            if(param!= null)
            {
                paramType = this.title;// param.paramKey;
                paramName = param.paramKey;
            }

            cboParamType.Text = paramType;
            cboParamName.Text = paramName;
        }

        private void InitializeParamNameCombo()
        {
            cboParamName.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboParamName.SelectedIndexChanged += CboParamName_SelectedIndexChanged;

        }

        private void InitializeParamTypeCombo()
        {
            cboParamType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboParamType.SelectedIndexChanged += CboParamType_SelectedIndexChanged;
        }

        private void CboParamType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void CboParamName_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.DeleteBtnClicked != null)
                this.DeleteBtnClicked(this, new EventArgs());
        }
    }
}
