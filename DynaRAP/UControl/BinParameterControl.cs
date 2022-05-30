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
    public partial class BinParameterControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler DeleteBtnClicked;

        public string Title
        {
            set
            {
                //labelControl1.Text = value;
            }
        }

        public BinParameterControl()
        {
            InitializeComponent();
        }

        private void BinParameterControl_Load(object sender, EventArgs e)
        {
            InitializeTypeCombo();
            InitializeNameCombo();
        }

        private void InitializeTypeCombo()
        {
            cboType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboType.SelectedIndexChanged += cboType_SelectedIndexChanged;

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
            cboName.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboName.SelectedIndexChanged += cboName_SelectedIndexChanged;

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
