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

namespace DynaRAP
{
    public partial class CreateScenarioProgressForm : DevExpress.XtraEditors.XtraForm
    {
        string strUploading = Properties.Resources.UploadingFlightData;
        string strExtracting = Properties.Resources.ExtractFlightData;
        string strProcessing = Properties.Resources.BasicProcessingFlightData;

        public CreateScenarioProgressForm()
        {
            InitializeComponent();
        }

        private void CreateScenarioProgressForm_Load(object sender, EventArgs e)
        {
            labelControl1.AllowHtmlString = true;
            labelControl2.AllowHtmlString = true;
            labelControl3.AllowHtmlString = true;
            labelControl1.Text = strUploading + "[완료]";
            labelControl2.Text = strExtracting + "[완료]";
            labelControl3.Text = "<color=255, 0, 0>" + strProcessing + "</color>";

            progressBar.Properties.ShowTitle = true;
            //progressBar.Properties.Minimum = 0;
            //progressBar.Properties.Maximum = 100;
            progressBar.Position = 68;

        }
    }
}