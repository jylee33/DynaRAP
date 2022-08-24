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
    public partial class ImportProgressForm : DevExpress.XtraEditors.XtraForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int progress = 0;
        bool isCompleted = false;
        string uploadSeq = string.Empty;
        int fetchCount = 0;
        int totalFetchCount = 0;
        bool bFirst = true;
        List<string> notMappedParams = new List<string>();

        public List<string> NotMappedParams { get => notMappedParams; set => notMappedParams = value; }

        public ImportProgressForm()
        {
            InitializeComponent();
            bFirst = true;

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        public ImportProgressForm(string uploadSeq) : this()
        {
            timer.Interval = 3000;
            timer.Tick += Timer_Tick;

            this.uploadSeq = uploadSeq;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isCompleted)
            {
                timer.Stop();
                MessageBox.Show("완료");
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

        private void ImportProgressForm_Load(object sender, EventArgs e)
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
            try
            {
                string url = ConfigurationManager.AppSettings["UrlImport"];
                string sendData = string.Format(@"
            {{
            ""command"":""progress"",
            ""uploadSeq"":""{0}""
            }}"
                , uploadSeq);

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                ImportResponse result = JsonConvert.DeserializeObject<ImportResponse>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        if (result.response.status.Equals("import-done"))
                        {
                            isCompleted = true;
                            timer.Stop();

                            if (bFirst)
                                MessageBox.Show(result.response.statusMessage);
                            else
                                MessageBox.Show("완료");

                            this.Close();
                        }
                        else if (result.response.status.Equals("error"))
                        {
                            isCompleted = true;
                            timer.Stop();

                            MessageBox.Show(result.response.statusMessage);

                            this.notMappedParams = result.response.notMappedParams;
                            DialogResult = DialogResult.Cancel;
                            this.Close();
                        }
                        else
                        {
                            this.fetchCount = result.response.fetchCount;
                            this.totalFetchCount = result.response.totalFetchCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }
    }
}