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
    public partial class BinParameterControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler DeleteBtnClicked;

        ResponseParam param = null;

        public string Title
        {
            set
            {
                //labelControl1.Text = value;
            }
        }

        public ResponseParam Param
        {
            get { return this.param; }
        }

        public BinParameterControl()
        {
            InitializeComponent();
        }

        public BinParameterControl(ResponseParam param) : this()
        {
            this.param = param;
        }

        private void BinParameterControl_Load(object sender, EventArgs e)
        {
            cboType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboName.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
          
            cboType.SelectedIndexChanged += cboType_SelectedIndexChanged;
            cboName.SelectedIndexChanged += cboName_SelectedIndexChanged;

            InitializeTypeCombo();
            InitializeNameCombo();

            string paramType = string.Empty;
            string paramName = string.Empty;

            if (param != null)
            {
                paramType = "";// param.paramKey;
                paramName = param.paramKey;
            }

            cboType.Text = paramType;
            cboName.Text = paramName;
        }

        private void InitializeTypeCombo()
        {


            cboType.Properties.Items.Add("ShortBlock 파라미터 Preset #1");
            cboType.Properties.Items.Add("ShortBlock 파라미터 Preset #2");
            cboType.Properties.Items.Add("ShortBlock 파라미터 Preset #3");

            cboType.SelectedIndex = 0;
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void InitializeNameCombo()
        {


            cboName.Properties.Items.Add("SW903P_NM");
            cboName.Properties.Items.Add("SW903P_NM");
            cboName.Properties.Items.Add("SW903P_NM");

            cboName.SelectedIndex = 0;
        }

        private void cboName_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.DeleteBtnClicked != null)
                this.DeleteBtnClicked(this, new EventArgs());
        }
    }
}
