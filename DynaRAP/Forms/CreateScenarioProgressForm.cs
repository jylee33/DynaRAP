using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.Forms
{
    public partial class CreateScenarioProgressForm : DevExpress.XtraEditors.XtraForm
    {
        string strUploading = Properties.Resources.UploadingFlightData;
        string strExtracting = Properties.Resources.ExtractFlightData;
        string strProcessing = Properties.Resources.BasicProcessingFlightData;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int progress = 0;
        bool isCompleted = false;

        public CreateScenarioProgressForm()
        {
            InitializeComponent();

            timer.Interval = 500;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isCompleted)
            {
                timer.Stop();
                this.Close();
            }

            Random rand = new Random();

            progress += rand.Next(5, 15);

            if(progress > 100)
            {
                progress = 100;
                isCompleted = true;
            }
            progressBar.Position = progress;

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

            isCompleted = false;
            timer.Start();

        }
    }
}