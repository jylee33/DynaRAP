using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace DynaRAP.UControl
{
    public partial class BinModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        string selectedFuselage = string.Empty;
        Series series1 = new Series();
        ChartArea myChartArea = new ChartArea("LineChartArea");
        DockPanel sbListPanel = null;

        public BinModuleControl()
        {
            InitializeComponent();
        }

        private void BinModuleControl_Load(object sender, EventArgs e)
        {
            InitializeSBParamComboList();

            //DateTime dtNow = DateTime.Now;
            //string strNow = string.Format("{0:yyyy-MM-dd}", dtNow);
            //dateScenario.Text = strNow;

            panelData.AutoScroll = true;
            panelData.WrapContents = false;
            panelData.HorizontalScroll.Visible = false;
            panelData.VerticalScroll.Visible = true;

            btnAddParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddParameter.Properties.AllowFocused = false;

        }


        private void InitializeSBParamComboList()
        {
            cboSBParameter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboSBParameter.SelectedIndexChanged += CboSBParameter_SelectedIndexChanged;

            cboSBParameter.Properties.Items.Add("ShortBlock 파라미너 Preset #1");
            cboSBParameter.Properties.Items.Add("ShortBlock 파라미너 Preset #2");
            cboSBParameter.Properties.Items.Add("ShortBlock 파라미너 Preset #3");

            cboSBParameter.SelectedIndex = 0;
        }

        private void CboSBParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnAddParameter_ButtonClick(object sender, EventArgs e)
        {
            AddParameter();
        }

        int index = 10;

        private void AddParameter()
        {
            BinParameterControl ctrl = new BinParameterControl();
            ctrl.Title = "Parameter " + index.ToString();
            panelData.Controls.Add(ctrl);
            panelData.Controls.SetChildIndex(ctrl, index++);

        }

        private void hyperlinkBrowseSB_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;

            mainForm.PanelBinSbList.Show();

        }

        private void btnCreateBIN_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;

            mainForm.PanelBinTable.Show();
        }
    }

    
}
