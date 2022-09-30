using DevExpress.XtraEditors;
using DynaRAP.Data;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class SBProgressForm : DevExpress.XtraEditors.XtraForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int progress = 0;
        bool isCompleted = false;
        string uploadSeq = string.Empty;
        int fetchCount = 0;
        int totalFetchCount = 100;
        int nowCount = 0;
        bool bFirst = true;
        public SBProgressForm()
        {
            InitializeComponent();
            bFirst = true;

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        public SBProgressForm(int totalCount) : this()
        {
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            this.totalFetchCount = totalCount;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isCompleted)
            {
                timer.Stop();
                this.Close();
            }

            if (GetProgress())
            {
                if (fetchCount >= totalFetchCount)
                {
                    progress = totalFetchCount;
                    isCompleted = true;
                }
                progressBar.Position = fetchCount;
                lblProgress.Text = string.Format("{0} / {1}", fetchCount, totalFetchCount);

                bFirst = false;
            }

        }

        private void SBProgressForm_Load(object sender, EventArgs e)
        {
            isCompleted = false;
            lblProgress.Text = String.Empty;

            progressBar.Properties.ShowTitle = true;

            if (GetProgress())
            {
                progressBar.Properties.Minimum = 0;
                progressBar.Properties.Maximum = this.totalFetchCount;

                if (fetchCount >= totalFetchCount)
                {
                    progress = totalFetchCount;
                    isCompleted = true;
                }
                progressBar.Position = fetchCount;
                lblProgress.Text = string.Format("{0} / {1}", fetchCount, totalFetchCount);

                timer.Start();
            }
        }

        private bool GetProgress()
        {
            if (nowCount == totalFetchCount)
            {
                isCompleted = true;
                timer.Stop();
                DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                this.fetchCount = nowCount;
                //this.totalFetchCount = result.response.totalFetchCount;
            }
            return true;
        }

        public void SetNowCount(int nowCount)
        {
            this.fetchCount = nowCount;
            this.nowCount = nowCount;
        }
    }
}