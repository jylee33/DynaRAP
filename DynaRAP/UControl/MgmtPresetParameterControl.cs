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
    public partial class MgmtPresetParameterControl : DevExpress.XtraEditors.XtraUserControl
    {
        List<ResponseParam> presetParamList = null;
        ResponseParam selectedParam = null;

        public string Title
        {
            set
            {
                cboParamType.Text = value;
            }
        }

        public ResponseParam SelectedParam { get => selectedParam; set => selectedParam = value; }

        public MgmtPresetParameterControl()
        {
            InitializeComponent();
        }

        public MgmtPresetParameterControl(List<ResponseParam> presetParamList) : this()
        {
            this.presetParamList = presetParamList;
        }

        private void MgmtPresetParameterControl_Load(object sender, EventArgs e)
        {
            InitializeParamTypeCombo();

            if(this.SelectedParam != null)
            {
                cboParamKey.Text = selectedParam.paramKey;
                edtX.Text = selectedParam.lrpX.ToString();
                edtY.Text = selectedParam.lrpY.ToString();
                edtZ.Text = selectedParam.lrpZ.ToString();
            }
        }

        private void InitializeParamTypeCombo()
        {
            cboParamType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboParamType.SelectedIndexChanged += CboParamType_SelectedIndexChanged;
        }

        private void CboParamType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


    }
}
