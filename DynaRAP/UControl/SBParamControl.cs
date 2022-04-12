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
    public partial class SBParamControl : DevExpress.XtraEditors.XtraUserControl
    {
        public string Title
        {
            set
            {
            }
        }

        public SBParamControl()
        {
            InitializeComponent();
        }

        private void SBParamControl_Load(object sender, EventArgs e)
        {
            InitializeParamTypeCombo();
            InitializeParamNameCombo();
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

    }
}
