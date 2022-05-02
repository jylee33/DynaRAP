using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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
        public string Title
        {
            set
            {
            }
        }

        public MgmtPresetParameterControl()
        {
            InitializeComponent();
        }

        private void MgmtPresetParameterControl_Load(object sender, EventArgs e)
        {
            InitializeParamTypeCombo();
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
